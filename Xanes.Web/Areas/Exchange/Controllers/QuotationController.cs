using Microsoft.AspNetCore.Mvc;
using Xanes.DataAccess.Repository.IRepository;
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
using static Xanes.Utility.Enumeradores;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.IO.Compression;
using System.Security.Claims;
using AutoMapper;
using Stimulsoft.Report.Export;
using Xanes.DataAccess.ServicesApi.Interface.eSiafN4;
using Xanes.LoggerService;
using Xanes.Models.Dtos.eSiafN4;
using Xanes.Models.Dtos.XanesN8;

namespace Xanes.Web.Areas.Exchange.Controllers;

#pragma warning disable CS8604

[Area("Exchange")]
[Authorize()]

public class QuotationController : Controller
{
    private readonly IUnitOfWork _uow;
    private readonly IConfiguration _cfg;
    private readonly ITransaccionBcoService _srvTransaBco;
    private readonly ITransaccionBcoDetalleService _srvTransaBcoDetalle;
    private readonly IAsientoContableService _srvAsiento;
    private readonly IAsientoContableDetalleService _srvAsientoDetalle;
    private readonly ICuentaBancariaService _srvCuentaBancaria;
    private readonly IBancoService _srvBanco;
    private readonly IModuloService _srvModulo;
    private readonly IModuloDocumentoService _srvModuloDocumento;
    private readonly IConfigBcoService _srvConfigBco;
    private readonly IConfigCntService _srvConfigCnt;
    private readonly IMapper _mapper;
    private readonly string _eSiafN4BeneficiaryUid;
    private readonly string _eSiafN4EntityUid;
    private readonly string _eSiafN4CustomerName;
    private readonly string _eSiafN4CustomerIdentificationNumber;
    private readonly int _companyId;
    private readonly int _decimalTransa;
    private readonly int _decimalExchange;
    private readonly int _decimalExchangeFull;
    private readonly decimal _variationMaxDeposit;
    private readonly int _limitBatchCreditNote;
    private Dictionary<ParametersReport, object?> _parametersReport;
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ILoggerManager _logger;
    private readonly string? _userName;
    private readonly System.Net.IPAddress? _ipAddress;
    private readonly string _sessionToken;

    public QuotationController(
        IUnitOfWork uow,
        IConfiguration configuration,
        IWebHostEnvironment hostEnvironment,
        IHttpContextAccessor contextAccessor,
        ILoggerManager logger,
        ITransaccionBcoService srvTransaBco,
        ITransaccionBcoDetalleService srvTransaBcoDetalle,
        ICuentaBancariaService srvCuentaBancaria,
        IBancoService srvBanco, IConfigBcoService srvConfigBco,
        IAsientoContableService srvAsiento,
        IAsientoContableDetalleService srvAsientoDetalle,
        IConfigCntService srvConfigCnt,
        IModuloService srvModulo, IModuloDocumentoService srvModuloDocumento, IMapper mapper)
    {
        _uow = uow;
        _cfg = configuration;
        _companyId = _cfg.GetValue<int>("ApplicationSettings:CompanyId");
        _decimalTransa = _cfg.GetValue<int>("ApplicationSettings:DecimalTransa");
        _decimalExchange = _cfg.GetValue<int>("ApplicationSettings:DecimalExchange");
        _decimalExchangeFull = _cfg.GetValue<int>("ApplicationSettings:DecimalExchangeFull");
        _variationMaxDeposit = _cfg.GetValue<decimal>("ApplicationSettings:VariationMaxDeposit");
        _limitBatchCreditNote = _cfg.GetValue<int>("ApplicationSettings:LimitBatchCreditNote");
        _hostEnvironment = hostEnvironment;
        _parametersReport = new();

        _eSiafN4BeneficiaryUid = _cfg.GetValue<string>(AC.SecreteSiafN4BeneficiaryUid) ?? string.Empty;
        _eSiafN4EntityUid = _cfg.GetValue<string>(AC.SecreteSiafN4EntityUid) ?? string.Empty;
        _eSiafN4CustomerName = _cfg.GetValue<string>(AC.SecreteSiafN4CustomerName) ?? string.Empty;
        _eSiafN4CustomerIdentificationNumber = _cfg.GetValue<string>(AC.SecreteSiafN4CustomerIdentificationNumber) ?? string.Empty;

        var path = Path.Combine(hostEnvironment.ContentRootPath, "License\\license.key");
        FileInfo file = new FileInfo(path);
        if (file.Exists)
        {
            Stimulsoft.Base.StiLicense.LoadFromFile(path);
        }

        _contextAccessor = contextAccessor;
        _logger = logger;
        _srvTransaBco = srvTransaBco;
        _srvTransaBcoDetalle = srvTransaBcoDetalle;
        _srvCuentaBancaria = srvCuentaBancaria;
        _srvBanco = srvBanco;
        _srvConfigBco = srvConfigBco;
        _srvAsiento = srvAsiento;
        _srvAsientoDetalle = srvAsientoDetalle;
        _srvConfigCnt = srvConfigCnt;
        _srvModulo = srvModulo;
        _srvModuloDocumento = srvModuloDocumento;
        _mapper = mapper;
        _userName = _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
        _ipAddress = _contextAccessor.HttpContext?.Connection.RemoteIpAddress?.MapToIPv4();
        _sessionToken = _contextAccessor.HttpContext?.Session.GetString(SD.SessionToken) ?? string.Empty;
    }

    public IActionResult Index()
    {
        string? processingDateString = _contextAccessor.HttpContext?.Session.GetString(AC.ProcessingDate);
        string? changeProcessingDateString = _contextAccessor.HttpContext?.Session.GetString(AC.ChangeProcessingDate);

        if (processingDateString is null)
        {
            processingDateString = DateOnly.FromDateTime(DateTime.Now).ToString();
        }

        if (changeProcessingDateString is not null)
        {
            ViewBag.ChangeProcessingDate = JsonConvert.SerializeObject(true);
            HttpContext.Session.Remove(AC.ChangeProcessingDate);
        }
        else
        {
            ViewBag.ChangeProcessingDate = JsonConvert.SerializeObject(false);
        }

        DateOnly dateFilter = DateOnly.Parse(processingDateString);
        ViewBag.DecimalTransa = JsonConvert.SerializeObject(_decimalTransa);
        ViewBag.DecimalExchange = JsonConvert.SerializeObject(_decimalExchange);
        ViewBag.ProcessingDate = JsonConvert.SerializeObject(dateFilter.ToString(AC.DefaultDateFormatWeb));
        ViewBag.IsNewEntry = JsonConvert.SerializeObject(true);
        ViewBag.LimitBatchCreditNote = JsonConvert.SerializeObject(_limitBatchCreditNote);
        TransactionReportVM modelVM = new();
        ViewData[AC.Title] = "Listado de Transacciones";
        return View(modelVM);
    }

    public IActionResult Upsert(int id = 0)
    {

        try
        {
            ViewBag.DecimalTransa = JsonConvert.SerializeObject(_decimalTransa);
            ViewBag.DecimalExchange = JsonConvert.SerializeObject(_decimalExchange);
            string? processingDateString = _contextAccessor.HttpContext?.Session.GetString(AC.ProcessingDate);
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
                .GetAll(x => (x.CompanyId == _companyId)
                && (x.IsActive))
                .ToList();

            if (objBusinessExecutiveList == null || objBusinessExecutiveList.Count == 0)
            {
                TempData[AC.Error] = $"Ejecutivo no encontrado";
                return RedirectToAction(nameof(Index));
            }

            if (id == 0)
            {
                //Ejecutivo por defecto
                ViewBag.BusinessExecutiveIdByDefault = JsonConvert.SerializeObject(objBusinessExecutiveList.FirstOrDefault(x => x.IsDefault)!.Id);

                ViewData[AC.Title] = "Crear - Cotización";

                objData = new Quotation
                {
                    DateTransa = processingDateString != null ? model.ProcessingDate : DateOnly.FromDateTime(DateTime.Now),
                    TypeNumeral = SD.QuotationType.Buy,
                    CurrencyTransaType = CurrencyType.Foreign,
                    CurrencyTransferType = CurrencyType.Base,
                    CurrencyDepositType = CurrencyType.Base,
                    CompanyId = _companyId,
                    BusinessExecutiveCode = new string(AC.CharDefaultEmpty, AC.RepeatCharTimes),
                    CustomerId = 0,
                    CreatedBy = _userName ?? AC.LOCALHOSTME,
                    CreatedDate = DateTime.UtcNow,
                    CreatedHostName = AC.LOCALHOSTPC,
                    CreatedIpv4 = _ipAddress?.ToString() ?? AC.Ipv4Default
                };

            }
            else
            {
                ViewData[AC.Title] = "Actualizar - Cotización";

                objData = _uow.Quotation
                    .Get(filter: x =>
                        (x.CompanyId == _companyId &&
                         x.Id == id),
                        includeProperties: "CustomerTrx");

                if (objData == null)
                {
                    TempData[AC.Error] = $"Cotización no encontrada";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.BusinessExecutiveIdByDefault = JsonConvert.SerializeObject(objData.BusinessExecutiveId);
            }

            model.CurrencyTransaList = objCurrencyList
                .Where(x => (x.IsActive && (x.Numeral != (int)CurrencyType.Base)))
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

                objCustomer.BusinessExecutiveId =
                    (objCustomer.BusinessExecutiveId is null ? 0 : objCustomer.BusinessExecutiveId);

                listCustomer.Add(objCustomer);
                //model.CustomerList = listCustomer.Select(x => new SelectListItem { Text = x.BusinessName, Value = x.Id.ToString() });
                model.CustomerList = listCustomer;
            }

            return View(model);
        }
        catch (Exception ex)
        {
            TempData[AC.Error] = ex.Message;
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public JsonResult Upsert([FromForm] Quotation obj, bool redirectHome = false, bool redirectDetail = false, bool showMessages = false)
    {
        StringBuilder errorsMessagesBuilder = new();
        JsonResultResponse? jsonResponse = new();
        var objBankAccountTarget = new BankAccount();
        var objBankAccountSource = new BankAccount();
        var objCurrencyTransa = new Currency();
        var objCurrencyTransfer = new Currency();
        var objCurrencyDeposit = new Currency();

        var objBusinessExecutive = new BusinessExecutive();
        try
        {
            if (!ModelState.IsValid)
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

            //Obtener reg del tipo de operacion de MC
            var objQuotationType = _uow.QuotationType.Get(filter: x =>
                x.CompanyId == obj.CompanyId && x.Numeral == (int)obj.TypeNumeral);

            if (objQuotationType == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Tipo de transacción no encontrado";
                return Json(jsonResponse);
            }

            //ToDo: Se traee el tipo de cambio sin especificar la moneda
            var objCurrencyRateList = _uow.CurrencyExchangeRate.GetAll
            (x => (x.CompanyId == _companyId) && (x.DateTransa == obj.DateTransa)
                , includeProperties: "CurrencyTrx").ToList();

            if (objCurrencyRateList is null || objCurrencyRateList.Count == 0)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Tipo de cambio no encontrado";
                return Json(jsonResponse);
            }

            //Obtener valor oficial del tipo de cambio foraneo
            var currencyForeignValue = objCurrencyRateList
                .FirstOrDefault(t => (t.CurrencyType == CurrencyType.Foreign))
                    ?.OfficialRate
                    ?? AC.ExchangeRateDefaultValue;

            obj.ExchangeRateOfficialBase = currencyForeignValue;

            //Setear Id tipo de operación de mesa de cambio
            obj.TypeId = objQuotationType.Id;

            //Operación de Mesa de Cambio es distinta de Traslado
            if (objQuotationType.Numeral != (int)SD.QuotationType.Transport)
            {
                //Verificamos si existe la moneda de la Transaccion
                objCurrencyTransa = _uow.Currency.Get(filter: x =>
                    (x.CompanyId == obj.CompanyId) && (x.Numeral == (int)obj.CurrencyTransaType));

                if (objCurrencyTransa == null)
                {
                    jsonResponse.IsSuccess = false;
                    jsonResponse.ErrorMessages = $"Moneda de la transacción no encontrada";
                    return Json(jsonResponse);
                }

                //Setear Id de la moneda de la transacción mesa de cambio
                obj.CurrencyTransaId = objCurrencyTransa.Id;

                //Operación de Mesa de Cambio es COMPRA
                if (objQuotationType.Numeral == (int)SD.QuotationType.Buy)
                {
                    //Cuando se compra U$ ó €, la moneda de la transacción es la misma moneda en el depósito
                    obj.CurrencyDepositType = (CurrencyType)objCurrencyTransa.Numeral;
                    obj.CurrencyDepositId = objCurrencyTransa.Id;

                    //Tipo de cambio de la compra debe ser distinto de cero
                    if (obj.ExchangeRateBuyTransa == 0)
                    {
                        jsonResponse.IsSuccess = false;
                        jsonResponse.ErrorMessages = $"El tipo de cambio de compra no puede ser cero.";
                        return Json(jsonResponse);
                    }

                    //Verificamos si existe la moneda de transferencia
                    //Si COM U$ => Transferencia C$
                    //Si COM € => Transferencia U$ ó C$
                    objCurrencyTransfer = _uow.Currency.Get(filter: x =>
                        (x.CompanyId == obj.CompanyId) && (x.Numeral == (int)obj.CurrencyTransferType));

                    if (objCurrencyTransfer == null)
                    {
                        jsonResponse.IsSuccess = false;
                        jsonResponse.ErrorMessages = $"Moneda de transferencia no encontrada";
                        return Json(jsonResponse);
                    }

                    obj.CurrencyTransferId = objCurrencyTransfer.Id;

                    //TC COMPRA MENOR AL TC OFICIAL
                    obj.ExchangeRateOfficialReal = obj.ExchangeRateOfficialTransa;
                    if (obj.ExchangeRateBuyTransa < obj.ExchangeRateOfficialTransa)
                    {
                        obj.AmountRevenue = (obj.ExchangeRateOfficialTransa - obj.ExchangeRateBuyTransa) * obj.AmountTransaction;
                        obj.AmountRevenue = obj.AmountRevenue.RoundTo(AC.DecimalTransa);
                        obj.AmountCost = 0m;
                    }
                    //TC COMPRA MAYOR AL TC OFICIAL
                    else
                    {
                        obj.AmountCost = (obj.ExchangeRateBuyTransa - obj.ExchangeRateOfficialTransa) * obj.AmountTransaction;
                        obj.AmountCost = obj.AmountCost.RoundTo(AC.DecimalTransa);
                        obj.AmountRevenue = 0m;
                    }

                    //Compra de dolares 
                    if (obj.CurrencyTransaType == CurrencyType.Foreign)
                    {
                        //Factoring paga en Cordobas
                        //Si COM U$ => Transferencia C$
                        if (obj.CurrencyTransferType == CurrencyType.Base)
                        {
                            obj.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateBuyTransa);
                            obj.ExchangeRateBuyReal = obj.ExchangeRateBuyTransa;

                            obj.AmountCostReal = obj.AmountCost;
                            obj.AmountRevenueReal = obj.AmountRevenue;
                            obj.AmountCostReal = obj.AmountCostReal.RoundTo(AC.DecimalTransa);
                            obj.AmountRevenueReal = obj.AmountRevenueReal.RoundTo(AC.DecimalTransa);
                        }
                    }
                    //Compra de Euros
                    else if (obj.CurrencyTransaType == CurrencyType.Additional)
                    {
                        //Factoring paga en Cordobas
                        //Si COM € => Transferencia C$
                        if (obj.CurrencyTransferType == CurrencyType.Base)
                        {
                            obj.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateBuyTransa);
                            obj.AmountExchange = obj.AmountExchange.RoundTo(AC.DecimalTransa);
                            obj.ExchangeRateBuyReal = obj.ExchangeRateBuyTransa;

                            obj.AmountCostReal = obj.AmountCost;
                            obj.AmountRevenueReal = obj.AmountRevenue;
                            obj.AmountCostReal = obj.AmountCostReal.RoundTo(AC.DecimalTransa);
                            obj.AmountRevenueReal = obj.AmountRevenueReal.RoundTo(AC.DecimalTransa);
                        }
                        //Factoring paga en Dolares
                        //Si COM € => Transferencia U$  
                        else if (obj.CurrencyTransferType == CurrencyType.Foreign)
                        {
                            obj.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateBuyTransa);
                            obj.AmountExchange = obj.AmountExchange.RoundTo(AC.DecimalTransa);
                            obj.ExchangeRateBuyReal = (obj.ExchangeRateBuyTransa * obj.ExchangeRateOfficialTransa);

                            obj.AmountCostReal = obj.AmountCost * currencyForeignValue;
                            obj.AmountRevenueReal = obj.AmountRevenue * currencyForeignValue;

                            obj.ExchangeRateOfficialReal = currencyForeignValue * obj.ExchangeRateOfficialTransa;
                            obj.ExchangeRateBuyReal = currencyForeignValue * obj.ExchangeRateBuyTransa;

                            obj.AmountCostReal = obj.AmountCostReal.RoundTo(AC.DecimalTransa);
                            obj.AmountRevenueReal = obj.AmountRevenueReal.RoundTo(AC.DecimalTransa);
                        }
                    }
                }
                //Operación de Mesa de Cambio es VENTA
                else
                {
                    //Cuando se vende U$ ó €, la moneda de la transacción es la misma moneda de la transferencia

                    obj.CurrencyTransferType = (CurrencyType)objCurrencyTransa.Numeral;
                    obj.CurrencyTransferId = objCurrencyTransa.Id;

                    if (obj.ExchangeRateSellTransa == 0)
                    {
                        jsonResponse.IsSuccess = false;
                        jsonResponse.ErrorMessages = $"El tipo de cambio de venta no puede ser cero.";
                        return Json(jsonResponse);
                    }

                    //Verificamos si existe la moneda de deposito
                    //Si VTA U$ => Depósito C$
                    //Si VTA € => Depósito € ó C$ 

                    objCurrencyDeposit = _uow.Currency.Get(filter: x =>
                        (x.CompanyId == obj.CompanyId) && (x.Numeral == (int)obj.CurrencyDepositType));

                    if (objCurrencyDeposit == null)
                    {
                        jsonResponse.IsSuccess = false;
                        jsonResponse.ErrorMessages = $"Moneda de deposito no encontrada";
                        return Json(jsonResponse);
                    }

                    obj.CurrencyDepositId = objCurrencyDeposit.Id;
                    obj.ExchangeRateOfficialReal = obj.ExchangeRateOfficialTransa;
                    //TC VENTA MENOR AL TC OFICIAL
                    if (obj.ExchangeRateSellTransa < obj.ExchangeRateOfficialTransa)
                    {
                        obj.AmountCost = (obj.ExchangeRateOfficialTransa - obj.ExchangeRateSellTransa) * obj.AmountTransaction;
                        obj.AmountCost = obj.AmountCost.RoundTo(AC.DecimalTransa);
                        obj.AmountRevenue = 0m;
                    }
                    //TC VENTA MAYOR AL TC OFICIAL
                    else
                    {
                        obj.AmountRevenue = (obj.ExchangeRateSellTransa - obj.ExchangeRateOfficialTransa) * obj.AmountTransaction;
                        obj.AmountRevenue = obj.AmountRevenue.RoundTo(AC.DecimalTransa);
                        obj.AmountCost = 0;
                    }

                    //Venta de dolares 
                    if (obj.CurrencyTransaType == CurrencyType.Foreign)
                    {
                        //Cliente paga en Cordobas
                        if (obj.CurrencyDepositType == CurrencyType.Base)
                        {
                            obj.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateSellTransa);
                            obj.AmountExchange = obj.AmountExchange.RoundTo(AC.DecimalTransa);
                            obj.ExchangeRateSellReal = obj.ExchangeRateSellTransa;
                            obj.AmountCostReal = obj.AmountCost;
                            obj.AmountRevenueReal = obj.AmountRevenue;
                            obj.AmountCostReal = obj.AmountCostReal.RoundTo(AC.DecimalTransa);
                            obj.AmountRevenueReal = obj.AmountRevenueReal.RoundTo(AC.DecimalTransa);
                        }
                    }
                    //Venta de Euros
                    else if (obj.CurrencyTransaType == CurrencyType.Additional)
                    {
                        //Cliente paga en Cordobas
                        if (obj.CurrencyDepositType == CurrencyType.Base)
                        {
                            obj.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateSellTransa);
                            obj.AmountExchange = obj.AmountExchange.RoundTo(AC.DecimalTransa);
                            obj.ExchangeRateSellReal = obj.ExchangeRateSellTransa;
                            obj.AmountCostReal = obj.AmountCost;
                            obj.AmountRevenueReal = obj.AmountRevenue;
                            obj.AmountCostReal = obj.AmountCostReal.RoundTo(AC.DecimalTransa);
                            obj.AmountRevenueReal = obj.AmountRevenueReal.RoundTo(AC.DecimalTransa);
                        }
                        //Cliente paga en Dolares
                        else if (obj.CurrencyDepositType == CurrencyType.Foreign)
                        {
                            obj.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateSellTransa);
                            obj.AmountExchange = obj.AmountExchange.RoundTo(AC.DecimalTransa);
                            obj.ExchangeRateSellReal = (obj.ExchangeRateSellTransa * obj.ExchangeRateOfficialTransa);
                            obj.AmountCostReal = obj.AmountCost * currencyForeignValue;
                            obj.AmountRevenueReal = obj.AmountRevenue * currencyForeignValue;

                            obj.ExchangeRateOfficialReal = currencyForeignValue * obj.ExchangeRateOfficialTransa;
                            obj.ExchangeRateSellReal = currencyForeignValue * obj.ExchangeRateSellTransa;
                            obj.AmountCostReal = obj.AmountCostReal.RoundTo(AC.DecimalTransa);
                            obj.AmountRevenueReal = obj.AmountRevenueReal.RoundTo(AC.DecimalTransa);
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
                obj.ExchangeRateOfficialReal = obj.ExchangeRateOfficialTransa;
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

            if (obj.IsLoan || obj.IsPayment)
            {
                if (obj.TypeNumeral == SD.QuotationType.Sell)
                {
                    obj.TotalDeposit = obj.AmountTransaction * obj.ExchangeRateSellTransa;
                    obj.TotalDeposit = obj.TotalDeposit.RoundTo(AC.DecimalTransa);
                    obj.TotalTransfer = obj.AmountTransaction;
                }
                else if (obj.TypeNumeral == SD.QuotationType.Buy)
                {
                    obj.TotalTransfer = obj.AmountTransaction * obj.ExchangeRateBuyTransa;
                    obj.TotalTransfer = obj.TotalTransfer.RoundTo(AC.DecimalTransa);
                    obj.TotalDeposit = obj.AmountTransaction;
                }
            }

            //Verificamos si existe el cliente
            var objCustomer = _uow.Customer.Get(filter: x => (x.CompanyId == obj.CompanyId) && (x.Id == obj.CustomerId));
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
                obj.CreatedBy = _userName ?? AC.LOCALHOSTME;
                obj.CreatedDate = DateTime.UtcNow;
                obj.CreatedHostName = AC.LOCALHOSTPC;
                obj.CreatedIpv4 = _ipAddress?.ToString() ?? AC.Ipv4Default;
                obj.IsPosted = false;
                obj.IsClosed = false;
                obj.TotalLines = 0;
                obj.TotalDepositLines = 0;
                obj.TotalTransferLines = 0;
                obj.IsBank = objCustomer.IsBank;
                _uow.Quotation.Add(obj);
                _uow.Save();
                if (showMessages)
                {
                    TempData["success"] = "Cotización creada exitosamente";
                }

                if (obj.TypeNumeral == SD.QuotationType.Transport)
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
                            CreatedBy = _userName ?? AC.LOCALHOSTME,
                            CreatedDate = DateTime.UtcNow,
                            CreatedHostName = AC.LOCALHOSTPC,
                            CreatedIpv4 = _ipAddress?.ToString() ?? AC.Ipv4Default
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
                            CreatedBy = _userName ?? AC.LOCALHOSTME,
                            CreatedDate = DateTime.UtcNow,
                            CreatedHostName = AC.LOCALHOSTPC,
                            CreatedIpv4 = _ipAddress?.ToString() ?? AC.Ipv4Default
                        };
                        _uow.QuotationDetail.Add(objDetailBankAccountTarget);
                    }

                    obj.TotalLines = 2;
                    obj.TotalDepositLines = 1;
                    obj.TotalTransferLines = 1;
                    _uow.Quotation.Update(obj);
                    _uow.Save();
                }
            }
            //Actualizar
            else
            {
                //Seteamos campos de auditoria
                obj.UpdatedBy = _userName ?? AC.LOCALHOSTME;
                obj.UpdatedDate = DateTime.UtcNow;
                obj.UpdatedHostName = AC.LOCALHOSTPC;
                obj.UpdatedIpv4 = _ipAddress?.ToString() ?? AC.Ipv4Default;
                obj.IsBank = objCustomer.IsBank;
                _uow.Quotation.Update(obj);
                _uow.Save();
                if (showMessages)
                    TempData["success"] = "Cotización actualizada exitosamente";

                if (obj.TypeNumeral == SD.QuotationType.Transport)
                {
                    var objDetails = _uow.QuotationDetail
                        .GetAll(filter: x => (x.CompanyId == _companyId) &&
                                             (x.ParentId == obj.Id)).ToList();

                    obj.TotalLines = 0;
                    obj.TotalDepositLines = 0;
                    obj.TotalTransferLines = 0;

                    foreach (var detail in objDetails)
                    {
                        detail.AmountDetail = obj.AmountTransaction;
                        //Seteamos campos de auditoria
                        detail.UpdatedBy = _userName ?? AC.LOCALHOSTME;
                        detail.UpdatedDate = DateTime.UtcNow;
                        detail.UpdatedHostName = AC.LOCALHOSTPC;
                        detail.UpdatedIpv4 = _ipAddress?.ToString() ?? AC.Ipv4Default;
                        obj.TotalDepositLines += ((detail.QuotationDetailType == SD.QuotationDetailType.Deposit)
                             || (detail.QuotationDetailType == SD.QuotationDetailType.CreditTransfer)) ? (short)1 : (short)0;
                        obj.TotalTransferLines += ((detail.QuotationDetailType == SD.QuotationDetailType.Transfer)
                                                  || (detail.QuotationDetailType == SD.QuotationDetailType.DebitTransfer)) ? (short)1 : (short)0;
                        _uow.QuotationDetail.Update(detail);
                    }

                    obj.TotalLines = (short)(obj.TotalDepositLines + obj.TotalTransferLines);
                    _uow.Quotation.Update(obj);
                    _uow.Save();
                }
            }

            if (redirectDetail)
            {
                jsonResponse.UrlRedirect = Url.Action(action: "UpsertDetail", controller: "Quotation", new { id = obj.Id });
            }
            else if (redirectHome)
            {
                jsonResponse.UrlRedirect = Url.Action(action: "Index", controller: "Quotation");
            }
            else
            {
                jsonResponse.Data = new
                {
                    obj.Id
                };
            }
            jsonResponse.IsSuccess = true;
            return Json(jsonResponse);
        }
        catch (Exception e)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = e.Message;
            return Json(jsonResponse);
        }
    }

    [HttpPost]
    public JsonResult Update([FromForm] Quotation obj)
    {
        StringBuilder errorsMessagesBuilder = new();
        JsonResultResponse? jsonResponse = new();

        try
        {
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
            var objCustomer = _uow.Customer.Get(filter: x =>
                x.CompanyId == obj.CompanyId && x.Id == obj.CustomerId, isTracking: false);
            if (objCustomer == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Cliente no encontrado";
                return Json(jsonResponse);
            }

            objQt.CustomerId = obj.CustomerId;

            //Verificamos si existe el ejecutivo
            var objBusinessExecutive = _uow.BusinessExecutive
                .Get(filter: x =>
                    x.CompanyId == obj.CompanyId &&
                    x.Id == obj.BusinessExecutiveId, isTracking: false);
            if (objBusinessExecutive == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Ejecutivo no encontrado";
                return Json(jsonResponse);
            }

            objQt.BusinessExecutiveId = obj.BusinessExecutiveId;


            var objCurrencyRateList = _uow.CurrencyExchangeRate.GetAll
            (x => (x.CompanyId == _companyId) && (x.DateTransa == objQt.DateTransa)
                , includeProperties: "CurrencyTrx").ToList();

            if (objCurrencyRateList is null || objCurrencyRateList.Count == 0)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Tipo de cambio no encontrado";
                return Json(jsonResponse);
            }

            var currencyForeign = objCurrencyRateList
                .FirstOrDefault(t => (t.CurrencyType == CurrencyType.Foreign))?.OfficialRate ?? 1;

            obj.ExchangeRateOfficialReal = obj.ExchangeRateOfficialTransa;
            if (objQt.TypeNumeral == SD.QuotationType.Buy)
            {
                //TC COMPRA MENOR AL TC OFICIAL
                if (obj.ExchangeRateBuyTransa < obj.ExchangeRateOfficialTransa)
                {
                    objQt.AmountRevenue = (obj.ExchangeRateOfficialTransa - obj.ExchangeRateBuyTransa) * obj.AmountTransaction;
                    objQt.AmountCost = 0;
                    objQt.AmountRevenue = objQt.AmountRevenue.RoundTo(AC.DecimalTransa);

                }
                //TC COMPRA MAYOR AL TC OFICIAL
                else
                {
                    objQt.AmountCost = (obj.ExchangeRateBuyTransa - obj.ExchangeRateOfficialTransa) * obj.AmountTransaction;
                    objQt.AmountRevenue = 0;
                    objQt.AmountCost = objQt.AmountCost.RoundTo(AC.DecimalTransa);
                }

                //Compra de dolares 
                if (objQt.CurrencyTransaType == CurrencyType.Foreign)
                {
                    //Factoring paga en Cordobas
                    if (objQt.CurrencyTransferType == CurrencyType.Base)
                    {
                        objQt.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateBuyTransa);
                        objQt.ExchangeRateBuyReal = obj.ExchangeRateBuyTransa;
                        objQt.AmountCostReal = objQt.AmountCost;
                        objQt.AmountRevenueReal = objQt.AmountRevenue;
                        objQt.AmountCostReal = objQt.AmountCostReal.RoundTo(AC.DecimalTransa);
                        objQt.AmountRevenueReal = objQt.AmountRevenueReal.RoundTo(AC.DecimalTransa);
                    }
                }
                //Compra de Euros
                else if (objQt.CurrencyTransaType == CurrencyType.Additional)
                {
                    //Factoring paga en Cordobas
                    if (objQt.CurrencyTransferType == CurrencyType.Base)
                    {
                        objQt.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateBuyTransa);
                        objQt.ExchangeRateBuyReal = obj.ExchangeRateBuyTransa;
                        objQt.AmountCostReal = objQt.AmountCost;
                        objQt.AmountRevenueReal = objQt.AmountRevenue;
                        objQt.AmountCostReal = objQt.AmountCostReal.RoundTo(AC.DecimalTransa);
                        objQt.AmountRevenueReal = objQt.AmountRevenueReal.RoundTo(AC.DecimalTransa);
                    }
                    //Factoring paga en Dolares
                    else if (objQt.CurrencyTransferType == CurrencyType.Foreign)
                    {
                        objQt.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateBuyTransa);
                        objQt.ExchangeRateBuyReal = (obj.ExchangeRateBuyTransa * obj.ExchangeRateOfficialTransa);
                        objQt.AmountCostReal = objQt.AmountCost * currencyForeign;
                        objQt.AmountRevenueReal = objQt.AmountRevenue * currencyForeign;
                        objQt.ExchangeRateOfficialReal = currencyForeign * obj.ExchangeRateOfficialTransa;
                        objQt.ExchangeRateBuyReal = currencyForeign * obj.ExchangeRateBuyTransa;
                        objQt.AmountCostReal = objQt.AmountCostReal.RoundTo(AC.DecimalTransa);
                        objQt.AmountRevenueReal = objQt.AmountRevenueReal.RoundTo(AC.DecimalTransa);
                    }
                }
            }
            else if (objQt.TypeNumeral == SD.QuotationType.Sell)
            {
                //TC VENTA MENOR AL TC OFICIAL
                if (obj.ExchangeRateSellTransa < obj.ExchangeRateOfficialTransa)
                {
                    objQt.AmountCost = (obj.ExchangeRateOfficialTransa - obj.ExchangeRateSellTransa) * obj.AmountTransaction;
                    objQt.AmountRevenue = 0;
                    objQt.AmountCost = objQt.AmountCost.RoundTo(AC.DecimalTransa);
                }
                //TC VENTA MAYOR AL TC OFICIAL
                else
                {
                    objQt.AmountRevenue = (obj.ExchangeRateSellTransa - obj.ExchangeRateOfficialTransa) * obj.AmountTransaction;
                    objQt.AmountCost = 0;
                    objQt.AmountRevenue = objQt.AmountRevenue.RoundTo(AC.DecimalTransa);

                }

                //Venta de dolares 
                if (objQt.CurrencyTransaType == CurrencyType.Foreign)
                {
                    //Cliente paga en Cordobas
                    if (objQt.CurrencyDepositType == CurrencyType.Base)
                    {
                        objQt.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateSellTransa);
                        objQt.ExchangeRateSellReal = obj.ExchangeRateSellTransa;
                        objQt.AmountCostReal = objQt.AmountCost;
                        objQt.AmountRevenueReal = objQt.AmountRevenue;
                        objQt.AmountCostReal = objQt.AmountCostReal.RoundTo(AC.DecimalTransa);
                        objQt.AmountRevenueReal = objQt.AmountRevenueReal.RoundTo(AC.DecimalTransa);
                    }
                }
                //Venta de Euros
                else if (objQt.CurrencyTransaType == CurrencyType.Additional)
                {
                    //Cliente paga en Cordobas
                    if (objQt.CurrencyDepositType == CurrencyType.Base)
                    {
                        objQt.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateSellTransa);
                        objQt.ExchangeRateSellReal = obj.ExchangeRateSellTransa;
                        objQt.AmountCostReal = objQt.AmountCost;
                        objQt.AmountRevenueReal = objQt.AmountRevenue;
                        objQt.AmountCostReal = objQt.AmountCostReal.RoundTo(AC.DecimalTransa);
                        objQt.AmountRevenueReal = objQt.AmountRevenueReal.RoundTo(AC.DecimalTransa);

                    }
                    //Cliente paga en Dolares
                    else if (objQt.CurrencyDepositType == CurrencyType.Foreign)
                    {
                        objQt.AmountExchange = (obj.AmountTransaction * obj.ExchangeRateSellTransa);
                        objQt.ExchangeRateSellReal = (obj.ExchangeRateSellTransa * obj.ExchangeRateOfficialTransa);

                        objQt.AmountCostReal = objQt.AmountCost * currencyForeign;
                        objQt.AmountRevenueReal = objQt.AmountRevenue * currencyForeign;

                        objQt.ExchangeRateOfficialReal = currencyForeign * obj.ExchangeRateOfficialTransa;
                        objQt.ExchangeRateSellReal = currencyForeign * obj.ExchangeRateSellTransa;

                        objQt.AmountCostReal = objQt.AmountCostReal.RoundTo(AC.DecimalTransa);
                        objQt.AmountRevenueReal = objQt.AmountRevenueReal.RoundTo(AC.DecimalTransa);
                    }
                }
            }
            else
            {
                obj.ExchangeRateOfficialReal = obj.ExchangeRateOfficialTransa;
            }

            //objQt.AmountRevenue = obj.AmountRevenue;
            //objQt.AmountCommission = obj.AmountCommission;
            //objQt.AmountCost = obj.AmountCost;
            //objQt.AmountExchange = obj.AmountExchange;
            objQt.AmountTransaction = obj.AmountTransaction;
            objQt.DateTransa = obj.DateTransa;
            objQt.ExchangeRateBuyTransa = obj.ExchangeRateBuyTransa;
            objQt.ExchangeRateSellTransa = obj.ExchangeRateSellTransa;
            objQt.ExchangeRateOfficialReal = obj.ExchangeRateOfficialTransa;
            //Seteamos campos de auditoria
            objQt.UpdatedBy = _userName ?? AC.LOCALHOSTME;
            objQt.UpdatedDate = DateTime.UtcNow;
            objQt.UpdatedHostName = AC.LOCALHOSTPC;
            objQt.UpdatedIpv4 = _ipAddress?.ToString() ?? AC.Ipv4Default;
            _uow.Quotation.Update(objQt);
            _uow.Save();

            if (objQt.TypeNumeral == SD.QuotationType.Transport)
            {
                var objDetails = _uow.QuotationDetail
                    .GetAll(filter: x => x.CompanyId == _companyId &&
                                         x.ParentId == objQt.Id).ToList();

                foreach (var detail in objDetails)
                {
                    detail.AmountDetail = objQt.AmountTransaction;
                    //Seteamos campos de auditoria
                    detail.UpdatedBy = _userName ?? AC.LOCALHOSTME;
                    detail.UpdatedDate = DateTime.UtcNow;
                    detail.UpdatedHostName = AC.LOCALHOSTPC;
                    detail.UpdatedIpv4 = _ipAddress?.ToString() ?? AC.Ipv4Default;
                    _uow.QuotationDetail.Update(detail);
                }

                objQt.TotalLines = (short)objDetails.Count;
                objQt.TotalDepositLines = (short)objDetails
                    .Where(x => x.QuotationDetailType == QuotationDetailType.CreditTransfer).ToList()
                    .Count;
                objQt.TotalTransferLines = (short)objDetails
                    .Where(x => x.QuotationDetailType == QuotationDetailType.DebitTransfer).ToList()
                    .Count;
                _uow.Quotation.Update(objQt);
                _uow.Save();
            }

            TempData["success"] = "Cotización actualizada exitosamente";

            jsonResponse.IsSuccess = true;
            jsonResponse.UrlRedirect = Url.Action(action: "UpsertDetail", controller: "Quotation", new { id = obj.Id });
            return Json(jsonResponse);
        }
        catch (Exception e)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = e.Message;
            return Json(jsonResponse);
        }
    }

    public IActionResult UpsertDetail(int id)
    {
        try
        {
            ViewData[AC.Title] = "Dellate - Cotización";
            List<Models.Customer>? listCustomer = new();
            QuotationDetailVM model = new();
            ViewBag.DecimalTransa = JsonConvert.SerializeObject(_decimalTransa);
            ViewBag.VariationMaxDeposit = JsonConvert.SerializeObject(_variationMaxDeposit);
            ViewBag.DecimalExchangeFull = JsonConvert.SerializeObject(_decimalExchangeFull);

            Quotation objHeader = _uow.Quotation.Get(filter: x => x.CompanyId == _companyId && x.Id == id,
                includeProperties: "TypeTrx,CustomerTrx,CurrencyDepositTrx,CurrencyTransferTrx,CurrencyTransaTrx,BankAccountSourceTrx,BankAccountTargetTrx", isTracking: false);
            if (objHeader == null)
            {
                TempData[AC.Error] = $"Cotización no encontrada";
                return RedirectToAction(nameof(Index));

            }

            ViewBag.DecimalExchange = objHeader.IsAdjustment 
                ? JsonConvert.SerializeObject(_decimalExchangeFull) 
                : JsonConvert.SerializeObject(_decimalExchange);

            if (objHeader.IsClosed && !objHeader.IsPosted)
            {
                ViewData["IsReClosed"] = true;
            }

            var objBankList = _uow.Bank
                .GetAll(x => (x.CompanyId == _companyId))
                .ToList();

            if (objBankList == null)
            {
                TempData[AC.Error] = $"Banco no encontrado";
                return RedirectToAction(nameof(Index));

            }

            var objCurrencyList = _uow.Currency
                .GetAll(x => (x.CompanyId == _companyId))
                .ToList();

            if (objCurrencyList == null)
            {
                return RedirectToAction(nameof(Index));

            }

            var objTypeList = _uow.QuotationType
                .GetAll(x => (x.CompanyId == _companyId))
                .ToList();

            if (objTypeList == null)
            {
                TempData[AC.Error] = $"Tipo de Transacción no encontrado";
                return RedirectToAction(nameof(Index));
            }

            var objBankAccountList = _uow.BankAccount
                .GetAll(filter: x => x.CompanyId == _companyId).ToList();
            if (objBankAccountList == null)
            {
                TempData[AC.Error] = $"Cuenta Bancaria no encontrada";
                return RedirectToAction(nameof(Index));
            }

            var objBusinessExecutiveList = _uow.BusinessExecutive
                .GetAll(x => (x.CompanyId == _companyId)
                             && (x.IsActive)).ToList();

            if (objBusinessExecutiveList == null || objBusinessExecutiveList.Count == 0)
            {
                TempData[AC.Error] = $"Ejecutivo no encontrado";
                return RedirectToAction(nameof(Index));
            }

            model.ModelCreateVM.BusinessExecutiveList = objBusinessExecutiveList;
            model.ModelCreateVM.BankAccountSourceList = objBankAccountList.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            model.ModelCreateVM.BankAccountTargetList = objBankAccountList.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            model.ModelCreateVM.CurrencyTransaList = objCurrencyList
                .Where(x => (x.IsActive && (x.Numeral != (int)CurrencyType.Base)))
                .ToList();
            model.ModelCreateVM.CurrencyDepositList = objCurrencyList.Where(x => x.IsActive).ToList();
            model.ModelCreateVM.CurrencyTransferList = objCurrencyList.Where(x => x.IsActive).ToList();
            model.ModelCreateVM.QuotationTypeList = objTypeList;
            var currencyTarget =
                objHeader.TypeNumeral != SD.QuotationType.Sell ?
                    objHeader.CurrencyTransferTrx.Code : objHeader.CurrencyDepositTrx.Code;

            model.ModelCreateVM.CurrencySourceTarget =
                $"{objHeader.CurrencyTransaTrx.Code} - {currencyTarget}";
            model.BankList = objBankList;
            model.ModelCreateVM.DataModel = objHeader;
            model.CustomerFullName = $"{objHeader.CustomerTrx.BusinessName}";
            model.NumberTransa = $"COT-{objHeader.TypeTrx.Code}-{objHeader.Numeral.ToString().PadLeft(3, AC.CharDefaultEmpty)}";
            model.DataModel = new();
            model.DataModel.CompanyId = _companyId;

            if (model.ModelCreateVM.DataModel.CustomerId != 0)
            {
                var objCustomer =
                    _uow.Customer.Get(filter: x => (x.CompanyId == _companyId && x.Id == model.ModelCreateVM.DataModel.CustomerId));

                listCustomer.Add(objCustomer);
                //model.ModelCreateVM.CustomerList = listCustomer.Select(x => new SelectListItem { Text = x.BusinessName, Value = x.Id.ToString() });
                model.ModelCreateVM.CustomerList = listCustomer;
            }

            return View(model);
        }
        catch (Exception ex)
        {
            TempData[AC.Error] = ex.Message;
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public async Task<JsonResult> UpsertDetail([FromForm] QuotationDetail obj)
    {
        StringBuilder errorsMessagesBuilder = new();
        JsonResultResponse? jsonResponse = new();

        try
        {
            if (!ModelState.IsValid)
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

            obj.PercentageCostRevenue = objHeader.TypeNumeral == SD.QuotationType.Sell
                && (obj.QuotationDetailType == QuotationDetailType.Deposit
                || obj.QuotationDetailType == QuotationDetailType.CreditTransfer)
                ? obj.AmountDetail / objHeader.AmountExchange
                : obj.AmountDetail / objHeader.AmountTransaction;

            if (objHeader.TypeNumeral == SD.QuotationType.Buy)
            {
                if (objHeader.CurrencyTransaType == CurrencyType.Foreign)
                {
                    if (objHeader.CurrencyTransferType == CurrencyType.Base)
                    {
                        obj.PercentageCostRevenue = (obj.AmountDetail / objHeader.AmountExchange);
                    }
                }
                else if (objHeader.CurrencyTransaType == CurrencyType.Additional)
                {
                    if (objHeader.CurrencyTransferType == CurrencyType.Base)
                    {
                        obj.PercentageCostRevenue = (obj.AmountDetail / objHeader.AmountExchange);
                    }
                    else if (objHeader.CurrencyTransferType == CurrencyType.Foreign)
                    {
                    }
                }

                if (obj.QuotationDetailType == QuotationDetailType.Deposit)
                {
                    obj.CurrencyDetailId = objHeader.CurrencyTransaId;
                }
                else if (obj.QuotationDetailType == QuotationDetailType.Transfer)
                {
                    obj.CurrencyDetailId = objHeader.CurrencyTransferId;
                }
            }
            else if (objHeader.TypeNumeral == SD.QuotationType.Sell)
            {
                if (objHeader.CurrencyTransaType == CurrencyType.Additional)
                {
                    if (objHeader.CurrencyDepositType == CurrencyType.Base)
                    {

                    }
                    else if (objHeader.CurrencyDepositType == CurrencyType.Foreign)
                    {
                        obj.PercentageCostRevenue = (obj.AmountDetail / objHeader.AmountTransaction);
                    }
                }


                if (obj.QuotationDetailType == QuotationDetailType.Deposit)
                {
                    obj.CurrencyDetailId = objHeader.CurrencyDepositId;
                }
                else if (obj.QuotationDetailType == QuotationDetailType.Transfer)
                {
                    obj.CurrencyDetailId = objHeader.CurrencyTransaId;
                }
            }

            obj.PercentageCostRevenue = obj.PercentageCostRevenue.RoundTo(AC.DecimalExchange);

            if (objHeader.AmountCost != 0)
            {
                obj.AmountCost = (obj.PercentageCostRevenue * objHeader.AmountCostReal);
                obj.AmountCost = obj.AmountCost.RoundTo(AC.DecimalTransa);
            }
            else if (objHeader.AmountRevenue != 0)
            {
                obj.AmountRevenue = (obj.PercentageCostRevenue * objHeader.AmountRevenueReal);
                obj.AmountRevenue = obj.AmountRevenue.RoundTo(AC.DecimalTransa);
            }

            if (obj.QuotationDetailType == QuotationDetailType.Deposit)
            {
                obj.PercentageCostRevenue = 0;
                obj.AmountRevenue = 0;
                obj.AmountCost = 0;
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
                obj.LineNumber = await _uow.QuotationDetail
                    .NextLineNumber(filter: x =>
                     x.CompanyId == obj.CompanyId &&
                     x.ParentId == objHeader.Id &&
                     x.QuotationDetailType == obj.QuotationDetailType);

                obj.CreatedBy = _userName ?? AC.LOCALHOSTME;
                obj.CreatedDate = DateTime.UtcNow;
                obj.CreatedHostName = AC.LOCALHOSTPC;
                obj.CreatedIpv4 = _ipAddress?.ToString() ?? AC.Ipv4Default;
                _uow.QuotationDetail.Add(obj);
                _uow.Save();
                //TempData[AC.Success] = $"Cotización creada correctamente";
            }
            else
            {
                var objDetail = _uow.QuotationDetail
                    .Get(filter: x =>
                    x.CompanyId == obj.CompanyId &&
                    x.ParentId == objHeader.Id &&
                    x.Id == obj.Id);

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
                objDetail.AmountRevenue = obj.AmountRevenue;
                objDetail.AmountCost = obj.AmountCost;
                objDetail.PercentageCostRevenue = obj.PercentageCostRevenue;
                objDetail.QuotationDetailType = obj.QuotationDetailType;
                objDetail.UpdatedBy = _userName ?? AC.LOCALHOSTME;
                objDetail.UpdatedDate = DateTime.UtcNow;
                objDetail.UpdatedHostName = AC.LOCALHOSTPC;
                objDetail.UpdatedIpv4 = _ipAddress?.ToString() ?? AC.Ipv4Default;
                _uow.QuotationDetail.Update(objDetail);
                _uow.Save();

                //TempData[AC.Success] = $"Cotización actualizada correctamente";
            }

            //Obtenemos los hijos
            var objDetails = _uow.QuotationDetail
                .GetAll(filter: x =>
                x.CompanyId == obj.CompanyId &&
                x.ParentId == objHeader.Id,
                    includeProperties: "ParentTrx,CurrencyDetailTrx,BankSourceTrx,BankTargetTrx").ToList();

            if (objDetails == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Detalle de cotización no encontrado";
                return Json(jsonResponse);
            }

            //Actualizamos los totales del padre
            objHeader.TotalDeposit = objDetails
                .Where(x => x.QuotationDetailType == QuotationDetailType.Deposit ||
                            x.QuotationDetailType == QuotationDetailType.CreditTransfer)
                .Sum(x => x.AmountDetail);

            objHeader.TotalTransfer = objDetails
                .Where(x => x.QuotationDetailType == QuotationDetailType.Transfer ||
                            x.QuotationDetailType == QuotationDetailType.DebitTransfer)
                .Sum(x => x.AmountDetail);

            objHeader.TotalLines = (short)objDetails.Count;

            objHeader.TotalDepositLines = (short)objDetails
                .Where(x => x.QuotationDetailType == QuotationDetailType.Deposit ||
                            x.QuotationDetailType == QuotationDetailType.CreditTransfer).ToList()
                .Count;

            objHeader.TotalTransferLines = (short)objDetails
                .Where(x => x.QuotationDetailType == QuotationDetailType.Transfer ||
                            x.QuotationDetailType == QuotationDetailType.DebitTransfer).ToList()
                .Count;

            objHeader.UpdatedBy = _userName ?? AC.LOCALHOSTME;
            objHeader.UpdatedDate = DateTime.UtcNow;
            objHeader.UpdatedHostName = AC.LOCALHOSTPC;
            objHeader.UpdatedIpv4 = _ipAddress?.ToString() ?? AC.Ipv4Default;
            _uow.Quotation.Update(objHeader);
            _uow.Save();

            jsonResponse.IsSuccess = true;
            jsonResponse.UrlRedirect = Url.Action(action: "UpsertDetail", controller: "Quotation", new { id = obj.ParentId });
            return Json(jsonResponse);
        }
        catch (Exception e)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = e.Message;
            return Json(jsonResponse);
        }

    }

    public IActionResult Delete(int id)
    {

        try
        {
            ViewData[AC.Title] = "Eliminar - Cotización";

            QuotationDetailVM model = new();
            ViewBag.DecimalTransa = JsonConvert.SerializeObject(_decimalTransa);
            //ViewBag.DecimalExchange = JsonConvert.SerializeObject(_decimalExchange);

            var objHeader = _uow.Quotation.Get(filter: x => x.CompanyId == _companyId && x.Id == id,
                includeProperties: "TypeTrx,CustomerTrx,CurrencyTransferTrx,CurrencyDepositTrx,CurrencyTransaTrx,BankAccountSourceTrx,BankAccountTargetTrx", isTracking: false);
            if (objHeader == null)
            {
                TempData[AC.Error] = $"Cotización no encontrada";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.DecimalExchange = objHeader.IsAdjustment ? JsonConvert.SerializeObject(_decimalExchangeFull) : JsonConvert.SerializeObject(_decimalExchange);

            var objBankList = _uow.Bank
                .GetAll(x => x.CompanyId == _companyId)
                .ToList();

            if (objBankList == null)
            {
                TempData[AC.Error] = $"Bancos no encontrados";
                return RedirectToAction(nameof(Index));
            }

            model.BankList = objBankList;
            model.ModelCreateVM.DataModel = objHeader;
            model.CustomerFullName = $"{objHeader.CustomerTrx.BusinessName}";
            model.NumberTransa = $"COT-{objHeader.TypeTrx.Code}-{objHeader.Numeral.ToString().PadLeft(3, AC.CharDefaultEmpty)}";
            var currencyTarget =
                objHeader.TypeNumeral != SD.QuotationType.Sell ?
                    objHeader.CurrencyTransferTrx.Code : objHeader.CurrencyDepositTrx.Code;

            model.ModelCreateVM.CurrencySourceTarget =
                $"{objHeader.CurrencyTransaTrx.Code} - {currencyTarget}";

            return View(model);
        }
        catch (Exception ex)
        {
            TempData[AC.Error] = ex.Message;
            return RedirectToAction("Index");
        }

    }

    public IActionResult Detail(int id)
    {

        try
        {
            ViewData[AC.Title] = "Visualizar - Cotización";

            QuotationDetailVM model = new();
            ViewBag.DecimalTransa = JsonConvert.SerializeObject(_decimalTransa);

            Quotation objHeader = _uow.Quotation.Get(filter: x => x.CompanyId == _companyId && x.Id == id,
                includeProperties: "TypeTrx,CustomerTrx,CurrencyTransferTrx,CurrencyDepositTrx,CurrencyTransaTrx,BankAccountSourceTrx,BankAccountTargetTrx", isTracking: false);

            if (objHeader == null)
            {
                ViewData["TransactionMessageNotFound"] = $"Operación de Mesa de cambio {id} no encontrada";
                ViewData["UrlReturn"] = Url.Action(action: "Index", controller: "Quotation");
                return View("TransactionNotFound");
            }

            ViewBag.DecimalExchange = objHeader.IsAdjustment 
                ? JsonConvert.SerializeObject(_decimalExchangeFull) 
                : JsonConvert.SerializeObject(_decimalExchange);

            var objBankList = _uow.Bank
                .GetAll(x => x.CompanyId == _companyId)
                .ToList();

            if (objBankList == null)
            {
                TempData[AC.Error] = $"Bancos no encontrados";
                return RedirectToAction(nameof(Index));
            }

            model.BankList = objBankList;
            model.ModelCreateVM.DataModel = objHeader;
            model.CustomerFullName = $"{objHeader.CustomerTrx.BusinessName}";
            model.NumberTransa = $"COT-{objHeader.TypeTrx.Code}-{objHeader.Numeral.ToString().PadLeft(3, AC.CharDefaultEmpty)}";
            var currencyTarget =
                objHeader.TypeNumeral != SD.QuotationType.Sell ?
                    objHeader.CurrencyTransferTrx.Code : objHeader.CurrencyDepositTrx.Code;

            model.ModelCreateVM.CurrencySourceTarget =
                $"{objHeader.CurrencyTransaTrx.Code} - {currencyTarget}";

            ViewBag.TotalTransfer = JsonConvert.SerializeObject(objHeader.TotalTransfer);
            ViewBag.TotalDeposit = JsonConvert.SerializeObject(objHeader.TotalDeposit);

            ViewBag.IsClosedNotPosted = false;

            if (objHeader.IsClosed && !objHeader.IsPosted)
            {
                ViewBag.IsClosedNotPosted = true;
                ViewBag.UrlGoToUpsert = objHeader.IsPayment || objHeader.IsLoan
                    ? Url.Action(action: "Upsert", controller: "Quotation", new { id = objHeader.Id })
                    : Url.Action(action: "UpsertDetail", controller: "Quotation", new { id = objHeader.Id });
            }

            return View(model);
        }
        catch (Exception ex)
        {
            TempData[AC.Error] = ex.Message;
            return RedirectToAction("Index");
        }

    }

    public IActionResult ProcessingDate()
    {
        try
        {

            ProcessingDateVM model = new();
            string processingDateString = HttpContext.Session.GetString(AC.ProcessingDate) ?? DateOnly.FromDateTime(DateTime.Now).ToString();
            var processingDate = DateOnly.Parse(processingDateString);
            model.ProcessingDate = processingDate;
            ViewData[AC.DefaultDateCurrent] = JsonConvert.SerializeObject(model.ProcessingDate.ToString(AC.DefaultDateFormatWeb));
            return View(model);
        }
        catch (Exception ex)
        {
            TempData[AC.Error] = ex.Message;
            return RedirectToAction("Index");
        }
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
                HttpContext.Session.SetString(AC.ChangeProcessingDate, true.ToString());
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

        try
        {

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
            jsonResponse.Data = objList.OrderByDescending(x => x.Id);
            return Json(jsonResponse);
        }
        catch (Exception e)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = e.Message;
            return Json(jsonResponse);
        }

    }

    public async Task<JsonResult> GetAllByCustomer(int customerId, SD.QuotationType type, CurrencyType currency)
    {
        JsonResultResponse? jsonResponse = new();
        try
        {
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
        catch (Exception e)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = e.Message;
            return Json(jsonResponse);
        }
    }

    public async Task<JsonResult> GetAllByParent(int parentId = 0, QuotationDetailType type = QuotationDetailType.Deposit)
    {
        StringBuilder errorsMessagesBuilder = new();
        JsonResultResponse? jsonResponse = new();
        try
        {
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

            var objListMapped = _mapper.Map<List<QuotationDetailDto>>(objList);

            foreach (var item in objListMapped)
            {
                if (item.BankTransactionId.HasValue)
                {

                    var srvResponse = await _srvTransaBco.GetStatusAsync<APIResponse>(_sessionToken, item.BankTransactionId.Value);
                    if (srvResponse is null)
                    {
                        jsonResponse.IsSuccess = false;
                        jsonResponse.ErrorMessages = "No se pudo obtener la respuesta";
                        return Json(jsonResponse);
                    }

                    if (srvResponse is { isSuccess: true })
                    {
                        var transaDtoStatus = JsonConvert.DeserializeObject<TransaccionesBcoDtoStatus>(Convert.ToString(srvResponse.result));

                        if (transaDtoStatus != null)
                        {
                            if (transaDtoStatus.TransaBcoFullName != null && transaDtoStatus.TransaAsiFullName != null)
                            {
                                item.TransactionBcoFullName = $"Trx: {transaDtoStatus.TransaBcoFullName} [{transaDtoStatus.TransaBcoEstado.Trim()}]";
                                item.JournalEntryFullName = $"Asi: {transaDtoStatus.TransaAsiFullName} [{transaDtoStatus.TransaAsiEstado.Trim()}]";
                                if (item.JournalEntryVoidId.HasValue && transaDtoStatus.TransaAsiAnuladoFullName != null && transaDtoStatus.TransaAsiAnuladoEstado != null)
                                {
                                    item.JournalEntryVoidFullName = $"Asi Anu: {transaDtoStatus.TransaAsiAnuladoFullName} [{transaDtoStatus.TransaAsiAnuladoEstado.Trim()}]";
                                }
                            }
                        }

                    }
                    else if (srvResponse is { isSuccess: false })
                    {
                        errorsMessagesBuilder.AppendJoin("", srvResponse.errorMessages);
                        jsonResponse.IsSuccess = false;
                        jsonResponse.ErrorMessages = errorsMessagesBuilder.ToString();
                        return Json(jsonResponse);
                    }
                }
            }

            jsonResponse.IsSuccess = true;
            jsonResponse.Data = objListMapped;
            return Json(jsonResponse);

        }
        catch (Exception e)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = e.Message;
            return Json(jsonResponse);
        }
    }

    public async Task<JsonResult> GetAverageExchangeRate(int customerId, SD.QuotationType type, CurrencyType currency)
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

            var objCurrencyRateList = _uow.CurrencyExchangeRate.GetAll
            (x => (x.CompanyId == _companyId) && (x.DateTransa == objQuotation.DateTransa)
                , includeProperties: "CurrencyTrx").ToList();

            if (objCurrencyRateList is null || objCurrencyRateList.Count == 0)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Tipo de cambio no encontrado";
                return Json(jsonResponse);
            }

            var currencyForeign = objCurrencyRateList
                .FirstOrDefault(t => (t.CurrencyType == CurrencyType.Foreign))?.OfficialRate ?? 1;


            if (objQuotation.TypeNumeral == SD.QuotationType.Buy)
            {
                objQuotation.ExchangeRateBuyTransa = (objQuotation.TotalTransfer / objQuotation.AmountTransaction);

                //TC COMPRA MENOR AL TC OFICIAL
                if (objQuotation.ExchangeRateBuyTransa < objQuotation.ExchangeRateOfficialTransa)
                {
                    objQuotation.AmountRevenue = (objQuotation.ExchangeRateOfficialTransa - objQuotation.ExchangeRateBuyTransa) * objQuotation.AmountTransaction;
                    objQuotation.AmountCost = 0;
                    objQuotation.AmountRevenue = objQuotation.AmountRevenue.RoundTo(AC.DecimalTransa);

                }
                //TC COMPRA MAYOR AL TC OFICIAL
                else
                {
                    objQuotation.AmountCost = (objQuotation.ExchangeRateBuyTransa - objQuotation.ExchangeRateOfficialTransa) * objQuotation.AmountTransaction;
                    objQuotation.AmountRevenue = 0;
                    objQuotation.AmountCost = objQuotation.AmountCost.RoundTo(AC.DecimalTransa);
                }

                //Compra de dolares 
                if (objQuotation.CurrencyTransaType == CurrencyType.Foreign)
                {
                    //Factoring paga en Cordobas
                    if (objQuotation.CurrencyTransferType == CurrencyType.Base)
                    {
                        objQuotation.AmountExchange = (objQuotation.AmountTransaction * objQuotation.ExchangeRateBuyTransa);
                        objQuotation.ExchangeRateBuyReal = objQuotation.ExchangeRateBuyTransa;

                        objQuotation.AmountCostReal = objQuotation.AmountCost;
                        objQuotation.AmountRevenueReal = objQuotation.AmountRevenue;

                        objQuotation.AmountCostReal = objQuotation.AmountCostReal.RoundTo(AC.DecimalTransa);
                        objQuotation.AmountRevenueReal = objQuotation.AmountRevenueReal.RoundTo(AC.DecimalTransa);
                    }
                }
                //Compra de Euros
                else if (objQuotation.CurrencyTransaType == CurrencyType.Additional)
                {
                    //Factoring paga en Cordobas
                    if (objQuotation.CurrencyTransferType == CurrencyType.Base)
                    {
                        objQuotation.AmountExchange = (objQuotation.AmountTransaction * objQuotation.ExchangeRateBuyTransa);
                        objQuotation.ExchangeRateBuyReal = objQuotation.ExchangeRateBuyTransa;

                        objQuotation.AmountCostReal = objQuotation.AmountCost;
                        objQuotation.AmountRevenueReal = objQuotation.AmountRevenue;

                        objQuotation.AmountCostReal = objQuotation.AmountCostReal.RoundTo(AC.DecimalTransa);
                        objQuotation.AmountRevenueReal = objQuotation.AmountRevenueReal.RoundTo(AC.DecimalTransa);
                    }
                    //Factoring paga en Dolares
                    else if (objQuotation.CurrencyTransferType == CurrencyType.Foreign)
                    {
                        objQuotation.AmountExchange = (objQuotation.AmountTransaction * objQuotation.ExchangeRateBuyTransa);
                        objQuotation.ExchangeRateBuyReal = (objQuotation.ExchangeRateBuyTransa * objQuotation.ExchangeRateOfficialTransa);

                        objQuotation.AmountCostReal = objQuotation.AmountCost * currencyForeign;
                        objQuotation.AmountRevenueReal = objQuotation.AmountRevenue * currencyForeign;

                        objQuotation.ExchangeRateOfficialReal = currencyForeign * objQuotation.ExchangeRateOfficialTransa;
                        objQuotation.ExchangeRateBuyReal = currencyForeign * objQuotation.ExchangeRateBuyTransa;

                        objQuotation.AmountCostReal = objQuotation.AmountCostReal.RoundTo(AC.DecimalTransa);
                        objQuotation.AmountRevenueReal = objQuotation.AmountRevenueReal.RoundTo(AC.DecimalTransa);
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
                    objQuotation.AmountCost = objQuotation.AmountCost.RoundTo(AC.DecimalTransa);
                }
                //TC VENTA MAYOR AL TC OFICIAL
                else
                {
                    objQuotation.AmountRevenue = (objQuotation.ExchangeRateSellTransa - objQuotation.ExchangeRateOfficialTransa) * objQuotation.AmountTransaction;
                    objQuotation.AmountCost = 0;
                    objQuotation.AmountRevenue = objQuotation.AmountRevenue.RoundTo(AC.DecimalTransa);

                }

                //Venta de dolares 
                if (objQuotation.CurrencyTransaType == CurrencyType.Foreign)
                {
                    //Cliente paga en Cordobas
                    if (objQuotation.CurrencyDepositType == CurrencyType.Base)
                    {
                        objQuotation.AmountExchange = (objQuotation.AmountTransaction * objQuotation.ExchangeRateSellTransa);
                        objQuotation.ExchangeRateSellReal = objQuotation.ExchangeRateSellTransa;

                        objQuotation.AmountCostReal = objQuotation.AmountCost;
                        objQuotation.AmountRevenueReal = objQuotation.AmountRevenue;

                        objQuotation.AmountCostReal = objQuotation.AmountCostReal.RoundTo(AC.DecimalTransa);
                        objQuotation.AmountRevenueReal = objQuotation.AmountRevenueReal.RoundTo(AC.DecimalTransa);
                    }
                }
                //Venta de Euros
                else if (objQuotation.CurrencyTransaType == CurrencyType.Additional)
                {
                    //Cliente paga en Cordobas
                    if (objQuotation.CurrencyDepositType == CurrencyType.Base)
                    {
                        objQuotation.AmountExchange = (objQuotation.AmountTransaction * objQuotation.ExchangeRateSellTransa);
                        objQuotation.ExchangeRateSellReal = objQuotation.ExchangeRateSellTransa;

                        objQuotation.AmountCostReal = objQuotation.AmountCost;
                        objQuotation.AmountRevenueReal = objQuotation.AmountRevenue;

                        objQuotation.AmountCostReal = objQuotation.AmountCostReal.RoundTo(AC.DecimalTransa);
                        objQuotation.AmountRevenueReal = objQuotation.AmountRevenueReal.RoundTo(AC.DecimalTransa);
                    }
                    //Cliente paga en Dolares
                    else if (objQuotation.CurrencyDepositType == CurrencyType.Foreign)
                    {
                        objQuotation.AmountExchange = (objQuotation.AmountTransaction * objQuotation.ExchangeRateSellTransa);
                        objQuotation.ExchangeRateSellReal = (objQuotation.ExchangeRateSellTransa * objQuotation.ExchangeRateOfficialTransa);

                        objQuotation.AmountCostReal = objQuotation.AmountCost * currencyForeign;
                        objQuotation.AmountRevenueReal = objQuotation.AmountRevenue * currencyForeign;

                        objQuotation.ExchangeRateOfficialReal = currencyForeign * objQuotation.ExchangeRateOfficialTransa;
                        objQuotation.ExchangeRateSellReal = currencyForeign * objQuotation.ExchangeRateSellTransa;

                        objQuotation.AmountCostReal = objQuotation.AmountCostReal.RoundTo(AC.DecimalTransa);
                        objQuotation.AmountRevenueReal = objQuotation.AmountRevenueReal.RoundTo(AC.DecimalTransa);
                    }
                }
            }

            //Seteamos campos de auditoria
            objQuotation.UpdatedBy = _userName ?? AC.LOCALHOSTME;
            objQuotation.UpdatedDate = DateTime.UtcNow;
            objQuotation.UpdatedHostName = AC.LOCALHOSTPC;
            objQuotation.UpdatedIpv4 = _ipAddress?.ToString() ?? AC.Ipv4Default;
            objQuotation.IsAdjustment = true;
            _uow.Quotation.Update(objQuotation);
            _uow.Save();
            jsonResponse.UrlRedirect = Url.Action(action: "UpsertDetail", controller: "Quotation", new { id = objQuotation.Id });


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
            objHeader.UpdatedBy = _userName ?? AC.LOCALHOSTME;
            objHeader.UpdatedDate = DateTime.UtcNow;
            objHeader.UpdatedHostName = AC.LOCALHOSTPC;
            objHeader.UpdatedIpv4 = _ipAddress?.ToString() ?? AC.Ipv4Default;
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
            var objDetails = _uow.QuotationDetail
                .GetAll(filter: x => x.CompanyId == _companyId &&
                                     x.ParentId == parentId).ToList();

            if (objDetails == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Detalle de cotización invalido";
                return Json(jsonResponse);
            }

            objHeader.TotalDeposit = objDetails
                .Where(x => x.QuotationDetailType == QuotationDetailType.Deposit ||
                            x.QuotationDetailType == QuotationDetailType.CreditTransfer)
                .Sum(x => x.AmountDetail);

            objHeader.TotalTransfer = objDetails
                .Where(x => x.QuotationDetailType == QuotationDetailType.Transfer ||
                            x.QuotationDetailType == QuotationDetailType.DebitTransfer)
                .Sum(x => x.AmountDetail);

            objHeader.TotalLines = (short)objDetails.Count;

            objHeader.TotalDepositLines = (short)objDetails
                .Where(x => x.QuotationDetailType == QuotationDetailType.Deposit ||
                            x.QuotationDetailType == QuotationDetailType.CreditTransfer).ToList()
                .Count;

            objHeader.TotalTransferLines = (short)objDetails
                .Where(x => x.QuotationDetailType == QuotationDetailType.Transfer ||
                            x.QuotationDetailType == QuotationDetailType.DebitTransfer).ToList()
                .Count;

            objHeader.UpdatedBy = _userName ?? AC.LOCALHOSTME;
            objHeader.UpdatedDate = DateTime.UtcNow;
            objHeader.UpdatedHostName = AC.LOCALHOSTPC;
            objHeader.UpdatedIpv4 = _ipAddress?.ToString() ?? AC.Ipv4Default;
            _uow.Quotation.Update(objHeader);
            _uow.Save();

            jsonResponse.IsSuccess = true;
            //jsonResponse.SuccessMessages = $"Detalle eliminado correctamente";
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
            jsonResponse.Data = objCustomerList;
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
            objCustomerList = search == "." || search == null || search == string.Empty
                ? _uow.Customer
                    .GetAll(filter: x => x.CompanyId == _companyId &&
                                         x.IsSystemRow == onlyCompanies).ToList()
                : _uow.Customer
                    .GetAll(filter: x => x.CompanyId == _companyId &&
                                         x.IsSystemRow == onlyCompanies &&
                                         (x.BusinessName.Contains(search) ||
                                          x.IdentificationNumber.Contains(search) ||
                                          x.Code.Contains(search) ||
                                          x.AddressPrimary.Contains(search))).ToList();


            if (objCustomerList == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Cliente invalido";
                return Json(jsonResponse);
            }

            jsonResponse.IsSuccess = true;
            jsonResponse.Data = objCustomerList;
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

    #region Contabilización

    [HttpPost, ActionName("Regenerate")]
    public async Task<JsonResult> RegenerateTransactions([FromBody] List<int> quotationIds)
    {
        JsonResultResponse? jsonResponse = new();
        StringBuilder errorsMessagesBuilder = new();
        DateTime beginProcess = DateTime.Now;

        //ToDo : 
        try
        {
            var transactionList = _uow.Quotation
                .GetAll(filter: x => (x.CompanyId == _companyId)
                                         && (quotationIds.Contains(x.Id))
                                         && (x.IsClosed)
                                         && (!x.IsPayment) && (!x.IsLoan) && (!x.IsPosted))
                                        .ToList();

            if (transactionList is null || transactionList.Count() == 0)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.IsWarning = true;
                jsonResponse.TitleMessages = "Lo Sentimos";
                jsonResponse.ErrorMessages = $"No hay operaciones disponibles";
                return Json(jsonResponse);
            }

            quotationIds = transactionList.Select(x => x.Id).ToList();

            foreach (var id in quotationIds)
            {
                var resultResponse = await
                    fnLogicGenerateTransactions(id);

                if (resultResponse.IsInfo)
                {
                    if (resultResponse.Data != null)
                    {
                        var errorList = (List<string>)resultResponse.Data;
                        foreach (var error in errorList)
                        {
                            jsonResponse.ErrorMessages += error + Environment.NewLine;
                        }
                    }
                    else
                    {
                        jsonResponse.ErrorMessages += resultResponse.ErrorMessages + Environment.NewLine;
                    }

                    jsonResponse.TitleMessages = "Detalles Contabilizados";
                    jsonResponse.IsWarning = true;
                    //return Json(jsonResponse);
                }
                else if (!resultResponse.IsSuccess)
                {
                    jsonResponse.IsSuccess = false;
                    //jsonResponse.TitleMessages = AC.Error.ToUpper();
                    jsonResponse.ErrorMessages += resultResponse.ErrorMessages + Environment.NewLine;
                    //return Json(jsonResponse);
                }
            }

            if (jsonResponse.ErrorMessages != string.Empty)
            {
                jsonResponse.IsSuccess = false;
                return Json(jsonResponse);
            }

            DateTime endProcess = DateTime.Now;
            TimeSpan durationProcess = new TimeSpan(endProcess.Ticks - beginProcess.Ticks);
            var time = durationProcess.TotalSeconds;
            jsonResponse.DurationTime = $"Tiempo Transcurrido: {time} segundos";
            jsonResponse.IsSuccess = true;
            jsonResponse.SuccessMessages = "Regeneración completada exitosamente";
            return Json(jsonResponse);
        }
        catch (Exception ex)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.TitleMessages = AC.Error.ToUpper();
            jsonResponse.ErrorMessages = ex.Message.ToString();
            return Json(jsonResponse);
        }
    }

    [HttpPost, ActionName("Closed")]
    public async Task<JsonResult> ClosedPost(int id)
    {
        JsonResultResponse? jsonResponse = new();

        try
        {
            var resultResponse = await
                 fnLogicGenerateTransactions(id);

            if (resultResponse.IsInfo)
            {
                jsonResponse.IsSuccess = false;
                if (resultResponse.Data != null)
                {
                    var errorList = (List<string>)resultResponse.Data;
                    foreach (var error in errorList)
                    {
                        jsonResponse.ErrorMessages += error + Environment.NewLine;
                    }
                }
                else
                {
                    jsonResponse.ErrorMessages = resultResponse.ErrorMessages;
                }

                jsonResponse.TitleMessages = "Detalles Contabilizados";
                jsonResponse.IsWarning = true;
                //jsonResponse.UrlRedirect = Url.Action(action: "Index", controller: "Quotation");
                return Json(jsonResponse);
            }
            else if (!resultResponse.IsSuccess)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.TitleMessages = AC.Error.ToUpper();
                jsonResponse.ErrorMessages = resultResponse.ErrorMessages;
                return Json(jsonResponse);
            }

            jsonResponse.IsSuccess = true;
            jsonResponse.UrlRedirect = Url.Action(action: "Index", controller: "Quotation");

            return Json(jsonResponse);
        }
        catch (Exception ex)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.TitleMessages = AC.Error.ToUpper();
            jsonResponse.ErrorMessages = ex.Message.ToString();
            return Json(jsonResponse);
        }
    }

    private async Task<ResultResponse> fnLogicGenerateTransactions(int id)
    {
        ResultResponse? resultResponse = new() { IsSuccess = true };
        ConfigBcoDto? configBcoDto = new();
        ConfigCntDto? configCntDto = new();
        Guid? objDebitId = new();
        Guid? objCreditId = new();
        Guid? objCommisionId = new();

        bool isReclosed = false;

        if (id == 0)
        {
            resultResponse.IsSuccess = false;
            resultResponse.ErrorMessages = $"El id es requerido";
            return resultResponse;
        }

        var objHeader = _uow.Quotation
            .Get(filter: x => x.CompanyId == _companyId && x.Id == id,
                includeProperties: "TypeTrx,CustomerTrx,BankAccountSourceTrx,BankAccountTargetTrx,CurrencyTransferTrx,CurrencyDepositTrx,CurrencyTransaTrx");

        if (objHeader == null)
        {
            resultResponse.IsSuccess = false;
            resultResponse.ErrorMessages = $"Cotización: {id} no encontrada";
            return resultResponse;
        }

        if (objHeader.IsPosted)
        {
            resultResponse.IsSuccess = false;
            resultResponse.IsInfo = true;
            resultResponse.ErrorMessages = $"Cotización: {id} ya está contabilizada";
            return resultResponse;
        }

        //Si esta cerrado entonces recerramos wey
        isReclosed = (objHeader.IsClosed);

        if (isReclosed)
        {
            TempData["success"] = $"Cotización re-cerrada correctamente";
            objHeader.ReClosedBy = _userName ?? AC.LOCALHOSTME;
            objHeader.ReClosedDate = DateTime.UtcNow;
            objHeader.ReClosedHostName = AC.LOCALHOSTPC;
            objHeader.ReClosedIpv4 = _ipAddress?.ToString() ?? AC.Ipv4Default;

        }
        else
        {
            TempData["success"] = $"Cotización cerrada correctamente";

            var nextSeq = await _uow.Quotation.NextSequentialNumber(filter: x => x.CompanyId == objHeader.CompanyId &&
                x.TypeNumeral == objHeader.TypeNumeral &&
                x.DateTransa == objHeader.DateTransa &&
                x.InternalSerial == AC.InternalSerialOfficial);

            if (nextSeq == null)
            {
                resultResponse.IsSuccess = false;
                resultResponse.ErrorMessages = $"Consecutivo de cotización: {objHeader.Id} {objHeader.TypeTrx.Code}-#{objHeader.Numeral} no encontrado";
                return resultResponse;
            }

            objHeader.IsClosed = true;
            objHeader.Numeral = nextSeq;
            objHeader.InternalSerial = AC.InternalSerialOfficial;
            objHeader.ClosedBy = _userName ?? AC.LOCALHOSTME;
            objHeader.ClosedDate = DateTime.UtcNow;
            objHeader.ClosedHostName = AC.LOCALHOSTPC;
            objHeader.ClosedIpv4 = _ipAddress?.ToString() ?? AC.Ipv4Default;
        }

        try
        {
            resultResponse = await fnGetConfigurations();

            if (!resultResponse.IsSuccess || resultResponse.Data == null || resultResponse.DataChildren == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            configBcoDto = (ConfigBcoDto)resultResponse.Data;
            configCntDto = (ConfigCntDto)resultResponse.DataChildren;

            if (!objHeader.IsLoan && objHeader is { IsPayment: false })
            {
                var objDetailList = _uow.QuotationDetail
                    .GetAll(filter: x => x.ParentId == objHeader.Id,
                        includeProperties: "CurrencyDetailTrx,BankSourceTrx,BankTargetTrx").ToList();

                if (objDetailList == null || objDetailList.Count == 0)
                {
                    resultResponse.IsSuccess = false;
                    resultResponse.ErrorMessages = $"Detalles de Cotización: {objHeader.Id} {objHeader.TypeTrx.Code}-#{objHeader.Numeral} no encontrados";
                    return resultResponse;
                }

                resultResponse = await
                    fnCheckChildrenPosted(objHeader, objDetailList);

                if (!resultResponse.IsSuccess || resultResponse.IsInfo)
                {
                    resultResponse.IsSuccess = false;
                    return resultResponse;
                }

                objHeader = (Quotation)resultResponse.Data!;
                objDetailList = (List<QuotationDetail>)resultResponse.DataList!;

                foreach (var detail in objDetailList.OrderBy(x => x.QuotationDetailType))
                {
                    if (isReclosed)
                    {
                        if (detail is { BankTransactionId: not null })
                        {
                            //Eliminamos las transacciones relacionadas
                            resultResponse = await
                                fnDeleteTransactions(detail.BankTransactionId!.Value);

                            if (!resultResponse.IsSuccess)
                            {
                                resultResponse.IsSuccess = false;
                                return resultResponse;
                            }

                            detail.JournalEntryId = null;
                            detail.IsJournalEntryPosted = false;
                            detail.BankTransactionId = null;
                            detail.IsBankTransactionPosted = false;
                            detail.JournalEntryTransferFeeId = null;
                            detail.IsJournalEntryTransferFeePosted = false;
                            detail.BankTransactionTransferFeeId = null;
                            detail.IsBankTransactionTransferFeePosted = false;
                        }
                    }

                    //*:*Compra o Venta
                    if (objHeader.TypeNumeral != SD.QuotationType.Transport)
                    {
                        if (detail.QuotationDetailType == QuotationDetailType.Deposit)
                        {
                            resultResponse = await
                                fnLogicDeposit(objHeader, detail, configBcoDto);

                            if (!resultResponse.IsSuccess || resultResponse.Data == null)
                            {
                                resultResponse.IsSuccess = false;
                                return resultResponse;
                            }

                            var detailUpdate = (QuotationDetail)resultResponse.Data;
                            detailUpdate.UpdatedBy = _userName ?? AC.LOCALHOSTME;
                            detailUpdate.UpdatedDate = DateTime.UtcNow;
                            detailUpdate.UpdatedHostName = AC.LOCALHOSTPC;
                            detailUpdate.UpdatedIpv4 = _ipAddress?.ToString() ?? AC.Ipv4Default;
                            _uow.QuotationDetail.Update(detailUpdate);
                            _uow.Save();
                        }
                        else if (detail.QuotationDetailType == QuotationDetailType.Transfer)
                        {
                            resultResponse = await
                                fnLogicTransfer(objHeader, detail, configBcoDto, configCntDto);

                            if (!resultResponse.IsSuccess || resultResponse.Data == null)
                            {
                                resultResponse.IsSuccess = false;
                                return resultResponse;
                            }

                            var detailUpdate = (QuotationDetail)resultResponse.Data;
                            detailUpdate.UpdatedBy = _userName ?? AC.LOCALHOSTME;
                            detailUpdate.UpdatedDate = DateTime.UtcNow;
                            detailUpdate.UpdatedHostName = AC.LOCALHOSTPC;
                            detailUpdate.UpdatedIpv4 = _ipAddress?.ToString() ?? AC.Ipv4Default;
                            _uow.QuotationDetail.Update(detailUpdate);
                            _uow.Save();
                        }
                    }
                    else
                    {
                        if (detail.QuotationDetailType == QuotationDetailType.CreditTransfer)
                        {
                            resultResponse = await
                                fnLogicCreditTransfer(objHeader, detail, configBcoDto);

                            if (!resultResponse.IsSuccess || resultResponse.Data == null)
                            {
                                resultResponse.IsSuccess = false;
                                return resultResponse;
                            }

                            var detailUpdate = (QuotationDetail)resultResponse.Data;
                            detailUpdate.UpdatedBy = _userName ?? AC.LOCALHOSTME;
                            detailUpdate.UpdatedDate = DateTime.UtcNow;
                            detailUpdate.UpdatedHostName = AC.LOCALHOSTPC;
                            detailUpdate.UpdatedIpv4 = _ipAddress?.ToString() ?? AC.Ipv4Default;
                            _uow.QuotationDetail.Update(detailUpdate);
                            _uow.Save();

                            objCreditId = detailUpdate.BankTransactionId;
                        }
                        else if (detail.QuotationDetailType == QuotationDetailType.DebitTransfer)
                        {
                            resultResponse = await
                                fnLogicDebitTransfer(objHeader, detail, configBcoDto);

                            if (!resultResponse.IsSuccess || resultResponse.Data == null)
                            {
                                resultResponse.IsSuccess = false;
                                return resultResponse;
                            }

                            var detailUpdate = (QuotationDetail)resultResponse.Data;

                            if (objHeader.AmountCommission != 0)
                            {
                                resultResponse = await
                                    fnLogicDebitTransferCommision(objHeader, detail, configBcoDto);

                                if (!resultResponse.IsSuccess || resultResponse.Data == null)
                                {
                                    resultResponse.IsSuccess = false;
                                    return resultResponse;
                                }

                                detailUpdate = (QuotationDetail)resultResponse.Data;
                            }

                            detailUpdate.UpdatedBy = _userName ?? AC.LOCALHOSTME;
                            detailUpdate.UpdatedDate = DateTime.UtcNow;
                            detailUpdate.UpdatedHostName = AC.LOCALHOSTPC;
                            detailUpdate.UpdatedIpv4 = _ipAddress?.ToString() ?? AC.Ipv4Default;
                            _uow.QuotationDetail.Update(detailUpdate);
                            _uow.Save();

                            objDebitId = detailUpdate.BankTransactionId;
                            objCommisionId = detailUpdate.BankTransactionTransferFeeId;

                            resultResponse = await
                                fnCreateTransactionBcoRel(objDebitId.Value, objCreditId.Value, objCommisionId);

                            if (!resultResponse.IsSuccess)
                            {
                                resultResponse.IsSuccess = false;
                                return resultResponse;
                            }
                        }
                    }
                }
            }

            //Seteamos campos de auditoria
            objHeader.UpdatedBy = _userName ?? AC.LOCALHOSTME;
            objHeader.UpdatedDate = DateTime.UtcNow;
            objHeader.UpdatedHostName = AC.LOCALHOSTPC;
            objHeader.UpdatedIpv4 = _ipAddress?.ToString() ?? AC.Ipv4Default;
            _uow.Quotation.Update(objHeader);
            _uow.Save();

            resultResponse.IsSuccess = true;
            return resultResponse;
        }
        catch (Exception ex)
        {
            resultResponse.IsSuccess = false;
            resultResponse.ErrorMessages = ex.Message + $"{objHeader.Id} {objHeader.TypeTrx.Code}-#{objHeader.Numeral}";
            return resultResponse;
        }
    }

    private async Task<ResultResponse> fnCheckChildrenPosted(
        Quotation objHeader, List<QuotationDetail> objDetailList)
    {
        ResultResponse? resultResponse = new() { IsSuccess = true };
        StringBuilder errorsMessagesBuilder = new();
        var isPosted = true;
        List<string> errorsMessages = new();

        try
        {
            //Actualizar en rango los detalles
            foreach (var detail in objDetailList)
            {
                string typeName = string.Empty;

                switch (detail.QuotationDetailType)
                {
                    case QuotationDetailType.Deposit:
                        typeName = QuotationDetailTypeName[(short)QuotationDetailType.Deposit];
                        break;
                    case QuotationDetailType.Transfer:
                        typeName = QuotationDetailTypeName[(short)QuotationDetailType.Transfer];
                        break;
                    case QuotationDetailType.DebitTransfer:
                        typeName = QuotationDetailTypeName[(short)QuotationDetailType.DebitTransfer];
                        break;
                    case QuotationDetailType.CreditTransfer:
                        typeName = QuotationDetailTypeName[(short)QuotationDetailType.CreditTransfer];
                        break;
                }

                if (detail.BankTransactionId.HasValue)
                {
                    var srvResponse = await _srvTransaBco.GetIsAprovalAsync<APIResponse>(_sessionToken, detail.BankTransactionId!.Value);
                    if (srvResponse is null)
                    {
                        resultResponse.IsSuccess = false;
                        resultResponse.ErrorMessages = "No se pudo obtener la respuesta";
                        return resultResponse;
                    }

                    if (srvResponse is { isSuccess: true })
                    {
                        var isaproval = JsonConvert.DeserializeObject<bool>(Convert.ToString(srvResponse.result));

                        if (isaproval)
                        {
                            detail.IsBankTransactionPosted = true;
                            var informationMessage = $" {typeName} {detail.BankSourceTrx.Code} {detail.AmountDetail.ToString(AC.DecimalTransaFormat)} - COT: {objHeader.Id} {objHeader.TypeTrx.Code}-#{objHeader.Numeral}.";
                            if (errorsMessages.Count == 0)
                            {
                                errorsMessages.Add($"{informationMessage}");
                            }
                            else
                            {
                                errorsMessages.Add($"{informationMessage} {Environment.NewLine}");
                            }
                        }
                        else
                        {
                            detail.IsBankTransactionPosted = false;
                            isPosted = false;
                        }
                    }
                    else if (srvResponse is { isSuccess: false })
                    {
                        errorsMessagesBuilder.AppendJoin("", srvResponse.errorMessages);
                        resultResponse.IsSuccess = false;
                        resultResponse.ErrorMessages = errorsMessagesBuilder.ToString();
                        return resultResponse;
                    }
                }
                else
                {
                    detail.IsBankTransactionPosted = false;
                    isPosted = false;
                }

                detail.UpdatedBy = _userName ?? AC.LOCALHOSTME;
                detail.UpdatedDate = DateTime.UtcNow;
                detail.UpdatedHostName = AC.LOCALHOSTPC;
                detail.UpdatedIpv4 = _ipAddress?.ToString() ?? AC.Ipv4Default;
                _uow.QuotationDetail.Update(detail);
                _uow.Save();
            }

            if (!isPosted)
            {
                if (errorsMessages.Count != 0)
                {
                    resultResponse.IsSuccess = true;
                    resultResponse.IsInfo = true;
                    resultResponse.Data = errorsMessages;
                    return resultResponse;
                }
            }
            else
            {
                objHeader.IsPosted = true;
                objHeader.UpdatedBy = _userName ?? AC.LOCALHOSTME;
                objHeader.UpdatedDate = DateTime.UtcNow;
                objHeader.UpdatedHostName = AC.LOCALHOSTPC;
                objHeader.UpdatedIpv4 = _ipAddress?.ToString() ?? AC.Ipv4Default;
                _uow.Quotation.Update(objHeader);
                _uow.Save();
                resultResponse.IsSuccess = false;
                resultResponse.IsInfo = true;
                resultResponse.ErrorMessages =
                    $"Verificamos que la cotización: {objHeader.Id} {objHeader.TypeTrx.Code}-#{objHeader.Numeral} está contabilizada, por lo que no se podrá re-cerrar";
                return resultResponse;
            }

            resultResponse.Data = objHeader;
            resultResponse.DataList = objDetailList;
            resultResponse.IsSuccess = true;
            return resultResponse;
        }
        catch (Exception ex)
        {
            resultResponse.IsSuccess = false;
            resultResponse.ErrorMessages = ex.Message;
            return resultResponse;
        }
    }

    private async Task<ResultResponse> fnDeleteTransactions(Guid transaId)
    {
        ResultResponse? resultResponse = new() { IsSuccess = true };
        StringBuilder errorsMessagesBuilder = new();

        try
        {
            var srvResponse = await _srvTransaBco.DeleteAsync<APIResponse>(_sessionToken, transaId);
            if (srvResponse is null)
            {
                resultResponse.IsSuccess = false;
                resultResponse.ErrorMessages = "No se pudo obtener la respuesta";
                return resultResponse;
            }
            else if (srvResponse is { isSuccess: false })
            {
                errorsMessagesBuilder.AppendJoin("", srvResponse.errorMessages);
                resultResponse.IsSuccess = false;
                resultResponse.ErrorMessages = errorsMessagesBuilder.ToString();
                return resultResponse;
            }

            resultResponse.IsSuccess = true;
            return resultResponse;
        }
        catch (Exception ex)
        {
            resultResponse.IsSuccess = false;
            resultResponse.ErrorMessages = ex.Message;
            return resultResponse;
        }
    }

    private async Task<ResultResponse> fnLogicDeposit(Quotation objHeader, QuotationDetail detail
        , ConfigBcoDto configBcoDto)
    {
        ResultResponse? resultResponse = new() { IsSuccess = true };

        try
        {
            BancosDto? bankDto = new();
            CuentasBancariasDto? bankAccountDto = new();
            TransaccionResponse? transaResponse = new();
            TransaccionesBcoDto? transaBcoDto = new();
            AsientosContablesDto? asientoDto = new();
            TransaccionesBcoDetalleDto? transaBcoDetalleDto = new();
            List<TransaccionesBcoDetalleDto>? transaBcoDetalleDtoList = new();
            ModulosDto? moduloDto = new();
            ModulosDocumentosDto? moduloDocumentoDto = new();

            resultResponse = await fnGetBank(detail.BankSourceTrx.Code);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            bankDto = (BancosDto)resultResponse.Data;

            resultResponse = await fnGetBankAccount(
                bankDto.Codigo,
                detail.BankSourceTrx.BankAccountExcludeUId,
                (short)detail.CurrencyDetailTrx.Numeral,
                detail.CurrencyDetailTrx.Abbreviation);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            bankAccountDto = (CuentasBancariasDto)resultResponse.Data;

            resultResponse = await fnGetDocument((int)mexModules.Bank, (int)mexModuleBankDocument.Deposit);

            if (!resultResponse.IsSuccess || resultResponse.Data == null || resultResponse.DataChildren == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            moduloDto = (ModulosDto)resultResponse.Data;
            moduloDocumentoDto = (ModulosDocumentosDto)resultResponse.DataChildren;

            short tipo = (short)(TransaccionBcoTipo.Deposito);
            short subtipo = (short)(TransaccionBcoDepositoSubtipo.Deposito);

            resultResponse = await fnGetNextSecuential(
                mexModules.Bank,
                bankAccountDto.UidRegist,
                objHeader.DateTransa.Year,
                objHeader.DateTransa.Month, tipo, subtipo);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            transaResponse = (TransaccionResponse)resultResponse.Data;

            string numberTransaCnt = string.Empty;

            resultResponse = await fnGetNextSecuential(
                mexModules.Account,
                bankAccountDto.UidRegist,
                objHeader.DateTransa.Year,
                objHeader.DateTransa.Month, tipo, subtipo);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            numberTransaCnt = (string)resultResponse.Data;

            decimal exchangeRateOficial = objHeader.ExchangeRateOfficialReal;

            decimal exchangeRateTransa = objHeader.TypeNumeral switch
            {
                SD.QuotationType.Sell => objHeader.ExchangeRateSellReal,
                SD.QuotationType.Buy => objHeader.ExchangeRateBuyReal,
                _ => objHeader.ExchangeRateOfficialReal
            };

            decimal exchangeRate = objHeader.TypeNumeral switch
            {
                SD.QuotationType.Sell => exchangeRateTransa,
                SD.QuotationType.Buy => exchangeRateOficial,
                _ => objHeader.ExchangeRateOfficialReal
            };

            ConverterExchange cvtExc = new();

            var mtosExc = cvtExc.ConverterExchangeTo((CurrencyType)detail.CurrencyDetailTrx.Numeral, detail.AmountDetail,
                exchangeRate, exchangeRate,
                decimalTrx: AC.DecimalTransa);

            if (objHeader.TypeNumeral == SD.QuotationType.Sell)
            {
                if (objHeader.CurrencyTransaType == CurrencyType.Additional)
                {
                    if (objHeader.CurrencyDepositType == CurrencyType.Foreign)
                    {
                        exchangeRate = objHeader.ExchangeRateOfficialBase;

                        mtosExc = cvtExc.ConverterExchangeTo((CurrencyType)detail.CurrencyDetailTrx.Numeral, detail.AmountDetail,
                            exchangeRate, exchangeRateTransa,
                            decimalTrx: AC.DecimalTransa);
                    }
                }
            }
            else if (objHeader.TypeNumeral == SD.QuotationType.Buy)
            {
                if (objHeader.CurrencyTransaType == CurrencyType.Additional)
                {
                    if (objHeader.CurrencyTransferType == CurrencyType.Base)
                    {
                        exchangeRate = objHeader.ExchangeRateOfficialBase;

                        exchangeRateTransa = objHeader.ExchangeRateOfficialReal;

                        mtosExc = cvtExc.ConverterExchangeTo((CurrencyType)detail.CurrencyDetailTrx.Numeral, detail.AmountDetail,
                            exchangeRate, exchangeRateTransa,
                            decimalTrx: AC.DecimalTransa);
                    }
                }
            }

            var numberTransaFull =
                $"MC-{objHeader.TypeTrx.Code}-{objHeader.Numeral.ToString().PadLeft(3, AC.CharDefaultEmpty)}";


            TransaccionesBcoDtoCreate transaBco = new()
            {
                IndMesaDeCambio = true,
                IndConciliable = true,
                IndConciliado = false,
                IndCompensado = false,
                IndRetencion = false,
                IndImpresoCheque = false,
                IndImpresoComprobante = false,
                IndFlotante = false,
                IndOkay = true,
                IndTransaccionInicial = false,
                Comentarios = $"{numberTransaFull} - D{detail.LineNumber} - {objHeader.CurrencyTransaTrx.Abbreviation} {objHeader.AmountTransaction.ToString(AC.DecimalTransaFormat)} - {objHeader.CustomerTrx.CommercialName}",
                FechaTransa = objHeader.DateTransa.ToDateTimeConvert(),
                MesFiscal = (short)objHeader.DateTransa.Month,
                YearFiscal = (short)objHeader.DateTransa.Year,
                YearMonthFiscal = $"{objHeader.DateTransa.Year}{objHeader.DateTransa.Month.ToString().PadLeft(2, AC.CharDefaultEmpty)}",
                UidBanco = bankDto.UidRegist,
                UidCuentaBancaria = bankAccountDto.UidRegist,
                UidTipo = transaResponse.TipoId,
                UidSubtipo = transaResponse.SubtipoId,
                NumeroLineas = 2,
                NumeroTransaccion = transaResponse.NumberTransa,
                NumeroMoneda = (short)detail.CurrencyDetailTrx.Numeral,
                NumeroObjeto = (int)mexBankObjects.Transaction,
                NumeroEstado = (int)mexBankTransactionStages.Draft,
                NumeroTransaccionRef = numberTransaFull,
                TipoCambioMonfor = exchangeRate,
                TipoCambioMonxtr = exchangeRateTransa,
                TipoCambioParaMonfor = exchangeRate,
                TipoCambioParaMonxtr = exchangeRateTransa,
                MontoMonbas = mtosExc.AmountBase,
                MontoMonfor = mtosExc.AmountForeign,
                MontoMonxtr = mtosExc.AmountAdditional,
                MontoDebitoMonbas = mtosExc.AmountBase,
                MontoDebitoMonfor = mtosExc.AmountForeign,
                MontoDebitoMonxtr = mtosExc.AmountAdditional,
                MontoCreditoMonbas = mtosExc.AmountBase,
                MontoCreditoMonfor = mtosExc.AmountForeign,
                MontoCreditoMonxtr = mtosExc.AmountAdditional,
                TransaMcRelacionada = detail.Id,
                TransaMcRelacionadaParent = objHeader.Id,
                SerieInterna = "B"
            };

            //mtosExc.SetInit(); // Limpiar

            //Crear cabecera
            resultResponse = await
                fnCreateTransactionBcoHeader(transaBco);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            transaBcoDto = (TransaccionesBcoDto)resultResponse.Data!;

            detail.BankTransactionId = transaBcoDto.UidRegist;
            detail.IsBankTransactionPosted = false;

            //Creamos el primer detalle
            TransaccionesBcoDetalleDtoCreate transaBcoDetalle = new()
            {
                UidDocumento = moduloDocumentoDto.UidRegist,
                CodigoDocumento = moduloDocumentoDto.Codigo,
                UidRegistPad = transaBcoDto.UidRegist,
                UidCuentaContable = bankAccountDto.UidCuentaContable!.Value,
                NumeroLinea = 1,
                TipoMovimiento = (short)mexAccountMovementType.Debit,
                TipoCambioMonfor = transaBcoDto.TipoCambioMonfor,
                TipoCambioMonxtr = transaBcoDto.TipoCambioMonxtr,
                MontoMonbas = mtosExc.AmountBase,
                MontoMonfor = mtosExc.AmountForeign,
                MontoMonxtr = mtosExc.AmountAdditional,
                IndDiferencial = false,
                InddeCuadratura = false,
                TipoRegistro = (short)mexBankDetailType.AutomaticCounterPart
            };

            resultResponse = await
                fnCreateTransactionBcoDetail(transaBcoDetalle);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            transaBcoDetalleDto = (TransaccionesBcoDetalleDto)resultResponse.Data!;

            //Agg a la lista
            transaBcoDetalleDtoList.Add(transaBcoDetalleDto);

            //Creamos el segundo detalle
            transaBcoDetalle = new()
            {
                UidRegist = Guid.Empty,
                UidDocumento = moduloDocumentoDto.UidRegist,
                CodigoDocumento = moduloDocumentoDto.Codigo,
                UidRegistPad = transaBcoDto.UidRegist,
                UidCuentaContable = configBcoDto.CuentacontableInterfaz!.Value,
                NumeroLinea = 2,
                TipoMovimiento = (short)mexAccountMovementType.Credit,
                TipoCambioMonfor = transaBcoDto.TipoCambioMonfor,
                TipoCambioMonxtr = transaBcoDto.TipoCambioMonxtr,
                MontoMonbas = mtosExc.AmountBase,
                MontoMonfor = mtosExc.AmountForeign,
                MontoMonxtr = mtosExc.AmountAdditional,
                IndDiferencial = false,
                InddeCuadratura = false,
                TipoRegistro = (short)mexBankDetailType.AutomaticCounterPart
            };

            resultResponse = await
                fnCreateTransactionBcoDetail(transaBcoDetalle);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            transaBcoDetalleDto = (TransaccionesBcoDetalleDto)resultResponse.Data!;

            //Agg a la lista
            transaBcoDetalleDtoList.Add(transaBcoDetalleDto);

            //Creamos el comprobante
            AsientosContablesDtoCreate asientoCnt = new()
            {
                UidModuloDocumento = moduloDocumentoDto.UidRegist,
                UidModulo = moduloDto.UidRegist,
                UidCia = transaBcoDto.UidCia,
                Comentarios = transaBcoDto.Comentarios,
                FechaTransa = transaBcoDto.FechaTransa,
                MesFiscal = transaBcoDto.MesFiscal,
                YearFiscal = transaBcoDto.YearFiscal,
                TipoCambioMonfor = transaBcoDto.TipoCambioMonfor,
                TipoCambioMonxtr = transaBcoDto.TipoCambioMonxtr,
                TipoCambioParaMonfor = transaBcoDto.TipoCambioParaMonfor,
                TipoCambioParaMonxtr = transaBcoDto.TipoCambioParaMonxtr,
                MontoCreditoMonbas = transaBcoDto.MontoCreditoMonbas,
                MontoCreditoMonfor = transaBcoDto.MontoCreditoMonfor,
                MontoCreditoMonxtr = transaBcoDto.MontoCreditoMonxtr,
                MontoDebitoMonbas = transaBcoDto.MontoDebitoMonbas,
                MontoDebitoMonfor = transaBcoDto.MontoDebitoMonfor,
                MontoDebitoMonxtr = transaBcoDto.MontoDebitoMonxtr,
                NumeroLineas = transaBcoDto.NumeroLineas,
                NumeroMoneda = transaBcoDto.NumeroMoneda,
                NumeroObjeto = (int)mexJournalObjects.JournalEntry,
                NumeroEstado = (int)mexJournalEntryStages.Draft,
                SerieInterna = transaBcoDto.SerieInterna,
                NumeroTransaccionRef = transaBcoDto.NumeroTransaccionRef,
                NumeroTransaccion = numberTransaCnt,
                IndOkay = true
            };

            //Crear cabecera
            resultResponse = await
                fnCreateJournalHeader(asientoCnt);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            asientoDto = (AsientosContablesDto)resultResponse.Data!;

            //Agg el id al detalle de la cotización
            detail.JournalEntryId = asientoDto.UidRegist;
            detail.IsJournalEntryPosted = false;

            foreach (var detailTransa in transaBcoDetalleDtoList)
            {
                //Creamos los detalles del comprobante en base a los detalles de la transaBco
                AsientosContablesDetalleDtoCreate asientoDetalle = new()
                {
                    UidDocumento = moduloDocumentoDto.UidRegist,
                    CodigoDocumento = moduloDocumentoDto.Codigo,
                    UidRegistPad = asientoDto.UidRegist,
                    UidCuentaContable = detailTransa.UidCuentaContable,
                    NumeroLinea = detailTransa.NumeroLinea,
                    TipoMovimiento = detailTransa.TipoMovimiento,
                    TipoCambioMonfor = detailTransa.TipoCambioMonfor,
                    TipoCambioMonxtr = detailTransa.TipoCambioMonxtr,
                    MontoMonbas = detailTransa.MontoMonbas,
                    MontoMonfor = detailTransa.MontoMonfor,
                    MontoMonxtr = detailTransa.MontoMonxtr,
                    IndDiferencial = false,
                    InddeCuadratura = false
                };

                resultResponse = await
                    fnCreateJournalDetail(asientoDetalle);

                if (!resultResponse.IsSuccess)
                {
                    resultResponse.IsSuccess = false;
                    return resultResponse;
                }
            }

            //Actualizar la transaccion bancaria con la referencia del comprobante
            transaBcoDto.UidAsientoContable = asientoDto.UidRegist;

            var transaBcoDtoUpdate = _mapper.Map<TransaccionesBcoDtoUpdate>(transaBcoDto);
            resultResponse = await
                fnUpdateTransactionBcoHeader(transaBcoDtoUpdate);

            if (!resultResponse.IsSuccess)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            resultResponse.IsSuccess = true;
            resultResponse.Data = detail;

            return resultResponse;
        }
        catch (Exception ex)
        {
            resultResponse.IsSuccess = false;
            resultResponse.ErrorMessages = ex.Message;
            return resultResponse;
        }
    }

    private async Task<ResultResponse> fnLogicTransfer(Quotation objHeader, QuotationDetail detail
        , ConfigBcoDto configBcoDto, ConfigCntDto configCntDto)
    {
        ResultResponse? resultResponse = new() { IsSuccess = true };

        try
        {
            BancosDto? bankDto = new();
            CuentasBancariasDto? bankAccountDto = new();
            TransaccionResponse? transaResponse = new();
            TransaccionesBcoDto? transaBcoDto = new();
            AsientosContablesDto? asientoDto = new();
            TransaccionesBcoDetalleDto? transaBcoDetalleDto = new();
            List<TransaccionesBcoDetalleDto>? transaBcoDetalleDtoList = new();
            ModulosDto? moduloDto = new();
            ModulosDocumentosDto? moduloDocumentoDto = new();
            int lineCounter = 0;
            resultResponse = await fnGetBank(detail.BankSourceTrx.Code);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            bankDto = (BancosDto)resultResponse.Data;

            resultResponse = await fnGetBankAccount(
                bankDto.Codigo,
                detail.BankSourceTrx.BankAccountExcludeUId,
                (short)detail.CurrencyDetailTrx.Numeral,
                detail.CurrencyDetailTrx.Abbreviation);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            bankAccountDto = (CuentasBancariasDto)resultResponse.Data;

            resultResponse = await fnGetDocument((int)mexModules.Bank, (int)mexModuleBankDocument.Check);

            if (!resultResponse.IsSuccess || resultResponse.Data == null || resultResponse.DataChildren == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            moduloDto = (ModulosDto)resultResponse.Data;
            moduloDocumentoDto = (ModulosDocumentosDto)resultResponse.DataChildren;

            short tipo = (short)(TransaccionBcoTipo.Pago);
            short subtipo = (short)(TransaccionBcoPagoSubtipo.MesaCambio);

            resultResponse = await fnGetNextSecuential(
                mexModules.Bank,
                bankAccountDto.UidRegist,
                objHeader.DateTransa.Year,
                objHeader.DateTransa.Month,
                tipo,
                subtipo);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            transaResponse = (TransaccionResponse)resultResponse.Data;

            string numberTransaCnt = string.Empty;

            resultResponse = await fnGetNextSecuential(
                mexModules.Account,
                bankAccountDto.UidRegist,
                objHeader.DateTransa.Year,
                objHeader.DateTransa.Month, tipo, subtipo);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            numberTransaCnt = (string)resultResponse.Data;

            decimal exchangeRateOficial = objHeader.ExchangeRateOfficialReal;

            decimal exchangeRateTransa = objHeader.TypeNumeral switch
            {
                SD.QuotationType.Sell => objHeader.ExchangeRateSellReal,
                SD.QuotationType.Buy => objHeader.ExchangeRateBuyReal,
                _ => objHeader.ExchangeRateOfficialReal
            };

            decimal exchangeRate = objHeader.TypeNumeral switch
            {
                SD.QuotationType.Sell => exchangeRateOficial,
                SD.QuotationType.Buy => exchangeRateTransa,
                _ => objHeader.ExchangeRateOfficialReal
            };

            ConverterExchange cvtExc = new();

            var mtosExc = cvtExc.ConverterExchangeTo((CurrencyType)detail.CurrencyDetailTrx.Numeral, detail.AmountDetail,
                exchangeRate, exchangeRate,
                decimalTrx: AC.DecimalTransa);

            if (objHeader.TypeNumeral == SD.QuotationType.Sell)
            {
                if (objHeader.CurrencyTransaType == CurrencyType.Additional)
                {
                    if (objHeader.CurrencyDepositType == CurrencyType.Foreign)
                    {
                        exchangeRateOficial = objHeader.ExchangeRateOfficialBase;
                        exchangeRateTransa = objHeader.ExchangeRateOfficialReal;

                        mtosExc = cvtExc.ConverterExchangeTo((CurrencyType)detail.CurrencyDetailTrx.Numeral, detail.AmountDetail,
                            objHeader.ExchangeRateOfficialBase, exchangeRateTransa,
                            decimalTrx: AC.DecimalTransa);
                    }
                }
            }
            else if (objHeader.TypeNumeral == SD.QuotationType.Buy)
            {
                if (objHeader.CurrencyTransaType == CurrencyType.Additional)
                {
                    if (objHeader.CurrencyTransferType == CurrencyType.Base)
                    {
                        exchangeRateOficial = objHeader.ExchangeRateOfficialBase;
                        exchangeRateTransa = objHeader.ExchangeRateBuyReal;

                        mtosExc = cvtExc.ConverterExchangeTo((CurrencyType)detail.CurrencyDetailTrx.Numeral, detail.AmountDetail,
                            objHeader.ExchangeRateOfficialBase, exchangeRateTransa,
                            decimalTrx: AC.DecimalTransa);
                    }
                }
            }

            var numberTransaFull =
                $"MC-{objHeader.TypeTrx.Code}-{objHeader.Numeral.ToString().PadLeft(3, AC.CharDefaultEmpty)}";

            //Creamos transaccion bancaria
            TransaccionesBcoDtoCreate transaBco = new()
            {
                IndMesaDeCambio = true,
                IndConciliable = true,
                IndConciliado = false,
                IndCompensado = false,
                IndRetencion = false,
                IndImpresoCheque = false,
                IndImpresoComprobante = false,
                IndFlotante = false,
                IndOkay = true,
                IndTransaccionInicial = false,
                Comentarios = $"{numberTransaFull} - T{detail.LineNumber} - {objHeader.CurrencyTransaTrx.Abbreviation} {objHeader.AmountTransaction.ToString(AC.DecimalTransaFormat)} - {objHeader.CustomerTrx.CommercialName}",
                FechaTransa = objHeader.DateTransa.ToDateTimeConvert(),
                MesFiscal = (short)objHeader.DateTransa.Month,
                YearFiscal = (short)objHeader.DateTransa.Year,
                YearMonthFiscal = $"{objHeader.DateTransa.Year}{objHeader.DateTransa.Month.ToString().PadLeft(2, AC.CharDefaultEmpty)}",
                UidBanco = bankDto.UidRegist,
                UidCuentaBancaria = bankAccountDto.UidRegist,
                UidTipo = transaResponse.TipoId,
                UidSubtipo = transaResponse.SubtipoId,
                NumeroLineas = 2,
                NumeroTransaccion = transaResponse.NumberTransa,
                NumeroMoneda = (short)detail.CurrencyDetailTrx.Numeral,
                NumeroObjeto = (int)mexBankObjects.Transaction,
                NumeroEstado = (int)mexBankTransactionStages.Draft,
                NumeroTransaccionRef = numberTransaFull,
                TipoCambioMonfor = exchangeRateOficial,
                TipoCambioMonxtr = exchangeRateOficial,
                TipoCambioParaMonfor = exchangeRateTransa,
                TipoCambioParaMonxtr = exchangeRateTransa,
                MontoMonbas = mtosExc.AmountBase,
                MontoMonfor = mtosExc.AmountForeign,
                MontoMonxtr = mtosExc.AmountAdditional,
                SubtotalNetoMonbas = mtosExc.AmountBase,
                SubtotalNetoMonfor = mtosExc.AmountForeign,
                SubtotalNetoMonxtr = mtosExc.AmountAdditional,
                TotalMonbas = mtosExc.AmountBase,
                TotalMonfor = mtosExc.AmountForeign,
                TotalMonxtr = mtosExc.AmountAdditional,
                MontoDebitoMonbas = mtosExc.AmountBase,
                MontoDebitoMonfor = mtosExc.AmountForeign,
                MontoDebitoMonxtr = mtosExc.AmountAdditional,
                MontoCreditoMonbas = mtosExc.AmountBase,
                MontoCreditoMonfor = mtosExc.AmountForeign,
                MontoCreditoMonxtr = mtosExc.AmountAdditional,
                TransaMcRelacionada = detail.Id,
                TransaMcRelacionadaParent = objHeader.Id,
                SerieInterna = "B",
                TipoBeneficiario = (short)mexBeneficiaryTypeNumber.Customer,
                UidBeneficiario = Guid.Parse(_eSiafN4BeneficiaryUid),
                UidEntidad = Guid.Parse(_eSiafN4EntityUid),
                ImprimirChequeaNombrede = _eSiafN4CustomerName,
                NumeroIdentificacion = _eSiafN4CustomerIdentificationNumber
            };

            //Crear cabecera
            resultResponse = await
                fnCreateTransactionBcoHeader(transaBco);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            transaBcoDto = (TransaccionesBcoDto)resultResponse.Data!;

            //Agg referencia al detalle de la cotización
            detail.BankTransactionId = transaBcoDto.UidRegist;
            detail.IsBankTransactionPosted = false;

            //Creamos el primer detalle
            TransaccionesBcoDetalleDtoCreate transaBcoDetalle = new()
            {
                UidDocumento = moduloDocumentoDto.UidRegist,
                CodigoDocumento = moduloDocumentoDto.Codigo,
                UidRegistPad = transaBcoDto.UidRegist,
                UidCuentaContable = bankAccountDto.UidCuentaContable!.Value,
                NumeroLinea = ++lineCounter,
                TipoMovimiento = (short)mexAccountMovementType.Credit,
                TipoCambioMonfor = transaBcoDto.TipoCambioMonfor,
                TipoCambioMonxtr = transaBcoDto.TipoCambioMonxtr,
                MontoMonbas = mtosExc.AmountBase,
                MontoMonfor = mtosExc.AmountForeign,
                MontoMonxtr = mtosExc.AmountAdditional,
                IndDiferencial = false,
                InddeCuadratura = false,
                TipoRegistro = (short)mexBankDetailType.AutomaticCounterPart,
                UidBeneficiario = transaBcoDto.UidBeneficiario,
                UidEntidad = transaBcoDto.UidEntidad
            };

            mtosExc.SetInit(); // Limpiar

            resultResponse = await
                fnCreateTransactionBcoDetail(transaBcoDetalle);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            transaBcoDetalleDto = (TransaccionesBcoDetalleDto)resultResponse.Data!;

            //Agg al detalle
            transaBcoDetalleDtoList.Add(transaBcoDetalleDto);

            if (objHeader.TypeNumeral == SD.QuotationType.Sell)
            {
                //Venta de dolares 
                if (objHeader.CurrencyTransaType == CurrencyType.Foreign)
                {
                    //Cliente paga en Cordobas
                    if (objHeader.CurrencyDepositType == CurrencyType.Base)
                    {
                        exchangeRate = exchangeRateTransa;

                        mtosExc = cvtExc.ConverterExchangeTo((CurrencyType)detail.CurrencyDetailTrx.Numeral, detail.AmountDetail,
                            exchangeRate, exchangeRate,
                            decimalTrx: AC.DecimalTransa);
                    }
                }//Venta de Euros
                else if (objHeader.CurrencyTransaType == CurrencyType.Additional)
                {
                    //Cliente paga en Cordobas
                    if (objHeader.CurrencyDepositType == CurrencyType.Base)
                    {
                        exchangeRate = exchangeRateTransa;

                        mtosExc = cvtExc.ConverterExchangeTo((CurrencyType)detail.CurrencyDetailTrx.Numeral, detail.AmountDetail,
                            exchangeRate, exchangeRate,
                            decimalTrx: AC.DecimalTransa);
                    }//Cliente paga en Dolares
                    else if (objHeader.CurrencyDepositType == CurrencyType.Foreign)
                    {
                        mtosExc = cvtExc.ConverterExchangeTo((CurrencyType)detail.CurrencyDetailTrx.Numeral, detail.AmountDetail,
                            objHeader.ExchangeRateOfficialBase, objHeader.ExchangeRateSellReal,
                            decimalTrx: AC.DecimalTransa);
                    }
                }
            }
            else if (objHeader.TypeNumeral == SD.QuotationType.Buy)
            {
                if (objHeader.CurrencyTransaType == CurrencyType.Foreign)
                {
                    if (objHeader.CurrencyTransferType == CurrencyType.Base)
                    {
                        exchangeRate = exchangeRateOficial;

                        mtosExc = cvtExc.ConverterExchangeTo(CurrencyType.Foreign, transaBcoDetalleDto.MontoMonfor,
                            exchangeRate, exchangeRate,
                            decimalTrx: AC.DecimalTransa);
                    }
                }
                else if (objHeader.CurrencyTransaType == CurrencyType.Additional)
                {
                    if (objHeader.CurrencyTransferType == CurrencyType.Base)
                    {
                        exchangeRateOficial = objHeader.ExchangeRateOfficialBase;
                        exchangeRateTransa = objHeader.ExchangeRateOfficialReal;

                        mtosExc = cvtExc.ConverterExchangeTo(CurrencyType.Additional, transaBcoDetalleDto.MontoMonxtr,
                            exchangeRateOficial, exchangeRateTransa,
                            decimalTrx: AC.DecimalTransa);
                    }
                    else if (objHeader.CurrencyTransferType == CurrencyType.Foreign)
                    {

                    }
                }
            }

            //Creamos el segundo detalle
            transaBcoDetalle = new()
            {
                UidRegist = Guid.Empty,
                UidDocumento = moduloDocumentoDto.UidRegist,
                CodigoDocumento = moduloDocumentoDto.Codigo,
                UidRegistPad = transaBcoDto.UidRegist,
                UidCuentaContable = configBcoDto.CuentacontableInterfaz!.Value,
                NumeroLinea = ++lineCounter,
                TipoMovimiento = (short)mexAccountMovementType.Debit,
                TipoCambioMonfor = transaBcoDto.TipoCambioParaMonfor,
                TipoCambioMonxtr = transaBcoDto.TipoCambioParaMonxtr,
                MontoMonbas = mtosExc.AmountBase,
                MontoMonfor = mtosExc.AmountForeign,
                MontoMonxtr = mtosExc.AmountAdditional,
                IndDiferencial = false,
                InddeCuadratura = false,
                TipoRegistro = (short)mexBankDetailType.AutomaticCounterPart,
                UidBeneficiario = transaBcoDto.UidBeneficiario,
                UidEntidad = transaBcoDto.UidEntidad
            };

            resultResponse = await
                fnCreateTransactionBcoDetail(transaBcoDetalle);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            transaBcoDetalleDto = (TransaccionesBcoDetalleDto)resultResponse.Data!;

            //Agg al listado
            transaBcoDetalleDtoList.Add(transaBcoDetalleDto);

            Guid accountId = new Guid();

            short tipoMov = 0;

            //Si tiene ingreso o costo agg el tercer detalle
            if (objHeader.AmountCostReal != 0 || objHeader.AmountRevenueReal != 0)
            {
                bool isIngreso = objHeader.AmountRevenueReal != 0;
                decimal amountContraPart = (isIngreso ? detail.AmountRevenue : detail.AmountCost);

                accountId = (isIngreso
                ? configCntDto.CuentaContableGananciaDiferencial!.Value
                    : configCntDto.CuentaContablePerdidaDiferencial!.Value);

                tipoMov = (isIngreso ? (short)mexAccountMovementType.Credit : (short)mexAccountMovementType.Debit);

                //Creamos el tercer detalle
                transaBcoDetalle = new()
                {
                    UidRegist = Guid.Empty,
                    UidDocumento = moduloDocumentoDto.UidRegist,
                    CodigoDocumento = moduloDocumentoDto.Codigo,
                    UidRegistPad = transaBcoDto.UidRegist,
                    UidCuentaContable = accountId,
                    NumeroLinea = ++lineCounter,
                    TipoMovimiento = tipoMov,
                    MontoMonbas = amountContraPart.RoundTo(AC.DecimalTransa),
                    IndDiferencial = false,
                    InddeCuadratura = false,
                    TipoRegistro = (short)mexBankDetailType.Differential,
                    UidBeneficiario = transaBcoDto.UidBeneficiario,
                    UidEntidad = transaBcoDto.UidEntidad
                };

                if (objHeader.TypeNumeral == SD.QuotationType.Sell)
                {
                    if (objHeader.CurrencyTransaType == CurrencyType.Foreign)
                    {
                        if (objHeader.CurrencyDepositType == CurrencyType.Base)
                        {
                        }
                        else if (objHeader.CurrencyTransaType == CurrencyType.Additional)
                        {
                            if (objHeader.CurrencyDepositType == CurrencyType.Base)
                            {
                            }
                            else if (objHeader.CurrencyDepositType == CurrencyType.Foreign)
                            {
                                decimal amountContraPartForeign = (isIngreso
                                    ? (detail.PercentageCostRevenue * objHeader.AmountRevenue)
                                    : (detail.PercentageCostRevenue * objHeader.AmountCost));

                                if (objHeader.CurrencyTransaType == CurrencyType.Additional)
                                {
                                    transaBcoDetalle.MontoMonfor = amountContraPartForeign.RoundTo(AC.DecimalTransa);
                                }
                                else if (objHeader.CurrencyTransaType == CurrencyType.Foreign)
                                {
                                    transaBcoDetalle.MontoMonxtr = amountContraPartForeign.RoundTo(AC.DecimalTransa);
                                }
                            }
                        }
                    }
                }
                else if (objHeader.TypeNumeral == SD.QuotationType.Buy)
                {
                    if (objHeader.CurrencyTransaType == CurrencyType.Foreign)
                    {
                        if (objHeader.CurrencyTransferType == CurrencyType.Base)
                        {
                        }
                    }
                    else if (objHeader.CurrencyTransaType == CurrencyType.Additional)
                    {
                        if (objHeader.CurrencyTransferType == CurrencyType.Base)
                        {
                            mtosExc = cvtExc.ConverterExchangeTo(CurrencyType.Base, amountContraPart,
                                objHeader.ExchangeRateOfficialBase, objHeader.ExchangeRateOfficialReal,
                                decimalTrx: AC.DecimalTransa);

                            transaBcoDetalle.MontoMonfor = mtosExc.AmountForeign.RoundTo(AC.DecimalTransa);
                        }
                        else if (objHeader.CurrencyTransferType == CurrencyType.Foreign)
                        {
                        }
                    }
                }


                resultResponse = await
                    fnCreateTransactionBcoDetail(transaBcoDetalle);

                if (!resultResponse.IsSuccess || resultResponse.Data == null)
                {
                    resultResponse.IsSuccess = false;
                    return resultResponse;
                }

                transaBcoDetalleDto = (TransaccionesBcoDetalleDto)resultResponse.Data!;

                //Agg al listado
                transaBcoDetalleDtoList.Add(transaBcoDetalleDto);

                //Actualizamos el numero de lineas del padre
                transaBcoDto.NumeroLineas = (short)transaBcoDetalleDtoList.Count;
            }

            //bool isbalanceDifference = false;

            //if ((objHeader.TypeNumeral == SD.QuotationType.Buy)
            //    && (objHeader.CurrencyTransaType == CurrencyType.Foreign)
            //    && (objHeader.CurrencyTransferType == CurrencyType.Base))
            //{
            //    isbalanceDifference = true;
            //}
            //else if ((objHeader.TypeNumeral == SD.QuotationType.Sell)
            //         && (objHeader.CurrencyTransaType == CurrencyType.Foreign)
            //         && (objHeader.CurrencyTransferType == CurrencyType.Foreign))
            //{
            //    isbalanceDifference = true;
            //}

            //if (isbalanceDifference)
            //{

            //}

            var amountDebit = transaBcoDetalleDtoList
                 .Where(x => x.TipoMovimiento == (short)mexAccountMovementType.Debit)
                 .Sum(x => x.MontoMonbas);

            var amountCredit = transaBcoDetalleDtoList
                .Where(x => x.TipoMovimiento == (short)mexAccountMovementType.Credit)
                .Sum(x => x.MontoMonbas);

            var amountDiferential = amountDebit - amountCredit;

            if (amountDiferential != 0)
            {
                accountId = (amountDiferential > 0
                    ? configBcoDto.CuentaContableDifPositivaConciliacion!.Value
                    : configBcoDto.CuentaContableDifNegativaConciliacion!.Value);

                tipoMov = (amountDiferential > 0 ? (short)mexAccountMovementType.Credit : (short)mexAccountMovementType.Debit);

                transaBcoDetalle = new()
                {
                    UidRegist = Guid.Empty,
                    UidDocumento = moduloDocumentoDto.UidRegist,
                    CodigoDocumento = moduloDocumentoDto.Codigo,
                    UidRegistPad = transaBcoDto.UidRegist,
                    UidCuentaContable = accountId,
                    NumeroLinea = ++lineCounter,
                    TipoMovimiento = tipoMov,
                    MontoMonbas = Math.Abs(amountDiferential.RoundTo(AC.DecimalTransa)),
                    IndDiferencial = false,
                    InddeCuadratura = false,
                    TipoRegistro = (short)mexBankDetailType.Differential,
                    UidBeneficiario = transaBcoDto.UidBeneficiario,
                    UidEntidad = transaBcoDto.UidEntidad
                };

                resultResponse = await
                    fnCreateTransactionBcoDetail(transaBcoDetalle);

                if (!resultResponse.IsSuccess || resultResponse.Data == null)
                {
                    resultResponse.IsSuccess = false;
                    return resultResponse;
                }

                transaBcoDetalleDto = (TransaccionesBcoDetalleDto)resultResponse.Data!;

                //Agg al listado
                transaBcoDetalleDtoList.Add(transaBcoDetalleDto);

                //Actualizamos el numero de lineas del padre
                transaBcoDto.NumeroLineas = (short)transaBcoDetalleDtoList.Count;

            }

            amountDebit = transaBcoDetalleDtoList
               .Where(x => x.TipoMovimiento == (short)mexAccountMovementType.Debit)
               .Sum(x => x.MontoMonfor);

            amountCredit = transaBcoDetalleDtoList
               .Where(x => x.TipoMovimiento == (short)mexAccountMovementType.Credit)
               .Sum(x => x.MontoMonfor);

            amountDiferential = amountDebit - amountCredit;

            if (amountDiferential != 0)
            {
                accountId = (amountDiferential > 0
                    ? configBcoDto.CuentaContableDifPositivaConciliacion!.Value
                    : configBcoDto.CuentaContableDifNegativaConciliacion!.Value);

                tipoMov = (amountDiferential > 0 ? (short)mexAccountMovementType.Credit : (short)mexAccountMovementType.Debit);

                transaBcoDetalle = new()
                {
                    UidRegist = Guid.Empty,
                    UidDocumento = moduloDocumentoDto.UidRegist,
                    CodigoDocumento = moduloDocumentoDto.Codigo,
                    UidRegistPad = transaBcoDto.UidRegist,
                    UidCuentaContable = accountId,
                    NumeroLinea = ++lineCounter,
                    TipoMovimiento = tipoMov,
                    MontoMonfor = Math.Abs(amountDiferential.RoundTo(AC.DecimalTransa)),
                    IndDiferencial = false,
                    InddeCuadratura = false,
                    TipoRegistro = (short)mexBankDetailType.Differential,
                    UidBeneficiario = transaBcoDto.UidBeneficiario,
                    UidEntidad = transaBcoDto.UidEntidad
                };

                resultResponse = await
                    fnCreateTransactionBcoDetail(transaBcoDetalle);

                if (!resultResponse.IsSuccess || resultResponse.Data == null)
                {
                    resultResponse.IsSuccess = false;
                    return resultResponse;
                }

                transaBcoDetalleDto = (TransaccionesBcoDetalleDto)resultResponse.Data!;

                //Agg al listado
                transaBcoDetalleDtoList.Add(transaBcoDetalleDto);

                //Actualizamos el numero de lineas del padre
                transaBcoDto.NumeroLineas = (short)transaBcoDetalleDtoList.Count;

            }

            amountDebit = transaBcoDetalleDtoList
                .Where(x => x.TipoMovimiento == (short)mexAccountMovementType.Debit)
                .Sum(x => x.MontoMonxtr);

            amountCredit = transaBcoDetalleDtoList
                .Where(x => x.TipoMovimiento == (short)mexAccountMovementType.Credit)
                .Sum(x => x.MontoMonxtr);

            amountDiferential = amountDebit - amountCredit;

            if (amountDiferential != 0)
            {
                accountId = (amountDiferential > 0
                    ? configBcoDto.CuentaContableDifPositivaConciliacion!.Value
                    : configBcoDto.CuentaContableDifNegativaConciliacion!.Value);

                tipoMov = (amountDiferential > 0 ? (short)mexAccountMovementType.Credit : (short)mexAccountMovementType.Debit);

                transaBcoDetalle = new()
                {
                    UidRegist = Guid.Empty,
                    UidDocumento = moduloDocumentoDto.UidRegist,
                    CodigoDocumento = moduloDocumentoDto.Codigo,
                    UidRegistPad = transaBcoDto.UidRegist,
                    UidCuentaContable = accountId,
                    NumeroLinea = ++lineCounter,
                    TipoMovimiento = tipoMov,
                    MontoMonxtr = Math.Abs(amountDiferential.RoundTo(AC.DecimalTransa)),
                    IndDiferencial = false,
                    InddeCuadratura = false,
                    TipoRegistro = (short)mexBankDetailType.Differential,
                    UidBeneficiario = transaBcoDto.UidBeneficiario,
                    UidEntidad = transaBcoDto.UidEntidad
                };

                resultResponse = await
                    fnCreateTransactionBcoDetail(transaBcoDetalle);

                if (!resultResponse.IsSuccess || resultResponse.Data == null)
                {
                    resultResponse.IsSuccess = false;
                    return resultResponse;
                }

                transaBcoDetalleDto = (TransaccionesBcoDetalleDto)resultResponse.Data!;

                //Agg al listado
                transaBcoDetalleDtoList.Add(transaBcoDetalleDto);

                //Actualizamos el numero de lineas del padre
                transaBcoDto.NumeroLineas = (short)transaBcoDetalleDtoList.Count;

            }

            //Creamos el comprobante
            AsientosContablesDtoCreate asientoCnt = new()
            {
                UidModuloDocumento = moduloDocumentoDto.UidRegist,
                UidModulo = moduloDto.UidRegist,
                UidCia = transaBcoDto.UidCia,
                Comentarios = transaBcoDto.Comentarios,
                FechaTransa = transaBcoDto.FechaTransa,
                MesFiscal = transaBcoDto.MesFiscal,
                YearFiscal = transaBcoDto.YearFiscal,
                TipoCambioMonfor = transaBcoDto.TipoCambioMonfor,
                TipoCambioMonxtr = transaBcoDto.TipoCambioMonxtr,
                TipoCambioParaMonfor = transaBcoDto.TipoCambioParaMonfor,
                TipoCambioParaMonxtr = transaBcoDto.TipoCambioParaMonxtr,
                MontoCreditoMonbas = transaBcoDto.MontoCreditoMonbas,
                MontoCreditoMonfor = transaBcoDto.MontoCreditoMonfor,
                MontoCreditoMonxtr = transaBcoDto.MontoCreditoMonxtr,
                MontoDebitoMonbas = transaBcoDto.MontoDebitoMonbas,
                MontoDebitoMonfor = transaBcoDto.MontoDebitoMonfor,
                MontoDebitoMonxtr = transaBcoDto.MontoDebitoMonxtr,
                NumeroLineas = transaBcoDto.NumeroLineas,
                NumeroMoneda = transaBcoDto.NumeroMoneda,
                NumeroObjeto = (int)mexJournalObjects.JournalEntry,
                NumeroEstado = (int)mexJournalEntryStages.Draft,
                SerieInterna = transaBcoDto.SerieInterna,
                NumeroTransaccionRef = transaBcoDto.NumeroTransaccionRef,
                NumeroTransaccion = numberTransaCnt,
                IndOkay = true
            };

            //Crear cabecera
            resultResponse = await
                fnCreateJournalHeader(asientoCnt);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            asientoDto = (AsientosContablesDto)resultResponse.Data!;

            //Agg referencia al detalle de la cotizacion
            detail.JournalEntryId = asientoDto.UidRegist;
            detail.IsJournalEntryPosted = false;

            foreach (var detailTransa in transaBcoDetalleDtoList)
            {
                //Creamos los detalles del comprobante en base a los detalles de la transaBco
                AsientosContablesDetalleDtoCreate asientoDetalle = new()
                {
                    UidDocumento = moduloDocumentoDto.UidRegist,
                    CodigoDocumento = moduloDocumentoDto.Codigo,
                    UidRegistPad = asientoDto.UidRegist,
                    UidCuentaContable = detailTransa.UidCuentaContable,
                    NumeroLinea = detailTransa.NumeroLinea,
                    TipoMovimiento = detailTransa.TipoMovimiento,
                    TipoCambioMonfor = detailTransa.TipoCambioMonfor,
                    TipoCambioMonxtr = detailTransa.TipoCambioMonxtr,
                    MontoMonbas = detailTransa.MontoMonbas,
                    MontoMonfor = detailTransa.MontoMonfor,
                    MontoMonxtr = detailTransa.MontoMonxtr,
                    IndDiferencial = false,
                    InddeCuadratura = false
                };

                resultResponse = await
                    fnCreateJournalDetail(asientoDetalle);

                if (!resultResponse.IsSuccess)
                {
                    resultResponse.IsSuccess = false;
                    return resultResponse;
                }
            };

            //Actualizar la transaccion bancaria con la referencia del comprobante
            transaBcoDto.UidAsientoContable = asientoDto.UidRegist;

            var transaBcoDtoUpdate = _mapper.Map<TransaccionesBcoDtoUpdate>(transaBcoDto);
            resultResponse = await
                fnUpdateTransactionBcoHeader(transaBcoDtoUpdate);

            if (!resultResponse.IsSuccess)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            resultResponse.IsSuccess = true;
            resultResponse.Data = detail;

            return resultResponse;
        }
        catch (Exception ex)
        {
            resultResponse.IsSuccess = false;
            resultResponse.ErrorMessages = ex.Message;
            return resultResponse;
        }
    }

    private async Task<ResultResponse> fnLogicCreditTransfer(Quotation objHeader, QuotationDetail detail, ConfigBcoDto configBcoDto)
    {
        ResultResponse? resultResponse = new() { IsSuccess = true };
        try
        {
            BancosDto? bankSourceDto = new();
            BancosDto? bankTargetDto = new();
            CuentasBancariasDto? bankAccountSourceDto = new();
            CuentasBancariasDto? bankAccountTargetDto = new();
            TransaccionResponse? transaResponse = new();
            TransaccionesBcoDto? transaBcoDto = new();
            AsientosContablesDto? asientoDto = new();
            TransaccionesBcoDetalleDto? transaBcoDetalleDto = new();
            List<TransaccionesBcoDetalleDto>? transaBcoDetalleDtoList = new();
            ModulosDto? moduloDto = new();
            ModulosDocumentosDto? moduloDocumentoDto = new();

            resultResponse = await fnGetBank(objHeader.BankAccountTargetTrx.ParentTrx.Code);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            bankSourceDto = (BancosDto)resultResponse.Data;

            resultResponse = await fnGetBank(objHeader.BankAccountSourceTrx.ParentTrx.Code);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            bankTargetDto = (BancosDto)resultResponse.Data;

            resultResponse = await fnGetBankAccountTransfer(
                bankSourceDto.Codigo,
                objHeader.BankAccountTargetTrx!.Code,
                (short)objHeader.BankAccountTargetTrx!.CurrencyType);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            bankAccountSourceDto = (CuentasBancariasDto)resultResponse.Data;

            resultResponse = await fnGetBankAccountTransfer(
                objHeader.BankAccountSourceTrx.ParentTrx.Code,
                objHeader.BankAccountSourceTrx!.Code,
                (short)objHeader.BankAccountSourceTrx!.CurrencyType);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            bankAccountTargetDto = (CuentasBancariasDto)resultResponse.Data;

            resultResponse = await fnGetDocument((int)mexModules.Bank, (int)mexModuleBankDocument.CreditTransfer);

            if (!resultResponse.IsSuccess || resultResponse.Data == null || resultResponse.DataChildren == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            moduloDto = (ModulosDto)resultResponse.Data;
            moduloDocumentoDto = (ModulosDocumentosDto)resultResponse.DataChildren;

            short tipo = (short)(TransaccionBcoTipo.Transferencia);
            short subtipo = (short)(TransaccionBcoTransferenciaSubtipo.TransferenciaCredito);

            resultResponse = await fnGetNextSecuential(
                mexModules.Bank,
                bankAccountSourceDto.UidRegist,
                objHeader.DateTransa.Year,
                objHeader.DateTransa.Month, tipo, subtipo);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            transaResponse = (TransaccionResponse)resultResponse.Data;

            string numberTransaCnt = string.Empty;

            resultResponse = await fnGetNextSecuential(
                mexModules.Account,
                bankAccountSourceDto.UidRegist,
                objHeader.DateTransa.Year,
                objHeader.DateTransa.Month, tipo, subtipo);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            numberTransaCnt = (string)resultResponse.Data;

            decimal exchangeRate = objHeader.ExchangeRateOfficialTransa;

            ConverterExchange cvtExc = new();

            var mtosExc = cvtExc.ConverterExchangeTo((CurrencyType)detail.CurrencyDetailTrx.Numeral, detail.AmountDetail,
                exchangeRate, exchangeRate,
                decimalTrx: AC.DecimalTransa);

            var numberTransaFull =
                $"MC-{objHeader.TypeTrx.Code}-{objHeader.Numeral.ToString().PadLeft(3, AC.CharDefaultEmpty)}";


            TransaccionesBcoDtoCreate transaBco = new()
            {
                IndMesaDeCambio = true,
                IndConciliable = true,
                IndConciliado = false,
                IndCompensado = false,
                IndRetencion = false,
                IndImpresoCheque = false,
                IndImpresoComprobante = false,
                IndFlotante = false,
                IndOkay = true,
                IndTransaccionInicial = false,
                Comentarios = $"TRASLADO DE FONDOS DESDE {bankSourceDto.Codigo.Trim()} A {bankTargetDto.Codigo.Trim()} - {numberTransaFull} - D{detail.LineNumber} - {objHeader.CurrencyTransaTrx.Abbreviation} {objHeader.AmountTransaction.ToString(AC.DecimalTransaFormat)} - {objHeader.CustomerTrx.CommercialName}",
                FechaTransa = objHeader.DateTransa.ToDateTimeConvert(),
                MesFiscal = (short)objHeader.DateTransa.Month,
                YearFiscal = (short)objHeader.DateTransa.Year,
                YearMonthFiscal = $"{objHeader.DateTransa.Year}{objHeader.DateTransa.Month.ToString().PadLeft(2, AC.CharDefaultEmpty)}",
                UidBanco = bankSourceDto.UidRegist,
                UidCuentaBancaria = bankAccountSourceDto.UidRegist,
                UidCuentaBancariaRef = bankAccountTargetDto.UidRegist,
                UidTipo = transaResponse.TipoId,
                UidSubtipo = transaResponse.SubtipoId,
                NumeroLineas = 2,
                NumeroTransaccion = transaResponse.NumberTransa,
                NumeroMoneda = (short)detail.CurrencyDetailTrx.Numeral,
                NumeroObjeto = (int)mexBankObjects.Transaction,
                NumeroEstado = (int)mexBankTransactionStages.Draft,
                NumeroTransaccionRef = $"{numberTransaFull}",
                TipoCambioMonfor = exchangeRate,
                TipoCambioMonxtr = exchangeRate,
                TipoCambioParaMonfor = 1,
                TipoCambioParaMonxtr = 1,
                MontoMonbas = mtosExc.AmountBase,
                MontoMonfor = mtosExc.AmountForeign,
                MontoMonxtr = mtosExc.AmountAdditional,
                MontoDebitoMonbas = mtosExc.AmountBase,
                MontoDebitoMonfor = mtosExc.AmountForeign,
                MontoDebitoMonxtr = mtosExc.AmountAdditional,
                MontoCreditoMonbas = mtosExc.AmountBase,
                MontoCreditoMonfor = mtosExc.AmountForeign,
                MontoCreditoMonxtr = mtosExc.AmountAdditional,
                TransaMcRelacionada = detail.Id,
                TransaMcRelacionadaParent = objHeader.Id,
                SerieInterna = "B",
                TipoBeneficiario = (short)mexBeneficiaryTypeNumber.Other
            };

            //mtosExc.SetInit(); // Limpiar

            //Crear cabecera
            resultResponse = await
                fnCreateTransactionBcoHeader(transaBco);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            transaBcoDto = (TransaccionesBcoDto)resultResponse.Data!;

            detail.BankTransactionId = transaBcoDto.UidRegist;
            detail.IsBankTransactionPosted = false;

            //Creamos el primer detalle
            TransaccionesBcoDetalleDtoCreate transaBcoDetalle = new()
            {
                UidDocumento = moduloDocumentoDto.UidRegist,
                CodigoDocumento = moduloDocumentoDto.Codigo,
                UidRegistPad = transaBcoDto.UidRegist,
                UidCuentaContable = bankAccountSourceDto.UidCuentaContable!.Value,
                NumeroLinea = 1,
                TipoMovimiento = (short)mexAccountMovementType.Debit,
                TipoCambioMonfor = transaBcoDto.TipoCambioMonfor,
                TipoCambioMonxtr = transaBcoDto.TipoCambioMonxtr,
                MontoMonbas = mtosExc.AmountBase,
                MontoMonfor = mtosExc.AmountForeign,
                MontoMonxtr = mtosExc.AmountAdditional,
                IndDiferencial = false,
                InddeCuadratura = false,
                TipoRegistro = (short)mexBankDetailType.AutomaticCounterPart
            };

            resultResponse = await
                fnCreateTransactionBcoDetail(transaBcoDetalle);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            transaBcoDetalleDto = (TransaccionesBcoDetalleDto)resultResponse.Data!;

            //Agg a la lista
            transaBcoDetalleDtoList.Add(transaBcoDetalleDto);

            //Creamos el segundo detalle
            transaBcoDetalle = new()
            {
                UidRegist = Guid.Empty,
                UidDocumento = moduloDocumentoDto.UidRegist,
                CodigoDocumento = moduloDocumentoDto.Codigo,
                UidRegistPad = transaBcoDto.UidRegist,
                UidCuentaContable = configBcoDto.CuentacontableInterfaz!.Value,
                NumeroLinea = 2,
                TipoMovimiento = (short)mexAccountMovementType.Credit,
                TipoCambioMonfor = transaBcoDto.TipoCambioMonfor,
                TipoCambioMonxtr = transaBcoDto.TipoCambioMonxtr,
                MontoMonbas = mtosExc.AmountBase,
                MontoMonfor = mtosExc.AmountForeign,
                MontoMonxtr = mtosExc.AmountAdditional,
                IndDiferencial = false,
                InddeCuadratura = false,
                TipoRegistro = (short)mexBankDetailType.AutomaticCounterPart
            };

            resultResponse = await
                fnCreateTransactionBcoDetail(transaBcoDetalle);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            transaBcoDetalleDto = (TransaccionesBcoDetalleDto)resultResponse.Data!;

            //Agg a la lista
            transaBcoDetalleDtoList.Add(transaBcoDetalleDto);

            //Creamos el comprobante
            AsientosContablesDtoCreate asientoCnt = new()
            {
                UidModuloDocumento = moduloDocumentoDto.UidRegist,
                UidModulo = moduloDto.UidRegist,
                UidCia = transaBcoDto.UidCia,
                Comentarios = transaBcoDto.Comentarios,
                FechaTransa = transaBcoDto.FechaTransa,
                MesFiscal = transaBcoDto.MesFiscal,
                YearFiscal = transaBcoDto.YearFiscal,
                TipoCambioMonfor = transaBcoDto.TipoCambioMonfor,
                TipoCambioMonxtr = transaBcoDto.TipoCambioMonxtr,
                TipoCambioParaMonfor = transaBcoDto.TipoCambioParaMonfor,
                TipoCambioParaMonxtr = transaBcoDto.TipoCambioParaMonxtr,
                MontoCreditoMonbas = transaBcoDto.MontoCreditoMonbas,
                MontoCreditoMonfor = transaBcoDto.MontoCreditoMonfor,
                MontoCreditoMonxtr = transaBcoDto.MontoCreditoMonxtr,
                MontoDebitoMonbas = transaBcoDto.MontoDebitoMonbas,
                MontoDebitoMonfor = transaBcoDto.MontoDebitoMonfor,
                MontoDebitoMonxtr = transaBcoDto.MontoDebitoMonxtr,
                NumeroLineas = transaBcoDto.NumeroLineas,
                NumeroMoneda = transaBcoDto.NumeroMoneda,
                NumeroObjeto = (int)mexJournalObjects.JournalEntry,
                NumeroEstado = (int)mexJournalEntryStages.Draft,
                SerieInterna = transaBcoDto.SerieInterna,
                NumeroTransaccionRef = transaBcoDto.NumeroTransaccionRef,
                NumeroTransaccion = numberTransaCnt,
                IndOkay = true
            };

            //Crear cabecera
            resultResponse = await
                fnCreateJournalHeader(asientoCnt);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            asientoDto = (AsientosContablesDto)resultResponse.Data!;

            //Agg el id al detalle de la cotización
            detail.JournalEntryId = asientoDto.UidRegist;
            detail.IsJournalEntryPosted = false;

            foreach (var detailTransa in transaBcoDetalleDtoList)
            {
                //Creamos los detalles del comprobante en base a los detalles de la transaBco
                AsientosContablesDetalleDtoCreate asientoDetalle = new()
                {
                    UidDocumento = moduloDocumentoDto.UidRegist,
                    CodigoDocumento = moduloDocumentoDto.Codigo,
                    UidRegistPad = asientoDto.UidRegist,
                    UidCuentaContable = detailTransa.UidCuentaContable,
                    NumeroLinea = detailTransa.NumeroLinea,
                    TipoMovimiento = detailTransa.TipoMovimiento,
                    TipoCambioMonfor = detailTransa.TipoCambioMonfor,
                    TipoCambioMonxtr = detailTransa.TipoCambioMonxtr,
                    MontoMonbas = detailTransa.MontoMonbas,
                    MontoMonfor = detailTransa.MontoMonfor,
                    MontoMonxtr = detailTransa.MontoMonxtr,
                    IndDiferencial = false,
                    InddeCuadratura = false
                };

                resultResponse = await
                    fnCreateJournalDetail(asientoDetalle);

                if (!resultResponse.IsSuccess)
                {
                    resultResponse.IsSuccess = false;
                    return resultResponse;
                }
            }

            //Actualizar la transaccion bancaria con la referencia del comprobante
            transaBcoDto.UidAsientoContable = asientoDto.UidRegist;

            var transaBcoDtoUpdate = _mapper.Map<TransaccionesBcoDtoUpdate>(transaBcoDto);
            resultResponse = await
                fnUpdateTransactionBcoHeader(transaBcoDtoUpdate);

            if (!resultResponse.IsSuccess)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            resultResponse.IsSuccess = true;
            resultResponse.Data = detail;

            return resultResponse;
        }
        catch (Exception ex)
        {
            resultResponse.IsSuccess = false;
            resultResponse.ErrorMessages = ex.Message;
            return resultResponse;
        }
    }

    private async Task<ResultResponse> fnLogicDebitTransfer(Quotation objHeader, QuotationDetail detail, ConfigBcoDto configBcoDto)
    {
        ResultResponse? resultResponse = new() { IsSuccess = true };
        try
        {
            BancosDto? bankSourceDto = new();
            BancosDto? bankTargetDto = new();
            CuentasBancariasDto? bankAccountSourceDto = new();
            CuentasBancariasDto? bankAccountTargetDto = new();
            TransaccionResponse? transaResponse = new();
            TransaccionesBcoDto? transaBcoDto = new();
            AsientosContablesDto? asientoDto = new();
            TransaccionesBcoDetalleDto? transaBcoDetalleDto = new();
            List<TransaccionesBcoDetalleDto>? transaBcoDetalleDtoList = new();
            ModulosDto? moduloDto = new();
            ModulosDocumentosDto? moduloDocumentoDto = new();

            resultResponse = await fnGetBank(objHeader.BankAccountSourceTrx.ParentTrx.Code);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            bankSourceDto = (BancosDto)resultResponse.Data;

            resultResponse = await fnGetBank(objHeader.BankAccountTargetTrx.ParentTrx.Code);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            bankTargetDto = (BancosDto)resultResponse.Data;

            resultResponse = await fnGetBankAccountTransfer(
                bankSourceDto.Codigo,
                objHeader.BankAccountSourceTrx!.Code,
                (short)objHeader.BankAccountSourceTrx!.CurrencyType);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            bankAccountSourceDto = (CuentasBancariasDto)resultResponse.Data;

            resultResponse = await fnGetBankAccountTransfer(
                objHeader.BankAccountTargetTrx.ParentTrx.Code,
                objHeader.BankAccountTargetTrx!.Code,
                (short)objHeader.BankAccountTargetTrx!.CurrencyType);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            bankAccountTargetDto = (CuentasBancariasDto)resultResponse.Data;

            resultResponse = await fnGetDocument((int)mexModules.Bank, (int)mexModuleBankDocument.DebitTransfer);

            if (!resultResponse.IsSuccess || resultResponse.Data == null || resultResponse.DataChildren == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            moduloDto = (ModulosDto)resultResponse.Data;
            moduloDocumentoDto = (ModulosDocumentosDto)resultResponse.DataChildren;

            short tipo = (short)(TransaccionBcoTipo.Transferencia);
            short subtipo = (short)(TransaccionBcoTransferenciaSubtipo.TransferenciaDebito);

            resultResponse = await fnGetNextSecuential(
                mexModules.Bank,
                bankAccountSourceDto.UidRegist,
                objHeader.DateTransa.Year,
                objHeader.DateTransa.Month, tipo, subtipo);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            transaResponse = (TransaccionResponse)resultResponse.Data;

            string numberTransaCnt = string.Empty;

            resultResponse = await fnGetNextSecuential(
                mexModules.Account,
                bankAccountSourceDto.UidRegist,
                objHeader.DateTransa.Year,
                objHeader.DateTransa.Month, tipo, subtipo);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            numberTransaCnt = (string)resultResponse.Data;

            decimal exchangeRate = objHeader.ExchangeRateOfficialTransa;

            ConverterExchange cvtExc = new();

            var mtosExc = cvtExc.ConverterExchangeTo((CurrencyType)detail.CurrencyDetailTrx.Numeral, detail.AmountDetail,
                exchangeRate, exchangeRate,
                decimalTrx: AC.DecimalTransa);

            var numberTransaFull =
                $"MC-{objHeader.TypeTrx.Code}-{objHeader.Numeral.ToString().PadLeft(3, AC.CharDefaultEmpty)}";


            TransaccionesBcoDtoCreate transaBco = new()
            {
                IndMesaDeCambio = true,
                IndConciliable = true,
                IndConciliado = false,
                IndCompensado = false,
                IndRetencion = false,
                IndImpresoCheque = false,
                IndImpresoComprobante = false,
                IndFlotante = false,
                IndOkay = true,
                IndTransaccionInicial = false,
                Comentarios = $"TRASLADO DE FONDOS DESDE {bankSourceDto.Codigo.Trim()} A {bankTargetDto.Codigo.Trim()} - {numberTransaFull} - T{detail.LineNumber} - {objHeader.CurrencyTransaTrx.Abbreviation} {objHeader.AmountTransaction.ToString(AC.DecimalTransaFormat)} - {objHeader.CustomerTrx.CommercialName}",
                FechaTransa = objHeader.DateTransa.ToDateTimeConvert(),
                MesFiscal = (short)objHeader.DateTransa.Month,
                YearFiscal = (short)objHeader.DateTransa.Year,
                YearMonthFiscal = $"{objHeader.DateTransa.Year}{objHeader.DateTransa.Month.ToString().PadLeft(2, AC.CharDefaultEmpty)}",
                UidBanco = bankSourceDto.UidRegist,
                UidCuentaBancaria = bankAccountSourceDto.UidRegist,
                UidCuentaBancariaRef = bankAccountTargetDto.UidRegist,
                UidTipo = transaResponse.TipoId,
                UidSubtipo = transaResponse.SubtipoId,
                NumeroLineas = 2,
                NumeroTransaccion = transaResponse.NumberTransa,
                NumeroMoneda = (short)detail.CurrencyDetailTrx.Numeral,
                NumeroObjeto = (int)mexBankObjects.Transaction,
                NumeroEstado = (int)mexBankTransactionStages.Draft,
                NumeroTransaccionRef = $"{numberTransaFull}",
                TipoCambioMonfor = exchangeRate,
                TipoCambioMonxtr = exchangeRate,
                TipoCambioParaMonfor = 1,
                TipoCambioParaMonxtr = 1,
                MontoMonbas = mtosExc.AmountBase,
                MontoMonfor = mtosExc.AmountForeign,
                MontoMonxtr = mtosExc.AmountAdditional,
                MontoDebitoMonbas = mtosExc.AmountBase,
                MontoDebitoMonfor = mtosExc.AmountForeign,
                MontoDebitoMonxtr = mtosExc.AmountAdditional,
                MontoCreditoMonbas = mtosExc.AmountBase,
                MontoCreditoMonfor = mtosExc.AmountForeign,
                MontoCreditoMonxtr = mtosExc.AmountAdditional,
                TransaMcRelacionada = detail.Id,
                TransaMcRelacionadaParent = objHeader.Id,
                SerieInterna = "B",
                TipoBeneficiario = (short)mexBeneficiaryTypeNumber.Other
            };

            //mtosExc.SetInit(); // Limpiar

            //Crear cabecera
            resultResponse = await
                fnCreateTransactionBcoHeader(transaBco);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            transaBcoDto = (TransaccionesBcoDto)resultResponse.Data!;

            detail.BankTransactionId = transaBcoDto.UidRegist;
            detail.IsBankTransactionPosted = false;

            //Creamos el primer detalle
            TransaccionesBcoDetalleDtoCreate transaBcoDetalle = new()
            {
                UidDocumento = moduloDocumentoDto.UidRegist,
                CodigoDocumento = moduloDocumentoDto.Codigo,
                UidRegistPad = transaBcoDto.UidRegist,
                UidCuentaContable = bankAccountSourceDto.UidCuentaContable!.Value,
                NumeroLinea = 1,
                TipoMovimiento = (short)mexAccountMovementType.Credit,
                TipoCambioMonfor = transaBcoDto.TipoCambioMonfor,
                TipoCambioMonxtr = transaBcoDto.TipoCambioMonxtr,
                MontoMonbas = mtosExc.AmountBase,
                MontoMonfor = mtosExc.AmountForeign,
                MontoMonxtr = mtosExc.AmountAdditional,
                IndDiferencial = false,
                InddeCuadratura = false,
                TipoRegistro = (short)mexBankDetailType.AutomaticCounterPart
            };

            resultResponse = await
                fnCreateTransactionBcoDetail(transaBcoDetalle);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            transaBcoDetalleDto = (TransaccionesBcoDetalleDto)resultResponse.Data!;

            //Agg a la lista
            transaBcoDetalleDtoList.Add(transaBcoDetalleDto);

            //Creamos el segundo detalle
            transaBcoDetalle = new()
            {
                UidRegist = Guid.Empty,
                UidDocumento = moduloDocumentoDto.UidRegist,
                CodigoDocumento = moduloDocumentoDto.Codigo,
                UidRegistPad = transaBcoDto.UidRegist,
                UidCuentaContable = configBcoDto.CuentacontableInterfaz!.Value,
                NumeroLinea = 2,
                TipoMovimiento = (short)mexAccountMovementType.Debit,
                TipoCambioMonfor = transaBcoDto.TipoCambioMonfor,
                TipoCambioMonxtr = transaBcoDto.TipoCambioMonxtr,
                MontoMonbas = mtosExc.AmountBase,
                MontoMonfor = mtosExc.AmountForeign,
                MontoMonxtr = mtosExc.AmountAdditional,
                IndDiferencial = false,
                InddeCuadratura = false,
                TipoRegistro = (short)mexBankDetailType.AutomaticCounterPart
            };

            resultResponse = await
                fnCreateTransactionBcoDetail(transaBcoDetalle);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            transaBcoDetalleDto = (TransaccionesBcoDetalleDto)resultResponse.Data!;

            //Agg a la lista
            transaBcoDetalleDtoList.Add(transaBcoDetalleDto);

            //Creamos el comprobante
            AsientosContablesDtoCreate asientoCnt = new()
            {
                UidModuloDocumento = moduloDocumentoDto.UidRegist,
                UidModulo = moduloDto.UidRegist,
                UidCia = transaBcoDto.UidCia,
                Comentarios = transaBcoDto.Comentarios,
                FechaTransa = transaBcoDto.FechaTransa,
                MesFiscal = transaBcoDto.MesFiscal,
                YearFiscal = transaBcoDto.YearFiscal,
                TipoCambioMonfor = transaBcoDto.TipoCambioMonfor,
                TipoCambioMonxtr = transaBcoDto.TipoCambioMonxtr,
                TipoCambioParaMonfor = transaBcoDto.TipoCambioParaMonfor,
                TipoCambioParaMonxtr = transaBcoDto.TipoCambioParaMonxtr,
                MontoCreditoMonbas = transaBcoDto.MontoCreditoMonbas,
                MontoCreditoMonfor = transaBcoDto.MontoCreditoMonfor,
                MontoCreditoMonxtr = transaBcoDto.MontoCreditoMonxtr,
                MontoDebitoMonbas = transaBcoDto.MontoDebitoMonbas,
                MontoDebitoMonfor = transaBcoDto.MontoDebitoMonfor,
                MontoDebitoMonxtr = transaBcoDto.MontoDebitoMonxtr,
                NumeroLineas = transaBcoDto.NumeroLineas,
                NumeroMoneda = transaBcoDto.NumeroMoneda,
                NumeroObjeto = (int)mexJournalObjects.JournalEntry,
                NumeroEstado = (int)mexJournalEntryStages.Draft,
                SerieInterna = transaBcoDto.SerieInterna,
                NumeroTransaccionRef = transaBcoDto.NumeroTransaccionRef,
                NumeroTransaccion = numberTransaCnt,
                IndOkay = true
            };

            //Crear cabecera
            resultResponse = await
                fnCreateJournalHeader(asientoCnt);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            asientoDto = (AsientosContablesDto)resultResponse.Data!;

            //Agg el id al detalle de la cotización
            detail.JournalEntryId = asientoDto.UidRegist;
            detail.IsJournalEntryPosted = false;

            foreach (var detailTransa in transaBcoDetalleDtoList)
            {
                //Creamos los detalles del comprobante en base a los detalles de la transaBco
                AsientosContablesDetalleDtoCreate asientoDetalle = new()
                {
                    UidDocumento = moduloDocumentoDto.UidRegist,
                    CodigoDocumento = moduloDocumentoDto.Codigo,
                    UidRegistPad = asientoDto.UidRegist,
                    UidCuentaContable = detailTransa.UidCuentaContable,
                    NumeroLinea = detailTransa.NumeroLinea,
                    TipoMovimiento = detailTransa.TipoMovimiento,
                    TipoCambioMonfor = detailTransa.TipoCambioMonfor,
                    TipoCambioMonxtr = detailTransa.TipoCambioMonxtr,
                    MontoMonbas = detailTransa.MontoMonbas,
                    MontoMonfor = detailTransa.MontoMonfor,
                    MontoMonxtr = detailTransa.MontoMonxtr,
                    IndDiferencial = false,
                    InddeCuadratura = false
                };

                resultResponse = await
                    fnCreateJournalDetail(asientoDetalle);

                if (!resultResponse.IsSuccess)
                {
                    resultResponse.IsSuccess = false;
                    return resultResponse;
                }
            }

            //Actualizar la transaccion bancaria con la referencia del comprobante
            transaBcoDto.UidAsientoContable = asientoDto.UidRegist;

            var transaBcoDtoUpdate = _mapper.Map<TransaccionesBcoDtoUpdate>(transaBcoDto);
            resultResponse = await
                fnUpdateTransactionBcoHeader(transaBcoDtoUpdate);

            if (!resultResponse.IsSuccess)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            resultResponse.IsSuccess = true;
            resultResponse.Data = detail;

            return resultResponse;
        }
        catch (Exception ex)
        {
            resultResponse.IsSuccess = false;
            resultResponse.ErrorMessages = ex.Message;
            return resultResponse;
        }
    }

    private async Task<ResultResponse> fnLogicDebitTransferCommision(Quotation objHeader, QuotationDetail detail, ConfigBcoDto configBcoDto)
    {
        ResultResponse? resultResponse = new() { IsSuccess = true };
        try
        {
            BancosDto? bankSourceDto = new();
            BancosDto? bankTargetDto = new();
            CuentasBancariasDto? bankAccountSourceDto = new();
            TransaccionResponse? transaResponse = new();
            TransaccionesBcoDto? transaBcoDto = new();
            AsientosContablesDto? asientoDto = new();
            TransaccionesBcoDetalleDto? transaBcoDetalleDto = new();
            List<TransaccionesBcoDetalleDto>? transaBcoDetalleDtoList = new();
            ModulosDto? moduloDto = new();
            ModulosDocumentosDto? moduloDocumentoDto = new();

            resultResponse = await fnGetBank(objHeader.BankAccountSourceTrx.ParentTrx.Code);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            bankSourceDto = (BancosDto)resultResponse.Data;

            resultResponse = await fnGetBank(objHeader.BankAccountTargetTrx.ParentTrx.Code);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            bankTargetDto = (BancosDto)resultResponse.Data;

            resultResponse = await fnGetBankAccountTransfer(
                bankSourceDto.Codigo,
                objHeader.BankAccountSourceTrx!.Code,
                (short)objHeader.BankAccountSourceTrx!.CurrencyType);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            bankAccountSourceDto = (CuentasBancariasDto)resultResponse.Data;

            resultResponse = await fnGetDocument((int)mexModules.Bank, (int)mexModuleBankDocument.DebitNote);

            if (!resultResponse.IsSuccess || resultResponse.Data == null || resultResponse.DataChildren == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            moduloDto = (ModulosDto)resultResponse.Data;
            moduloDocumentoDto = (ModulosDocumentosDto)resultResponse.DataChildren;

            short tipo = (short)(TransaccionBcoTipo.NotaDebito);
            short subtipo = (short)(TransaccionBcoNotaDebitoSubtipo.NotaDebito);

            resultResponse = await fnGetNextSecuential(
                mexModules.Bank,
                bankAccountSourceDto.UidRegist,
                objHeader.DateTransa.Year,
                objHeader.DateTransa.Month, tipo, subtipo);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            transaResponse = (TransaccionResponse)resultResponse.Data;

            string numberTransaCnt = string.Empty;

            resultResponse = await fnGetNextSecuential(
                mexModules.Account,
                bankAccountSourceDto.UidRegist,
                objHeader.DateTransa.Year,
                objHeader.DateTransa.Month, tipo, subtipo);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            numberTransaCnt = (string)resultResponse.Data;

            decimal exchangeRate = objHeader.ExchangeRateOfficialTransa;

            ConverterExchange cvtExc = new();

            var mtosExc = cvtExc.ConverterExchangeTo((CurrencyType)detail.CurrencyDetailTrx.Numeral, objHeader.AmountCommission,
                exchangeRate, exchangeRate,
                decimalTrx: AC.DecimalTransa);

            var numberTransaFull =
                $"MC-{objHeader.TypeTrx.Code}-{objHeader.Numeral.ToString().PadLeft(3, AC.CharDefaultEmpty)}";

            TransaccionesBcoDtoCreate transaBco = new()
            {
                IndMesaDeCambio = true,
                IndConciliable = true,
                IndConciliado = false,
                IndCompensado = false,
                IndRetencion = false,
                IndImpresoCheque = false,
                IndImpresoComprobante = false,
                IndFlotante = false,
                IndOkay = true,
                IndTransaccionInicial = false,
                Comentarios = $"COMISION BANCARIA POR TRANSFERENCIA DESDE {bankSourceDto.Codigo.Trim()} A {bankTargetDto.Codigo.Trim()} - {numberTransaFull} - D{detail.LineNumber} - {objHeader.CurrencyTransaTrx.Abbreviation} {objHeader.AmountTransaction.ToString(AC.DecimalTransaFormat)} - {objHeader.CustomerTrx.CommercialName}",
                FechaTransa = objHeader.DateTransa.ToDateTimeConvert(),
                MesFiscal = (short)objHeader.DateTransa.Month,
                YearFiscal = (short)objHeader.DateTransa.Year,
                YearMonthFiscal = $"{objHeader.DateTransa.Year}{objHeader.DateTransa.Month.ToString().PadLeft(2, AC.CharDefaultEmpty)}",
                UidBanco = bankSourceDto.UidRegist,
                UidCuentaBancaria = bankAccountSourceDto.UidRegist,
                UidTipo = transaResponse.TipoId,
                UidSubtipo = transaResponse.SubtipoId,
                NumeroLineas = 2,
                NumeroTransaccion = transaResponse.NumberTransa,
                NumeroMoneda = (short)detail.CurrencyDetailTrx.Numeral,
                NumeroObjeto = (int)mexBankObjects.Transaction,
                NumeroEstado = (int)mexBankTransactionStages.Draft,
                NumeroTransaccionRef = $"{numberTransaFull}",
                TipoCambioMonfor = exchangeRate,
                TipoCambioMonxtr = exchangeRate,
                TipoCambioParaMonfor = 1,
                TipoCambioParaMonxtr = 1,
                MontoMonbas = mtosExc.AmountBase,
                MontoMonfor = mtosExc.AmountForeign,
                MontoMonxtr = mtosExc.AmountAdditional,
                MontoDebitoMonbas = mtosExc.AmountBase,
                MontoDebitoMonfor = mtosExc.AmountForeign,
                MontoDebitoMonxtr = mtosExc.AmountAdditional,
                MontoCreditoMonbas = mtosExc.AmountBase,
                MontoCreditoMonfor = mtosExc.AmountForeign,
                MontoCreditoMonxtr = mtosExc.AmountAdditional,
                TransaMcRelacionada = detail.Id,
                TransaMcRelacionadaParent = objHeader.Id,
                SerieInterna = "B"
            };

            //mtosExc.SetInit(); // Limpiar

            //Crear cabecera
            resultResponse = await
                fnCreateTransactionBcoHeader(transaBco);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            transaBcoDto = (TransaccionesBcoDto)resultResponse.Data!;

            detail.BankTransactionTransferFeeId = transaBcoDto.UidRegist;
            detail.IsBankTransactionTransferFeePosted = false;

            //Creamos el primer detalle
            TransaccionesBcoDetalleDtoCreate transaBcoDetalle = new()
            {
                UidDocumento = moduloDocumentoDto.UidRegist,
                CodigoDocumento = moduloDocumentoDto.Codigo,
                UidRegistPad = transaBcoDto.UidRegist,
                UidCuentaContable = bankAccountSourceDto.UidCuentaContable!.Value,
                NumeroLinea = 1,
                TipoMovimiento = (short)mexAccountMovementType.Credit,
                TipoCambioMonfor = transaBcoDto.TipoCambioMonfor,
                TipoCambioMonxtr = transaBcoDto.TipoCambioMonxtr,
                MontoMonbas = mtosExc.AmountBase,
                MontoMonfor = mtosExc.AmountForeign,
                MontoMonxtr = mtosExc.AmountAdditional,
                IndDiferencial = false,
                InddeCuadratura = false,
                TipoRegistro = (short)mexBankDetailType.AutomaticCounterPart
            };

            resultResponse = await
                fnCreateTransactionBcoDetail(transaBcoDetalle);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            transaBcoDetalleDto = (TransaccionesBcoDetalleDto)resultResponse.Data!;

            //Agg a la lista
            transaBcoDetalleDtoList.Add(transaBcoDetalleDto);

            //Creamos el segundo detalle
            transaBcoDetalle = new()
            {
                UidRegist = Guid.Empty,
                UidDocumento = moduloDocumentoDto.UidRegist,
                CodigoDocumento = moduloDocumentoDto.Codigo,
                UidRegistPad = transaBcoDto.UidRegist,
                UidCuentaContable = configBcoDto.CuentaContableComisionTransferencia!.Value,
                NumeroLinea = 2,
                TipoMovimiento = (short)mexAccountMovementType.Debit,
                TipoCambioMonfor = transaBcoDto.TipoCambioMonfor,
                TipoCambioMonxtr = transaBcoDto.TipoCambioMonxtr,
                MontoMonbas = mtosExc.AmountBase,
                MontoMonfor = mtosExc.AmountForeign,
                MontoMonxtr = mtosExc.AmountAdditional,
                IndDiferencial = false,
                InddeCuadratura = false,
                TipoRegistro = (short)mexBankDetailType.AutomaticCounterPart
            };

            resultResponse = await
                fnCreateTransactionBcoDetail(transaBcoDetalle);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            transaBcoDetalleDto = (TransaccionesBcoDetalleDto)resultResponse.Data!;

            //Agg a la lista
            transaBcoDetalleDtoList.Add(transaBcoDetalleDto);

            //Creamos el comprobante
            AsientosContablesDtoCreate asientoCnt = new()
            {
                UidModuloDocumento = moduloDocumentoDto.UidRegist,
                UidModulo = moduloDto.UidRegist,
                UidCia = transaBcoDto.UidCia,
                Comentarios = transaBcoDto.Comentarios,
                FechaTransa = transaBcoDto.FechaTransa,
                MesFiscal = transaBcoDto.MesFiscal,
                YearFiscal = transaBcoDto.YearFiscal,
                TipoCambioMonfor = transaBcoDto.TipoCambioMonfor,
                TipoCambioMonxtr = transaBcoDto.TipoCambioMonxtr,
                TipoCambioParaMonfor = transaBcoDto.TipoCambioParaMonfor,
                TipoCambioParaMonxtr = transaBcoDto.TipoCambioParaMonxtr,
                MontoCreditoMonbas = transaBcoDto.MontoCreditoMonbas,
                MontoCreditoMonfor = transaBcoDto.MontoCreditoMonfor,
                MontoCreditoMonxtr = transaBcoDto.MontoCreditoMonxtr,
                MontoDebitoMonbas = transaBcoDto.MontoDebitoMonbas,
                MontoDebitoMonfor = transaBcoDto.MontoDebitoMonfor,
                MontoDebitoMonxtr = transaBcoDto.MontoDebitoMonxtr,
                NumeroLineas = transaBcoDto.NumeroLineas,
                NumeroMoneda = transaBcoDto.NumeroMoneda,
                NumeroObjeto = (int)mexJournalObjects.JournalEntry,
                NumeroEstado = (int)mexJournalEntryStages.Draft,
                SerieInterna = transaBcoDto.SerieInterna,
                NumeroTransaccionRef = transaBcoDto.NumeroTransaccionRef,
                NumeroTransaccion = numberTransaCnt,
                IndOkay = true
            };

            //Crear cabecera
            resultResponse = await
                fnCreateJournalHeader(asientoCnt);

            if (!resultResponse.IsSuccess || resultResponse.Data == null)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            asientoDto = (AsientosContablesDto)resultResponse.Data!;

            //Agg el id al detalle de la cotización
            detail.JournalEntryTransferFeeId = asientoDto.UidRegist;
            detail.IsJournalEntryTransferFeePosted = false;

            foreach (var detailTransa in transaBcoDetalleDtoList)
            {
                //Creamos los detalles del comprobante en base a los detalles de la transaBco
                AsientosContablesDetalleDtoCreate asientoDetalle = new()
                {
                    UidDocumento = moduloDocumentoDto.UidRegist,
                    CodigoDocumento = moduloDocumentoDto.Codigo,
                    UidRegistPad = asientoDto.UidRegist,
                    UidCuentaContable = detailTransa.UidCuentaContable,
                    NumeroLinea = detailTransa.NumeroLinea,
                    TipoMovimiento = detailTransa.TipoMovimiento,
                    TipoCambioMonfor = detailTransa.TipoCambioMonfor,
                    TipoCambioMonxtr = detailTransa.TipoCambioMonxtr,
                    MontoMonbas = detailTransa.MontoMonbas,
                    MontoMonfor = detailTransa.MontoMonfor,
                    MontoMonxtr = detailTransa.MontoMonxtr,
                    IndDiferencial = false,
                    InddeCuadratura = false
                };

                resultResponse = await
                    fnCreateJournalDetail(asientoDetalle);

                if (!resultResponse.IsSuccess)
                {
                    resultResponse.IsSuccess = false;
                    return resultResponse;
                }
            }

            //Actualizar la transaccion bancaria con la referencia del comprobante
            transaBcoDto.UidAsientoContable = asientoDto.UidRegist;

            var transaBcoDtoUpdate = _mapper.Map<TransaccionesBcoDtoUpdate>(transaBcoDto);
            resultResponse = await
                fnUpdateTransactionBcoHeader(transaBcoDtoUpdate);

            if (!resultResponse.IsSuccess)
            {
                resultResponse.IsSuccess = false;
                return resultResponse;
            }

            resultResponse.IsSuccess = true;
            resultResponse.Data = detail;

            return resultResponse;
        }
        catch (Exception ex)
        {
            resultResponse.IsSuccess = false;
            resultResponse.ErrorMessages = ex.Message;
            return resultResponse;
        }
    }

    private async Task<ResultResponse> fnUpdateTransactionBcoHeader(TransaccionesBcoDtoUpdate transaBco)
    {
        ResultResponse? resultResponse = new() { IsSuccess = true };
        StringBuilder errorsMessagesBuilder = new();
        TransaccionesBcoDto? transaBcoDto = new();
        try
        {
            var srvResponse = await _srvTransaBco.UpdateAsync<APIResponse>(_sessionToken, transaBco);
            if (srvResponse is null)
            {
                resultResponse.IsSuccess = false;
                resultResponse.ErrorMessages = "No se pudo obtener la respuesta";
                return resultResponse;
            }
            else if (srvResponse is { isSuccess: false })
            {
                errorsMessagesBuilder.AppendJoin("", srvResponse.errorMessages);
                resultResponse.IsSuccess = false;
                resultResponse.ErrorMessages = errorsMessagesBuilder.ToString();
                return resultResponse;
            }

            resultResponse.IsSuccess = true;
            return resultResponse;
        }
        catch (Exception ex)
        {
            resultResponse.IsSuccess = false;
            resultResponse.ErrorMessages = ex.Message;
            return resultResponse;
        }
    }

    private async Task<ResultResponse> fnCreateTransactionBcoRel(Guid transaBcoDebitId, Guid transaBcoCreditId, Guid? transaBcoCommisionId)
    {
        ResultResponse? resultResponse = new() { IsSuccess = true };
        StringBuilder errorsMessagesBuilder = new();
        try
        {
            var srvResponse = await _srvTransaBco.CreateRelationAsync<APIResponse>(_sessionToken, transaBcoDebitId, transaBcoCreditId, transaBcoCommisionId);
            if (srvResponse is null)
            {
                resultResponse.IsSuccess = false;
                resultResponse.ErrorMessages = "No se pudo obtener la respuesta";
                return resultResponse;
            }
            else if (srvResponse is { isSuccess: false })
            {
                errorsMessagesBuilder.AppendJoin("", srvResponse.errorMessages);
                resultResponse.IsSuccess = false;
                resultResponse.ErrorMessages = errorsMessagesBuilder.ToString();
                return resultResponse;
            }

            resultResponse.IsSuccess = true;
            return resultResponse;
        }
        catch (Exception ex)
        {
            resultResponse.IsSuccess = false;
            resultResponse.ErrorMessages = ex.Message;
            return resultResponse;
        }
    }

    private async Task<ResultResponse> fnCreateTransactionBcoHeader(TransaccionesBcoDtoCreate transaBco)
    {
        ResultResponse? resultResponse = new() { IsSuccess = true };
        StringBuilder errorsMessagesBuilder = new();
        TransaccionesBcoDto? transaBcoDto = new();
        try
        {
            var srvResponse = await _srvTransaBco.CreateAsync<APIResponse>(_sessionToken, transaBco);
            if (srvResponse is null)
            {
                resultResponse.IsSuccess = false;
                resultResponse.ErrorMessages = "No se pudo obtener la respuesta";
                return resultResponse;
            }

            if (srvResponse is { isSuccess: true })
            {
                transaBcoDto = JsonConvert.DeserializeObject<TransaccionesBcoDto>(Convert.ToString(srvResponse.result));

                if (transaBcoDto is null)
                {
                    resultResponse.IsSuccess = false;
                    resultResponse.ErrorMessages = $"Transacción bancaria no encontrada";
                    return resultResponse;
                }

                resultResponse.IsSuccess = true;
                resultResponse.Data = transaBcoDto;
            }
            else if (srvResponse is { isSuccess: false })
            {
                errorsMessagesBuilder.AppendJoin("", srvResponse.errorMessages);
                resultResponse.IsSuccess = false;
                resultResponse.ErrorMessages = errorsMessagesBuilder.ToString();
                return resultResponse;
            }

            return resultResponse;
        }
        catch (Exception ex)
        {
            resultResponse.IsSuccess = false;
            resultResponse.ErrorMessages = ex.Message;
            return resultResponse;
        }
    }

    private async Task<ResultResponse> fnCreateTransactionBcoDetail(TransaccionesBcoDetalleDtoCreate transaBcoDetalle)
    {
        ResultResponse? resultResponse = new() { IsSuccess = true };
        StringBuilder errorsMessagesBuilder = new();
        TransaccionesBcoDetalleDto? transaBcoDetalleDto = new();
        try
        {
            var srvResponse = await _srvTransaBcoDetalle.CreateAsync<APIResponse>(_sessionToken, transaBcoDetalle);
            if (srvResponse is null)
            {
                resultResponse.IsSuccess = false;
                resultResponse.ErrorMessages = "No se pudo obtener la respuesta";
                return resultResponse;
            }

            if (srvResponse is { isSuccess: true })
            {
                transaBcoDetalleDto = JsonConvert.DeserializeObject<TransaccionesBcoDetalleDto>(Convert.ToString(srvResponse.result));

                if (transaBcoDetalleDto is null)
                {
                    resultResponse.IsSuccess = false;
                    resultResponse.ErrorMessages = $"Detalle transacción bancaria no encontrada";
                    return resultResponse;
                }

                resultResponse.IsSuccess = true;
                resultResponse.Data = transaBcoDetalleDto;
            }
            else if (srvResponse is { isSuccess: false })
            {
                errorsMessagesBuilder.AppendJoin("", srvResponse.errorMessages);
                resultResponse.IsSuccess = false;
                resultResponse.ErrorMessages = errorsMessagesBuilder.ToString();
                return resultResponse;
            }

            return resultResponse;
        }
        catch (Exception ex)
        {
            resultResponse.IsSuccess = false;
            resultResponse.ErrorMessages = ex.Message;
            return resultResponse;
        }
    }

    private async Task<ResultResponse> fnCreateJournalHeader(AsientosContablesDtoCreate asientoCnt)
    {
        ResultResponse? resultResponse = new() { IsSuccess = true };
        StringBuilder errorsMessagesBuilder = new();
        AsientosContablesDto? asientoDto = new();
        try
        {
            var srvResponse = await _srvAsiento.CreateAsync<APIResponse>(_sessionToken, asientoCnt);
            if (srvResponse is null)
            {
                resultResponse.IsSuccess = false;
                resultResponse.ErrorMessages = "No se pudo obtener la respuesta";
                return resultResponse;
            }

            if (srvResponse is { isSuccess: true })
            {
                asientoDto = JsonConvert.DeserializeObject<AsientosContablesDto>(Convert.ToString(srvResponse.result));

                if (asientoDto is null)
                {
                    resultResponse.IsSuccess = false;
                    resultResponse.ErrorMessages = $"Asiento contable no encontrado";
                    return resultResponse;
                }

                resultResponse.IsSuccess = true;
                resultResponse.Data = asientoDto;
            }
            else if (srvResponse is { isSuccess: false })
            {
                errorsMessagesBuilder.AppendJoin("", srvResponse.errorMessages);
                resultResponse.IsSuccess = false;
                resultResponse.ErrorMessages = errorsMessagesBuilder.ToString();
                return resultResponse;
            }


            return resultResponse;
        }
        catch (Exception ex)
        {
            resultResponse.IsSuccess = false;
            resultResponse.ErrorMessages = ex.Message;
            return resultResponse;
        }
    }

    private async Task<ResultResponse> fnCreateJournalDetail(AsientosContablesDetalleDtoCreate asientoDetalle)
    {
        ResultResponse? resultResponse = new() { IsSuccess = true };
        StringBuilder errorsMessagesBuilder = new();
        AsientosContablesDetalleDto? asientoDetalleDto = new();
        try
        {
            var srvResponse = await _srvAsientoDetalle.CreateAsync<APIResponse>(_sessionToken, asientoDetalle);
            if (srvResponse is null)
            {
                resultResponse.IsSuccess = false;
                resultResponse.ErrorMessages = "No se pudo obtener la respuesta";
                return resultResponse;
            }

            if (srvResponse is { isSuccess: true })
            {
                asientoDetalleDto = JsonConvert.DeserializeObject<AsientosContablesDetalleDto>(Convert.ToString(srvResponse.result));

                if (asientoDetalleDto is null)
                {
                    resultResponse.IsSuccess = false;
                    resultResponse.ErrorMessages = $"Detalle asiento contable no encontrado";
                    return resultResponse;
                }

                resultResponse.IsSuccess = true;
            }
            else if (srvResponse is { isSuccess: false })
            {
                errorsMessagesBuilder.AppendJoin("", srvResponse.errorMessages);
                resultResponse.IsSuccess = false;
                resultResponse.ErrorMessages = errorsMessagesBuilder.ToString();
                return resultResponse;
            }

            return resultResponse;
        }
        catch (Exception ex)
        {
            resultResponse.IsSuccess = false;
            resultResponse.ErrorMessages = ex.Message;
            return resultResponse;
        }
    }

    private async Task<ResultResponse> fnGetBank(string bankCode)
    {
        ResultResponse? resultResponse = new() { IsSuccess = true };
        StringBuilder errorsMessagesBuilder = new();
        BancosDto? bankDto = new();
        try
        {
            var srvResponse = await _srvBanco.GetByCodeAsync<APIResponse>(_sessionToken, bankCode);
            if (srvResponse is null)
            {
                resultResponse.IsSuccess = false;
                resultResponse.ErrorMessages = "No se pudo obtener la respuesta";
                return resultResponse;
            }

            if (srvResponse is { isSuccess: true })
            {
                bankDto = JsonConvert.DeserializeObject<BancosDto>(Convert.ToString(srvResponse.result));

                if (bankDto is null)
                {
                    resultResponse.IsSuccess = false;
                    resultResponse.ErrorMessages = $"Banco: {bankCode} no encontrado";
                    return resultResponse;
                }
            }
            else if (srvResponse is { isSuccess: false })
            {
                errorsMessagesBuilder.AppendJoin("", srvResponse.errorMessages);
                resultResponse.IsSuccess = false;
                resultResponse.ErrorMessages = errorsMessagesBuilder.ToString();
                return resultResponse;
            }

            resultResponse.IsSuccess = true;
            resultResponse.Data = bankDto;

            return resultResponse;
        }
        catch (Exception ex)
        {
            resultResponse.IsSuccess = false;
            resultResponse.ErrorMessages = ex.Message;
            return resultResponse;
        }
    }

    private async Task<ResultResponse> fnGetBankAccount(string bankCode, Guid? bankAccountExcludeId, short currencyType
        , string currencyAbbreviation)
    {
        ResultResponse? resultResponse = new() { IsSuccess = true };
        StringBuilder errorsMessagesBuilder = new();
        List<CuentasBancariasDto>? bankAccountList = new();
        CuentasBancariasDto? bankAccountDto = new();
        try
        {
            var srvResponse = await _srvCuentaBancaria.GetAllByBankAsync<APIResponse>(_sessionToken, bankCode);
            if (srvResponse is null)
            {
                resultResponse.IsSuccess = false;
                resultResponse.ErrorMessages = "No se pudo obtener la respuesta";
                return resultResponse;
            }

            if (srvResponse is { isSuccess: true })
            {
                bankAccountList = JsonConvert.DeserializeObject<List<CuentasBancariasDto>>(Convert.ToString(srvResponse.result));

                if (bankAccountList is null || bankAccountList.Count == 0)
                {
                    resultResponse.IsSuccess = false;
                    resultResponse.ErrorMessages = $"Cuentas bancarias para el banco: {bankCode} no encontradas";
                    return resultResponse;
                }

                if (bankAccountExcludeId.HasValue)
                {
                    bankAccountList = bankAccountList
                        .Where(x => x.UidRegist != bankAccountExcludeId.Value).ToList();
                }

                bankAccountDto = bankAccountList
                    .FirstOrDefault(x => x.NumeroMoneda == currencyType);

                if (bankAccountDto is null)
                {
                    resultResponse.IsSuccess = false;
                    resultResponse.ErrorMessages = $"No se encontró la Cuenta bancaria en moneda {currencyAbbreviation} del Banco {bankCode}";
                    return resultResponse;
                }
            }
            else if (srvResponse is { isSuccess: false })
            {
                errorsMessagesBuilder.AppendJoin("", srvResponse.errorMessages);
                resultResponse.IsSuccess = false;
                resultResponse.ErrorMessages = errorsMessagesBuilder.ToString();
                return resultResponse;
            }

            resultResponse.IsSuccess = true;
            resultResponse.Data = bankAccountDto;

            return resultResponse;
        }
        catch (Exception ex)
        {
            resultResponse.IsSuccess = false;
            resultResponse.ErrorMessages = ex.Message;
            return resultResponse;
        }
    }

    private async Task<ResultResponse> fnGetBankAccountTransfer(string bankCode, string codigo, short currencyType)
    {
        ResultResponse? resultResponse = new() { IsSuccess = true };
        StringBuilder errorsMessagesBuilder = new();
        List<CuentasBancariasDto>? bankAccountList = new();
        CuentasBancariasDto? bankAccountDto = new();
        try
        {
            var srvResponse = await _srvCuentaBancaria.GetAllByBankAsync<APIResponse>(_sessionToken, bankCode);
            if (srvResponse is null)
            {
                resultResponse.IsSuccess = false;
                resultResponse.ErrorMessages = "No se pudo obtener la respuesta";
                return resultResponse;
            }

            if (srvResponse is { isSuccess: true })
            {
                bankAccountList = JsonConvert.DeserializeObject<List<CuentasBancariasDto>>(Convert.ToString(srvResponse.result));

                if (bankAccountList is null || bankAccountList.Count == 0)
                {
                    resultResponse.IsSuccess = false;
                    resultResponse.ErrorMessages = $"Cuentas bancarias para el banco: {bankCode} no encontradas";
                    return resultResponse;
                }

                bankAccountDto = bankAccountList
                    .FirstOrDefault(x => x.NumeroMoneda == currencyType &&
                                         x.Codigo.Trim() == codigo.Trim());

                if (bankAccountDto is null)
                {
                    resultResponse.IsSuccess = false;
                    resultResponse.ErrorMessages = "Cuenta bancaria no encontrada";
                    return resultResponse;
                }
            }
            else if (srvResponse is { isSuccess: false })
            {
                errorsMessagesBuilder.AppendJoin("", srvResponse.errorMessages);
                resultResponse.IsSuccess = false;
                resultResponse.ErrorMessages = errorsMessagesBuilder.ToString();
                return resultResponse;
            }

            resultResponse.IsSuccess = true;
            resultResponse.Data = bankAccountDto;

            return resultResponse;
        }
        catch (Exception ex)
        {
            resultResponse.IsSuccess = false;
            resultResponse.ErrorMessages = ex.Message;
            return resultResponse;
        }
    }

    private async Task<ResultResponse> fnGetDocument(int numberModule, int numberDocument)
    {
        ResultResponse? resultResponse = new() { IsSuccess = true };
        StringBuilder errorsMessagesBuilder = new();
        ModulosDto? moduloDto = new();
        ModulosDocumentosDto? moduloDocumentoDto = new();
        try
        {
            var srvResponse = await _srvModulo
                .GetByNumberAsync<APIResponse>(_sessionToken, numberModule);
            if (srvResponse is null)
            {
                resultResponse.IsSuccess = false;
                resultResponse.ErrorMessages = "No se pudo obtener la respuesta";
                return resultResponse;
            }

            if (srvResponse is { isSuccess: true })
            {
                moduloDto = JsonConvert.DeserializeObject<ModulosDto>(Convert.ToString(srvResponse.result));

                if (moduloDto is null)
                {
                    resultResponse.IsSuccess = false;
                    resultResponse.ErrorMessages = $"Modulo bancario no encontrado";
                    return resultResponse;
                }
            }
            else if (srvResponse is { isSuccess: false })
            {
                errorsMessagesBuilder.AppendJoin("", srvResponse.errorMessages);
                resultResponse.IsSuccess = false;
                resultResponse.ErrorMessages = errorsMessagesBuilder.ToString();
                return resultResponse;
            }

            srvResponse =
                await _srvModuloDocumento
                    .GetByNumberAsync<APIResponse>(_sessionToken, moduloDto.UidRegist,
                    numberDocument);

            if (srvResponse is null)
            {
                resultResponse.IsSuccess = false;
                resultResponse.ErrorMessages = "No se pudo obtener la respuesta";
                return resultResponse;
            }

            if (srvResponse is { isSuccess: true })
            {
                moduloDocumentoDto = JsonConvert.DeserializeObject<ModulosDocumentosDto>(Convert.ToString(srvResponse.result));

                if (moduloDocumentoDto is null)
                {
                    resultResponse.IsSuccess = false;
                    resultResponse.ErrorMessages = $"Modulo documento bancario no encontrado";
                    return resultResponse;
                }
            }
            else if (srvResponse is { isSuccess: false })
            {
                errorsMessagesBuilder.AppendJoin("", srvResponse.errorMessages);
                resultResponse.IsSuccess = false;
                resultResponse.ErrorMessages = errorsMessagesBuilder.ToString();
                return resultResponse;
            }

            resultResponse.IsSuccess = true;
            resultResponse.Data = moduloDto;
            resultResponse.DataChildren = moduloDocumentoDto;

            return resultResponse;
        }
        catch (Exception ex)
        {
            resultResponse.IsSuccess = false;
            resultResponse.ErrorMessages = ex.Message;
            return resultResponse;
        }
    }

    private async Task<ResultResponse> fnGetConfigurations()
    {
        ResultResponse? resultResponse = new() { IsSuccess = true };
        StringBuilder errorsMessagesBuilder = new();
        ConfigBcoDto? configBcoDto = new();
        ConfigCntDto? configCntDto = new();
        try
        {
            var srvResponse = await _srvConfigBco.GetAsync<APIResponse>(_sessionToken);
            if (srvResponse is null)
            {
                resultResponse.IsSuccess = false;
                resultResponse.ErrorMessages = "No se pudo obtener la respuesta";
                return resultResponse;
            }

            if (srvResponse is { isSuccess: true })
            {
                configBcoDto = JsonConvert.DeserializeObject<ConfigBcoDto>(Convert.ToString(srvResponse.result));

                if (configBcoDto is null)
                {
                    resultResponse.IsSuccess = false;
                    resultResponse.ErrorMessages = $"Configuración bancaria no encontrada";
                    return resultResponse;
                }

                if (configBcoDto.CuentacontableInterfaz == null || configBcoDto.CuentacontableInterfaz == Guid.Empty)
                {
                    resultResponse.IsSuccess = false;
                    resultResponse.ErrorMessages = $"Configuración bancaria cuenta contable de interfaz es requerida";
                    return resultResponse;
                }

                if (configBcoDto.CuentaContableComisionTransferencia == null || configBcoDto.CuentaContableComisionTransferencia == Guid.Empty)
                {
                    resultResponse.IsSuccess = false;
                    resultResponse.ErrorMessages = $"Configuración bancaria cuenta contable de comisión por transferencia es requerida";
                    return resultResponse;
                }

                if (configBcoDto.CuentaContableDifPositivaConciliacion == null || configBcoDto.CuentaContableDifPositivaConciliacion == Guid.Empty)
                {
                    resultResponse.IsSuccess = false;
                    resultResponse.ErrorMessages = $"Configuración bancaria cuenta contable de diferencia positiva conciliación es requerida";
                    return resultResponse;
                }

                if (configBcoDto.CuentaContableDifNegativaConciliacion == null || configBcoDto.CuentaContableDifNegativaConciliacion == Guid.Empty)
                {
                    resultResponse.IsSuccess = false;
                    resultResponse.ErrorMessages = $"Configuración bancaria cuenta contable de diferencia negativa conciliación es requerida";
                    return resultResponse;
                }
            }
            else if (srvResponse is { isSuccess: false })
            {
                errorsMessagesBuilder.AppendJoin("", srvResponse.errorMessages);
                resultResponse.IsSuccess = false;
                resultResponse.ErrorMessages = errorsMessagesBuilder.ToString();
                return resultResponse;
            }

            srvResponse = await _srvConfigCnt.GetAsync<APIResponse>(_sessionToken);
            if (srvResponse is null)
            {
                resultResponse.IsSuccess = false;
                resultResponse.ErrorMessages = "No se pudo obtener la respuesta";
                return resultResponse;
            }

            if (srvResponse is { isSuccess: true })
            {
                configCntDto = JsonConvert.DeserializeObject<ConfigCntDto>(Convert.ToString(srvResponse.result));

                if (configCntDto is null)
                {
                    resultResponse.IsSuccess = false;
                    resultResponse.ErrorMessages = $"Configuración contable no encontrada";
                    return resultResponse;
                }

                if (configCntDto.CuentaContableGananciaDiferencial == null || configCntDto.CuentaContableGananciaDiferencial == Guid.Empty)
                {
                    resultResponse.IsSuccess = false;
                    resultResponse.ErrorMessages = $"Configuración contable cuentas contable de ganancia por dif. cambiario es requerida";
                    return resultResponse;
                }

                if (configCntDto.CuentaContablePerdidaDiferencial == null || configCntDto.CuentaContablePerdidaDiferencial == Guid.Empty)
                {
                    resultResponse.IsSuccess = false;
                    resultResponse.ErrorMessages = $"Configuración contable cuentas contable de perdida por dif. cambiario es requerida";
                    return resultResponse;
                }
            }
            else if (srvResponse is { isSuccess: false })
            {
                errorsMessagesBuilder.AppendJoin("", srvResponse.errorMessages);
                resultResponse.IsSuccess = false;
                resultResponse.ErrorMessages = errorsMessagesBuilder.ToString();
                return resultResponse;
            }

            resultResponse.IsSuccess = true;
            resultResponse.Data = configBcoDto;
            resultResponse.DataChildren = configCntDto;

            return resultResponse;
        }
        catch (Exception ex)
        {
            resultResponse.IsSuccess = false;
            resultResponse.ErrorMessages = ex.Message;
            return resultResponse;
        }
    }

    private async Task<ResultResponse> fnGetNextSecuential(
        mexModules modulo,
        Guid bankAccountId,
        int fiscalYear,
        int fiscalMonth,
        short tipo,
        short subtipo)
    {
        ResultResponse? resultResponse = new() { IsSuccess = true };
        StringBuilder errorsMessagesBuilder = new();
        APIResponse? srvResponse;
        TransaccionResponse? transaResponse = new();

        try
        {
            if (modulo == mexModules.Bank)
            {
                srvResponse = await _srvTransaBco.GetNextSecuentialNumberAsync<APIResponse>(
                    _sessionToken,
                    bankAccountId,
                    fiscalYear,
                    fiscalMonth,
                    tipo, subtipo,
                    ConsecutivoTipo.Temporal,
                    isSave: true);

                if (srvResponse is null)
                {
                    resultResponse.IsSuccess = false;
                    resultResponse.ErrorMessages = "No se pudo obtener la respuesta";
                    return resultResponse;
                }
                if (srvResponse is { isSuccess: true })
                {
                    transaResponse = JsonConvert.DeserializeObject<TransaccionResponse>(Convert.ToString(srvResponse.result));

                    if (transaResponse is null)
                    {
                        resultResponse.IsSuccess = false;
                        resultResponse.ErrorMessages = $"Consecutivo bancario no encontrado";
                        return resultResponse;
                    }
                }
                else if (srvResponse is { isSuccess: false })
                {
                    errorsMessagesBuilder.AppendJoin("", srvResponse.errorMessages);
                    resultResponse.IsSuccess = false;
                    resultResponse.ErrorMessages = errorsMessagesBuilder.ToString();
                    return resultResponse;
                }

                resultResponse.IsSuccess = true;
                resultResponse.Data = transaResponse;
            }
            else if (modulo == mexModules.Account)
            {
                string numberTransaCnt = string.Empty;

                srvResponse = await _srvAsiento.GetNextSequentialNumberAsync<APIResponse>(
                    _sessionToken, fiscalYear, fiscalMonth,
                    tipo, subtipo, ConsecutivoTipo.Temporal, isSave: true);

                if (srvResponse is null)
                {
                    resultResponse.IsSuccess = false;
                    resultResponse.ErrorMessages = "No se pudo obtener la respuesta";
                    return resultResponse;
                }

                if (srvResponse is { isSuccess: true })
                {
                    numberTransaCnt = JsonConvert.DeserializeObject<string>(Convert.ToString(srvResponse.result)) ?? string.Empty;

                    if (numberTransaCnt is null)
                    {
                        resultResponse.IsSuccess = false;
                        resultResponse.ErrorMessages = $"Consecutivo contable no encontrado";
                        return resultResponse;
                    }
                }
                else if (srvResponse is { isSuccess: false })
                {
                    errorsMessagesBuilder.AppendJoin("", srvResponse.errorMessages);
                    resultResponse.IsSuccess = false;
                    resultResponse.ErrorMessages = errorsMessagesBuilder.ToString();
                    return resultResponse;
                }

                resultResponse.IsSuccess = true;
                resultResponse.Data = numberTransaCnt;
            }

            return resultResponse;
        }
        catch (Exception ex)
        {
            resultResponse.IsSuccess = false;
            resultResponse.ErrorMessages = ex.Message;
            return resultResponse;
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
        using var wb = new XLWorkbook();
        int sheetIndex = 1; // Inicializar el índice de la hoja de trabajo

        foreach (Quotation header in listEntities)
        {
            string sheetName = $"{header.TypeTrx.Code}_{header.Numeral.ToString().PadLeft(3, AC.CharDefaultEmpty)}_{header.DateTransa.ToString(AC.DefaultDateFormatWeb)}";
            IXLWorksheet worksheet = wb.Worksheets.Add(sheetName);

            Company objCompany = _uow.Company.Get(filter: x => x.Id == _companyId);

            // Escribir el nombre de la compañía en la primera fila
            worksheet.Cell(1, 1).Value = objCompany.Name;
            worksheet.Range(1, 1, 1, 7).Merge().Style.Font.Bold = true;
            worksheet.Range(1, 1, 1, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Range(1, 1, 1, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            IXLRow headerRow = worksheet.Row(3);
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
            worksheet.Cell(6, 3).Value = header.AmountCostReal;
            worksheet.Cell(6, 3).Style.NumberFormat.Format = AC.XlsFormatNumeric;
            worksheet.Cell(6, 4).Value = header.AmountRevenueReal;
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

                                if (objQuotationType.Numeral != (int)SD.QuotationType.Transport)
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

                                header.CreatedBy = worksheet.Cell(4, 13).GetString().Trim();

                                var createdDate = worksheet.Cell(4, 14).GetString().Trim();

                                if (!string.IsNullOrEmpty(createdDate))
                                {
                                    header.CreatedDate = DateTime.Parse(createdDate);
                                }

                                header.UpdatedBy = worksheet.Cell(4, 15).GetString().Trim();

                                var updatedDate = worksheet.Cell(4, 16).GetString().Trim();

                                if (!string.IsNullOrEmpty(updatedDate))
                                {
                                    header.UpdatedDate = DateTime.Parse(updatedDate);
                                }

                                header.ClosedBy = worksheet.Cell(6, 13).GetString().Trim();

                                var closedDate = worksheet.Cell(6, 14).GetString().Trim();

                                if (!string.IsNullOrEmpty(closedDate))
                                {
                                    header.ClosedDate = DateTime.Parse(closedDate);
                                }

                                header.ReClosedBy = worksheet.Cell(6, 15).GetString().Trim();

                                var reClosedDate = worksheet.Cell(6, 16).GetString().Trim();

                                if (!string.IsNullOrEmpty(reClosedDate))
                                {
                                    header.ReClosedDate = DateTime.Parse(reClosedDate);
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

                var objCurrencyRateList = _uow.CurrencyExchangeRate.GetAll
                (x => (x.CompanyId == _companyId) && (x.DateTransa == header.DateTransa)
                    , includeProperties: "CurrencyTrx").ToList();

                if (objCurrencyRateList is null || objCurrencyRateList.Count == 0)
                {
                    jsonResponse.IsSuccess = false;
                    jsonResponse.ErrorMessages = $"Tipo de cambio no encontrado";
                    return Json(jsonResponse);
                }

                var currencyForeign = objCurrencyRateList
                    .FirstOrDefault(t => (t.CurrencyType == CurrencyType.Foreign))?.OfficialRate ?? 1;

                header.ExchangeRateOfficialBase = currencyForeign;

                if (header.TypeNumeral != SD.QuotationType.Transport)
                {
                    if (header.TypeNumeral == SD.QuotationType.Buy)
                    {
                        //TC COMPRA MENOR AL TC OFICIAL
                        if (header.ExchangeRateBuyTransa < header.ExchangeRateOfficialTransa)
                        {
                            header.AmountRevenue = (header.ExchangeRateOfficialTransa - header.ExchangeRateBuyTransa) * header.AmountTransaction;
                            header.AmountCost = 0;
                            header.AmountRevenue = header.AmountRevenue.RoundTo(AC.DecimalTransa);

                        }
                        //TC COMPRA MAYOR AL TC OFICIAL
                        else
                        {
                            header.AmountCost = (header.ExchangeRateBuyTransa - header.ExchangeRateOfficialTransa) * header.AmountTransaction;
                            header.AmountRevenue = 0;
                            header.AmountCost = header.AmountCost.RoundTo(AC.DecimalTransa);
                        }

                        //Compra de dolares 
                        if (header.CurrencyTransaType == CurrencyType.Foreign)
                        {
                            //Factoring paga en Cordobas
                            if (header.CurrencyTransferType == CurrencyType.Base)
                            {
                                header.AmountExchange = (header.AmountTransaction * header.ExchangeRateBuyTransa);
                                header.ExchangeRateBuyReal = header.ExchangeRateBuyTransa;

                                header.AmountCostReal = header.AmountCost;
                                header.AmountRevenueReal = header.AmountRevenue;

                                header.AmountCostReal = header.AmountCostReal.RoundTo(AC.DecimalTransa);
                                header.AmountRevenueReal = header.AmountRevenueReal.RoundTo(AC.DecimalTransa);
                            }
                        }
                        //Compra de Euros
                        else if (header.CurrencyTransaType == CurrencyType.Additional)
                        {
                            //Factoring paga en Cordobas
                            if (header.CurrencyTransferType == CurrencyType.Base)
                            {
                                header.AmountExchange = (header.AmountTransaction * header.ExchangeRateBuyTransa);
                                header.ExchangeRateBuyReal = header.ExchangeRateBuyTransa;

                                header.AmountCostReal = header.AmountCost;
                                header.AmountRevenueReal = header.AmountRevenue;

                                header.AmountCostReal = header.AmountCostReal.RoundTo(AC.DecimalTransa);
                                header.AmountRevenueReal = header.AmountRevenueReal.RoundTo(AC.DecimalTransa);

                            }
                            //Factoring paga en Dolares
                            else if (header.CurrencyTransferType == CurrencyType.Foreign)
                            {
                                header.AmountExchange = (header.AmountTransaction * header.ExchangeRateBuyTransa);
                                header.ExchangeRateBuyReal = (header.ExchangeRateBuyTransa * header.ExchangeRateOfficialTransa);

                                header.AmountCostReal = header.AmountCost * currencyForeign;
                                header.AmountRevenueReal = header.AmountRevenue * currencyForeign;

                                header.ExchangeRateOfficialReal = currencyForeign * header.ExchangeRateOfficialTransa;
                                header.ExchangeRateBuyReal = currencyForeign * header.ExchangeRateBuyTransa;

                                header.AmountCostReal = header.AmountCostReal.RoundTo(AC.DecimalTransa);
                                header.AmountRevenueReal = header.AmountRevenueReal.RoundTo(AC.DecimalTransa);
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
                            header.AmountCost = header.AmountCost.RoundTo(AC.DecimalTransa);
                        }
                        //TC VENTA MAYOR AL TC OFICIAL
                        else
                        {
                            header.AmountRevenue = (header.ExchangeRateSellTransa - header.ExchangeRateOfficialTransa) * header.AmountTransaction;
                            header.AmountCost = 0;
                            header.AmountRevenue = header.AmountRevenue.RoundTo(AC.DecimalTransa);
                        }

                        //Venta de dolares 
                        if (header.CurrencyTransaType == CurrencyType.Foreign)
                        {
                            //Cliente paga en Cordobas
                            if (header.CurrencyDepositType == CurrencyType.Base)
                            {
                                header.AmountExchange = (header.AmountTransaction * header.ExchangeRateSellTransa);
                                header.ExchangeRateSellReal = header.ExchangeRateSellTransa;

                                header.AmountCostReal = header.AmountCost;
                                header.AmountRevenueReal = header.AmountRevenue;

                                header.AmountCostReal = header.AmountCostReal.RoundTo(AC.DecimalTransa);
                                header.AmountRevenueReal = header.AmountRevenueReal.RoundTo(AC.DecimalTransa);
                            }
                        }
                        //Venta de Euros
                        else if (header.CurrencyTransaType == CurrencyType.Additional)
                        {
                            //Cliente paga en Cordobas
                            if (header.CurrencyDepositType == CurrencyType.Base)
                            {
                                header.AmountExchange = (header.AmountTransaction * header.ExchangeRateSellTransa);
                                header.ExchangeRateSellReal = header.ExchangeRateSellTransa;

                                header.AmountCostReal = header.AmountCost;
                                header.AmountRevenueReal = header.AmountRevenue;

                                header.AmountCostReal = header.AmountCostReal.RoundTo(AC.DecimalTransa);
                                header.AmountRevenueReal = header.AmountRevenueReal.RoundTo(AC.DecimalTransa);

                            }
                            //Cliente paga en Dolares
                            else if (header.CurrencyDepositType == CurrencyType.Foreign)
                            {
                                header.AmountExchange = (header.AmountTransaction * header.ExchangeRateSellTransa);
                                header.ExchangeRateSellReal = (header.ExchangeRateSellTransa * header.ExchangeRateOfficialTransa);

                                header.AmountCostReal = header.AmountCost * currencyForeign;
                                header.AmountRevenueReal = header.AmountRevenue * currencyForeign;

                                header.ExchangeRateOfficialReal = currencyForeign * header.ExchangeRateOfficialTransa;
                                header.ExchangeRateSellReal = currencyForeign * header.ExchangeRateSellTransa;

                                header.AmountCostReal = header.AmountCostReal.RoundTo(AC.DecimalTransa);
                                header.AmountRevenueReal = header.AmountRevenueReal.RoundTo(AC.DecimalTransa);
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
                    header.ExchangeRateOfficialReal = header.ExchangeRateOfficialTransa;

                    var objDetailCredit = objQuotationDetailList.First(x => x.ParentId == header.Id &&
                                                                            x.QuotationDetailType == QuotationDetailType.CreditTransfer);
                    var objDetailDebit = objQuotationDetailList.First(x => x.ParentId == header.Id &&
                                                                            x.QuotationDetailType == QuotationDetailType.DebitTransfer);


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
                        CreatedIpv4 = AC.Ipv4Default,
                        BankTransactionId = objDetailCredit.BankTransactionId,
                        JournalEntryId = objDetailCredit.JournalEntryId
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
                        CreatedIpv4 = AC.Ipv4Default,
                        BankTransactionId = objDetailDebit.BankTransactionId,
                        JournalEntryId = objDetailDebit.JournalEntryId,
                        BankTransactionTransferFeeId = objDetailDebit.BankTransactionTransferFeeId,
                        JournalEntryTransferFeeId = objDetailDebit.JournalEntryTransferFeeId

                    };

                    objQuotationDetailList.Add(objDetailBankAccountSource);
                }

                header.BusinessExecutiveCode = objBusiness.Code;
                header.IsLoan = objBusiness.IsLoan;
                header.IsPayment = objBusiness.IsPayment;

                ////Seteamos campos de auditoria
                header.CreatedBy = _userName ?? AC.LOCALHOSTME;
                header.CreatedDate = DateTime.UtcNow;
                header.CreatedHostName = AC.LOCALHOSTPC;
                header.CreatedIpv4 = _ipAddress?.ToString() ?? AC.Ipv4Default;

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
                    header.ClosedBy = _userName ?? AC.LOCALHOSTME;
                    header.ClosedDate = DateTime.UtcNow;
                    header.ClosedHostName = AC.LOCALHOSTPC;
                    header.ClosedIpv4 = _ipAddress?.ToString() ?? AC.Ipv4Default;
                }

                if (header.TypeNumeral != SD.QuotationType.Transport)
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
                        detail.PercentageCostRevenue = (detail.AmountDetail / header.AmountTransaction);

                        if (header.TypeNumeral == SD.QuotationType.Buy)
                        {
                            if (header.CurrencyTransaType == CurrencyType.Foreign)
                            {
                                if (header.CurrencyTransferType == CurrencyType.Base)
                                {
                                    detail.PercentageCostRevenue = (detail.AmountDetail / header.AmountExchange);
                                }
                            }
                            else if (header.CurrencyTransaType == CurrencyType.Additional)
                            {
                                if (header.CurrencyTransferType == CurrencyType.Base)
                                {
                                }
                                else if (header.CurrencyTransferType == CurrencyType.Foreign)
                                {
                                }
                            }

                            if (detail.QuotationDetailType == QuotationDetailType.Deposit)
                            {
                                detail.CurrencyDetailId = header.CurrencyTransaId;
                                detail.LineNumber = lineNumberDeposit;
                                lineNumberDeposit++;
                            }
                            else if (detail.QuotationDetailType == QuotationDetailType.Transfer)
                            {
                                detail.CurrencyDetailId = header.CurrencyTransferId;
                                detail.LineNumber = lineNumberTransfer;
                                lineNumberTransfer++;
                            }
                        }
                        else if (header.TypeNumeral == SD.QuotationType.Sell)
                        {
                            if (detail.QuotationDetailType == QuotationDetailType.Deposit)
                            {
                                detail.CurrencyDetailId = header.CurrencyDepositId;
                                detail.LineNumber = lineNumberDeposit;
                                lineNumberDeposit++;
                            }
                            else if (detail.QuotationDetailType == QuotationDetailType.Transfer)
                            {
                                detail.CurrencyDetailId = header.CurrencyTransaId;
                                detail.LineNumber = lineNumberTransfer;
                                lineNumberTransfer++;
                            }
                        }

                        if (header.AmountCost != 0)
                        {
                            detail.AmountCost = (detail.PercentageCostRevenue * header.AmountCostReal);
                            detail.AmountCost = detail.AmountCost.RoundTo(AC.DecimalTransa);
                        }
                        else if (header.AmountRevenue != 0)
                        {
                            detail.AmountRevenue = (detail.PercentageCostRevenue * header.AmountRevenueReal);
                            detail.AmountRevenue = detail.AmountRevenue.RoundTo(AC.DecimalTransa);
                        }

                        if (detail.QuotationDetailType == QuotationDetailType.Deposit)
                        {
                            detail.PercentageCostRevenue = 0;
                            detail.AmountRevenue = 0;
                            detail.AmountCost = 0;
                        }

                    }
                }
            }

            //Mandar al Log

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
                report.RegBusinessObject(AC.DatRep, JsonConvert.DeserializeObject<List<QuotationReportVM>>(datRepJson));

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

            //Tipo de Cambio
            decimal tcExchange = transaction.TypeNumeral == SD.QuotationType.Buy ? transaction.ExchangeRateBuyTransa
                : transaction.ExchangeRateSellTransa;

            var tcExchangeString = !transaction.IsAdjustment ? tcExchange.RoundTo(_decimalExchange).ToString()
                : tcExchange.RoundTo(_decimalExchangeFull).ToString();

            // Crear listado de objetos basados en el detalle
            var dataDetails = new List<QuotationReportVM>();
            foreach (var itemDetail in transaDetails)
            {
                dataDetails.Add(new QuotationReportVM()
                {
                    CustomerFullName = transaction.CustomerTrx.BusinessName,
                    // Campos de agrupamiento
                    ParentQuotationId = transaction.Id,
                    ParentTransactionNumber = transaction.Numeral,
                    // --
                    BankTargetFullName = itemDetail.BankTargetTrx.Code,
                    IsClosed = transaction.IsClosed,
                    CurrencyTransferCode = transaction.CurrencyTransferTrx.Code,
                    AmountTransaction = itemDetail.AmountDetail,
                    ConceptGeneral = $"{Enum.GetName(typeof(SD.QuotationTypeName), (int)transaction.TypeNumeral)} de {transaction.CurrencyTransaTrx.NameSingular} TC:{tcExchangeString}",
                    NumberReferen = $"{Enum.GetName(typeof(SD.QuotationTypeNameAbrv), (int)transaction.TypeNumeral)!.ToUpper()}-{transaction.Numeral.ToString().PadLeft(3, AC.CharDefaultEmpty)}-{itemDetail.BankTargetTrx.Code}{transaction.DateTransa.Year}{transaction.DateTransa.Month.ToString().PadLeft(2, AC.CharDefaultEmpty)}{transaction.DateTransa.Day.ToString().PadLeft(2, AC.CharDefaultEmpty)}",
                    DescriptionGeneral = $"Por este medio se confirma el envío por transferencia bancaria, producto de la operación de cambio afectuada el dia de hoy {transaction.DateTransa.Day} de {Enum.GetName(typeof(SD.MonthName), transaction.DateTransa.Month)} del año {transaction.DateTransa.Year}"
                });
            }

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
            HttpContext.Session.SetString(AC.DatRep, JsonConvert.SerializeObject(dataDetails));

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

    // Exportar a PDF las cotizaciones
    [HttpPost]
    public JsonResult ExportCreditNoteToPDF([FromBody] List<int> quotationIds, bool isFileSeparated = false)
    {
        JsonResultResponse jsonResponse = new();
        StiReport reportResult = new();

        try
        {
            var fileName = "Quotation.mrt";
            var filePath = $"{Path.Combine(_hostEnvironment.ContentRootPath, "Areas", "Exchange", "Reports", fileName)}";

            if (!System.IO.File.Exists(filePath))
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = "Reporte no encontrado";
                return Json(jsonResponse);
            }

            if (Path.GetExtension(fileName).ToUpper() != ".MRT")
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = "Reporte invalido";
                return Json(jsonResponse);
            }

            var configFac = _uow.ConfigFac.Get(filter: x => x.CompanyId == _companyId);
            if (configFac is null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = "Configuración de facturación no encontrada";
                return Json(jsonResponse);
            }

            var company = _uow.Company.Get(filter: x => x.Id == _companyId);
            if (company is null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = "Compañia no encontrada";
                return Json(jsonResponse);
            }

            var transaDetails = _uow.QuotationDetail.GetAll(filter: x => (x.CompanyId == _companyId)
                                                                         && quotationIds.Contains(x.ParentId)
                                                                         && x.QuotationDetailType == QuotationDetailType.Transfer,
                includeProperties: "ParentTrx,CurrencyDetailTrx,BankSourceTrx,BankTargetTrx").ToList();
            if (transaDetails is null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = "Cotización sin hijos encontrados";
                return Json(jsonResponse);
            }

            var dataDetails = new List<QuotationReportVM>();
            var transaDetailsGroup = transaDetails.GroupBy(x => x.ParentId).ToList();
            var listPdfFiles = new List<(string FileName, byte[] FileContent)>();

            foreach (var itemDetailList in transaDetailsGroup)
            {
                var transaction = _uow
                    .Quotation.Get(filter: x =>
                        x.CompanyId == _companyId &&
                        x.Id == itemDetailList.Key,
                        includeProperties: "TypeTrx,CustomerTrx,CurrencyDepositTrx,CurrencyTransferTrx,CurrencyTransaTrx");

                if (transaction is null)
                {
                    jsonResponse.IsSuccess = false;
                    jsonResponse.ErrorMessages = "Cotización no encontrada";
                    return Json(jsonResponse);
                }

                var tcExchange = transaction.TypeNumeral == SD.QuotationType.Buy ?
                    transaction.ExchangeRateBuyTransa : transaction.ExchangeRateSellTransa;
                var tcExchangeString = !transaction.IsAdjustment ?
                    tcExchange.RoundTo(_decimalExchange).ToString() :
                    tcExchange.RoundTo(_decimalExchangeFull).ToString();

                foreach (var itemDetail in itemDetailList)
                {
                    dataDetails.Add(new QuotationReportVM()
                    {
                        CustomerFullName = transaction.CustomerTrx.BusinessName,
                        ParentQuotationId = transaction.Id,
                        ParentTransactionNumber = transaction.Numeral,
                        ParentDateTransaFormat = transaction.DateTransa.ToString(AC.DefaultDateFormatView),
                        ParentTransactionNumberFormat = $"{Enum.GetName(typeof(SD.QuotationTypeNameAbrv), (int)transaction.TypeNumeral)!.ToUpper()}-{transaction.Numeral.ToString().PadLeft(3, AC.CharDefaultEmpty)}",
                        BankTargetFullName = itemDetail.BankTargetTrx.Code,
                        IsClosed = transaction.IsClosed,
                        CurrencyTransferCode = transaction.CurrencyTransferTrx.Code,
                        AmountTransaction = itemDetail.AmountDetail,
                        ConceptGeneral = $"{Enum.GetName(typeof(SD.QuotationTypeName), (int)transaction.TypeNumeral)} de {transaction.CurrencyTransaTrx.NameSingular} TC:{tcExchangeString}",
                        NumberReferen = $"{Enum.GetName(typeof(SD.QuotationTypeNameAbrv), (int)transaction.TypeNumeral)!.ToUpper()}-{transaction.Numeral.ToString().PadLeft(3, AC.CharDefaultEmpty)}-{itemDetail.BankTargetTrx.Code}{transaction.DateTransa.Year}{transaction.DateTransa.Month.ToString().PadLeft(2, AC.CharDefaultEmpty)}{transaction.DateTransa.Day.ToString().PadLeft(2, AC.CharDefaultEmpty)}",
                        DescriptionGeneral = $"Por este medio se confirma el envío por transferencia bancaria, producto de la operación de cambio afectuada el dia de hoy {transaction.DateTransa.Day} de {Enum.GetName(typeof(SD.MonthName), transaction.DateTransa.Month)} del año {transaction.DateTransa.Year}"
                    });
                }

                if (isFileSeparated)
                {
                    var tipoTransa = Enum.GetName(typeof(SD.QuotationTypeNameAbrv), (int)transaction.TypeNumeral);
                    var numberTransa = transaction.Numeral.ToString().PadLeft(3, AC.CharDefaultEmpty);
                    var pdfFileName = $"NotaCredito_{tipoTransa}_{numberTransa}.pdf";

                    var individualReport = new StiReport();
                    individualReport.Load(StiNetCoreHelper.MapPath(this, filePath));

                    // Decimales
                    individualReport.Dictionary.Variables[AC.ParDecimalTransaction].ValueObject = _decimalTransa;
                    individualReport.Dictionary.Variables[AC.ParDecimalExchangeRate].ValueObject = _decimalExchange;
                    individualReport.Dictionary.Variables[AC.ParNameCompany].ValueObject = $"{company.Name}";
                    individualReport.Dictionary.Variables[AC.ParNameReport].ValueObject = "Nota de Crédito";
                    individualReport.Dictionary.Variables[AC.ParFileImagePath].ValueObject = $"{company.ImageLogoUrl}";
                    individualReport.Dictionary.Variables[AC.ParIsGeneral].ValueObject = true;
                    string isClosed = transaction.IsClosed ? "" : "No Cerrado";
                    individualReport.Dictionary.Variables["parIsClosed"].ValueObject = isClosed;

                    individualReport.ReportName = $"NotaCredito_{tipoTransa}_{numberTransa}";

                    individualReport.RegBusinessObject(AC.DatRep, dataDetails);
                    individualReport.Compile();
                    individualReport.Render();

                    using var stream = new MemoryStream();
                    individualReport.ExportDocument(StiExportFormat.Pdf, stream);
                    listPdfFiles.Add((pdfFileName, stream.ToArray()));

                    // Limpiar los detalles
                    dataDetails.Clear();
                }
            }

            // Si es archivos separados colocar dentro de un ZIP
            if (isFileSeparated)
            {
                using var memoryStream = new MemoryStream();
                using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var (pdfFileName, pdfData) in listPdfFiles)
                    {
                        var fileEntry = zip.CreateEntry(pdfFileName, CompressionLevel.Optimal);
                        using var entryStream = fileEntry.Open();
                        using var fileStream = new MemoryStream(pdfData);
                        fileStream.CopyTo(entryStream);
                    }
                }

                var zipBytes = memoryStream.ToArray();
                var exportReporteBase64 = Convert.ToBase64String(zipBytes);

                jsonResponse.IsSuccess = true;
                DateTime dateReport = DateTime.Now;

                jsonResponse.Data = new
                {
                    ContentFile = exportReporteBase64,
                    ContentType = AC.ContentTypeZip,
                    Filename = $"NotasDeCreditos_{dateReport:yyyyMMdd}_{dateReport:HHmmss}"
                };
                return Json(jsonResponse);
            }
            // Si es un solo PDF
            else
            {
                // Cargar reporte
                reportResult.Load(StiNetCoreHelper.MapPath(this, filePath));

                // Decimales
                reportResult.Dictionary.Variables[AC.ParDecimalTransaction].ValueObject = _decimalTransa;
                reportResult.Dictionary.Variables[AC.ParDecimalExchangeRate].ValueObject = _decimalExchange;
                reportResult.Dictionary.Variables[AC.ParNameCompany].ValueObject = $"{company.Name}";
                reportResult.Dictionary.Variables[AC.ParNameReport].ValueObject = "Nota de Crédito";
                reportResult.Dictionary.Variables[AC.ParFileImagePath].ValueObject = $"{company.ImageLogoUrl}";
                reportResult.Dictionary.Variables[AC.ParIsGeneral].ValueObject = true;

                reportResult.ReportName = "Notas de Créditos";

                reportResult.RegBusinessObject(AC.DatRep, dataDetails);

                reportResult.Compile();
                reportResult.Render();

                // Generar el PDF en memoria
                byte[] pdfData;
                using (var stream = new MemoryStream())
                {
                    reportResult.ExportDocument(StiExportFormat.Pdf, stream);
                    pdfData = stream.ToArray();
                }

                // Convertir los bytes a base64
                var exportReporteBase64 = Convert.ToBase64String(pdfData);
                DateTime dateReport = DateTime.Now;

                jsonResponse.IsSuccess = true;
                jsonResponse.Data = new
                {
                    ContentFile = exportReporteBase64,
                    ContentType = AC.ContentTypePdf,
                    Filename = $"NotasDeCreditosEnLote_{dateReport:yyyyMMdd}_{dateReport:HHmmss}"
                };
                return Json(jsonResponse);
            }
        }
        catch (Exception ex)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = ex.Message;
            return Json(jsonResponse);
        }
    }

    // Exportar a Excel las Operaciones
    [HttpPost]
    public JsonResult ExportOperationToExcel([FromBody] List<int> quotationIds)
    {
        JsonResultResponse jsonResponse = new();
        StiReport reportResult = new();

        try
        {
            // Cargar y verificar el archivo del reporte
            var fileName = "OperationList.mrt";
            var filePath = Path.Combine(_hostEnvironment.ContentRootPath, "Areas", "Exchange", "Reports", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = "Reporte no encontrado";
                return Json(jsonResponse);
            }

            // Verificar extensión del archivo
            if (Path.GetExtension(fileName).ToUpper() != ".MRT")
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = "Reporte inválido";
                return Json(jsonResponse);
            }

            // Obtener configuraciones y validaciones adicionales
            var configFac = _uow.ConfigFac.Get(filter: x => x.CompanyId == _companyId);
            if (configFac == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = "Configuración de facturación no encontrada";
                return Json(jsonResponse);
            }

            var company = _uow.Company.Get(filter: x => x.Id == _companyId);
            if (company == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = "Compañía no encontrada";
                return Json(jsonResponse);
            }

            // Obtener la cotización
            var transactionList = _uow.Quotation.GetAll(filter: x => (x.CompanyId == _companyId)
                                                                     && (quotationIds.Contains(x.Id))
                                                                        // No se incluyen TRA
                                                                        && (x.TypeNumeral != SD.QuotationType.Transport),
                includeProperties: "TypeTrx,CustomerTrx,CurrencyDepositTrx,CurrencyTransferTrx,CurrencyTransaTrx")
                .OrderByDescending(x => x.Id)
                .ToList();

            if (transactionList is null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Cotización invalida";
                return Json(jsonResponse);
            }

            if (transactionList.Count() == 0)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.IsInfo = true;
                jsonResponse.ErrorMessages = $"No hay operaciones";
                return Json(jsonResponse);
            }

            List<TransaODTVM> transaListVM = new();

            foreach (Quotation? transaction in transactionList)
            {
                string currency = transaction.TypeNumeral == SD.QuotationType.Buy ||
                    transaction.TypeNumeral == SD.QuotationType.Transport
                    ? $"{transaction.CurrencyTransaTrx.Code}-{transaction.CurrencyTransferTrx.Code}"
                    : $"{transaction.CurrencyTransaTrx.Code}-{transaction.CurrencyDepositTrx.Code}";


                var transa = new TransaODTVM
                {
                    Id = transaction.Id,
                    CompanyId = transaction.CompanyId,
                    TypeNumeral = transaction.TypeNumeral,
                    NumberTransa = $"{Enum.GetName(typeof(SD.QuotationTypeNameAbrv), (int)transaction.TypeNumeral)}-{transaction.Numeral.ToString().PadLeft(3, AC.CharDefaultEmpty)}",
                    CustomerFullName = transaction.CustomerTrx.BusinessName,
                    CurrencySourceTarget = currency,
                    ExchangeRateTransa = transaction.TypeNumeral == SD.QuotationType.Buy ? transaction.ExchangeRateBuyTransa : transaction.ExchangeRateSellTransa,
                    ExchangeRateOfficialTransa = transaction.ExchangeRateOfficialTransa,
                    AmountTransaction = transaction.AmountTransactionRpt,
                    AmountRevenue = transaction.AmountRevenueRpt,
                    AmountCost = transaction.AmountCostRpt,
                    TotalDeposit = transaction.TotalDepositRpt,
                    TotalTransfer = transaction.TotalTransferRpt,
                    ExecutiveCode = transaction.BusinessExecutiveCode,
                    IsClosed = transaction.IsClosed,
                    IsVoid = transaction.IsVoid,
                    CreatedBy = transaction.CreatedBy,
                    ClosedBy = transaction.ClosedBy,
                    DateTransa = transaction.DateTransa
                };

                transaListVM.Add(transa);
            }


            int countBuy = 0, countSell = 0;
            decimal amountNetBuy = 0, amountNetSell = 0, amountNetDepositBuy = 0, amountNetDepositSell = 0, amountNetTransferBuy = 0, amountNetTransferSell = 0;
            decimal amountNetCostBuy = 0, amountNetCostSell = 0, amountNetRevenueBuy = 0, amountNetRevenueSell = 0;

            countBuy = transaListVM.Where(x => x.TypeNumeral == SD.QuotationType.Buy).Count();
            countSell = transaListVM.Where(x => x.TypeNumeral == SD.QuotationType.Sell).Count();
            amountNetBuy = transaListVM.Where(x => x.TypeNumeral == SD.QuotationType.Buy).Sum(x => x.AmountTransaction);
            amountNetCostBuy = transaListVM.Where(x => x.TypeNumeral == SD.QuotationType.Buy).Sum(x => x.AmountCostReal);
            amountNetDepositBuy = transaListVM.Where(x => x.TypeNumeral == SD.QuotationType.Buy).Sum(x => x.TotalDeposit);
            amountNetTransferBuy = transaListVM.Where(x => x.TypeNumeral == SD.QuotationType.Buy).Sum(x => x.TotalTransfer);
            amountNetRevenueBuy = transaListVM.Where(x => x.TypeNumeral == SD.QuotationType.Buy).Sum(x => x.AmountRevenueReal);
            amountNetSell = transaListVM.Where(x => x.TypeNumeral == SD.QuotationType.Sell).Sum(x => x.AmountTransaction);
            amountNetCostSell = transaListVM.Where(x => x.TypeNumeral == SD.QuotationType.Sell).Sum(x => x.AmountCostReal);
            amountNetDepositSell = transaListVM.Where(x => x.TypeNumeral == SD.QuotationType.Sell).Sum(x => x.TotalDeposit);
            amountNetTransferSell = transaListVM.Where(x => x.TypeNumeral == SD.QuotationType.Sell).Sum(x => x.TotalTransfer);
            amountNetRevenueSell = transaListVM.Where(x => x.TypeNumeral == SD.QuotationType.Sell).Sum(x => x.AmountRevenueReal);

            // Cargar reporte
            reportResult.Load(StiNetCoreHelper.MapPath(this, filePath));
            // Configuración del reporte
            reportResult.Dictionary.Variables["parCountBuy"].ValueObject = countBuy;
            reportResult.Dictionary.Variables["parCountSell"].ValueObject = countSell;
            reportResult.Dictionary.Variables["parAmountNetBuy"].ValueObject = amountNetBuy;
            reportResult.Dictionary.Variables["parAmountNetCostBuy"].ValueObject = amountNetCostBuy;
            reportResult.Dictionary.Variables["parAmountNetDepositBuy"].ValueObject = amountNetDepositBuy;
            reportResult.Dictionary.Variables["parAmountNetTransferBuy"].ValueObject = amountNetTransferBuy;
            reportResult.Dictionary.Variables["parAmountNetRevenueBuy"].ValueObject = amountNetRevenueBuy;
            reportResult.Dictionary.Variables["parAmountNetSell"].ValueObject = amountNetSell;
            reportResult.Dictionary.Variables["parAmountNetCostSell"].ValueObject = amountNetCostSell;
            reportResult.Dictionary.Variables["parAmountNetDepositSell"].ValueObject = amountNetDepositSell;
            reportResult.Dictionary.Variables["parAmountNetTransferSell"].ValueObject = amountNetTransferSell;
            reportResult.Dictionary.Variables["parAmountNetRevenueSell"].ValueObject = amountNetRevenueSell;
            reportResult.Dictionary.Variables[AC.ParNameReport].ValueObject = SD.SystemInformationReportTypeName[(short)ReportTransaType.Operation];
            reportResult.Dictionary.Variables[AC.ParDecimalTransaction].ValueObject = _decimalTransa;
            reportResult.Dictionary.Variables[AC.ParDecimalExchangeRate].ValueObject = _decimalExchange;
            reportResult.Dictionary.Variables[AC.ParNameCompany].ValueObject = $"{company.Name}";
            reportResult.Dictionary.Variables[AC.ParFileImagePath].ValueObject = $"{company.ImageLogoUrl}";

            DateOnly minDate = transaListVM.Min(t => t.DateTransa);
            DateOnly maxDate = transaListVM.Max(t => t.DateTransa);

            reportResult.Dictionary.Variables[AC.ParFilterDescription].ValueObject = $"Fecha Inicial: {minDate} Fecha Final: {maxDate}";

            reportResult.RegBusinessObject(AC.DatRep, transaListVM);
            reportResult.Render();

            // Exportar reporte a Excel y guardar en memoria
            byte[] excelData;
            //StiExcel2007ExportSettings
            var excelSettings = new StiExcelExportSettings
            {
                // Exportar solo los datos, sin incluir encabezados de página o pies de página
                DataExportMode = StiDataExportMode.Data,

                //ExportDataOnly = true,
                // Exportar formatos de objetos activos
                ExportObjectFormatting = true,

                // No usar un encabezado y pie de página en todas las páginas (esto debería ser el comportamiento predeterminado)
                UseOnePageHeaderAndFooter = false,

                // No incluir saltos de página si no son necesarios
                ExportPageBreaks = false
            };


            using (var excelStream = new MemoryStream())
            {
                reportResult.ExportDocument(StiExportFormat.Excel, excelStream, excelSettings);
                excelData = excelStream.ToArray();
            }

            // Convertir los bytes a base64
            var exportReporteBase64 = Convert.ToBase64String(excelData);
            DateTime dateReport = DateTime.Now;

            // Devolver el base64 como parte de la respuesta JSON
            jsonResponse.IsSuccess = true;
            jsonResponse.Data = new
            {
                ContentFile = exportReporteBase64,
                ContentType = AC.ContentTypeExcel,
                Filename = $"ListadoDeOperaciones_{dateReport:yyyyMMdd}_{dateReport:HHmmss}.xlsx"
            };

            return Json(jsonResponse);

            //reportResult.Compile();
        }
        catch (Exception ex)
        {
            // Log error details for debugging
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");

            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = ex.Message;
            return Json(jsonResponse);
        }
    }

    #endregion
}
