using Microsoft.AspNetCore.Mvc;
using Xanes.DataAccess.Repository.IRepository;
using System.Text.Json;
using Xanes.Models;
using Xanes.Models.Shared;
using Xanes.Models.ViewModels;
using Xanes.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using ClosedXML.Excel;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using static Xanes.Utility.SD;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;

namespace Xanes.Web.Areas.Exchange.Controllers;

[Area("Exchange")]
[Authorize()]

public class QuotationController : Controller
{
    private readonly IUnitOfWork _uow;
    private readonly IConfiguration _configuration;
    private readonly int _companyId;
    private readonly int _decimalTransa;
    private readonly int _decimalExchange;
    private readonly int _decimalExchangeFull;
    private readonly decimal _variationMaxDeposit;


    private Dictionary<ParametersReport, object?> _parametersReport;
    private readonly IWebHostEnvironment _hostEnvironment;

    public QuotationController(IUnitOfWork uow, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        _uow = uow;
        _configuration = configuration;
        _companyId = _configuration.GetValue<int>("ApplicationSettings:CompanyId");
        _decimalTransa = _configuration.GetValue<int>("ApplicationSettings:DecimalTransa");
        _decimalExchange = _configuration.GetValue<int>("ApplicationSettings:DecimalExchange");
        _decimalExchangeFull = _configuration.GetValue<int>("ApplicationSettings:DecimalExchangeFull");
        _variationMaxDeposit = _configuration.GetValue<decimal>("ApplicationSettings:VariationMaxDeposit");
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
        string processingDateString = HttpContext.Session.GetString(AC.ProcessingDate) ?? DateOnly.FromDateTime(DateTime.Now).ToString();
        DateOnly dateFilter = DateOnly.Parse(processingDateString);
        ViewBag.DecimalTransa = JsonSerializer.Serialize(_decimalTransa);
        ViewBag.DecimalExchange = JsonSerializer.Serialize(_decimalExchange);
        ViewBag.ProcessingDate = JsonSerializer.Serialize(dateFilter.ToString(AC.DefaultDateFormatWeb));
        ViewBag.IsNewEntry = JsonSerializer.Serialize(true);
        TransactionReportVM modelVM = new();
        ViewData[AC.Title] = "Listado de Transacciones";
        return View(modelVM);
    }

    public IActionResult Upsert(int id = 0)
    {
        ViewBag.DecimalTransa = JsonSerializer.Serialize(_decimalTransa);
        ViewBag.DecimalExchange = JsonSerializer.Serialize(_decimalExchange);
        string processingDateString = HttpContext.Session.GetString(AC.ProcessingDate);
        QuotationCreateVM model = new();
        Quotation objData = new();
        List<Models.Customer>? listCustomer = new();

        if (processingDateString != null)
        {
            DateOnly processingDate = DateOnly.Parse(processingDateString);
            model.ProcessingDate = processingDate;
        }
        else
        {
            model.ExistProcessingDate = false;
        }


        var objCurrencyList = _uow.Currency
            .GetAll(x => (x.CompanyId == _companyId))
            .ToList();

        if (objCurrencyList == null)
        {
            TempData[AC.Error] = $"Moneda no encontrada";
            return RedirectToAction(nameof(Index));
        }

        var objTypeList = _uow.QuotationType
            .GetAll(x => (x.CompanyId == _companyId))
            .ToList();

        if (objTypeList == null)
        {
            TempData[AC.Error] = $"Tipo de cotización no encontrado";
            return RedirectToAction(nameof(Index));
        }

        var objBankAccountList = _uow.BankAccount
            .GetAll(x => (x.CompanyId == _companyId))
            .ToList();

        if (objBankAccountList == null)
        {
            TempData[AC.Error] = $"Cuenta bancaria no encontrada";
            return RedirectToAction(nameof(Index));
        }

        var objBusinessExecutiveList = _uow.BusinessExecutive
            .GetAll(x => (x.CompanyId == _companyId))
            .ToList();

        if (objBusinessExecutiveList == null || objBusinessExecutiveList.Count == 0)
        {
            TempData[AC.Error] = $"Ejecutivo no encontrado";
            return RedirectToAction(nameof(Index));
        }

        if (id == 0)
        {
            ViewData[AC.Title] = "Crear - Cotización";

            objData = new Quotation
            {
                DateTransa = DateOnly.FromDateTime(DateTime.Now),
                TypeNumeral = SD.QuotationType.Buy,
                CurrencyTransaType = SD.CurrencyType.Foreign,
                CurrencyTransferType = SD.CurrencyType.Base,
                CurrencyDepositType = SD.CurrencyType.Base,
                CompanyId = _companyId,
                BusinessExecutiveCode = new string(AC.CharDefaultEmpty, AC.RepeatCharTimes),
                CustomerId = 0
            };

        }
        else
        {
            ViewData[AC.Title] = "Actualizar - Cotización";

            objData = _uow.Quotation.Get(filter: x => (x.CompanyId == _companyId && x.Id == id));

            if (objData == null)
            {
                TempData[AC.Error] = $"Cotización no encontrada";
                return RedirectToAction(nameof(Index));
            }
        }

        model.CurrencyTransaList = objCurrencyList
            .Where(x => (x.IsActive && (x.Numeral != (int)SD.CurrencyType.Base)))
            .ToList();
        model.CurrencyDepositList = objCurrencyList.Where(x => x.IsActive).ToList();
        model.CurrencyTransferList = objCurrencyList.Where(x => x.IsActive).ToList();
        model.QuotationTypeList = objTypeList;
        model.BusinessExecutiveList = objBusinessExecutiveList;
        model.BankAccountSourceList = objBankAccountList.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
        model.BankAccountTargetList = new List<SelectListItem>();
        model.DataModel = objData;

        if (model.DataModel.CustomerId != 0)
        {
            var objCustomer =
                 _uow.Customer.Get(filter: x => (x.CompanyId == _companyId && x.Id == model.DataModel.CustomerId));
            listCustomer.Add(objCustomer);
            model.CustomerList = listCustomer.Select(x => new SelectListItem { Text = x.CommercialName, Value = x.Id.ToString() });
        }

        return View(model);
    }

    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public IActionResult Create(Models.ViewModels.QuotationCreateVM objViewModel)
    //{
    //    var objBankAccountTarget = new BankAccount();
    //    var objBankAccountSource = new BankAccount();
    //    var objCurrency = new Currency();
    //    var objCurrencyList = new List<Currency>();
    //    var objTypeList = new List<QuotationType>();
    //    var objCustomerList = new List<Models.Customer>();
    //    objViewModel.DataModel.BusinessExecutiveCode = new string(AC.CharDefaultEmpty, AC.RepeatCharTimes);

    //    Models.Quotation obj = objViewModel.DataModel;
    //    //Datos son validos
    //    if (ModelState.IsValid)
    //    {
    //        if (obj.CompanyId != _companyId)
    //        {
    //            ModelState.AddModelError("CompanyId", $"Id de la compañía no puede ser distinto de {_companyId}");
    //        }

    //        if (obj.AmountTransaction == 0)
    //        {
    //            ModelState.AddModelError("AmountTransaction", "El monto no puede ser cero.");
    //        }

    //        //Verificamos si existe el tipo
    //        var objQuotationType = _uow.QuotationType.Get(filter: x =>
    //            x.CompanyId == obj.CompanyId && x.Numeral == (int)obj.TypeNumeral);

    //        if (objQuotationType == null)
    //        {
    //            ModelState.AddModelError("TypeNumeral", $"Tipo de transacción no encontrado");
    //        }
    //        else
    //        {
    //            obj.TypeId = objQuotationType.Id;

    //            if (objQuotationType.Numeral != (int)SD.QuotationType.Transfer)
    //            {
    //                if (objQuotationType.Numeral == (int)SD.QuotationType.Buy)
    //                {
    //                    obj.CurrencyDepositType = obj.CurrencyTransferType;

    //                    if (obj.ExchangeRateBuyTransa == 0)
    //                    {
    //                        ModelState.AddModelError("ExchangeRateBuyTransa", "El tipo de cambio de compra no puede ser cero.");
    //                    }
    //                }
    //                else
    //                {
    //                    obj.CurrencyTransferType = obj.CurrencyDepositType;

    //                    if (obj.ExchangeRateSellTransa == 0)
    //                    {
    //                        ModelState.AddModelError("ExchangeRateSellTransa", "El tipo de cambio de venta no puede ser cero.");
    //                    }
    //                }

    //                //Verificamos si existe la moneda de la Transaccion
    //                objCurrency = _uow.Currency.Get(filter: x =>
    //                   x.CompanyId == obj.CompanyId && x.Numeral == (int)obj.CurrencyTransaType);

    //                if (objCurrency == null)
    //                {
    //                    ModelState.AddModelError("CurrencyTransaType", $"Moneda de la transacción no encontrada");
    //                }
    //                else
    //                {
    //                    obj.CurrencyTransaId = objCurrency.Id;
    //                }

    //                //Verificamos si existe la moneda de deposito
    //                objCurrency = _uow.Currency.Get(filter: x =>
    //                    x.CompanyId == obj.CompanyId && x.Numeral == (int)obj.CurrencyDepositType);

    //                if (objCurrency == null)
    //                {
    //                    ModelState.AddModelError("CurrencyDepositType", $"Moneda de deposito no encontrada");
    //                }
    //                else
    //                {
    //                    obj.CurrencyDepositId = objCurrency.Id;
    //                }

    //                //Verificamos si existe la moneda de transferencia
    //                objCurrency = _uow.Currency.Get(filter: x =>
    //                    x.CompanyId == obj.CompanyId && x.Numeral == (int)obj.CurrencyTransferType);

    //                if (objCurrency == null)
    //                {
    //                    ModelState.AddModelError("CurrencyTransferType", $"Moneda de transferencia no encontrada");
    //                }
    //                else
    //                {
    //                    obj.CurrencyTransferId = objCurrency.Id;
    //                }
    //            }
    //            else
    //            {
    //                //Verificamos si existe la cuenta bancaria de origen
    //                objBankAccountSource = _uow.BankAccount.Get(filter: x => x.CompanyId == obj.CompanyId && x.Id == obj.BankAccountSourceId, includeProperties: "ParentTrx");
    //                if (objBankAccountSource == null)
    //                {
    //                    ModelState.AddModelError("BankAccountSourceId", $"Cuenta bancaria origen invalida");
    //                }

    //                //Verificamos si existe la cuenta bancaria de destino
    //                objBankAccountTarget = _uow.BankAccount.Get(filter: x => x.CompanyId == obj.CompanyId && x.Id == obj.BankAccountTargetId, includeProperties: "ParentTrx");
    //                if (objBankAccountTarget == null)
    //                {
    //                    ModelState.AddModelError("BankAccountTargetId", $"Cuenta bancaria destino invalida");
    //                }
    //            }
    //        }

    //        //Verificamos si existe la moneda de la Transaccion
    //        var objBusiness = _uow.BusinessExecutive.Get(filter: x =>
    //            x.CompanyId == obj.CompanyId && x.Id == obj.BusinessExecutiveId);

    //        if (objBusiness == null)
    //        {
    //            ModelState.AddModelError("BusinessExecutiveId", $"Ejecutivo no encontrado");
    //        }
    //        else
    //        {
    //            obj.BusinessExecutiveCode = objBusiness.Code;
    //        }

    //        //Verificamos si existe el cliente
    //        var objCustomer = _uow.Customer.Get(filter: x => x.CompanyId == obj.CompanyId && x.Id == obj.CustomerId);
    //        if (objCustomer == null)
    //        {
    //            ModelState.AddModelError("CustomerId", $"Cliente no encontrado");
    //        }

    //        if (!ModelState.IsValid)
    //        {
    //            ViewBag.DecimalTransa = JsonSerializer.Serialize(_decimalTransa);
    //            ViewBag.DecimalExchange = JsonSerializer.Serialize(_decimalExchange);

    //            objCurrencyList = _uow.Currency
    //               .GetAll(x => (x.CompanyId == _companyId))
    //               .ToList();

    //            if (objCurrencyList == null)
    //            {
    //                return NotFound();
    //            }

    //            objTypeList = _uow.QuotationType
    //               .GetAll(x => (x.CompanyId == _companyId))
    //               .ToList();

    //            if (objTypeList == null)
    //            {
    //                return NotFound();
    //            }

    //            objCustomerList = _uow.Customer
    //               .GetAll(x => (x.CompanyId == _companyId))
    //               .ToList();

    //            if (objCustomerList == null)
    //            {
    //                return NotFound();
    //            }

    //            var objBankAccountList = _uow.BankAccount
    //                .GetAll(x => (x.CompanyId == _companyId))
    //                .ToList();

    //            if (objBankAccountList == null)
    //            {
    //                return NotFound();
    //            }

    //            objViewModel.BankAccountSourceList = objBankAccountList.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
    //            objViewModel.BankAccountTargetList = new List<SelectListItem>();
    //            objViewModel.CurrencyTransaList = objCurrencyList
    //                .Where(x => (x.IsActive && (x.Numeral != (int)SD.CurrencyType.Base)))
    //                .ToList();
    //            objViewModel.CurrencyDepositList = objCurrencyList.Where(x => x.IsActive).ToList();
    //            objViewModel.CurrencyTransferList = objCurrencyList.Where(x => x.IsActive).ToList();
    //            objViewModel.QuotationTypeList = objTypeList;
    //            objViewModel.CustomerList = objCustomerList.Select(x => new SelectListItem { Text = x.CommercialName, Value = x.Id.ToString() });
    //            return View(objViewModel);
    //        }

    //        //Obtenemos el secuencial en borrador
    //        var numberTransa = _uow.ConfigFac.NextSequentialNumber(filter: x => x.CompanyId == obj.CompanyId,
    //            SD.TypeSequential.Draft, true);

    //        obj.Numeral = Convert.ToInt32(numberTransa.Result.ToString());
    //        obj.InternalSerial = AC.InternalSerialDraft;
    //        //COMPRA
    //        if (obj.TypeNumeral == SD.QuotationType.Buy)
    //        {
    //            //TC COMPRA MENOR AL TC OFICIAL
    //            if (obj.ExchangeRateBuyTransa < obj.ExchangeRateOfficialTransa)
    //            {
    //                obj.AmountRevenue = (obj.ExchangeRateOfficialTransa - obj.ExchangeRateBuyTransa) * obj.AmountTransaction;
    //                obj.AmountCost = 0;
    //            }
    //            //TC COMPRA MAYOR AL TC OFICIAL
    //            else
    //            {
    //                obj.AmountCost = (obj.ExchangeRateBuyTransa - obj.ExchangeRateOfficialTransa) * obj.AmountTransaction;
    //                obj.AmountRevenue = 0;
    //            }

    //            //Compra de dolares 
    //            if (obj.CurrencyTransaType == SD.CurrencyType.Foreign)
    //            {
    //                //Factoring paga en Cordobas
    //                if (obj.CurrencyTransferType == SD.CurrencyType.Base)
    //                {
    //                    obj.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateBuyTransa);
    //                    obj.ExchangeRateBuyReal = obj.ExchangeRateBuyTransa;
    //                }
    //            }
    //            //Compra de Euros
    //            else if (obj.CurrencyTransaType == SD.CurrencyType.Additional)
    //            {
    //                //Factoring paga en Cordobas
    //                if (obj.CurrencyTransferType == SD.CurrencyType.Base)
    //                {
    //                    obj.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateBuyTransa);
    //                    obj.ExchangeRateBuyReal = obj.ExchangeRateBuyTransa;

    //                }
    //                //Factoring paga en Dolares
    //                else if (obj.CurrencyTransferType == SD.CurrencyType.Foreign)
    //                {
    //                    obj.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateBuyTransa);
    //                    obj.ExchangeRateBuyReal = (obj.ExchangeRateBuyTransa * obj.ExchangeRateOfficialTransa);
    //                }
    //            }
    //        }
    //        //VENTA
    //        else if (obj.TypeNumeral == SD.QuotationType.Sell)
    //        {
    //            //TC VENTA MENOR AL TC OFICIAL
    //            if (obj.ExchangeRateSellTransa < obj.ExchangeRateOfficialTransa)
    //            {
    //                obj.AmountCost = (obj.ExchangeRateOfficialTransa - obj.ExchangeRateSellTransa) * obj.AmountTransaction;
    //                obj.AmountRevenue = 0;
    //            }
    //            //TC VENTA MAYOR AL TC OFICIAL
    //            else
    //            {
    //                obj.AmountRevenue = (obj.ExchangeRateSellTransa - obj.ExchangeRateOfficialTransa) * obj.AmountTransaction;
    //                obj.AmountCost = 0;
    //            }

    //            //Venta de dolares 
    //            if (obj.CurrencyTransaType == SD.CurrencyType.Foreign)
    //            {
    //                //Cliente paga en Cordobas
    //                if (obj.CurrencyDepositType == SD.CurrencyType.Base)
    //                {
    //                    obj.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateSellTransa);
    //                    obj.ExchangeRateSellReal = obj.ExchangeRateSellTransa;
    //                }
    //            }
    //            //Venta de Euros
    //            else if (obj.CurrencyTransaType == SD.CurrencyType.Additional)
    //            {
    //                //Cliente paga en Cordobas
    //                if (obj.CurrencyDepositType == SD.CurrencyType.Base)
    //                {
    //                    obj.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateSellTransa);
    //                    obj.ExchangeRateSellReal = obj.ExchangeRateSellTransa;

    //                }
    //                //Cliente paga en Dolares
    //                else if (obj.CurrencyDepositType == SD.CurrencyType.Foreign)
    //                {
    //                    obj.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateSellTransa);
    //                    obj.ExchangeRateSellReal = (obj.ExchangeRateSellTransa * obj.ExchangeRateOfficialTransa);
    //                }
    //            }
    //        }
    //        else
    //        {
    //            if (objBankAccountSource != null)
    //            {
    //                obj.CurrencyTransaType = objBankAccountSource.CurrencyType;
    //                obj.CurrencyDepositType = objBankAccountSource.CurrencyType;
    //                obj.CurrencyTransferType = objBankAccountSource.CurrencyType;
    //                obj.CurrencyTransaId = objBankAccountSource.CurrencyId;
    //                obj.CurrencyDepositId = objBankAccountSource.CurrencyId;
    //                obj.CurrencyTransferId = objBankAccountSource.CurrencyId;
    //            }
    //        }


    //        //Seteamos campos de auditoria
    //        obj.CreatedBy = AC.LOCALHOSTME;
    //        obj.CreatedDate = DateTime.UtcNow;
    //        obj.CreatedHostName = AC.LOCALHOSTPC;
    //        obj.CreatedIpv4 = AC.Ipv4Default;
    //        obj.IsPosted = false;
    //        obj.IsClosed = false;
    //        obj.IsLoan = false;
    //        obj.IsPayment = false;
    //        _uow.Quotation.Add(obj);
    //        _uow.Save();
    //        TempData["success"] = "Cotización creada exitosamente";

    //        if (obj.TypeNumeral == SD.QuotationType.Transfer)
    //        {
    //            if (obj.BankAccountTargetTrx != null)
    //            {
    //                var objDetailBankAccountSource = new QuotationDetail()
    //                {
    //                    ParentId = obj.Id,
    //                    CompanyId = obj.CompanyId,
    //                    QuotationDetailType = QuotationDetailType.CreditTransfer,
    //                    LineNumber = 1,
    //                    CurrencyDetailId = obj.CurrencyTransaId,
    //                    BankSourceId = obj.BankAccountTargetTrx.ParentId,
    //                    BankTargetId = obj.BankAccountTargetTrx.ParentId,
    //                    AmountDetail = obj.AmountTransaction,
    //                    CreatedBy = AC.LOCALHOSTME,
    //                    CreatedDate = DateTime.UtcNow,
    //                    CreatedHostName = AC.LOCALHOSTPC,
    //                    CreatedIpv4 = AC.Ipv4Default
    //                };

    //                _uow.QuotationDetail.Add(objDetailBankAccountSource);
    //            }

    //            if (obj.BankAccountSourceTrx != null)
    //            {
    //                var objDetailBankAccountTarget = new QuotationDetail()
    //                {
    //                    ParentId = obj.Id,
    //                    CompanyId = obj.CompanyId,
    //                    QuotationDetailType = QuotationDetailType.DebitTransfer,
    //                    LineNumber = 1,
    //                    CurrencyDetailId = obj.CurrencyTransaId,
    //                    BankTargetId = obj.BankAccountSourceTrx.ParentId,
    //                    BankSourceId = obj.BankAccountSourceTrx.ParentId,
    //                    AmountDetail = obj.AmountTransaction,
    //                    CreatedBy = AC.LOCALHOSTME,
    //                    CreatedDate = DateTime.UtcNow,
    //                    CreatedHostName = AC.LOCALHOSTPC,
    //                    CreatedIpv4 = AC.Ipv4Default
    //                };
    //                _uow.QuotationDetail.Add(objDetailBankAccountTarget);
    //            }
    //            _uow.Save();
    //        }

    //        return RedirectToAction("CreateDetail", "Quotation", new { id = obj.Id });
    //    }
    //    else
    //    {
    //        StringBuilder errorsMessagesBuilder = new();

    //        List<string> listErrorMessages = ModelState.Values
    //            .SelectMany(v => v.Errors)
    //            .Select(x => x.ErrorMessage)
    //            .ToList();
    //        foreach (var item in listErrorMessages)
    //        {
    //            errorsMessagesBuilder.Append(item);
    //        }

    //        ModelState.AddModelError("", errorsMessagesBuilder.ToString());
    //    }

    //    objCurrencyList = _uow.Currency
    //       .GetAll(x => (x.CompanyId == _companyId))
    //       .ToList();

    //    if (objCurrencyList == null)
    //    {
    //        return NotFound();
    //    }

    //    objTypeList = _uow.QuotationType
    //       .GetAll(x => (x.CompanyId == _companyId))
    //       .ToList();

    //    if (objTypeList == null)
    //    {
    //        return NotFound();
    //    }

    //    objCustomerList = _uow.Customer
    //        .GetAll(x => (x.CompanyId == _companyId))
    //        .ToList();

    //    if (objCustomerList == null)
    //    {
    //        return NotFound();
    //    }

    //    objViewModel.CurrencyTransaList = objCurrencyList
    //        .Where(x => (x.IsActive && (x.Numeral != (int)SD.CurrencyType.Base)))
    //        .ToList();
    //    objViewModel.CurrencyDepositList = objCurrencyList.Where(x => x.IsActive).ToList();
    //    objViewModel.CurrencyTransferList = objCurrencyList.Where(x => x.IsActive).ToList();
    //    objViewModel.QuotationTypeList = objTypeList;
    //    objViewModel.CustomerList = objCustomerList.Select(x => new SelectListItem { Text = x.CommercialName, Value = x.Id.ToString() });

    //    return View(objViewModel);
    //}

    [HttpPost]
    public JsonResult Upsert([FromForm] Quotation obj, bool redirectHome = false, bool redirectDetail = false, bool showMessages = false)
    {
        StringBuilder errorsMessagesBuilder = new();
        JsonResultResponse? jsonResponse = new();
        var objBankAccountTarget = new BankAccount();
        var objBankAccountSource = new BankAccount();
        var objCurrency = new Currency();
        var objBusinessExecutive = new BusinessExecutive();

        if (ModelState.IsValid)
        {
            if (obj.CompanyId != _companyId)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Id de la compañía no puede ser distinto de {_companyId}";
                return Json(jsonResponse);
            }

            if (obj.AmountTransaction == 0)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"El monto no puede ser cero.";
                return Json(jsonResponse);
            }

            //Verificamos si existe el tipo
            var objQuotationType = _uow.QuotationType.Get(filter: x =>
                x.CompanyId == obj.CompanyId && x.Numeral == (int)obj.TypeNumeral);

            if (objQuotationType == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Tipo de transacción no encontrado";
                return Json(jsonResponse);
            }

            obj.TypeId = objQuotationType.Id;

            if (objQuotationType.Numeral != (int)SD.QuotationType.Transfer)
            {
                //Verificamos si existe la moneda de la Transaccion
                objCurrency = _uow.Currency.Get(filter: x =>
                    x.CompanyId == obj.CompanyId && x.Numeral == (int)obj.CurrencyTransaType);

                if (objCurrency == null)
                {
                    jsonResponse.IsSuccess = false;
                    jsonResponse.ErrorMessages = $"Moneda de la transacción no encontrada";
                    return Json(jsonResponse);
                }

                obj.CurrencyTransaId = objCurrency.Id;

                if (objQuotationType.Numeral == (int)SD.QuotationType.Buy)
                {
                    obj.CurrencyDepositType = obj.CurrencyTransaType;
                    obj.CurrencyDepositId = objCurrency.Id;

                    if (obj.ExchangeRateBuyTransa == 0)
                    {
                        jsonResponse.IsSuccess = false;
                        jsonResponse.ErrorMessages = $"El tipo de cambio de compra no puede ser cero.";
                        return Json(jsonResponse);
                    }

                    //Verificamos si existe la moneda de transferencia
                    objCurrency = _uow.Currency.Get(filter: x =>
                        x.CompanyId == obj.CompanyId && x.Numeral == (int)obj.CurrencyTransferType);

                    if (objCurrency == null)
                    {
                        jsonResponse.IsSuccess = false;
                        jsonResponse.ErrorMessages = $"Moneda de transferencia no encontrada";
                        return Json(jsonResponse);
                    }

                    obj.CurrencyTransferId = objCurrency.Id;

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
                            obj.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateBuyTransa);
                            obj.ExchangeRateBuyReal = (obj.ExchangeRateBuyTransa * obj.ExchangeRateOfficialTransa);
                        }
                    }
                }
                else
                {
                    obj.CurrencyTransferType = obj.CurrencyTransaType;
                    obj.CurrencyTransferId = objCurrency.Id;

                    if (obj.ExchangeRateSellTransa == 0)
                    {
                        jsonResponse.IsSuccess = false;
                        jsonResponse.ErrorMessages = $"El tipo de cambio de venta no puede ser cero.";
                        return Json(jsonResponse);
                    }

                    //Verificamos si existe la moneda de deposito
                    objCurrency = _uow.Currency.Get(filter: x =>
                        x.CompanyId == obj.CompanyId && x.Numeral == (int)obj.CurrencyDepositType);

                    if (objCurrency == null)
                    {
                        jsonResponse.IsSuccess = false;
                        jsonResponse.ErrorMessages = $"Moneda de deposito no encontrada";
                        return Json(jsonResponse);
                    }

                    obj.CurrencyDepositId = objCurrency.Id;

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
                            obj.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateSellTransa);
                            obj.ExchangeRateSellReal = obj.ExchangeRateSellTransa;

                        }
                        //Cliente paga en Dolares
                        else if (obj.CurrencyDepositType == SD.CurrencyType.Foreign)
                        {
                            obj.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateSellTransa);
                            obj.ExchangeRateSellReal = (obj.ExchangeRateSellTransa * obj.ExchangeRateOfficialTransa);
                        }
                    }
                }
            }
            else
            {
                //Verificamos si existe la cuenta bancaria de origen
                objBankAccountSource = _uow.BankAccount.Get(filter: x => x.CompanyId == obj.CompanyId && x.Id == obj.BankAccountSourceId, includeProperties: "ParentTrx");
                if (objBankAccountSource == null)
                {
                    jsonResponse.IsSuccess = false;
                    jsonResponse.ErrorMessages = $"Cuenta bancaria origen no encontrada";
                    return Json(jsonResponse);
                }

                //Verificamos si existe la cuenta bancaria de destino
                objBankAccountTarget = _uow.BankAccount.Get(filter: x => x.CompanyId == obj.CompanyId && x.Id == obj.BankAccountTargetId, includeProperties: "ParentTrx");
                if (objBankAccountTarget == null)
                {
                    jsonResponse.IsSuccess = false;
                    jsonResponse.ErrorMessages = $"Cuenta bancaria destino no encontrada";
                    return Json(jsonResponse);
                }

                obj.CurrencyTransaType = objBankAccountSource.CurrencyType;
                obj.CurrencyDepositType = objBankAccountSource.CurrencyType;
                obj.CurrencyTransferType = objBankAccountSource.CurrencyType;
                obj.CurrencyTransaId = objBankAccountSource.CurrencyId;
                obj.CurrencyDepositId = objBankAccountSource.CurrencyId;
                obj.CurrencyTransferId = objBankAccountSource.CurrencyId;
            }

            //Verificamos si existe el ejecutivo
            objBusinessExecutive = _uow.BusinessExecutive.Get(filter: x =>
                x.CompanyId == obj.CompanyId && x.Id == obj.BusinessExecutiveId);

            if (objBusinessExecutive == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Ejecutivo no encontrado";
                return Json(jsonResponse);
            }

            obj.BusinessExecutiveCode = objBusinessExecutive.Code;
            obj.IsLoan = objBusinessExecutive.IsLoan;
            obj.IsPayment = objBusinessExecutive.IsPayment;

            //Verificamos si existe el cliente
            var objCustomer = _uow.Customer.Get(filter: x => x.CompanyId == obj.CompanyId && x.Id == obj.CustomerId);
            if (objCustomer == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Cliente no encontrado";
                return Json(jsonResponse);
            }

            //Crear
            if (obj.Id == 0)
            {
                //Obtenemos el secuencial en borrador
                var numberTransa = _uow.ConfigFac.NextSequentialNumber(filter: x => x.CompanyId == obj.CompanyId,
                    SD.TypeSequential.Draft, true);
                obj.Numeral = Convert.ToInt32(numberTransa.Result.ToString());
                obj.InternalSerial = AC.InternalSerialDraft;

                //Seteamos campos de auditoria
                obj.CreatedBy = AC.LOCALHOSTME;
                obj.CreatedDate = DateTime.UtcNow;
                obj.CreatedHostName = AC.LOCALHOSTPC;
                obj.CreatedIpv4 = AC.Ipv4Default;
                obj.IsPosted = false;
                obj.IsClosed = false;
                _uow.Quotation.Add(obj);
                _uow.Save();
                if (showMessages)
                    TempData["success"] = "Cotización creada exitosamente";

                if (obj.TypeNumeral == SD.QuotationType.Transfer)
                {
                    if (obj.BankAccountTargetTrx != null)
                    {
                        var objDetailBankAccountSource = new QuotationDetail()
                        {
                            ParentId = obj.Id,
                            CompanyId = obj.CompanyId,
                            QuotationDetailType = QuotationDetailType.CreditTransfer,
                            LineNumber = 1,
                            CurrencyDetailId = obj.CurrencyTransaId,
                            BankSourceId = obj.BankAccountTargetTrx.ParentId,
                            BankTargetId = obj.BankAccountTargetTrx.ParentId,
                            AmountDetail = obj.AmountTransaction,
                            CreatedBy = AC.LOCALHOSTME,
                            CreatedDate = DateTime.UtcNow,
                            CreatedHostName = AC.LOCALHOSTPC,
                            CreatedIpv4 = AC.Ipv4Default
                        };

                        _uow.QuotationDetail.Add(objDetailBankAccountSource);
                    }

                    if (obj.BankAccountSourceTrx != null)
                    {
                        var objDetailBankAccountTarget = new QuotationDetail()
                        {
                            ParentId = obj.Id,
                            CompanyId = obj.CompanyId,
                            QuotationDetailType = QuotationDetailType.DebitTransfer,
                            LineNumber = 1,
                            CurrencyDetailId = obj.CurrencyTransaId,
                            BankTargetId = obj.BankAccountSourceTrx.ParentId,
                            BankSourceId = obj.BankAccountSourceTrx.ParentId,
                            AmountDetail = obj.AmountTransaction,
                            CreatedBy = AC.LOCALHOSTME,
                            CreatedDate = DateTime.UtcNow,
                            CreatedHostName = AC.LOCALHOSTPC,
                            CreatedIpv4 = AC.Ipv4Default
                        };
                        _uow.QuotationDetail.Add(objDetailBankAccountTarget);
                    }
                    _uow.Save();
                }
            }
            //Actualizar
            else
            {
                //Seteamos campos de auditoria
                obj.UpdatedBy = AC.LOCALHOSTME;
                obj.UpdatedDate = DateTime.UtcNow;
                obj.UpdatedHostName = AC.LOCALHOSTPC;
                obj.UpdatedIpv4 = AC.Ipv4Default;
                _uow.Quotation.Update(obj);
                _uow.Save();
                if (showMessages)
                    TempData["success"] = "Cotización actualizada exitosamente";

                if (obj.TypeNumeral == SD.QuotationType.Transfer)
                {
                    var objDetails = _uow.QuotationDetail.GetAll(filter: x => x.CompanyId == _companyId && x.ParentId == obj.Id).ToList();
                    foreach (var detail in objDetails)
                    {
                        detail.AmountDetail = obj.AmountTransaction;
                        //Seteamos campos de auditoria
                        detail.UpdatedBy = AC.LOCALHOSTME;
                        detail.UpdatedDate = DateTime.UtcNow;
                        detail.UpdatedHostName = AC.LOCALHOSTPC;
                        detail.UpdatedIpv4 = AC.Ipv4Default;
                        _uow.QuotationDetail.Update(detail);
                    }
                    _uow.Save();
                }
            }

            if (redirectDetail)
            {
                jsonResponse.UrlRedirect = Url.Action(action: "CreateDetail", controller: "Quotation", new { id = obj.Id });
            }
            else if (redirectHome)
            {
                jsonResponse.UrlRedirect = Url.Action(action: "Index", controller: "Quotation");
            }
            else
            {
                jsonResponse.Data = new
                {
                    Id = obj.Id
                };
            }
        }
        else
        {
            var listErrorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(x => x.ErrorMessage)
                .ToList();
            foreach (var item in listErrorMessages)
            {
                errorsMessagesBuilder.Append(item);
            }
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = errorsMessagesBuilder.ToString();
            return Json(jsonResponse);

        }

        jsonResponse.IsSuccess = true;
        return Json(jsonResponse);
    }

    [HttpPost]
    public JsonResult Update([FromForm] Quotation obj)
    {
        StringBuilder errorsMessagesBuilder = new();
        JsonResultResponse? jsonResponse = new();

        if (obj.CompanyId != _companyId)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = $"Id de la compañía no puede ser distinto de {_companyId}";
            return Json(jsonResponse);
        }

        var objQt = _uow.Quotation.Get(filter: x => x.Id == obj.Id);
        if (objQt == null)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = $"Cotización no encontrada";
            return Json(jsonResponse);
        }
        //Verificamos si existe el cliente
        var objCustomer = _uow.Customer.Get(filter: x => x.CompanyId == obj.CompanyId && x.Id == obj.CustomerId, isTracking: false);
        if (objCustomer == null)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = $"Cliente no encontrado";
            return Json(jsonResponse);
        }

        objQt.CustomerId = obj.CustomerId;

        if (objQt.TypeNumeral == SD.QuotationType.Buy)
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
            if (objQt.CurrencyTransaType == SD.CurrencyType.Foreign)
            {
                //Factoring paga en Cordobas
                if (objQt.CurrencyTransferType == SD.CurrencyType.Base)
                {
                    obj.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateBuyTransa);
                    obj.ExchangeRateBuyReal = obj.ExchangeRateBuyTransa;
                }
            }
            //Compra de Euros
            else if (objQt.CurrencyTransaType == SD.CurrencyType.Additional)
            {
                //Factoring paga en Cordobas
                if (objQt.CurrencyTransferType == SD.CurrencyType.Base)
                {
                    obj.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateBuyTransa);
                    obj.ExchangeRateBuyReal = obj.ExchangeRateBuyTransa;

                }
                //Factoring paga en Dolares
                else if (objQt.CurrencyTransferType == SD.CurrencyType.Foreign)
                {
                    obj.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateBuyTransa);
                    obj.ExchangeRateBuyReal = (obj.ExchangeRateBuyTransa * obj.ExchangeRateOfficialTransa);
                }
            }
        }
        else if (objQt.TypeNumeral == SD.QuotationType.Sell)
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
            if (objQt.CurrencyTransaType == SD.CurrencyType.Foreign)
            {
                //Cliente paga en Cordobas
                if (objQt.CurrencyDepositType == SD.CurrencyType.Base)
                {
                    obj.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateSellTransa);
                    obj.ExchangeRateSellReal = obj.ExchangeRateSellTransa;
                }
            }
            //Venta de Euros
            else if (objQt.CurrencyTransaType == SD.CurrencyType.Additional)
            {
                //Cliente paga en Cordobas
                if (objQt.CurrencyDepositType == SD.CurrencyType.Base)
                {
                    obj.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateSellTransa);
                    obj.ExchangeRateSellReal = obj.ExchangeRateSellTransa;

                }
                //Cliente paga en Dolares
                else if (objQt.CurrencyDepositType == SD.CurrencyType.Foreign)
                {
                    obj.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateSellTransa);
                    obj.ExchangeRateSellReal = (obj.ExchangeRateSellTransa * obj.ExchangeRateOfficialTransa);
                }
            }
        }

        objQt.AmountRevenue = obj.AmountRevenue;
        objQt.AmountCommission = obj.AmountCommission;
        objQt.AmountCost = obj.AmountCost;
        objQt.AmountExchange = obj.AmountExchange;
        objQt.AmountTransaction = obj.AmountTransaction;
        objQt.DateTransa = obj.DateTransa;
        objQt.ExchangeRateBuyTransa = obj.ExchangeRateBuyTransa;
        objQt.ExchangeRateSellTransa = obj.ExchangeRateSellTransa;

        //Seteamos campos de auditoria
        objQt.UpdatedBy = AC.LOCALHOSTME;
        objQt.UpdatedDate = DateTime.UtcNow;
        objQt.UpdatedHostName = AC.LOCALHOSTPC;
        objQt.UpdatedIpv4 = AC.Ipv4Default;
        _uow.Quotation.Update(objQt);
        _uow.Save();

        if (objQt.TypeNumeral == SD.QuotationType.Transfer)
        {

            var objDetails = _uow.QuotationDetail.GetAll(filter: x => x.CompanyId == _companyId && x.ParentId == objQt.Id).ToList();
            foreach (var detail in objDetails)
            {
                detail.AmountDetail = objQt.AmountTransaction;
                //Seteamos campos de auditoria
                detail.UpdatedBy = AC.LOCALHOSTME;
                detail.UpdatedDate = DateTime.UtcNow;
                detail.UpdatedHostName = AC.LOCALHOSTPC;
                detail.UpdatedIpv4 = AC.Ipv4Default;
                _uow.QuotationDetail.Update(detail);
            }
            _uow.Save();
        }

        TempData["success"] = "Cotización actualizada exitosamente";

        jsonResponse.IsSuccess = true;
        jsonResponse.UrlRedirect = Url.Action(action: "CreateDetail", controller: "Quotation", new { id = obj.Id });
        return Json(jsonResponse);
    }

    public IActionResult CreateDetail(int id)
    {
        ViewData[AC.Title] = "Dellate - Cotización";
        List<Models.Customer>? listCustomer = new();
        QuotationDetailVM model = new();
        ViewBag.DecimalTransa = JsonSerializer.Serialize(_decimalTransa);
        ViewBag.VariationMaxDeposit = JsonSerializer.Serialize(_variationMaxDeposit);
        ViewBag.DecimalExchangeFull = JsonSerializer.Serialize(_decimalExchangeFull);

        var objHeader = _uow.Quotation.Get(filter: x => x.CompanyId == _companyId && x.Id == id,
            includeProperties: "TypeTrx,CustomerTrx,CurrencyTransferTrx,CurrencyTransaTrx,BankAccountSourceTrx,BankAccountTargetTrx", isTracking: false);
        if (objHeader == null)
        {
            return NotFound();
        }

        if (objHeader.IsAdjustment)
        {
            ViewBag.DecimalExchange = JsonSerializer.Serialize(_decimalExchangeFull);
        }
        else
        {
            ViewBag.DecimalExchange = JsonSerializer.Serialize(_decimalExchange);
        }



        if (objHeader.IsClosed && !objHeader.IsPosted)
        {
            ViewData["IsReClosed"] = true;
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

        var objBankAccountList = _uow.BankAccount
            .GetAll(filter: x => x.CompanyId == _companyId).ToList();
        if (objBankAccountList == null)
        {
            return NotFound();
        }

        model.ModelCreateVM.BankAccountSourceList = objBankAccountList.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
        model.ModelCreateVM.BankAccountTargetList = objBankAccountList.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
        model.ModelCreateVM.CurrencyTransaList = objCurrencyList
            .Where(x => (x.IsActive && (x.Numeral != (int)SD.CurrencyType.Base)))
            .ToList();
        model.ModelCreateVM.CurrencyDepositList = objCurrencyList.Where(x => x.IsActive).ToList();
        model.ModelCreateVM.CurrencyTransferList = objCurrencyList.Where(x => x.IsActive).ToList();
        model.ModelCreateVM.QuotationTypeList = objTypeList;
        model.ModelCreateVM.CurrencySourceTarget =
            $"{objHeader.CurrencyTransaTrx.Code} - {objHeader.CurrencyTransferTrx.Code}";
        //model.ModelCreateVM.CustomerList = objCustomerList.Select(x => new SelectListItem { Text = x.CommercialName, Value = x.Id.ToString() });

        //model.BankList = objBankList.Select(x => new SelectListItem { Text = $"{x.Code}", Value = x.Id.ToString() });
        model.BankList = objBankList;
        model.ModelCreateVM.DataModel = objHeader;
        model.CustomerFullName = $"{objHeader.CustomerTrx.CommercialName}";
        model.NumberTransa = $"COT-{objHeader.TypeTrx.Code}-{objHeader.Numeral}";
        model.DataModel = new();
        model.DataModel.CompanyId = _companyId;

        if (model.ModelCreateVM.DataModel.CustomerId != 0)
        {
            var objCustomer =
                _uow.Customer.Get(filter: x => (x.CompanyId == _companyId && x.Id == model.ModelCreateVM.DataModel.CustomerId));
            listCustomer.Add(objCustomer);
            model.ModelCreateVM.CustomerList = listCustomer.Select(x => new SelectListItem { Text = x.CommercialName, Value = x.Id.ToString() });
        }

        return View(model);
    }

    [HttpPost]
    public async Task<JsonResult> CreateDetail([FromForm] QuotationDetail obj)
    {
        StringBuilder errorsMessagesBuilder = new();
        JsonResultResponse? jsonResponse = new();

        if (ModelState.IsValid)
        {
            if (obj.CompanyId != _companyId)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Id de la compañía no puede ser distinto de {_companyId}";
                return Json(jsonResponse);
            }

            if (obj.AmountDetail == 0)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"El monto no puede ser cero";
                return Json(jsonResponse);
            }

            //Verificamos si existe el padre
            var objHeader = _uow.Quotation.Get(filter: x =>
                x.CompanyId == obj.CompanyId && x.Id == obj.ParentId);

            if (objHeader == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Registro padre no encontrado";
                return Json(jsonResponse);
            }

            //Verificamos si existe la moneda
            var objCurrency = _uow.Currency.Get(filter: x =>
                x.CompanyId == obj.CompanyId && x.Numeral == obj.CurrencyDetailId, isTracking: false);

            if (objCurrency == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Moneda no encontrada";
                return Json(jsonResponse);
            }

            //Verificamos si existe el banco origen
            var objBankSource = _uow.Bank.Get(filter: x =>
                x.CompanyId == obj.CompanyId && x.Id == obj.BankSourceId, isTracking: false);

            if (objBankSource == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Banco origen no encontrado";
                return Json(jsonResponse);
            }

            //Verificamos si tiene banco destino y verificamos si existe
            if (obj.BankTargetId != 0)
            {
                var objBankTarget = _uow.Bank.Get(filter: x =>
                    x.CompanyId == obj.CompanyId && x.Id == obj.BankTargetId, isTracking: false);

                if (objBankTarget == null)
                {
                    jsonResponse.IsSuccess = false;
                    jsonResponse.ErrorMessages = $"Banco destino no encontrado";
                    return Json(jsonResponse);
                }
            }
            else
            {
                obj.BankTargetId = obj.BankSourceId;
            }

            if (obj.Id == 0)
            {
                //Seteamos campos de auditoria
                obj.LineNumber = await _uow.QuotationDetail.NextLineNumber(filter: x =>
                     x.CompanyId == obj.CompanyId && x.ParentId == objHeader.Id && x.QuotationDetailType == obj.QuotationDetailType);
                obj.CreatedBy = AC.LOCALHOSTME;
                obj.CreatedDate = DateTime.UtcNow;
                obj.CreatedHostName = AC.LOCALHOSTPC;
                obj.CreatedIpv4 = AC.Ipv4Default;
                _uow.QuotationDetail.Add(obj);
                _uow.Save();
                TempData[AC.Success] = $"Cotización creada correctamente";
            }
            else
            {
                var objDetail = _uow.QuotationDetail.Get(filter: x =>
                    x.CompanyId == obj.CompanyId && x.ParentId == objHeader.Id && x.Id == obj.Id);

                if (objDetail == null)
                {
                    jsonResponse.IsSuccess = false;
                    jsonResponse.ErrorMessages = $"Detalle de cotización no encontrado";
                    return Json(jsonResponse);
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

                TempData[AC.Success] = $"Cotización actualizada correctamente";
            }

            //Obtenemos los hijos
            var objDetails = _uow.QuotationDetail.GetAll(filter: x =>
                x.CompanyId == obj.CompanyId && x.ParentId == objHeader.Id, includeProperties: "ParentTrx,CurrencyDetailTrx,BankSourceTrx,BankTargetTrx").ToList();

            if (objDetails == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Detalle de cotización no encontrado";
                return Json(jsonResponse);
            }

            //Actualizamos los totales del padre
            objHeader.TotalDeposit = objDetails
                .Where(x => x.QuotationDetailType == QuotationDetailType.Deposit || x.QuotationDetailType == QuotationDetailType.CreditTransfer)
                .Sum(x => x.AmountDetail);
            objHeader.TotalTransfer = objDetails
                .Where(x => x.QuotationDetailType == QuotationDetailType.Transfer || x.QuotationDetailType == QuotationDetailType.DebitTransfer)
                .Sum(x => x.AmountDetail);
            objHeader.UpdatedBy = AC.LOCALHOSTME;
            objHeader.UpdatedDate = DateTime.UtcNow;
            objHeader.UpdatedHostName = AC.LOCALHOSTPC;
            objHeader.UpdatedIpv4 = AC.Ipv4Default;
            _uow.Quotation.Update(objHeader);
            _uow.Save();
        }
        else
        {
            var listErrorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(x => x.ErrorMessage)
                .ToList();
            foreach (var item in listErrorMessages)
            {
                errorsMessagesBuilder.Append(item);
            }
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = errorsMessagesBuilder.ToString();
            return Json(jsonResponse);

        }

        jsonResponse.IsSuccess = true;
        jsonResponse.UrlRedirect = Url.Action(action: "CreateDetail", controller: "Quotation", new { id = obj.ParentId });
        return Json(jsonResponse);
    }

    public IActionResult Delete(int id)
    {
        ViewData[AC.Title] = "Eliminar - Cotización";

        QuotationDetailVM model = new();
        ViewBag.DecimalTransa = JsonSerializer.Serialize(_decimalTransa);
        ViewBag.DecimalExchange = JsonSerializer.Serialize(_decimalExchange);

        var objHeader = _uow.Quotation.Get(filter: x => x.CompanyId == _companyId && x.Id == id,
            includeProperties: "TypeTrx,CustomerTrx,CurrencyTransferTrx,CurrencyTransaTrx,BankAccountSourceTrx,BankAccountTargetTrx", isTracking: false);
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
        model.ModelCreateVM.CurrencySourceTarget =
            $"{objHeader.CurrencyTransaTrx.Code} - {objHeader.CurrencyTransferTrx.Code}";
        return View(model);
    }

    public IActionResult Detail(int id)
    {
        ViewData[AC.Title] = "Visualizar - Cotización";

        QuotationDetailVM model = new();
        ViewBag.DecimalTransa = JsonSerializer.Serialize(_decimalTransa);
        ViewBag.DecimalExchange = JsonSerializer.Serialize(_decimalExchange);

        var objHeader = _uow.Quotation.Get(filter: x => x.CompanyId == _companyId && x.Id == id,
            includeProperties: "TypeTrx,CustomerTrx,CurrencyTransferTrx,CurrencyTransaTrx,BankAccountSourceTrx,BankAccountTargetTrx", isTracking: false);
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
        model.ModelCreateVM.CurrencySourceTarget =
            $"{objHeader.CurrencyTransaTrx.Code} - {objHeader.CurrencyTransferTrx.Code}";
        return View(model);
    }

    public IActionResult ProcessingDate()
    {
        ProcessingDateVM model = new();
        string processingDateString = HttpContext.Session.GetString(AC.ProcessingDate) ?? DateOnly.FromDateTime(DateTime.Now).ToString();
        DateOnly processingDate = DateOnly.Parse(processingDateString);
        model.ProcessingDate = processingDate;
        ViewData[AC.DefaultDateCurrent] = JsonSerializer.Serialize(model.ProcessingDate.ToString(AC.DefaultDateFormatWeb));
        return View(model);
    }

    [HttpPost]
    public JsonResult ProcessingDate(DateOnly? processingDate)
    {
        JsonResultResponse? jsonResponse = new();
        try
        {
            if (processingDate != null)
            {
                HttpContext.Session.SetString(AC.ProcessingDate, processingDate.Value.ToString());
            }
            else
            {
                HttpContext.Session.Remove(AC.ProcessingDate);
            }

            jsonResponse.IsSuccess = true;
            TempData[AC.Success] = $"Fecha de Procesamiento guardada correctamente";
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

    #region API_CALL
    public JsonResult GetAll(string dateInitial, string dateFinal, bool includeVoid = false)
    {
        JsonResultResponse? jsonResponse = new();
        DateOnly dateTransaInitial = DateOnly.Parse(dateInitial);
        DateOnly dateTransaFinal = DateOnly.Parse(dateFinal);
        var objList = _uow.Quotation
            .GetAll(x => (x.CompanyId == _companyId && x.DateTransa >= dateTransaInitial && x.DateTransa <= dateTransaFinal && (x.IsVoid == includeVoid || !x.IsVoid))
            , includeProperties: "TypeTrx,CustomerTrx,CurrencyTransaTrx,CurrencyTransferTrx,CurrencyDepositTrx,BusinessExecutiveTrx").ToList();

        if (objList == null)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = "Error al cargar los datos";
            return Json(jsonResponse);
        }

        if (objList.Count <= 0)
        {
            jsonResponse.IsInfo = true;
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = "No hay registros que mostrar";
            return Json(jsonResponse);
        }

        jsonResponse.IsSuccess = true;
        jsonResponse.Data = objList;
        return Json(jsonResponse);
    }

    public async Task<JsonResult> GetAllByCustomer(int customerId, SD.QuotationType type, SD.CurrencyType currency)
    {
        JsonResultResponse? jsonResponse = new();

        var objPages = await _uow.Quotation
            .GetAllAsync(x => (x.CompanyId == _companyId &&
            x.CustomerId == customerId &&
            x.CurrencyTransaType == currency &&
            x.TypeNumeral == type &&
            x.IsClosed)
              , orderExpressions: new List<Expression<Func<Quotation, object>>>() { i => i.DateTransa }
              , orderDirection: OrderDirection.Desc
            , includeProperties: "TypeTrx,CustomerTrx,CurrencyTransaTrx,CurrencyTransferTrx,CurrencyDepositTrx,BusinessExecutiveTrx"
            , pageNumber: 1, pageSize: 5);

        if (objPages == null)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = "Error al cargar los datos";
            return Json(jsonResponse);
        }

        var objList = objPages.ToList();

        jsonResponse.IsSuccess = true;
        jsonResponse.Data = objList;
        return Json(jsonResponse);
    }

    public JsonResult GetAllByParent(int parentId = 0, QuotationDetailType type = QuotationDetailType.Deposit)
    {
        JsonResultResponse? jsonResponse = new();

        var objList = _uow.QuotationDetail
            .GetAll(x => (x.CompanyId == _companyId &&
                          x.ParentId == parentId &&
                          x.QuotationDetailType == type)
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

    public async Task<JsonResult> GetAverageExchangeRate(int customerId, SD.QuotationType type, SD.CurrencyType currency)
    {
        JsonResultResponse? jsonResponse = new();

        try
        {

            string processingDateString = HttpContext.Session.GetString(AC.ProcessingDate) ?? DateOnly.FromDateTime(DateTime.Now).ToString();
            DateOnly dateFilter = DateOnly.Parse(processingDateString);

            // Obtener la fecha un mes antes
            DateOnly dateOneMonthBefore = DateOnly.FromDateTime(dateFilter.ToDateTime(TimeOnly.MinValue).AddMonths(-1));

            // Obtener la fecha del primer día del mes anterior
            DateOnly dateInitial = new DateOnly(dateOneMonthBefore.Year, dateOneMonthBefore.Month, 1);

            // Obtener la fecha del último día del mes anterior
            DateTime lastDay = new DateTime(dateOneMonthBefore.Year, dateOneMonthBefore.Month, DateTime.DaysInMonth(dateOneMonthBefore.Year, dateOneMonthBefore.Month));

            DateOnly dateFinal = new DateOnly(lastDay.Year, lastDay.Month, lastDay.Day);


            var objPages = await _uow.Quotation
            .GetAllAsync(x => (x.CompanyId == _companyId &&
            x.CustomerId == customerId &&
            x.CurrencyTransaType == currency &&
            x.TypeNumeral == type &&
            x.DateTransa >= dateInitial &&
            x.DateTransa <= dateFinal &&
            x.IsClosed)
            , includeProperties: "TypeTrx,CustomerTrx,CurrencyTransaTrx,CurrencyTransferTrx,CurrencyDepositTrx,BusinessExecutiveTrx"
            , pageNumber: 1, pageSize: 0);

            if (objPages == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = "Error al cargar los datos";
                return Json(jsonResponse);
            }

            decimal average = 0;
            decimal weightedAverage = 0;

            foreach (var item in objPages.ToList())
            {
                average += type == SD.QuotationType.Buy ? item.ExchangeRateBuyTransa : item.ExchangeRateSellTransa;
                weightedAverage += type == SD.QuotationType.Buy ?
                    (item.ExchangeRateBuyTransa * item.AmountTransaction) :
                    (item.ExchangeRateSellTransa * item.AmountTransaction);
            }

            if (objPages.Count() != 0)
            {
                average = (average / objPages.Count());
                weightedAverage = (weightedAverage / objPages.Sum(x => x.AmountTransaction));
            }

            jsonResponse.IsSuccess = true;
            jsonResponse.Data = new
            {
                average,
                weightedAverage
            };

            return Json(jsonResponse);

        }
        catch (Exception ex)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = ex.Message.ToString();
            return Json(jsonResponse);
        }
    }

    [HttpPost]
    public async Task<JsonResult> AdjustmentExchange(int parentId)
    {
        JsonResultResponse? jsonResponse = new()
        {
            IsSuccess = true
        };

        try
        {
            decimal exchange = 0M;
            var objQuotation = _uow.Quotation
                .Get(filter: x => (x.CompanyId == _companyId && x.Id == parentId));

            if (objQuotation is null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = "Cotización no encontrada";
                return Json(jsonResponse);
            }

            if (objQuotation.TypeNumeral == SD.QuotationType.Buy)
            {
                objQuotation.ExchangeRateBuyTransa = (objQuotation.TotalTransfer / objQuotation.AmountTransaction);

                //TC COMPRA MENOR AL TC OFICIAL
                if (objQuotation.ExchangeRateBuyTransa < objQuotation.ExchangeRateOfficialTransa)
                {
                    objQuotation.AmountRevenue = (objQuotation.ExchangeRateOfficialTransa - objQuotation.ExchangeRateBuyTransa) * objQuotation.AmountTransaction;
                    objQuotation.AmountCost = 0;
                }
                //TC COMPRA MAYOR AL TC OFICIAL
                else
                {
                    objQuotation.AmountCost = (objQuotation.ExchangeRateBuyTransa - objQuotation.ExchangeRateOfficialTransa) * objQuotation.AmountTransaction;
                    objQuotation.AmountRevenue = 0;
                }

                //Compra de dolares 
                if (objQuotation.CurrencyTransaType == SD.CurrencyType.Foreign)
                {
                    //Factoring paga en Cordobas
                    if (objQuotation.CurrencyTransferType == SD.CurrencyType.Base)
                    {
                        objQuotation.AmountExchange = (objQuotation.AmountTransaction * objQuotation.ExchangeRateBuyTransa);
                        objQuotation.ExchangeRateBuyReal = objQuotation.ExchangeRateBuyTransa;
                    }
                }
                //Compra de Euros
                else if (objQuotation.CurrencyTransaType == SD.CurrencyType.Additional)
                {
                    //Factoring paga en Cordobas
                    if (objQuotation.CurrencyTransferType == SD.CurrencyType.Base)
                    {
                        objQuotation.AmountExchange = (objQuotation.AmountTransaction * objQuotation.ExchangeRateBuyTransa);
                        objQuotation.ExchangeRateBuyReal = objQuotation.ExchangeRateBuyTransa;

                    }
                    //Factoring paga en Dolares
                    else if (objQuotation.CurrencyTransferType == SD.CurrencyType.Foreign)
                    {
                        objQuotation.AmountExchange = (objQuotation.AmountTransaction * objQuotation.ExchangeRateBuyTransa);
                        objQuotation.ExchangeRateBuyReal = (objQuotation.ExchangeRateBuyTransa * objQuotation.ExchangeRateOfficialTransa);
                    }
                }
            }
            else if (objQuotation.TypeNumeral == SD.QuotationType.Sell)
            {
                objQuotation.ExchangeRateSellTransa = (objQuotation.TotalDeposit / objQuotation.AmountTransaction);

                //TC VENTA MENOR AL TC OFICIAL
                if (objQuotation.ExchangeRateSellTransa < objQuotation.ExchangeRateOfficialTransa)
                {
                    objQuotation.AmountCost = (objQuotation.ExchangeRateOfficialTransa - objQuotation.ExchangeRateSellTransa) * objQuotation.AmountTransaction;
                    objQuotation.AmountRevenue = 0;
                }
                //TC VENTA MAYOR AL TC OFICIAL
                else
                {
                    objQuotation.AmountRevenue = (objQuotation.ExchangeRateSellTransa - objQuotation.ExchangeRateOfficialTransa) * objQuotation.AmountTransaction;
                    objQuotation.AmountCost = 0;
                }

                //Venta de dolares 
                if (objQuotation.CurrencyTransaType == SD.CurrencyType.Foreign)
                {
                    //Cliente paga en Cordobas
                    if (objQuotation.CurrencyDepositType == SD.CurrencyType.Base)
                    {
                        objQuotation.AmountExchange = (objQuotation.AmountTransaction * objQuotation.ExchangeRateSellTransa);
                        objQuotation.ExchangeRateSellReal = objQuotation.ExchangeRateSellTransa;
                    }
                }
                //Venta de Euros
                else if (objQuotation.CurrencyTransaType == SD.CurrencyType.Additional)
                {
                    //Cliente paga en Cordobas
                    if (objQuotation.CurrencyDepositType == SD.CurrencyType.Base)
                    {
                        objQuotation.AmountExchange = (objQuotation.AmountTransaction * objQuotation.ExchangeRateSellTransa);
                        objQuotation.ExchangeRateSellReal = objQuotation.ExchangeRateSellTransa;

                    }
                    //Cliente paga en Dolares
                    else if (objQuotation.CurrencyDepositType == SD.CurrencyType.Foreign)
                    {
                        objQuotation.AmountExchange = (objQuotation.AmountTransaction * objQuotation.ExchangeRateSellTransa);
                        objQuotation.ExchangeRateSellReal = (objQuotation.ExchangeRateSellTransa * objQuotation.ExchangeRateOfficialTransa);
                    }
                }
            }

            //Seteamos campos de auditoria
            objQuotation.UpdatedBy = AC.LOCALHOSTME;
            objQuotation.UpdatedDate = DateTime.UtcNow;
            objQuotation.UpdatedHostName = AC.LOCALHOSTPC;
            objQuotation.UpdatedIpv4 = AC.Ipv4Default;
            objQuotation.IsAdjustment = true;
            _uow.Quotation.Update(objQuotation);
            _uow.Save();
            jsonResponse.UrlRedirect = Url.Action(action: "CreateDetail", controller: "Quotation", new { id = objQuotation.Id });


            return Json(jsonResponse);
        }
        catch (Exception ex)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = ex.Message.ToString();
            return Json(jsonResponse);
        }
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

    [HttpPost, ActionName("Closed")]
    public async Task<JsonResult> ClosedPost(int id)
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
            objHeader.InternalSerial = AC.InternalSerialOfficial;
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

    [HttpPost, ActionName("ReClosed")]
    public JsonResult ReClosedPost(int id)
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

            objHeader.IsPosted = true;

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
            TempData["success"] = $"Cotización re-cerrada correctamente";
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

    [HttpPost, ActionName("Void")]
    public JsonResult VoidPost(int id)
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

            objHeader.IsVoid = true;
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
            TempData["success"] = $"Cotización anulada correctamente";
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

    [HttpPost]
    public JsonResult GetCustomers(bool onlyCompanies = false)
    {
        JsonResultResponse? jsonResponse = new();
        StringBuilder errorsMessagesBuilder = new();

        try
        {
            var objCustomerList = _uow.Customer
                .GetAll(filter: x => x.CompanyId == _companyId && x.IsSystemRow == onlyCompanies).ToList();
            if (objCustomerList == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Cliente invalido";
                return Json(jsonResponse);
            }

            jsonResponse.IsSuccess = true;
            jsonResponse.Data = objCustomerList.Select(x => new SelectListItem { Text = x.CommercialName, Value = x.Id.ToString() });
            return Json(jsonResponse);
        }
        catch (Exception ex)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = ex.Message.ToString();
            return Json(jsonResponse);
        }
    }

    [HttpPost]
    public JsonResult GetCustomerByContain(string search, bool onlyCompanies = false)
    {
        JsonResultResponse? jsonResponse = new();
        StringBuilder errorsMessagesBuilder = new();
        var objCustomerList = new List<Models.Customer>();
        try
        {
            if (search == "." || search == null || search == string.Empty)
            {
                objCustomerList = _uow.Customer
                    .GetAll(filter: x => x.CompanyId == _companyId &&
                                         x.IsSystemRow == onlyCompanies).ToList();
            }
            else
            {
                objCustomerList = _uow.Customer
                    .GetAll(filter: x => x.CompanyId == _companyId &&
                                         x.IsSystemRow == onlyCompanies &&
                                         (x.CommercialName.Contains(search) ||
                                          x.IdentificationNumber.Contains(search) ||
                                          x.Code.Contains(search) ||
                                          x.AddressPrimary.Contains(search))).ToList();
            }


            if (objCustomerList == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Cliente invalido";
                return Json(jsonResponse);
            }

            jsonResponse.IsSuccess = true;
            jsonResponse.Data = objCustomerList.Select(x => new SelectListItem { Text = x.CommercialName, Value = x.Id.ToString() });
            return Json(jsonResponse);
        }
        catch (Exception ex)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = ex.Message.ToString();
            return Json(jsonResponse);
        }
    }

    [HttpPost]
    public JsonResult GetBankAccountTarget(int idSource)
    {
        JsonResultResponse? jsonResponse = new();
        StringBuilder errorsMessagesBuilder = new();

        try
        {
            var objBankAccountSource = _uow.BankAccount
                .Get(filter: x => x.CompanyId == _companyId && x.Id == idSource);
            if (objBankAccountSource == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Cuenta bancaria origen invalida";
                return Json(jsonResponse);
            }


            var objBankAccountTargetList = _uow.BankAccount
                .GetAll(filter: x => x.CompanyId == _companyId && x.CurrencyId == objBankAccountSource.CurrencyId).ToList();
            if (objBankAccountTargetList == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Cuenta bancaria destino invalida";
                return Json(jsonResponse);
            }
            jsonResponse.IsSuccess = true;
            jsonResponse.Data = objBankAccountTargetList.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            return Json(jsonResponse);
        }
        catch (Exception ex)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = ex.Message.ToString();
            return Json(jsonResponse);
        }
    }

    #endregion

    #region EXPORT - IMPORT

    [HttpGet]
    public IActionResult ExportExcel(string dateInitial, string dateFinal, bool includeVoid = true)
    {
        DateOnly dateTransaInitial = DateOnly.Parse(dateInitial);
        DateOnly dateTransaFinal = DateOnly.Parse(dateFinal);

        var objQuotationList = _uow.Quotation
            .GetAll(x => (x.CompanyId == _companyId && x.DateTransa >= dateTransaInitial && x.DateTransa <= dateTransaFinal && (x.IsVoid == includeVoid || !x.IsVoid))
                , includeProperties: "TypeTrx,CustomerTrx,CurrencyTransaTrx,CurrencyTransferTrx,CurrencyDepositTrx,BusinessExecutiveTrx,BankAccountSourceTrx,BankAccountTargetTrx").ToList();

        if (objQuotationList == null || objQuotationList.Count == 0)
        {
            return NoContent();
        }

        return GenerarExcel("Cotizaciones.xlsx", objQuotationList);
    }
    private FileResult GenerarExcel(string nombreArchivo, List<Models.Quotation> listEntities)
    {
        using (XLWorkbook wb = new XLWorkbook())
        {
            int sheetIndex = 1; // Inicializar el índice de la hoja de trabajo

            foreach (var header in listEntities)
            {
                var sheetName = $"{header.TypeTrx.Code}_{header.Numeral.ToString().PadLeft(3, AC.CharDefaultEmpty)}_{header.DateTransa.ToString(AC.DefaultDateFormatWeb)}";
                var worksheet = wb.Worksheets.Add(sheetName);

                var objCompany = _uow.Company.Get(filter: x => x.Id == _companyId);

                // Escribir el nombre de la compañía en la primera fila
                worksheet.Cell(1, 1).Value = objCompany.Name;
                worksheet.Range(1, 1, 1, 7).Merge().Style.Font.Bold = true;
                worksheet.Range(1, 1, 1, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Range(1, 1, 1, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


                var headerRow = worksheet.Row(3);
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Fill.BackgroundColor = XLColor.PastelBlue;
                headerRow = worksheet.Row(5);
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Fill.BackgroundColor = XLColor.PastelBlue;
                headerRow = worksheet.Row(7);
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Fill.BackgroundColor = XLColor.PastelGray;

                worksheet.Cell(3, 1).Value = "Tipo";
                worksheet.Cell(3, 2).Value = "# Transa";
                worksheet.Cell(3, 3).Value = "Fecha";
                worksheet.Cell(3, 4).Value = "Cliente Código";
                worksheet.Cell(3, 5).Value = "Cliente Nombre";
                worksheet.Cell(3, 6).Value = "Mon Transa";
                worksheet.Cell(3, 7).Value = "Mon Deposito";
                worksheet.Cell(3, 8).Value = "Mon Transfer";
                worksheet.Cell(3, 9).Value = "T/C Oficial";
                worksheet.Cell(3, 10).Value = "T/C Compra";
                worksheet.Cell(3, 11).Value = "T/C Venta";
                worksheet.Cell(3, 12).Value = "Preservar Numeración";

                worksheet.Cell(4, 1).Value = header.TypeTrx.Code;
                worksheet.Cell(4, 2).Value = header.Numeral.ToString().PadLeft(3, AC.CharDefaultEmpty);
                worksheet.Cell(4, 3).SetValue(header.DateTransa.ToDateTimeConvert());
                worksheet.Cell(4, 3).Style.NumberFormat.SetFormat(AC.DefaultDateFormatView);
                worksheet.Cell(4, 4).Value = header.CustomerTrx.Code;
                worksheet.Cell(4, 5).Value = header.CustomerTrx.BusinessName;
                worksheet.Cell(4, 6).Value = (short)header.CurrencyTransaType;
                worksheet.Cell(4, 7).Value = (short)header.CurrencyDepositType;
                worksheet.Cell(4, 8).Value = (short)header.CurrencyTransferType;
                worksheet.Cell(4, 9).Value = header.ExchangeRateOfficialTransa;
                worksheet.Cell(4, 9).Style.NumberFormat.Format = AC.XlsFormatRateExchange;
                worksheet.Cell(4, 10).Value = header.ExchangeRateBuyTransa;
                worksheet.Cell(4, 10).Style.NumberFormat.Format = AC.XlsFormatRateExchange;
                worksheet.Cell(4, 11).Value = header.ExchangeRateSellTransa;
                worksheet.Cell(4, 11).Style.NumberFormat.Format = AC.XlsFormatRateExchange;
                worksheet.Cell(4, 12).Value = "N";

                worksheet.Cell(5, 1).Value = "Monto";
                worksheet.Cell(5, 2).Value = "Monto M/C";
                worksheet.Cell(5, 3).Value = "Costo";
                worksheet.Cell(5, 4).Value = "Ingreso";
                worksheet.Cell(5, 5).Value = "Comisión TRF";
                worksheet.Cell(5, 6).Value = "Cta Ban Origen";
                worksheet.Cell(5, 7).Value = "Cta Ban Destino";
                worksheet.Cell(5, 8).Value = "Ejecutivo";
                worksheet.Cell(5, 9).Value = "Cerrado";
                worksheet.Cell(5, 10).Value = "Contabilizado";
                worksheet.Cell(5, 11).Value = "Anulado";


                worksheet.Cell(6, 1).Value = header.AmountTransaction;
                worksheet.Cell(6, 1).Style.NumberFormat.Format = AC.XlsFormatNumeric;
                worksheet.Cell(6, 2).Value = header.AmountExchange;
                worksheet.Cell(6, 2).Style.NumberFormat.Format = AC.XlsFormatNumeric;
                worksheet.Cell(6, 3).Value = header.AmountCost;
                worksheet.Cell(6, 3).Style.NumberFormat.Format = AC.XlsFormatNumeric;
                worksheet.Cell(6, 4).Value = header.AmountRevenue;
                worksheet.Cell(6, 4).Style.NumberFormat.Format = AC.XlsFormatNumeric;
                worksheet.Cell(6, 5).Value = header.AmountCommission;
                worksheet.Cell(6, 5).Style.NumberFormat.Format = AC.XlsFormatNumeric;
                worksheet.Cell(6, 6).Value = header.BankAccountSourceTrx?.Code ?? "";
                worksheet.Cell(6, 7).Value = header.BankAccountTargetTrx?.Code ?? "";
                worksheet.Cell(6, 8).Value = header.BusinessExecutiveTrx.Code;
                worksheet.Cell(6, 9).Value = header.IsClosed ? "S" : "N";
                worksheet.Cell(6, 10).Value = header.IsPosted ? "S" : "N";
                worksheet.Cell(6, 11).Value = header.IsVoid ? "S" : "N";

                worksheet.Cell(7, 1).Value = "#";
                worksheet.Cell(7, 2).Value = "Banco Origen";
                worksheet.Cell(7, 3).Value = "Cta Ban Origen";
                worksheet.Cell(7, 4).Value = "Banco Destino";
                worksheet.Cell(7, 5).Value = "Cta Ban Destino";
                worksheet.Cell(7, 6).Value = "Importe";
                worksheet.Cell(7, 7).Value = "Tipo";
                worksheet.Cell(7, 8).Value = "Asiento contable Id";
                worksheet.Cell(7, 9).Value = "Transaccion bancaria id";
                worksheet.Cell(7, 10).Value = "Asiento contable fee Id";
                worksheet.Cell(7, 11).Value = "Transaccion bancaria fee id";

                sheetIndex++;

                var children = _uow.QuotationDetail.GetAll(filter: x =>
                    (x.CompanyId == _companyId && x.ParentId == header.Id), includeProperties: "ParentTrx,CurrencyDetailTrx,BankSourceTrx,BankTargetTrx").ToList();

                int rowNum = 8;

                foreach (var detail in children)
                {
                    worksheet.Cell(rowNum, 1).Value = detail.LineNumber;
                    worksheet.Cell(rowNum, 2).Value = detail.BankSourceTrx.Code;
                    worksheet.Cell(rowNum, 3).Value = header.BankAccountSourceTrx?.Code ?? "";
                    worksheet.Cell(rowNum, 4).Value = detail.BankTargetTrx.Code;
                    worksheet.Cell(rowNum, 5).Value = header.BankAccountTargetTrx?.Code ?? "";
                    worksheet.Cell(rowNum, 6).Value = detail.AmountDetail;
                    worksheet.Cell(rowNum, 6).Style.NumberFormat.Format = AC.XlsFormatNumeric;
                    worksheet.Cell(rowNum, 7).Value = (short)detail.QuotationDetailType;
                    worksheet.Cell(rowNum, 8).Value = detail.JournalEntryId?.ToString();
                    worksheet.Cell(rowNum, 9).Value = detail.BankTransactionId?.ToString();
                    worksheet.Cell(rowNum, 10).Value = detail.JournalEntryTransferFeeId?.ToString();
                    worksheet.Cell(rowNum, 11).Value = detail.BankTransactionTransferFeeId?.ToString();

                    rowNum++;
                }

                worksheet.Column(1).AdjustToContents();
                worksheet.Column(2).AdjustToContents();
                worksheet.Column(3).AdjustToContents();
                worksheet.Column(4).AdjustToContents();
                worksheet.Column(5).AdjustToContents();
                worksheet.Column(6).AdjustToContents();
                worksheet.Column(7).AdjustToContents();
                worksheet.Column(8).AdjustToContents();
                worksheet.Column(9).AdjustToContents();
                worksheet.Column(10).AdjustToContents();
                worksheet.Column(11).AdjustToContents();
                worksheet.Column(12).AdjustToContents();
            }

            using (MemoryStream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                return File(stream.ToArray(),
                    AC.ContentTypeExcel,
                    nombreArchivo);
            }
        }
    }

    [HttpGet]
    public IActionResult Import()
    {
        // Titulo de la pagina
        ViewData[AC.Title] = $"Cotizaciones - Importar";
        ImportVM modelVm = new();
        return View(modelVm);
    }

    [HttpGet]
    public IActionResult Export()
    {
        // Titulo de la pagina
        ViewData[AC.Title] = $"Cotizaciones - Exportar";
        TransactionReportVM modelVm = new();
        modelVm.DateTransaFinal = DateOnly.FromDateTime(DateTime.Now);
        modelVm.DateTransaInitial = DateOnly.FromDateTime(DateTime.Now);
        return View(modelVm);
    }

    [HttpPost]
    public async Task<JsonResult> Import([FromForm] ImportVM objImportViewModel)
    {
        List<string> ErrorListMessages = new List<string>();
        var errorsMessagesBuilder = new StringBuilder();
        JsonResultResponse? jsonResponse = new();
        List<Models.Customer> objCustomerList = new();
        List<Models.CurrencyExchangeRate>? objCurrencyExchangeRateList = new();
        List<Models.Quotation> objQuotationList = new();
        List<Models.QuotationDetail> objQuotationDetailList = new();
        Models.QuotationType? objQuotationType = new();
        Models.Customer? objCustomer = new();
        Models.BusinessExecutive? objBusiness = new();
        Models.BankAccount? objBankAccountSource = new();
        Models.BankAccount? objBankAccountTarget = new();
        Models.Bank? objBankSource = new();
        Models.Bank? objBankTarget = new();
        int countMaxId = 0;
        int countMaxSeq = 0;

        try
        {
            if (objImportViewModel.FileExcel is null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"No hay registros para importar";
                return Json(jsonResponse);
            }

            var objCurrencyList = _uow.Currency.GetAll(filter: x => (x.CompanyId == _companyId)).ToList();
            if (objCurrencyList == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Moneda no encontrada";
                return Json(jsonResponse);
            }

            using (var stream = objImportViewModel.FileExcel.OpenReadStream())
            {
                using (var workbook = new XLWorkbook(stream))
                {
                    foreach (var worksheet in workbook.Worksheets)
                    {
                        string sheetName = worksheet.Name;
                        int firstRowNumber = 4;
                        int secondRowNumber = 6;
                        var isPreserverNumber = false;
                        var header = new Quotation();
                        var countMaxIdDataBase = await _uow.Quotation.NextId();
                        if (countMaxIdDataBase > countMaxId)
                        {
                            countMaxId = countMaxIdDataBase;
                        }
                        else
                        {
                            countMaxId++;
                        }

                        header.Id = countMaxId;
                        header.CompanyId = _companyId;
                        var type = worksheet.Cell(4, 1).GetString().Trim();
                        if (string.IsNullOrWhiteSpace(type))
                        {
                            ErrorListMessages.Add($"El tipo está vacio - En la hoja {sheetName} fila:{firstRowNumber}. |");
                        }
                        else
                        {
                            //En dependencia del tipo validamos los demas campos

                            objQuotationType = _uow.QuotationType.Get(filter: x =>
                                (x.CompanyId == _companyId && x.Code.ToUpper() == type.ToUpper()));

                            if (objQuotationType == null)
                            {
                                ErrorListMessages.Add($"El tipo no fue encontrado - En la hoja {sheetName} fila:{firstRowNumber}. |");
                            }
                            else
                            {
                                header.TypeId = objQuotationType.Id;
                                header.TypeNumeral = (SD.QuotationType)objQuotationType.Numeral;

                                var numeral = worksheet.Cell(4, 2).GetString().Trim();
                                if (string.IsNullOrWhiteSpace(numeral))
                                {
                                    ErrorListMessages.Add($"El número de transacción está vacio - En la hoja {sheetName} fila:{firstRowNumber}. |");
                                }

                                var date = worksheet.Cell(4, 3).GetString().Trim();
                                if (string.IsNullOrWhiteSpace(date))
                                {
                                    ErrorListMessages.Add($"La fecha está vacia - En la hoja {sheetName} fila:{firstRowNumber}. |");
                                }
                                else
                                {
                                    DateOnly dateTransa = DateOnly.Parse(date.Split(" ")[0]);
                                    header.DateTransa = dateTransa;
                                }

                                var customerCode = worksheet.Cell(4, 4).GetString().Trim();
                                if (string.IsNullOrWhiteSpace(customerCode))
                                {
                                    ErrorListMessages.Add($"El código del cliente está vacio - En la hoja {sheetName} fila:{firstRowNumber}. |");
                                }
                                else
                                {
                                    objCustomer = _uow.Customer.Get(filter: x =>
                                        (x.CompanyId == _companyId && x.Code == customerCode));

                                    if (objCustomer == null)
                                    {
                                        ErrorListMessages.Add($"El código del cliente no fue encontrado - En la hoja {sheetName} fila:{firstRowNumber}. |");
                                    }
                                    else
                                    {
                                        header.CustomerId = objCustomer.Id;
                                    }
                                }

                                var amountTransa = worksheet.Cell(6, 1).GetString().Trim();
                                if (string.IsNullOrWhiteSpace(amountTransa))
                                {
                                    ErrorListMessages.Add($"El monto está vacio - En la hoja {sheetName} fila:{secondRowNumber}. |");
                                }
                                else
                                {
                                    header.AmountTransaction = decimal.Parse(amountTransa);
                                    if (header.AmountTransaction <= 0)
                                    {
                                        ErrorListMessages.Add($"El monto no puede ser menor o igual a cero - En la hoja {sheetName} fila:{secondRowNumber}. |");
                                    }
                                }

                                var businessExecutive = worksheet.Cell(6, 8).GetString().Trim();
                                if (string.IsNullOrWhiteSpace(businessExecutive))
                                {
                                    ErrorListMessages.Add($"El ejecutivo está vacio - En la hoja {sheetName} fila:{secondRowNumber}. |");
                                }
                                else
                                {
                                    objBusiness = _uow.BusinessExecutive.Get(filter: x =>
                                        (x.CompanyId == _companyId && x.Code == businessExecutive));

                                    if (objBusiness == null)
                                    {
                                        ErrorListMessages.Add($"El ejecutivo no fue encontrado - En la hoja {sheetName} fila:{firstRowNumber}. |");
                                    }
                                    else
                                    {
                                        header.BusinessExecutiveCode = objBusiness.Code;
                                        header.BusinessExecutiveId = objBusiness.Id;
                                    }
                                }

                                var isClosed = worksheet.Cell(6, 9).GetString().Trim();
                                if (string.IsNullOrWhiteSpace(isClosed))
                                {
                                    ErrorListMessages.Add($"Estado de cerrado está vacio - En la hoja {sheetName} fila:{secondRowNumber}. |");
                                }
                                else
                                {
                                    if (isClosed == "S")
                                    {
                                        header.IsClosed = true;
                                    }
                                    else if (isClosed == "N")
                                    {
                                        header.IsClosed = false;
                                    }
                                    else
                                    {
                                        ErrorListMessages.Add($"Estado de cerrado es invalido - En la hoja {sheetName} fila:{secondRowNumber}. |");
                                    }
                                }

                                var isPosted = worksheet.Cell(6, 10).GetString().Trim();
                                if (string.IsNullOrWhiteSpace(isPosted))
                                {
                                    ErrorListMessages.Add($"Estado de contabilizado está vacio - En la hoja {sheetName} fila:{secondRowNumber}. |");
                                }
                                else
                                {
                                    if (isPosted == "S")
                                    {
                                        header.IsPosted = true;
                                    }
                                    else if (isPosted == "N")
                                    {
                                        header.IsPosted = false;
                                    }
                                    else
                                    {
                                        ErrorListMessages.Add($"Estado de contabilizado es invalido - En la hoja {sheetName} fila:{secondRowNumber}. |");
                                    }
                                }

                                var isVoid = worksheet.Cell(6, 11).GetString().Trim();
                                if (string.IsNullOrWhiteSpace(isVoid))
                                {
                                    ErrorListMessages.Add($"Estado de anulado está vacio - En la hoja {sheetName} fila:{secondRowNumber}. |");
                                }
                                else
                                {
                                    if (isVoid == "S")
                                    {
                                        header.IsVoid = true;
                                    }
                                    else if (isVoid == "N")
                                    {
                                        header.IsVoid = false;
                                    }
                                    else
                                    {
                                        ErrorListMessages.Add($"Estado de anulado es invalido - En la hoja {sheetName} fila:{secondRowNumber}. |");
                                    }
                                }

                                var exchangeOfficial = worksheet.Cell(4, 9).GetString().Trim();
                                if (string.IsNullOrWhiteSpace(exchangeOfficial))
                                {
                                    ErrorListMessages.Add($"El tipo de cambio oficial está vacio - En la hoja {sheetName} fila:{firstRowNumber}. |");
                                }
                                else
                                {
                                    header.ExchangeRateOfficialTransa = decimal.Parse(exchangeOfficial);
                                    if (header.ExchangeRateOfficialTransa <= 0)
                                    {
                                        ErrorListMessages.Add($"El tipo de cambio oficial no puede ser menor o igual a cero - En la hoja {sheetName} fila:{firstRowNumber}. |");
                                    }
                                }

                                if (objQuotationType.Numeral != (int)SD.QuotationType.Transfer)
                                {
                                    var monTransa = worksheet.Cell(4, 6).GetString().Trim();
                                    if (string.IsNullOrWhiteSpace(monTransa))
                                    {
                                        ErrorListMessages.Add($"La moneda de la transacción está vacia - En la hoja {sheetName} fila:{firstRowNumber}. |");
                                    }
                                    else
                                    {
                                        int currencyTypeTransa = int.Parse(monTransa);

                                        if (Enum.IsDefined(typeof(CurrencyType), currencyTypeTransa))
                                        {
                                            header.CurrencyTransaId = objCurrencyList
                                                .First(x => x.Numeral == currencyTypeTransa).Id;

                                            header.CurrencyTransaType = (CurrencyType)currencyTypeTransa;
                                        }
                                        else
                                        {
                                            ErrorListMessages.Add($"La moneda de la transacción es invalida - En la hoja {sheetName} fila:{firstRowNumber}. |");
                                        }
                                    }

                                    if (objQuotationType.Numeral == (int)SD.QuotationType.Buy)
                                    {
                                        var monTransfer = worksheet.Cell(4, 8).GetString().Trim();
                                        if (string.IsNullOrWhiteSpace(monTransfer))
                                        {
                                            ErrorListMessages.Add($"La moneda de la transferencia está vacia - En la hoja {sheetName} fila:{firstRowNumber}. |");
                                        }
                                        else
                                        {
                                            int currencyTypeTransfer = int.Parse(monTransfer);

                                            if (Enum.IsDefined(typeof(CurrencyType), currencyTypeTransfer))
                                            {
                                                header.CurrencyTransferId = objCurrencyList
                                                    .First(x => x.Numeral == currencyTypeTransfer).Id;

                                                header.CurrencyTransferType = (CurrencyType)currencyTypeTransfer;
                                                header.CurrencyDepositType = header.CurrencyTransaType;
                                                header.CurrencyDepositId = header.CurrencyTransaId;
                                            }
                                            else
                                            {
                                                ErrorListMessages.Add($"La moneda de la transferencia es invalida - En la hoja {sheetName} fila:{firstRowNumber}. |");
                                            }
                                        }

                                        var exchangeBuy = worksheet.Cell(4, 10).GetString().Trim();
                                        if (string.IsNullOrWhiteSpace(exchangeBuy))
                                        {
                                            ErrorListMessages.Add($"El tipo de cambio de compra está vacio - En la hoja {sheetName} fila:{firstRowNumber}. |");
                                        }
                                        else
                                        {
                                            header.ExchangeRateBuyTransa = decimal.Parse(exchangeBuy);
                                            if (header.ExchangeRateBuyTransa <= 0)
                                            {
                                                ErrorListMessages.Add($"El tipo de cambio de compra no puede ser menor o igual a cero - En la hoja {sheetName} fila:{firstRowNumber}. |");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var monDeposit = worksheet.Cell(4, 7).GetString().Trim();
                                        if (string.IsNullOrWhiteSpace(monDeposit))
                                        {
                                            ErrorListMessages.Add($"La moneda del deposito está vacia - En la hoja {sheetName} fila:{firstRowNumber}. |");
                                        }
                                        else
                                        {
                                            int currencyTypeDeposit = int.Parse(monDeposit);

                                            if (Enum.IsDefined(typeof(CurrencyType), currencyTypeDeposit))
                                            {
                                                header.CurrencyDepositId = objCurrencyList
                                                    .First(x => x.Numeral == currencyTypeDeposit).Id;

                                                header.CurrencyDepositType = (CurrencyType)currencyTypeDeposit;
                                                header.CurrencyTransferType = header.CurrencyTransaType;
                                                header.CurrencyTransferId = header.CurrencyTransaId;
                                            }
                                            else
                                            {
                                                ErrorListMessages.Add($"La moneda del deposito es invalido - En la hoja {sheetName} fila:{firstRowNumber}. |");
                                            }
                                        }

                                        var exchangeSell = worksheet.Cell(4, 11).GetString().Trim();
                                        if (string.IsNullOrWhiteSpace(exchangeSell))
                                        {
                                            ErrorListMessages.Add($"El tipo de cambio de venta está vacio - En la hoja {sheetName} fila:{firstRowNumber}. |");
                                        }
                                        else
                                        {
                                            header.ExchangeRateSellTransa = decimal.Parse(exchangeSell);
                                            if (header.ExchangeRateSellTransa <= 0)
                                            {
                                                ErrorListMessages.Add($"El típo de cambio de venta no puede ser menor o igual a cero - En la hoja {sheetName} fila:{firstRowNumber}. |");
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    var commission = worksheet.Cell(6, 5).GetString().Trim();
                                    if (string.IsNullOrWhiteSpace(commission))
                                    {
                                        ErrorListMessages.Add($"La comisión está vacia - En la hoja {sheetName} fila:{secondRowNumber}. |");
                                    }
                                    else
                                    {
                                        header.AmountCommission = decimal.Parse(commission);
                                        if (header.AmountCommission < 0)
                                        {
                                            ErrorListMessages.Add($"El monto no puede ser menor a cero - En la hoja {sheetName} fila:{secondRowNumber}. |");
                                        }
                                    }

                                    var bankAccountSource = worksheet.Cell(6, 6).GetString().Trim();
                                    if (string.IsNullOrWhiteSpace(bankAccountSource))
                                    {
                                        ErrorListMessages.Add($"La cuenta bancaria origen está vacia - En la hoja {sheetName} fila:{secondRowNumber}. |");
                                    }
                                    else
                                    {
                                        objBankAccountSource = _uow.BankAccount.Get(filter: x =>
                                            (x.CompanyId == _companyId && x.Code == bankAccountSource));

                                        if (objBankAccountSource == null)
                                        {
                                            ErrorListMessages.Add($"La cuenta bancaria origen no fue encontrada - En la hoja {sheetName} fila:{firstRowNumber}. |");
                                        }
                                        else
                                        {
                                            header.BankAccountSourceId = objBankAccountSource.Id;
                                        }
                                    }

                                    var bankAccountTarget = worksheet.Cell(6, 7).GetString().Trim();
                                    if (string.IsNullOrWhiteSpace(bankAccountTarget))
                                    {
                                        ErrorListMessages.Add($"La cuenta bancaria destino está vacia - En la hoja {sheetName} fila:{secondRowNumber}. |");
                                    }
                                    else
                                    {
                                        objBankAccountTarget = _uow.BankAccount.Get(filter: x =>
                                            (x.CompanyId == _companyId && x.Code == bankAccountTarget));

                                        if (objBankAccountTarget == null)
                                        {
                                            ErrorListMessages.Add($"La cuenta bancaria destino no fue encontrada - En la hoja {sheetName} fila:{firstRowNumber}. |");
                                        }
                                        else
                                        {
                                            header.BankAccountTargetId = objBankAccountTarget.Id;
                                        }
                                    }
                                }

                                var preserverNumber = worksheet.Cell(4, 12).GetString().Trim();
                                if (string.IsNullOrWhiteSpace(isVoid))
                                {
                                    ErrorListMessages.Add($"Preservar numeración está vacio - En la hoja {sheetName} fila:{firstRowNumber}. |");
                                }
                                else
                                {
                                    if (preserverNumber == "S")
                                    {
                                        header.Numeral = int.Parse(numeral);
                                    }
                                    else if (preserverNumber == "N")
                                    {
                                        header.Numeral = 0;
                                    }
                                    else
                                    {
                                        ErrorListMessages.Add($"Preservar numeración es invalido - En la hoja {sheetName} fila:{secondRowNumber}. |");
                                    }
                                }

                                objQuotationList.Add(header);

                                var firstRowUsed = worksheet.FirstRowUsed().RangeAddress.FirstAddress.RowNumber;
                                var lastUsedRow = worksheet.LastRowUsed().RangeAddress.FirstAddress.RowNumber;
                                for (int i = firstRowUsed + 7; i <= lastUsedRow; i++)
                                {
                                    var detail = new QuotationDetail();
                                    detail.CompanyId = _companyId;
                                    detail.ParentId = header.Id;
                                    detail.CurrencyDetailId = header.CurrencyTransaId;
                                    var bankSource = worksheet.Cell(i, 2).GetString().Trim();
                                    if (string.IsNullOrWhiteSpace(bankSource))
                                    {
                                        ErrorListMessages.Add($"El banco origen está vacio - En la hoja {sheetName} fila:{i}. |");
                                    }
                                    else
                                    {
                                        objBankSource = _uow.Bank.Get(filter: x =>
                                            (x.CompanyId == _companyId && x.Code == bankSource));

                                        if (objBankSource == null)
                                        {
                                            ErrorListMessages.Add($"El banco origen no fue encontrado - En la hoja {sheetName} fila:{firstRowNumber}. |");
                                        }
                                        else
                                        {
                                            detail.BankSourceId = objBankSource.Id;
                                        }
                                    }

                                    var bankTarget = worksheet.Cell(i, 4).GetString().Trim();
                                    if (!string.IsNullOrWhiteSpace(bankTarget))
                                    {
                                        objBankTarget = _uow.Bank.Get(filter: x =>
                                            (x.CompanyId == _companyId && x.Code == bankTarget));

                                        if (objBankTarget == null)
                                        {
                                            ErrorListMessages.Add($"El banco destino no fue encontrado - En la hoja {sheetName} fila:{firstRowNumber}. |");
                                        }
                                        else
                                        {
                                            detail.BankTargetId = objBankTarget.Id;
                                        }
                                    }

                                    var amountDetail = worksheet.Cell(i, 6).GetString().Trim();
                                    if (string.IsNullOrWhiteSpace(amountDetail))
                                    {
                                        ErrorListMessages.Add($"El importe está vacio - En la hoja {sheetName} fila:{i}. |");
                                    }
                                    else
                                    {
                                        detail.AmountDetail = decimal.Parse(amountDetail);
                                        if (detail.AmountDetail <= 0)
                                        {
                                            ErrorListMessages.Add($"El importe no puede ser menor o igual a cero - En la hoja {sheetName} fila:{i}. |");
                                        }
                                    }

                                    var typeDetail = worksheet.Cell(i, 7).GetString().Trim();
                                    if (string.IsNullOrWhiteSpace(typeDetail))
                                    {
                                        ErrorListMessages.Add($"El tipo de detalle está vacio - En la hoja {sheetName} fila:{firstRowNumber}. |");
                                    }
                                    else
                                    {
                                        short quotationDetailType = short.Parse(typeDetail);

                                        if (Enum.IsDefined(typeof(QuotationDetailType), quotationDetailType))
                                        {
                                            detail.QuotationDetailType = (QuotationDetailType)quotationDetailType;
                                        }
                                        else
                                        {
                                            ErrorListMessages.Add($"El tipo de detalle es invalido - En la hoja {sheetName} fila:{firstRowNumber}. |");
                                        }
                                    }

                                    var journalEntryId = worksheet.Cell(i, 8).GetString().Trim();
                                    var bankTransactionId = worksheet.Cell(i, 9).GetString().Trim();
                                    var journalEntryTransferFeeId = worksheet.Cell(i, 10).GetString().Trim();
                                    var bankTransactionTransferFeeId = worksheet.Cell(i, 11).GetString().Trim();

                                    detail.JournalEntryId = (journalEntryId != string.Empty && journalEntryId != null)
                                        ? Guid.Parse(journalEntryId)
                                        : null;

                                    detail.BankTransactionId = (bankTransactionId != string.Empty && bankTransactionId != null)
                                        ? Guid.Parse(bankTransactionId)
                                        : null;

                                    detail.JournalEntryTransferFeeId = (journalEntryTransferFeeId != string.Empty && journalEntryTransferFeeId != null)
                                        ? Guid.Parse(journalEntryTransferFeeId)
                                        : null;

                                    detail.BankTransactionTransferFeeId = (bankTransactionTransferFeeId != string.Empty && bankTransactionTransferFeeId != null)
                                        ? Guid.Parse(bankTransactionTransferFeeId)
                                        : null;

                                    objQuotationDetailList.Add(detail);
                                }

                            }
                        }
                    }
                }
            }

            if (ErrorListMessages.Count > 0)
            {
                foreach (var error in ErrorListMessages)
                {
                    errorsMessagesBuilder.Append(error);
                }

                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"{errorsMessagesBuilder}";
                return Json(jsonResponse);
            }

            foreach (var header in objQuotationList)
            {
                if (header.TypeNumeral != SD.QuotationType.Transfer)
                {
                    if (header.TypeNumeral == SD.QuotationType.Buy)
                    {
                        //TC COMPRA MENOR AL TC OFICIAL
                        if (header.ExchangeRateBuyTransa < header.ExchangeRateOfficialTransa)
                        {
                            header.AmountRevenue = (header.ExchangeRateOfficialTransa - header.ExchangeRateBuyTransa) * header.AmountTransaction;
                            header.AmountCost = 0;
                        }
                        //TC COMPRA MAYOR AL TC OFICIAL
                        else
                        {
                            header.AmountCost = (header.ExchangeRateBuyTransa - header.ExchangeRateOfficialTransa) * header.AmountTransaction;
                            header.AmountRevenue = 0;
                        }

                        //Compra de dolares 
                        if (header.CurrencyTransaType == SD.CurrencyType.Foreign)
                        {
                            //Factoring paga en Cordobas
                            if (header.CurrencyTransferType == SD.CurrencyType.Base)
                            {
                                header.AmountExchange = (header.AmountTransaction * header.ExchangeRateBuyTransa);
                                header.ExchangeRateBuyReal = header.ExchangeRateBuyTransa;
                            }
                        }
                        //Compra de Euros
                        else if (header.CurrencyTransaType == SD.CurrencyType.Additional)
                        {
                            //Factoring paga en Cordobas
                            if (header.CurrencyTransferType == SD.CurrencyType.Base)
                            {
                                header.AmountExchange = (header.AmountTransaction * header.ExchangeRateBuyTransa);
                                header.ExchangeRateBuyReal = header.ExchangeRateBuyTransa;

                            }
                            //Factoring paga en Dolares
                            else if (header.CurrencyTransferType == SD.CurrencyType.Foreign)
                            {
                                header.AmountExchange = (header.AmountTransaction * header.ExchangeRateBuyTransa);
                                header.ExchangeRateBuyReal = (header.ExchangeRateBuyTransa * header.ExchangeRateOfficialTransa);
                            }
                        }
                    }
                    else
                    {
                        //TC VENTA MENOR AL TC OFICIAL
                        if (header.ExchangeRateSellTransa < header.ExchangeRateOfficialTransa)
                        {
                            header.AmountCost = (header.ExchangeRateOfficialTransa - header.ExchangeRateSellTransa) * header.AmountTransaction;
                            header.AmountRevenue = 0;
                        }
                        //TC VENTA MAYOR AL TC OFICIAL
                        else
                        {
                            header.AmountRevenue = (header.ExchangeRateSellTransa - header.ExchangeRateOfficialTransa) * header.AmountTransaction;
                            header.AmountCost = 0;
                        }

                        //Venta de dolares 
                        if (header.CurrencyTransaType == SD.CurrencyType.Foreign)
                        {
                            //Cliente paga en Cordobas
                            if (header.CurrencyDepositType == SD.CurrencyType.Base)
                            {
                                header.AmountExchange = (header.AmountTransaction * header.ExchangeRateSellTransa);
                                header.ExchangeRateSellReal = header.ExchangeRateSellTransa;
                            }
                        }
                        //Venta de Euros
                        else if (header.CurrencyTransaType == SD.CurrencyType.Additional)
                        {
                            //Cliente paga en Cordobas
                            if (header.CurrencyDepositType == SD.CurrencyType.Base)
                            {
                                header.AmountExchange = (header.AmountTransaction * header.ExchangeRateSellTransa);
                                header.ExchangeRateSellReal = header.ExchangeRateSellTransa;

                            }
                            //Cliente paga en Dolares
                            else if (header.CurrencyDepositType == SD.CurrencyType.Foreign)
                            {
                                header.AmountExchange = (header.AmountTransaction * header.ExchangeRateSellTransa);
                                header.ExchangeRateSellReal = (header.ExchangeRateSellTransa * header.ExchangeRateOfficialTransa);
                            }
                        }
                    }
                }
                else
                {
                    header.CurrencyTransaType = objBankAccountSource.CurrencyType;
                    header.CurrencyDepositType = objBankAccountSource.CurrencyType;
                    header.CurrencyTransferType = objBankAccountSource.CurrencyType;
                    header.CurrencyTransaId = objBankAccountSource.CurrencyId;
                    header.CurrencyDepositId = objBankAccountSource.CurrencyId;
                    header.CurrencyTransferId = objBankAccountSource.CurrencyId;

                    objQuotationDetailList = objQuotationDetailList.Where(x => x.ParentId != header.Id).ToList();

                    var objDetailBankAccountTarget = new QuotationDetail()
                    {
                        ParentId = header.Id,
                        CompanyId = header.CompanyId,
                        QuotationDetailType = QuotationDetailType.CreditTransfer,
                        LineNumber = 1,
                        CurrencyDetailId = header.CurrencyTransaId,
                        BankSourceId = objBankAccountTarget.ParentId,
                        BankTargetId = objBankAccountTarget.ParentId,
                        AmountDetail = header.AmountTransaction,
                        CreatedBy = AC.LOCALHOSTME,
                        CreatedDate = DateTime.UtcNow,
                        CreatedHostName = AC.LOCALHOSTPC,
                        CreatedIpv4 = AC.Ipv4Default
                    };

                    objQuotationDetailList.Add(objDetailBankAccountTarget);

                    var objDetailBankAccountSource = new QuotationDetail()
                    {
                        ParentId = header.Id,
                        CompanyId = header.CompanyId,
                        QuotationDetailType = QuotationDetailType.DebitTransfer,
                        LineNumber = 1,
                        CurrencyDetailId = header.CurrencyTransaId,
                        BankTargetId = objBankAccountSource.ParentId,
                        BankSourceId = objBankAccountSource.ParentId,
                        AmountDetail = header.AmountTransaction,
                        CreatedBy = AC.LOCALHOSTME,
                        CreatedDate = DateTime.UtcNow,
                        CreatedHostName = AC.LOCALHOSTPC,
                        CreatedIpv4 = AC.Ipv4Default
                    };

                    objQuotationDetailList.Add(objDetailBankAccountSource);
                }

                header.BusinessExecutiveCode = objBusiness.Code;
                header.IsLoan = objBusiness.IsLoan;
                header.IsPayment = objBusiness.IsPayment;

                //Seteamos campos de auditoria
                header.CreatedBy = AC.LOCALHOSTME;
                header.CreatedDate = DateTime.UtcNow;
                header.CreatedHostName = AC.LOCALHOSTPC;
                header.CreatedIpv4 = AC.Ipv4Default;

                if (!header.IsClosed)
                {
                    if (header.Numeral == 0)
                    {
                        //Obtenemos el secuencial en borrador
                        var numberTransa = _uow.ConfigFac.NextSequentialNumber(filter: x => x.CompanyId == header.CompanyId,
                            SD.TypeSequential.Draft, true);

                        header.Numeral = Convert.ToInt32(numberTransa.Result.ToString());
                    }

                    header.InternalSerial = AC.InternalSerialDraft;
                    header.IsPosted = false;
                    header.IsClosed = false;
                    header.IsVoid = false;
                }
                else
                {
                    if (header.Numeral == 0)
                    {
                        var nextSeq = await _uow.Quotation.NextSequentialNumber(filter: x => x.CompanyId == header.CompanyId &&
                        x.TypeNumeral == header.TypeNumeral &&
                        x.DateTransa == header.DateTransa &&
                        x.InternalSerial == AC.InternalSerialOfficial);
                        if (nextSeq > countMaxSeq)
                        {
                            countMaxSeq = nextSeq;
                        }
                        else
                        {
                            countMaxSeq++;
                        }

                        header.Numeral = countMaxSeq;
                    }

                    header.InternalSerial = AC.InternalSerialOfficial;
                    header.ClosedBy = AC.LOCALHOSTME;
                    header.ClosedDate = DateTime.UtcNow;
                    header.ClosedHostName = AC.LOCALHOSTPC;
                    header.ClosedIpv4 = AC.Ipv4Default;
                }

                if (header.TypeNumeral != SD.QuotationType.Transfer)
                {
                    var childrens = objQuotationDetailList
                        .Where(x => x.ParentId == header.Id).ToList();

                    //Actualizamos los totales del padre
                    header.TotalDeposit = childrens
                        .Where(x => x.QuotationDetailType == QuotationDetailType.Deposit)
                        .Sum(x => x.AmountDetail);
                    header.TotalTransfer = childrens
                        .Where(x => x.QuotationDetailType == QuotationDetailType.Transfer)
                        .Sum(x => x.AmountDetail);


                    int lineNumberDeposit = 1, lineNumberTransfer = 1;

                    foreach (var detail in childrens)
                    {
                        if (detail.QuotationDetailType == QuotationDetailType.Deposit)
                        {
                            detail.LineNumber = lineNumberDeposit;
                            lineNumberDeposit++;
                        }
                        else if (detail.QuotationDetailType == QuotationDetailType.Transfer)
                        {
                            detail.LineNumber = lineNumberTransfer;
                            lineNumberTransfer++;
                        }
                    }
                }
            }


            await _uow.Quotation.ImportRangeAsync(objQuotationList, objQuotationDetailList);

            jsonResponse.SuccessMessages = "Importación exitosamente";
            jsonResponse.IsSuccess = true;
            jsonResponse.UrlRedirect = Url.Action(action: "Index", controller: "Quotation");
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
                numberReferenTarget += $"{Enum.GetName(typeof(SD.QuotationTypeNameAbrv), (int)transaction.TypeNumeral)}-{transaction.Numeral.ToString().PadLeft(3, AC.CharDefaultEmpty)}-{detail.Code}{transaction.DateTransa.Year}{transaction.DateTransa.Month.ToString().PadLeft(2, AC.CharDefaultEmpty)}{transaction.DateTransa.Day.ToString().PadLeft(2, AC.CharDefaultEmpty)}" + ", ";
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
            string isClosed = transaction.IsClosed ? "" : "No Cerrado";
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
