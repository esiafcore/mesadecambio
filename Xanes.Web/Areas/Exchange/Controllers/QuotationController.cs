﻿using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Xanes.DataAccess.Repository.IRepository;
using System.Text.Json;
using Xanes.Models;
using Xanes.Models.Shared;
using Xanes.Models.ViewModels;
using Xanes.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using static Xanes.Utility.SD;
using static Stimulsoft.Report.Help.StiHelpProvider;

namespace Xanes.Web.Areas.Exchange.Controllers;

[Area("Exchange")]
public class QuotationController : Controller
{
    private readonly IUnitOfWork _uow;
    private readonly IConfiguration _configuration;
    private readonly int _companyId;
    private readonly int _decimalTransa;
    private readonly int _decimalExchange;
    private Dictionary<ParametersReport, object?> _parametersReport;
    private readonly IWebHostEnvironment _hostEnvironment;

    public QuotationController(IUnitOfWork uow, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        _uow = uow;
        _configuration = configuration;
        _companyId = _configuration.GetValue<int>("ApplicationSettings:CompanyId");
        _decimalTransa = _configuration.GetValue<int>("ApplicationSettings:DecimalTransa");
        _decimalExchange = _configuration.GetValue<int>("ApplicationSettings:DecimalExchange");
        _hostEnvironment = hostEnvironment;

        _parametersReport = new();
        var path = Path.Combine(Directory.GetCurrentDirectory(), "License/license.key");
        // Verificar si el archivo existe
        FileInfo file = new FileInfo(path);
        if (file.Exists)
        {
            Stimulsoft.Base.StiLicense.LoadFromFile(path);
        }
    }

    public IActionResult Index()
    {
        ViewBag.DecimalTransa = JsonSerializer.Serialize(_decimalTransa);
        ViewBag.DecimalExchange = JsonSerializer.Serialize(_decimalExchange);
        return View();
    }

    public IActionResult Create()
    {
        ViewBag.DecimalTransa = JsonSerializer.Serialize(_decimalTransa);
        ViewBag.DecimalExchange = JsonSerializer.Serialize(_decimalExchange);
        QuotationCreateVM model = new();
        Quotation objData = new();

        var objCurrencyList = _uow.Currency
            .GetAll(x => (x.CompanyId == _companyId))
            .ToList();

        if (objCurrencyList == null)
        {
            return NotFound();
        }

        var objTypeList = _uow.QuotationType
            .GetAll(x => (x.CompanyId == _companyId))
            .ToList();

        if (objTypeList == null)
        {
            return NotFound();
        }

        var objCustomerList = _uow.Customer
            .GetAll(x => (x.CompanyId == _companyId))
            .ToList();

        if (objTypeList == null)
        {
            return NotFound();
        }

        objData = new Quotation
        {
            DateTransa = DateOnly.FromDateTime(DateTime.UtcNow),
            TypeNumeral = SD.QuotationType.Buy,
            CurrencyTransaType = SD.CurrencyType.Foreign,
            CurrencyDepositType = SD.CurrencyType.Base,
            CompanyId = _companyId
        };

        model.CurrencyTransaList = objCurrencyList
            .Where(x => (x.IsActive && (x.Numeral != (int)SD.CurrencyType.Base)))
            .ToList();
        model.CurrencyDepositList = objCurrencyList.Where(x => x.IsActive).ToList();
        model.CurrencyTransferList = objCurrencyList.Where(x => x.IsActive).ToList();
        model.QuotationTypeList = objTypeList;
        model.CustomerList = objCustomerList.Select(x => new SelectListItem { Text = x.CommercialName, Value = x.Id.ToString() });
        model.DataModel = objData;

        return View(model);
    }

    [HttpPost]
    public IActionResult Create(Models.ViewModels.QuotationCreateVM objViewModel)
    {
        Models.Quotation obj = objViewModel.DataModel;
        //Datos son validos
        if (ModelState.IsValid)
        {
            if (obj.CompanyId != _companyId)
            {
                ModelState.AddModelError("", $"Id de la compañía no puede ser distinto de {_companyId}");
            }

            //Verificamos si existe la moneda de la Transaccion
            var objCurrency = _uow.Currency.Get(filter: x =>
                x.CompanyId == obj.CompanyId && x.Numeral == (int)obj.CurrencyTransaType);

            if (objCurrency == null)
            {
                ModelState.AddModelError("", $"Moneda de la transacción no encontrada");
            }
            else
            {
                obj.CurrencyTransaId = objCurrency.Id;
            }

            //Verificamos si existe la moneda de deposito
            objCurrency = _uow.Currency.Get(filter: x =>
                x.CompanyId == obj.CompanyId && x.Numeral == (int)obj.CurrencyDepositType);

            if (objCurrency == null)
            {
                ModelState.AddModelError("", $"Moneda de deposito no encontrada");
            }
            else
            {
                obj.CurrencyDepositId = objCurrency.Id;
            }

            //Verificamos si existe la moneda de transferencia
            objCurrency = _uow.Currency.Get(filter: x =>
                x.CompanyId == obj.CompanyId && x.Numeral == (int)obj.CurrencyTransferType);

            if (objCurrency == null)
            {
                ModelState.AddModelError("", $"Moneda de transferencia no encontrada");
            }
            else
            {
                obj.CurrencyTransferId = objCurrency.Id;
            }

            //Verificamos si existe el tipo
            var objQuotationType = _uow.QuotationType.Get(filter: x =>
                x.CompanyId == obj.CompanyId && x.Numeral == (int)obj.TypeNumeral);

            if (objQuotationType == null)
            {
                ModelState.AddModelError("", $"Tipo de transacción no encontrado");
            }
            else
            {
                obj.TypeId = objQuotationType.Id;
            }

            //Verificamos si existe el cliente
            var objCustomer = _uow.Customer.Get(filter: x => x.CompanyId == obj.CompanyId && x.Id == obj.CustomerId);
            if (objCustomer == null)
            {
                ModelState.AddModelError("", $"Cliente no encontrado");
            }

            if (!ModelState.IsValid) return View(objViewModel);

            //Obtenemos el secuencial en borrador
            var numberTransa = _uow.ConfigFac.NextSequentialNumber(filter: x => x.CompanyId == obj.CompanyId,
                SD.TypeSequential.Draft, true);

            obj.Numeral = Convert.ToInt32(numberTransa.Result.ToString());
            obj.InternalSerial = AC.InternalSerialDraft;
            //COMPRA
            if (obj.TypeNumeral == SD.QuotationType.Buy)
            {
                //TC COMPRA MENOR AL TC OFICIAL
                if (obj.ExchangeRateBuyTransa < obj.ExchangeRateOfficialTransa)
                {
                    obj.AmountRevenue = (obj.ExchangeRateOfficialTransa - obj.ExchangeRateBuyTransa) * obj.AmountTransaction;
                    obj.AmountCost = 0;
                }
                //TC COMPRA MAYOR AL TC OFICIAL
                else
                {
                    obj.AmountCost = (obj.ExchangeRateBuyTransa - obj.ExchangeRateOfficialTransa) * obj.AmountTransaction;
                    obj.AmountRevenue = 0;
                }

                //Compra de dolares 
                if (obj.CurrencyTransaType == SD.CurrencyType.Foreign)
                {
                    //Factoring paga en Cordobas
                    if (obj.CurrencyTransferType == SD.CurrencyType.Base)
                    {
                        obj.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateBuyTransa);
                        obj.ExchangeRateBuyReal = obj.ExchangeRateBuyTransa;
                    }
                }
                //Compra de Euros
                else if (obj.CurrencyTransaType == SD.CurrencyType.Additional)
                {
                    //Factoring paga en Cordobas
                    if (obj.CurrencyTransferType == SD.CurrencyType.Base)
                    {
                        obj.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateBuyTransa);
                        obj.ExchangeRateBuyReal = obj.ExchangeRateBuyTransa;

                    }
                    //Factoring paga en Dolares
                    else if (obj.CurrencyTransferType == SD.CurrencyType.Foreign)
                    {
                        obj.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateSellTransa);
                        obj.ExchangeRateBuyReal = (obj.ExchangeRateBuyTransa * obj.ExchangeRateOfficialTransa);
                    }
                }
            }
            //VENTA
            else if (obj.TypeNumeral == SD.QuotationType.Sell)
            {
                //TC VENTA MENOR AL TC OFICIAL
                if (obj.ExchangeRateSellTransa < obj.ExchangeRateOfficialTransa)
                {
                    obj.AmountCost = (obj.ExchangeRateOfficialTransa - obj.ExchangeRateSellTransa) * obj.AmountTransaction;
                    obj.AmountRevenue = 0;
                }
                //TC VENTA MAYOR AL TC OFICIAL
                else
                {
                    obj.AmountRevenue = (obj.ExchangeRateSellTransa - obj.ExchangeRateOfficialTransa) * obj.AmountTransaction;
                    obj.AmountCost = 0;
                }

                //Venta de dolares 
                if (obj.CurrencyTransaType == SD.CurrencyType.Foreign)
                {
                    //Cliente paga en Cordobas
                    if (obj.CurrencyDepositType == SD.CurrencyType.Base)
                    {
                        obj.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateSellTransa);
                        obj.ExchangeRateSellReal = obj.ExchangeRateSellTransa;
                    }
                }
                //Venta de Euros
                else if (obj.CurrencyTransaType == SD.CurrencyType.Additional)
                {
                    //Cliente paga en Cordobas
                    if (obj.CurrencyDepositType == SD.CurrencyType.Base)
                    {
                        obj.AmountExchange = (obj.AmountTransaction / obj.ExchangeRateSellTransa);
                        obj.ExchangeRateSellReal = obj.ExchangeRateSellTransa;

                    }
                    //Cliente paga en Dolares
                    else if (obj.CurrencyDepositType == SD.CurrencyType.Foreign)
                    {
                        obj.AmountExchange = (obj.AmountTransaction / obj.ExchangeRateSellTransa);
                        obj.ExchangeRateSellReal = (obj.ExchangeRateSellTransa * obj.ExchangeRateOfficialTransa);
                    }
                }

            }


            //Seteamos campos de auditoria
            obj.CreatedBy = AC.LOCALHOSTME;
            obj.CreatedDate = DateTime.UtcNow;
            obj.CreatedHostName = AC.LOCALHOSTPC;
            obj.CreatedIpv4 = AC.Ipv4Default;
            obj.IsPosted = false;
            obj.IsClosed = false;
            obj.IsLoan = false;
            obj.IsPayment = false;
            _uow.Quotation.Add(obj);
            _uow.Save();
            TempData["success"] = "Cotización creada exitosamente";
            return RedirectToAction("CreateDetail", "Quotation", new { id = obj.Id });
        }
        else
        {
            StringBuilder errorsMessagesBuilder = new();

            List<string> listErrorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(x => x.ErrorMessage)
                .ToList();
            foreach (var item in listErrorMessages)
            {
                errorsMessagesBuilder.Append(item);
            }

            ModelState.AddModelError("", errorsMessagesBuilder.ToString());
        }

        var objCurrencyList = _uow.Currency
            .GetAll(x => (x.CompanyId == _companyId))
            .ToList();

        if (objCurrencyList == null)
        {
            return NotFound();
        }

        var objTypeList = _uow.QuotationType
            .GetAll(x => (x.CompanyId == _companyId))
            .ToList();

        if (objTypeList == null)
        {
            return NotFound();
        }

        var objCustomerList = _uow.Customer
            .GetAll(x => (x.CompanyId == _companyId))
            .ToList();

        if (objTypeList == null)
        {
            return NotFound();
        }

        objViewModel.CurrencyTransaList = objCurrencyList
            .Where(x => (x.IsActive && (x.Numeral != (int)SD.CurrencyType.Base)))
            .ToList();
        objViewModel.CurrencyDepositList = objCurrencyList.Where(x => x.IsActive).ToList();
        objViewModel.CurrencyTransferList = objCurrencyList.Where(x => x.IsActive).ToList();
        objViewModel.QuotationTypeList = objTypeList;
        objViewModel.CustomerList = objCustomerList.Select(x => new SelectListItem { Text = x.CommercialName, Value = x.Id.ToString() });

        return View(objViewModel);
    }

    [HttpPost]
    public IActionResult Update(Models.ViewModels.QuotationCreateVM objViewModel)
    {
        Models.Quotation obj = objViewModel.DataModel;
        //Datos son validos
        if (ModelState.IsValid)
        {
            if (obj.CompanyId != _companyId)
            {
                ModelState.AddModelError("", $"Id de la compañía no puede ser distinto de {_companyId}");
            }

            var objQt = _uow.Quotation.Get(filter: x => x.Id == obj.Id);
            if (objQt == null)
            {
                ModelState.AddModelError("", $"Cotización no encontrada");
            }
            //Verificamos si existe el cliente
            var objCustomer = _uow.Customer.Get(filter: x => x.CompanyId == obj.CompanyId && x.Id == obj.CustomerId, isTracking: false);
            if (objCustomer == null)
            {
                ModelState.AddModelError("", $"Cliente no encontrado");
            }

            if (!ModelState.IsValid) return RedirectToAction("CreateDetail", "Quotation", new { id = obj.Id });

            if (obj.TypeNumeral == SD.QuotationType.Buy)
            {
                if (obj.ExchangeRateBuyTransa < obj.ExchangeRateOfficialTransa)
                {
                    obj.AmountRevenue = (obj.ExchangeRateOfficialTransa - obj.ExchangeRateBuyTransa) * obj.AmountTransaction;
                    obj.AmountCost = 0;
                }
                else
                {
                    obj.AmountCost = (obj.ExchangeRateBuyTransa - obj.ExchangeRateOfficialTransa) * obj.AmountTransaction;
                    obj.AmountRevenue = 0;
                }
            }
            else if (obj.TypeNumeral == SD.QuotationType.Sell)
            {
                if (obj.ExchangeRateSellTransa < obj.ExchangeRateOfficialTransa)
                {
                    obj.AmountCost = (obj.ExchangeRateOfficialTransa - obj.ExchangeRateSellTransa) * obj.AmountTransaction;
                    obj.AmountRevenue = 0;
                }
                else
                {
                    obj.AmountRevenue = (obj.ExchangeRateSellTransa - obj.ExchangeRateOfficialTransa) * obj.AmountTransaction;
                    obj.AmountCost = 0;
                }
            }

            objQt.AmountRevenue = obj.AmountRevenue;
            objQt.AmountCost = obj.AmountCost;
            objQt.AmountTransaction = obj.AmountTransaction;
            objQt.DateTransa = obj.DateTransa;
            objQt.ExchangeRateBuyTransa = obj.ExchangeRateBuyTransa;
            objQt.ExchangeRateSellReal = obj.ExchangeRateSellReal;

            //Seteamos campos de auditoria
            objQt.UpdatedBy = AC.LOCALHOSTME;
            objQt.UpdatedDate = DateTime.UtcNow;
            objQt.UpdatedHostName = AC.LOCALHOSTPC;
            objQt.UpdatedIpv4 = AC.Ipv4Default;
            _uow.Quotation.Update(objQt);
            _uow.Save();
            TempData["success"] = "Cotización actualizada exitosamente";
        }
        else
        {
            StringBuilder errorsMessagesBuilder = new();

            List<string> listErrorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(x => x.ErrorMessage)
                .ToList();
            foreach (var item in listErrorMessages)
            {
                errorsMessagesBuilder.Append(item);
            }

            ModelState.AddModelError("", errorsMessagesBuilder.ToString());
        }

        return RedirectToAction("CreateDetail", "Quotation", new { id = obj.Id });
    }

    public IActionResult CreateDetail(int id)
    {
        QuotationDetailVM model = new();
        ViewBag.DecimalTransa = JsonSerializer.Serialize(_decimalTransa);
        ViewBag.DecimalExchange = JsonSerializer.Serialize(_decimalExchange);

        var objHeader = _uow.Quotation.Get(filter: x => x.CompanyId == _companyId && x.Id == id,
            includeProperties: "TypeTrx,CustomerTrx", isTracking: false);
        if (objHeader == null)
        {
            return NotFound();
        }

        var objBankList = _uow.Bank
            .GetAll(x => (x.CompanyId == _companyId))
            .ToList();

        if (objBankList == null)
        {
            return NotFound();
        }

        var objCurrencyList = _uow.Currency
            .GetAll(x => (x.CompanyId == _companyId))
            .ToList();

        if (objCurrencyList == null)
        {
            return NotFound();
        }

        var objTypeList = _uow.QuotationType
            .GetAll(x => (x.CompanyId == _companyId))
            .ToList();

        if (objTypeList == null)
        {
            return NotFound();
        }

        var objCustomerList = _uow.Customer
            .GetAll(x => (x.CompanyId == _companyId))
            .ToList();

        if (objTypeList == null)
        {
            return NotFound();
        }
        model.ModelCreateVM.CurrencyTransaList = objCurrencyList
            .Where(x => (x.IsActive && (x.Numeral != (int)SD.CurrencyType.Base)))
            .ToList();
        model.ModelCreateVM.CurrencyDepositList = objCurrencyList.Where(x => x.IsActive).ToList();
        model.ModelCreateVM.CurrencyTransferList = objCurrencyList.Where(x => x.IsActive).ToList();
        model.ModelCreateVM.QuotationTypeList = objTypeList;
        model.ModelCreateVM.CustomerList = objCustomerList.Select(x => new SelectListItem { Text = x.CommercialName, Value = x.Id.ToString() });

        //model.BankList = objBankList.Select(x => new SelectListItem { Text = $"{x.Code}", Value = x.Id.ToString() });
        model.BankList = objBankList;
        model.ModelCreateVM.DataModel = objHeader;
        model.CustomerFullName = $"{objHeader.CustomerTrx.CommercialName}";
        model.NumberTransa = $"COT-{objHeader.TypeTrx.Code}-{objHeader.Numeral}";
        model.DataModel = new();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDetail(Models.ViewModels.QuotationDetailVM objViewModel)
    {
        Models.QuotationDetail obj = objViewModel.DataModel;
        //Datos son validos
        //if (ModelState.IsValid)
        //{
        if (obj.CompanyId != _companyId)
        {
            ModelState.AddModelError("", $"Id de la compañía no puede ser distinto de {_companyId}");
        }

        //Verificamos si existe el padre
        var objHeader = _uow.Quotation.Get(filter: x =>
            x.CompanyId == obj.CompanyId && x.Id == obj.ParentId);

        if (objHeader == null)
        {
            ModelState.AddModelError("", $"Registro padre no encontrada");
        }

        //Verificamos si existe la moneda
        var objCurrency = _uow.Currency.Get(filter: x =>
            x.CompanyId == obj.CompanyId && x.Numeral == obj.CurrencyDetailId, isTracking: false);

        if (objCurrency == null)
        {
            ModelState.AddModelError("", $"Moneda no encontrada");
        }

        //Verificamos si existe el banco origen
        var objBankSource = _uow.Bank.Get(filter: x =>
            x.CompanyId == obj.CompanyId && x.Id == obj.BankSourceId, isTracking: false);

        if (objBankSource == null)
        {
            ModelState.AddModelError("", $"Banco Origen no encontrado");
        }

        //Verificamos si tiene banco destino y verificamos si existe
        if (obj.BankTargetId != 0)
        {
            var objBankTarget = _uow.Bank.Get(filter: x =>
                x.CompanyId == obj.CompanyId && x.Id == obj.BankTargetId, isTracking: false);

            if (objBankTarget == null)
            {
                ModelState.AddModelError("", $"Banco Destino no encontrado");
            }
        }
        else
        {
            obj.BankTargetId = obj.BankSourceId;
        }

        if (!ModelState.IsValid)
        {
            var objHeaderQt = _uow.Quotation.Get(filter: x => x.CompanyId == _companyId && x.Id == obj.ParentId,
                includeProperties: "TypeTrx,CustomerTrx", isTracking: false);
            if (objHeaderQt == null)
            {
                return NotFound();
            }

            var objBankList = _uow.Bank
                .GetAll(x => (x.CompanyId == _companyId))
                .ToList();

            if (objBankList == null)
            {
                return NotFound();
            }

            objViewModel.BankList = objBankList;
            objViewModel.ModelCreateVM.DataModel = objHeaderQt;
            objViewModel.CustomerFullName = $"{objHeaderQt.CustomerTrx.CommercialName}";
            objViewModel.NumberTransa = $"COT-{objHeaderQt.TypeTrx.Code}-{objHeaderQt.Numeral}";
            ViewBag.DecimalTransa = JsonSerializer.Serialize(_decimalTransa);
            return View(objViewModel);
        }


        if (obj.Id == 0)
        {
            //Seteamos campos de auditoria
            obj.LineNumber = await _uow.QuotationDetail.NextLineNumber(filter: x =>
                objHeader != null && x.CompanyId == obj.CompanyId && x.ParentId == objHeader.Id && x.QuotationDetailType == objViewModel.DataModel.QuotationDetailType);
            obj.CreatedBy = AC.LOCALHOSTME;
            obj.CreatedDate = DateTime.UtcNow;
            obj.CreatedHostName = AC.LOCALHOSTPC;
            obj.CreatedIpv4 = AC.Ipv4Default;
            _uow.QuotationDetail.Add(obj);
            _uow.Save();
            TempData["success"] = "Cotización creada exitosamente";
        }
        else
        {
            var objDetail = _uow.QuotationDetail.Get(filter: x => x.Id == obj.Id && x.CompanyId == _companyId);
            if (objDetail == null)
            {
                return NotFound();
            }
            //Seteamos campos de auditoria
            objDetail.AmountDetail = obj.AmountDetail;
            objDetail.BankSourceId = obj.BankSourceId;
            objDetail.BankTargetId = obj.BankTargetId;
            objDetail.QuotationDetailType = obj.QuotationDetailType;
            objDetail.UpdatedBy = AC.LOCALHOSTME;
            objDetail.UpdatedDate = DateTime.UtcNow;
            objDetail.UpdatedHostName = AC.LOCALHOSTPC;
            objDetail.UpdatedIpv4 = AC.Ipv4Default;
            _uow.QuotationDetail.Update(objDetail);
            _uow.Save();
            TempData["success"] = "Cotización actualizada exitosamente";
        }

        //Obtenemos los hijos
        var objDetails = _uow.QuotationDetail.GetAll(filter: x =>
                objHeader != null && x.CompanyId == obj.CompanyId && x.ParentId == objHeader.Id, includeProperties: "ParentTrx,CurrencyDetailTrx,BankSourceTrx,BankTargetTrx").ToList();

        if (objDetails == null)
        {
            return NotFound();
        }

        if (objHeader != null)
        {
            objHeader.TotalDeposit = objDetails
                .Where(x => x.QuotationDetailType == QuotationDetailType.Deposit)
                .Sum(x => x.AmountDetail);
            objHeader.TotalTransfer = objDetails
                .Where(x => x.QuotationDetailType == QuotationDetailType.Transfer)
                .Sum(x => x.AmountDetail);
            objHeader.UpdatedBy = AC.LOCALHOSTME;
            objHeader.UpdatedDate = DateTime.UtcNow;
            objHeader.UpdatedHostName = AC.LOCALHOSTPC;
            objHeader.UpdatedIpv4 = AC.Ipv4Default;
            _uow.Quotation.Update(objHeader);
            _uow.Save();
        }

        return RedirectToAction("CreateDetail", "Quotation", new { id = obj.ParentId });
    }

    public IActionResult Delete(int id)
    {
        QuotationDetailVM model = new();
        ViewBag.DecimalTransa = JsonSerializer.Serialize(_decimalTransa);
        ViewBag.DecimalExchange = JsonSerializer.Serialize(_decimalExchange);

        var objHeader = _uow.Quotation.Get(filter: x => x.CompanyId == _companyId && x.Id == id,
            includeProperties: "TypeTrx,CustomerTrx", isTracking: false);
        if (objHeader == null)
        {
            return NotFound();
        }

        var objBankList = _uow.Bank
            .GetAll(x => (x.CompanyId == _companyId))
            .ToList();

        if (objBankList == null)
        {
            return NotFound();
        }

        model.BankList = objBankList;
        model.ModelCreateVM.DataModel = objHeader;
        model.CustomerFullName = $"{objHeader.CustomerTrx.CommercialName}";
        model.NumberTransa = $"COT-{objHeader.TypeTrx.Code}-{objHeader.Numeral}";
        return View(model);
    }

    public IActionResult Detail(int id)
    {
        QuotationDetailVM model = new();
        ViewBag.DecimalTransa = JsonSerializer.Serialize(_decimalTransa);
        ViewBag.DecimalExchange = JsonSerializer.Serialize(_decimalExchange);

        var objHeader = _uow.Quotation.Get(filter: x => x.CompanyId == _companyId && x.Id == id,
            includeProperties: "TypeTrx,CustomerTrx", isTracking: false);
        if (objHeader == null)
        {
            return NotFound();
        }

        var objBankList = _uow.Bank
            .GetAll(x => (x.CompanyId == _companyId))
            .ToList();

        if (objBankList == null)
        {
            return NotFound();
        }

        model.BankList = objBankList;
        model.ModelCreateVM.DataModel = objHeader;
        model.CustomerFullName = $"{objHeader.CustomerTrx.CommercialName}";
        model.NumberTransa = $"COT-{objHeader.TypeTrx.Code}-{objHeader.Numeral}";
        return View(model);
    }

    #region API_CALL
    public JsonResult GetAll()
    {
        JsonResultResponse? jsonResponse = new();

        var objList = _uow.Quotation
            .GetAll(x => (x.CompanyId == _companyId)
            , includeProperties: "TypeTrx,CustomerTrx,CurrencyTransaTrx,CurrencyTransferTrx,CurrencyDepositTrx").ToList();
        if (objList.Count <= 0)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = "No hay registros que mostrar";
            return Json(jsonResponse);
        }

        jsonResponse.IsSuccess = true;
        jsonResponse.Data = objList;
        return Json(jsonResponse);
    }

    public JsonResult GetAllDepositByParent(int parentId)
    {
        JsonResultResponse? jsonResponse = new();

        var objList = _uow.QuotationDetail
            .GetAll(x => (x.CompanyId == _companyId &&
                          x.ParentId == parentId &&
                          x.QuotationDetailType == SD.QuotationDetailType.Deposit)
                , includeProperties: "ParentTrx,CurrencyDetailTrx,BankSourceTrx,BankTargetTrx").ToList();
        if (objList == null)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = "Error al cargar los datos";
            return Json(jsonResponse);
        }

        jsonResponse.IsSuccess = true;
        jsonResponse.Data = objList;
        return Json(jsonResponse);
    }

    public JsonResult GetAllTransferByParent(int parentId)
    {
        JsonResultResponse? jsonResponse = new();

        var objList = _uow.QuotationDetail
            .GetAll(x => (x.CompanyId == _companyId &&
                          x.ParentId == parentId &&
                          x.QuotationDetailType == SD.QuotationDetailType.Transfer)
                , includeProperties: "ParentTrx,CurrencyDetailTrx,BankSourceTrx,BankTargetTrx").ToList();
        if (objList == null)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = "Error al cargar los datos";
            return Json(jsonResponse);
        }

        jsonResponse.IsSuccess = true;
        jsonResponse.Data = objList;
        return Json(jsonResponse);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<JsonResult> DeletePost(int id)
    {
        JsonResultResponse? jsonResponse = new();
        StringBuilder errorsMessagesBuilder = new();
        if (id == 0)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = $"El id es requerido";
            return Json(jsonResponse);
        }
        try
        {
            var objHeader = _uow.Quotation
                .Get(filter: x => x.CompanyId == _companyId && x.Id == id, isTracking: false);
            if (objHeader == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Cotización no encontrada";
                return Json(jsonResponse);
            }


            if (!(await _uow.Quotation.RemoveWithChildren(objHeader.Id)))
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Cotización no encontrada";
                return Json(jsonResponse);
            }
            jsonResponse.IsSuccess = true;
            TempData["success"] = $"Cotización eliminada correctamente";
            jsonResponse.UrlRedirect = Url.Action(action: "Index", controller: "Quotation");

            return Json(jsonResponse);
        }
        catch (Exception ex)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = ex.Message.ToString();
            return Json(jsonResponse);
        }
    }

    [HttpPost, ActionName("Approved")]
    public async Task<JsonResult> ApprovedPost(int id)
    {
        JsonResultResponse? jsonResponse = new();
        StringBuilder errorsMessagesBuilder = new();
        if (id == 0)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = $"El id es requerido";
            return Json(jsonResponse);
        }

        try
        {
            var objHeader = _uow.Quotation
                .Get(filter: x => x.CompanyId == _companyId && x.Id == id);
            if (objHeader == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Cotización no encontrada";
                return Json(jsonResponse);
            }

            var nextSeq = await _uow.Quotation.NextSequentialNumber(filter: x => x.CompanyId == objHeader.CompanyId &&
                                                                           x.TypeNumeral == objHeader.TypeNumeral &&
                                                                           x.DateTransa == objHeader.DateTransa &&
                                                                           x.InternalSerial == AC.InternalSerialOfficial);
            if (nextSeq == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Consecutivo de cotización no encontrado";
                return Json(jsonResponse);
            }

            objHeader.IsClosed = true;
            objHeader.Numeral = nextSeq;
            //Seteamos campos de auditoria
            objHeader.UpdatedBy = AC.LOCALHOSTME;
            objHeader.UpdatedDate = DateTime.UtcNow;
            objHeader.UpdatedHostName = AC.LOCALHOSTPC;
            objHeader.UpdatedIpv4 = AC.Ipv4Default;
            objHeader.ClosedBy = AC.LOCALHOSTME;
            objHeader.ClosedDate = DateTime.UtcNow;
            objHeader.ClosedHostName = AC.LOCALHOSTPC;
            objHeader.ClosedIpv4 = AC.Ipv4Default;
            _uow.Quotation.Update(objHeader);
            _uow.Save();
            jsonResponse.IsSuccess = true;
            TempData["success"] = $"Cotización cerrada correctamente";
            jsonResponse.UrlRedirect = Url.Action(action: "Index", controller: "Quotation");

            return Json(jsonResponse);
        }
        catch (Exception ex)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = ex.Message.ToString();
            return Json(jsonResponse);
        }
    }

    [HttpPost, ActionName("DeleteDetail")]
    public JsonResult DeleteDetailPost(int id)
    {
        JsonResultResponse? jsonResponse = new();
        StringBuilder errorsMessagesBuilder = new();
        if (id == 0)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = $"El id es requerido";
            return Json(jsonResponse);
        }
        try
        {
            var objDetail = _uow.QuotationDetail
                .Get(filter: x => x.CompanyId == _companyId && x.Id == id, isTracking: false);
            if (objDetail == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Detalle de cotización no encontrado";
                return Json(jsonResponse);
            }

            int parentId = objDetail.ParentId;

            _uow.QuotationDetail.Remove(objDetail);
            _uow.Save();

            //Obtenemos el padre
            var objHeader = _uow.Quotation.Get(filter: x => x.CompanyId == _companyId && x.Id == parentId);

            if (objHeader == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Cotización invalida";
                return Json(jsonResponse);
            }

            //Obtenemos los hijos
            var objDetails = _uow.QuotationDetail.GetAll(filter: x => x.CompanyId == _companyId && x.ParentId == parentId).ToList();

            if (objDetails == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Detalle de cotización invalido";
                return Json(jsonResponse);
            }


            objHeader.TotalDeposit = objDetails
                .Where(x => x.QuotationDetailType == QuotationDetailType.Deposit)
                .Sum(x => x.AmountDetail);
            objHeader.TotalTransfer = objDetails
                .Where(x => x.QuotationDetailType == QuotationDetailType.Transfer)
                .Sum(x => x.AmountDetail);
            objHeader.UpdatedBy = AC.LOCALHOSTME;
            objHeader.UpdatedDate = DateTime.UtcNow;
            objHeader.UpdatedHostName = AC.LOCALHOSTPC;
            objHeader.UpdatedIpv4 = AC.Ipv4Default;
            _uow.Quotation.Update(objHeader);
            _uow.Save();


            jsonResponse.IsSuccess = true;
            jsonResponse.SuccessMessages = $"Detalle eliminado correctamente";
            return Json(jsonResponse);
        }
        catch (Exception ex)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = ex.Message.ToString();
            return Json(jsonResponse);
        }
    }

    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        var rowToBeDeleted = _uow.Quotation.Get(filter: u => (u.Id == id)
            , isTracking: false);

        if (rowToBeDeleted == null)
        {
            return Json(new { success = false, message = "Quotation don´t exist" });
        }

        _uow.Bank.RemoveByFilter(filter: u => (u.Id == rowToBeDeleted.Id));

        return Json(new
        {
            isSuccess = true
            ,
            successMessages = "Quotation Successfully Removed"
            ,
            errorMessages = string.Empty
        });
    }

    #endregion

    #region REPORT

    public IActionResult PrintReport()
    {
        // Titulo pestaña del reporte
        ViewData["Title"] = $"Rpt - Nota de Crédito";
        return View("~/Views/Shared/IndexReport.cshtml");
    }

    public IActionResult GetReport()
    {
        try
        {
            var report = new StiReport();
            // Veficar que hay datos del reporte guardados
            var reportDataJson = HttpContext.Session.GetString(AC.ObjectReportData);
            if (reportDataJson is null)
            {
                throw new Exception($"Error al cargar el informe");
            }
            var datRepJson = HttpContext.Session.GetString(AC.DatRep);
            // cargar reporte
            report.LoadFromJson(reportDataJson);

            // Cargar objetos ya deserialiozados
            if (datRepJson != null)
                report.RegBusinessObject(AC.DatRep, JsonSerializer.Deserialize<QuotationReportVM>(datRepJson));

            return StiNetCoreViewer.GetReportResult(this, report);
        }
        catch (Exception ex)
        {
            //TempData[AC.Error] = ex.Message;
            return Content($"Error al cargar el informe: {ex.Message}");
        }
    }

    public IActionResult ViewerEvent()
    {
        return StiNetCoreViewer.ViewerEventResult(this);
    }

    //Validar todos los datos de la cotización
    [HttpPost]
    public JsonResult ValidateDataToPrint(int id)
    {
        JsonResultResponse? jsonResponse = new();
        StringBuilder errorsMessagesBuilder = new();
        StiReport reportResult = new();
        ConfigFac? configFac = null;
        Company? company = null;
        Quotation? transaction = null;

        try
        {
            _parametersReport.Add(ParametersReport.FileName, "Quotation.mrt");
            _parametersReport.Add(ParametersReport.FilePath,
                $"{Path.Combine(_hostEnvironment.ContentRootPath, "Areas", "Exchange", "Reports", "Quotation.mrt")}");

            // Verificar que existe el archivo del reporte
            if (!System.IO.File.Exists(_parametersReport[ParametersReport.FilePath]?.ToString() ?? string.Empty))
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Reporte no encontrado";
                return Json(jsonResponse);
            }

            if (Path.GetExtension(_parametersReport[ParametersReport.FileName]?.ToString() ?? string.Empty).ToUpper() != ".MRT")
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Reporte invalido";
                return Json(jsonResponse);
            }

            // Obtener configurcion fac
            configFac = _uow.ConfigFac.Get(filter: x => x.CompanyId == _companyId);
            if (configFac is null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Configuración de facturación no encontrada";
                return Json(jsonResponse);
            }

            // Obtener compañia
            company = _uow.Company.Get(filter: x => x.Id == _companyId);
            if (company is null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Compañia no encontrada";
                return Json(jsonResponse);
            }

            // Obtener la cotización
            transaction = _uow.Quotation.Get(filter: x => x.CompanyId == _companyId && x.Id == id, includeProperties: "TypeTrx,CustomerTrx,CurrencyDepositTrx,CurrencyTransferTrx,CurrencyTransaTrx");
            if (transaction is null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Cotización no encontrada";
                return Json(jsonResponse);
            }

            //Obtener los hijos
            var transaDetails = _uow.QuotationDetail.GetAll(filter: x => x.CompanyId == _companyId && x.ParentId == id && x.QuotationDetailType == QuotationDetailType.Transfer, includeProperties: "ParentTrx,CurrencyDetailTrx,BankSourceTrx,BankTargetTrx").ToList();
            if (transaDetails is null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Cotización sin hijos encontrados";
                return Json(jsonResponse);
            }

            var detailDistinct = transaDetails.Select(x => x.BankTargetTrx).Distinct().ToList();
            string bankTargets = "", numberReferenTarget = "";
            foreach (var detail in detailDistinct)
            {
                bankTargets += detail.Code + ", ";
                numberReferenTarget += $"{transaction.Numeral}-{detail.Code}{transaction.DateTransa.Year}{transaction.DateTransa.Month.ToString().PadLeft(2, AC.CharDefaultEmpty)}{transaction.DateTransa.Day.ToString().PadLeft(2, AC.CharDefaultEmpty)}" + ", ";
            }

            // Remover la coma adicional al final, si es necesario
            if (!string.IsNullOrEmpty(bankTargets) && !string.IsNullOrEmpty(numberReferenTarget))
            {
                bankTargets = bankTargets.Trim().TrimEnd(',');
                numberReferenTarget = numberReferenTarget.Trim().TrimEnd(',');
            }

            //Tipo de Cambio
            decimal tcExchange = transaction.TypeNumeral == SD.QuotationType.Buy ? transaction.ExchangeRateBuyTransa
                : transaction.ExchangeRateSellTransa;

            // Crear objeto para pasar datos de cabecera al reporte
            var dataHead = new QuotationReportVM()
            {
                CustomerFullName = transaction.CustomerTrx.CommercialName,
                BankTargetFullName = bankTargets,
                IsClosed = transaction.IsClosed,
                CurrencyTransferCode = transaction.CurrencyTransferTrx.Code,
                AmountTransaction = transaction.AmountTransaction,
                ConceptGeneral = $"{Enum.GetName(typeof(SD.QuotationTypeName), (int)transaction.TypeNumeral)} de {transaction.CurrencyTransaTrx.NameSingular} TC:{tcExchange}",
                NumberReferen = numberReferenTarget,
                DescriptionGeneral = $"Por este medio se confirma el envío por transferencia bancaria, producto de la operación de cambio afectuada el dia de hoy {transaction.DateTransa.Day} de {Enum.GetName(typeof(SD.MonthName), transaction.DateTransa.Month)} del año {transaction.DateTransa.Year}"
            };

            // Cargar reporte
            reportResult.Load(StiNetCoreHelper.MapPath(this, _parametersReport[ParametersReport.FilePath]?.ToString() ?? string.Empty));

            // Decimales
            reportResult.Dictionary.Variables[AC.ParDecimalTransaction].ValueObject = _decimalTransa;
            reportResult.Dictionary.Variables[AC.ParDecimalExchangeRate].ValueObject = _decimalExchange;
            reportResult.Dictionary.Variables[AC.ParNameCompany].ValueObject = $"{company.Name}";
            reportResult.Dictionary.Variables[AC.ParNameReport].ValueObject = "Nota de Crédito";
            reportResult.Dictionary.Variables[AC.ParFileImagePath].ValueObject = $"{company.ImageLogoUrl}";
            string isClosed = transaction.IsClosed ? "Cerrado" : "No Cerrado";
            reportResult.Dictionary.Variables["parIsClosed"].ValueObject = isClosed;
            reportResult.ReportName = "Nota de Crédito";

            // Guardar los datos en el contexto
            // Reporte 
            HttpContext.Session.SetString(AC.ObjectReportData, reportResult.SaveToJsonString());
            // Objeros de negocio
            HttpContext.Session.SetString(AC.DatRep, JsonSerializer.Serialize(dataHead));

            jsonResponse.IsSuccess = true;
            jsonResponse.Data = new
            {
                urlRedirectTo = Url.Action("PrintReport", "Quotation", new { area = "Exchange" })
            };
            return Json(jsonResponse);
        }
        catch (Exception ex)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = ex.Message;
            return Json(jsonResponse);
        }
    }

    #endregion


}
