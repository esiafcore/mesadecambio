using System.Reflection;
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
using QuotationType = Xanes.Models.QuotationType;

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
        string processingDateString = HttpContext.Session.GetString(AC.ProcessingDate) ?? DateOnly.FromDateTime(DateTime.UtcNow).ToString();
        DateOnly dateFilter = DateOnly.Parse(processingDateString);
        ViewBag.DecimalTransa = JsonSerializer.Serialize(_decimalTransa);
        ViewBag.DecimalExchange = JsonSerializer.Serialize(_decimalExchange);
        ViewBag.ProcessingDate = JsonSerializer.Serialize(dateFilter.ToString(AC.DefaultDateFormatWeb));
        TransactionReportVM modelVM = new();

        return View(modelVM);
    }

    public IActionResult Upsert(int id = 0)
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

        if (objBusinessExecutiveList == null)
        {
            TempData[AC.Error] = $"Ejecutivo no encontrado";
            return RedirectToAction(nameof(Index));
        }

        if (id == 0)
        {
            objData = new Quotation
            {
                DateTransa = DateOnly.FromDateTime(DateTime.UtcNow),
                TypeNumeral = SD.QuotationType.Buy,
                CurrencyTransaType = SD.CurrencyType.Foreign,
                CurrencyTransferType = SD.CurrencyType.Base,
                CurrencyDepositType = SD.CurrencyType.Base,
                CompanyId = _companyId,
                BusinessExecutiveCode = new string(AC.CharDefaultEmpty, AC.RepeatCharTimes)
            };

        }
        else
        {
            objData = _uow.Quotation.Get(filter: x => (x.CompanyId == _companyId && x.Id == id));
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

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Models.ViewModels.QuotationCreateVM objViewModel)
    {
        var objBankAccountTarget = new BankAccount();
        var objBankAccountSource = new BankAccount();
        var objCurrency = new Currency();
        var objCurrencyList = new List<Currency>();
        var objTypeList = new List<QuotationType>();
        var objCustomerList = new List<Models.Customer>();
        objViewModel.DataModel.BusinessExecutiveCode = new string(AC.CharDefaultEmpty, AC.RepeatCharTimes);

        Models.Quotation obj = objViewModel.DataModel;
        //Datos son validos
        if (ModelState.IsValid)
        {
            if (obj.CompanyId != _companyId)
            {
                ModelState.AddModelError("CompanyId", $"Id de la compañía no puede ser distinto de {_companyId}");
            }

            if (obj.AmountTransaction == 0)
            {
                ModelState.AddModelError("AmountTransaction", "El monto no puede ser cero.");
            }

            //Verificamos si existe el tipo
            var objQuotationType = _uow.QuotationType.Get(filter: x =>
                x.CompanyId == obj.CompanyId && x.Numeral == (int)obj.TypeNumeral);

            if (objQuotationType == null)
            {
                ModelState.AddModelError("TypeNumeral", $"Tipo de transacción no encontrado");
            }
            else
            {
                obj.TypeId = objQuotationType.Id;

                if (objQuotationType.Numeral != (int)SD.QuotationType.Transfer)
                {
                    if (objQuotationType.Numeral == (int)SD.QuotationType.Buy)
                    {
                        obj.CurrencyDepositType = obj.CurrencyTransferType;

                        if (obj.ExchangeRateBuyTransa == 0)
                        {
                            ModelState.AddModelError("ExchangeRateBuyTransa", "El tipo de cambio de compra no puede ser cero.");
                        }
                    }
                    else
                    {
                        obj.CurrencyTransferType = obj.CurrencyDepositType;

                        if (obj.ExchangeRateSellTransa == 0)
                        {
                            ModelState.AddModelError("ExchangeRateSellTransa", "El tipo de cambio de venta no puede ser cero.");
                        }
                    }

                    //Verificamos si existe la moneda de la Transaccion
                    objCurrency = _uow.Currency.Get(filter: x =>
                       x.CompanyId == obj.CompanyId && x.Numeral == (int)obj.CurrencyTransaType);

                    if (objCurrency == null)
                    {
                        ModelState.AddModelError("CurrencyTransaType", $"Moneda de la transacción no encontrada");
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
                        ModelState.AddModelError("CurrencyDepositType", $"Moneda de deposito no encontrada");
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
                        ModelState.AddModelError("CurrencyTransferType", $"Moneda de transferencia no encontrada");
                    }
                    else
                    {
                        obj.CurrencyTransferId = objCurrency.Id;
                    }
                }
                else
                {
                    //Verificamos si existe la cuenta bancaria de origen
                    objBankAccountSource = _uow.BankAccount.Get(filter: x => x.CompanyId == obj.CompanyId && x.Id == obj.BankAccountSourceId, includeProperties: "ParentTrx");
                    if (objBankAccountSource == null)
                    {
                        ModelState.AddModelError("BankAccountSourceId", $"Cuenta bancaria origen invalida");
                    }

                    //Verificamos si existe la cuenta bancaria de destino
                    objBankAccountTarget = _uow.BankAccount.Get(filter: x => x.CompanyId == obj.CompanyId && x.Id == obj.BankAccountTargetId, includeProperties: "ParentTrx");
                    if (objBankAccountTarget == null)
                    {
                        ModelState.AddModelError("BankAccountTargetId", $"Cuenta bancaria destino invalida");
                    }
                }
            }

            //Verificamos si existe la moneda de la Transaccion
            var objBusiness = _uow.BusinessExecutive.Get(filter: x =>
                x.CompanyId == obj.CompanyId && x.Id == obj.BusinessExecutiveId);

            if (objBusiness == null)
            {
                ModelState.AddModelError("BusinessExecutiveId", $"Ejecutivo no encontrado");
            }
            else
            {
                obj.BusinessExecutiveCode = objBusiness.Code;
            }

            //Verificamos si existe el cliente
            var objCustomer = _uow.Customer.Get(filter: x => x.CompanyId == obj.CompanyId && x.Id == obj.CustomerId);
            if (objCustomer == null)
            {
                ModelState.AddModelError("CustomerId", $"Cliente no encontrado");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.DecimalTransa = JsonSerializer.Serialize(_decimalTransa);
                ViewBag.DecimalExchange = JsonSerializer.Serialize(_decimalExchange);

                objCurrencyList = _uow.Currency
                   .GetAll(x => (x.CompanyId == _companyId))
                   .ToList();

                if (objCurrencyList == null)
                {
                    return NotFound();
                }

                objTypeList = _uow.QuotationType
                   .GetAll(x => (x.CompanyId == _companyId))
                   .ToList();

                if (objTypeList == null)
                {
                    return NotFound();
                }

                objCustomerList = _uow.Customer
                   .GetAll(x => (x.CompanyId == _companyId))
                   .ToList();

                if (objCustomerList == null)
                {
                    return NotFound();
                }

                var objBankAccountList = _uow.BankAccount
                    .GetAll(x => (x.CompanyId == _companyId))
                    .ToList();

                if (objBankAccountList == null)
                {
                    return NotFound();
                }

                objViewModel.BankAccountSourceList = objBankAccountList.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
                objViewModel.BankAccountTargetList = new List<SelectListItem>();
                objViewModel.CurrencyTransaList = objCurrencyList
                    .Where(x => (x.IsActive && (x.Numeral != (int)SD.CurrencyType.Base)))
                    .ToList();
                objViewModel.CurrencyDepositList = objCurrencyList.Where(x => x.IsActive).ToList();
                objViewModel.CurrencyTransferList = objCurrencyList.Where(x => x.IsActive).ToList();
                objViewModel.QuotationTypeList = objTypeList;
                objViewModel.CustomerList = objCustomerList.Select(x => new SelectListItem { Text = x.CommercialName, Value = x.Id.ToString() });
                return View(objViewModel);
            }

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
                        obj.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateBuyTransa);
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
            else
            {
                if (objBankAccountSource != null)
                {
                    obj.CurrencyTransaType = objBankAccountSource.CurrencyType;
                    obj.CurrencyDepositType = objBankAccountSource.CurrencyType;
                    obj.CurrencyTransferType = objBankAccountSource.CurrencyType;
                    obj.CurrencyTransaId = objBankAccountSource.CurrencyId;
                    obj.CurrencyDepositId = objBankAccountSource.CurrencyId;
                    obj.CurrencyTransferId = objBankAccountSource.CurrencyId;
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

        objCurrencyList = _uow.Currency
           .GetAll(x => (x.CompanyId == _companyId))
           .ToList();

        if (objCurrencyList == null)
        {
            return NotFound();
        }

        objTypeList = _uow.QuotationType
           .GetAll(x => (x.CompanyId == _companyId))
           .ToList();

        if (objTypeList == null)
        {
            return NotFound();
        }

        objCustomerList = _uow.Customer
            .GetAll(x => (x.CompanyId == _companyId))
            .ToList();

        if (objCustomerList == null)
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
                    obj.CurrencyDepositType = obj.CurrencyTransferType;

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
                    obj.CurrencyDepositId = objCurrency.Id;

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
                    obj.CurrencyTransferType = obj.CurrencyDepositType;

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

                    obj.CurrencyTransferId = objCurrency.Id;
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

            //Verificamos si existe la moneda de la Transaccion
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
        QuotationDetailVM model = new();
        ViewBag.DecimalTransa = JsonSerializer.Serialize(_decimalTransa);
        ViewBag.DecimalExchange = JsonSerializer.Serialize(_decimalExchange);

        var objHeader = _uow.Quotation.Get(filter: x => x.CompanyId == _companyId && x.Id == id,
            includeProperties: "TypeTrx,CustomerTrx,CurrencyTransferTrx,CurrencyTransaTrx,BankAccountSourceTrx,BankAccountTargetTrx", isTracking: false);
        if (objHeader == null)
        {
            return NotFound();
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

            //Obtenemos los hijos
            var objDetails = _uow.QuotationDetail.GetAll(filter: x =>
                     x.CompanyId == obj.CompanyId && x.ParentId == objHeader.Id, includeProperties: "ParentTrx,CurrencyDetailTrx,BankSourceTrx,BankTargetTrx").ToList();

            if (objDetails == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Detalle de cotización no encontrado";
                return Json(jsonResponse);
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
                var objDetail = objDetails.First(x => x.Id == obj.Id);
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

            //Actualizamos los totales del padre
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
        //Verificar si ya hay una sesion guardada anteriormente
        string processingDateString = HttpContext.Session.GetString(AC.ProcessingDate) ?? string.Empty;
        DateOnly processingDate = DateOnly.FromDateTime(DateTime.UtcNow);
        if (processingDateString.Trim() != string.Empty)
        {
            processingDate = DateOnly.Parse(processingDateString);
        }

        model.ProcessingDate = processingDate;
        return View(model);
    }

    [HttpPost]
    public JsonResult ProcessingDate(string processingDate)
    {
        JsonResultResponse? jsonResponse = new();
        DateOnly dateTransa = DateOnly.Parse(processingDate);

        HttpContext.Session.SetString(AC.ProcessingDate, dateTransa.ToString());

        jsonResponse.IsSuccess = true;
        TempData[AC.Success] = $"Fecha de Procesamiento guardada correctamente";
        jsonResponse.UrlRedirect = Url.Action(action: "Index", controller: "Quotation");
        return Json(jsonResponse);
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
