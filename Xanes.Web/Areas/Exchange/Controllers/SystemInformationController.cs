using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System.Text;
using System.Text.Json;
using static Xanes.Utility.SD;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Utility;
using Xanes.Models.ViewModels;
using Xanes.Models;
using Xanes.Models.Shared;
using Microsoft.DotNet.MSIdentity.Shared;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Xanes.Web.Areas.Exchange.Controllers;

[Area("Exchange")]
public class SystemInformationController : Controller
{
    private readonly IUnitOfWork _uow;
    private readonly IConfiguration _configuration;
    private readonly int _companyId;
    private readonly int _decimalTransa;
    private readonly int _decimalExchange;
    private string _companyImageUrl;
    private string _companyName;
    private readonly IWebHostEnvironment _hostEnvironment;
    private Dictionary<ParametersReport, object?> _parametersReport;

    public SystemInformationController(IUnitOfWork uow, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
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

        _companyImageUrl = string.Empty;
        _companyName = string.Empty;
    }

    [HttpGet]
    public IActionResult Index()
    {
        // TITULO DE LA PAGINA
        ViewData[AC.Title] = "Sistema de Información";

        return View();
    }

    // TRANSACCIONES ====>
    // Operation List
    [HttpPost]
    public IActionResult PrintTransaOperationList(TransactionReportVM reportDataVm)
    {
        return PrintReport(reportDataVm, reportDataVm.ReportType);
    }

    // Deposit List
    [HttpPost]
    public IActionResult PrintTransaDepositList(TransactionReportVM reportDataVm)
    {
        return PrintReport(reportDataVm, reportDataVm.ReportType);
    }

    // Transfer List
    [HttpPost]
    public IActionResult PrintTransaTransferList(TransactionReportVM reportDataVm)
    {
        return PrintReport(reportDataVm, reportDataVm.ReportType);
    }

    // Funcion guardar datos del reporte (criterios) y establecer titulo de pestaña
    private IActionResult PrintReport<T>(T reportDataVm, SD.ReportTransaType reportTypeParam) where T : class
    {
        // Guardar datos en el contexto
        // Serializar el objeto FiltersReportDto a JSON
        var reportDtoJson = JsonSerializer.Serialize(reportDataVm);
        HttpContext.Session.SetString(AC.FilterReportData, reportDtoJson);
        HttpContext.Session.SetInt32(AC.ReportDataType, (short)reportTypeParam);

        // Titulo pestaña del reporte
        ViewData[AC.Title] = $"Rpt - {SD.SystemInformationReportTypeName[(short)reportTypeParam]}";

        return View("~/Views/Shared/IndexReport.cshtml");
    }


    //! RENDERIZACION DE REPORTE ===>
    #region ImplementacionReporte

    public async Task<IActionResult> GetReport()
    {
        try
        {
            StringBuilder errorsMessagesBuilder = new();
            Company? company = null;

            // Crear instancia de reporte
            var report = new StiReport();

            // Obtener tipo de reporte
            var reportTypeContext = HttpContext.Session.GetInt32(AC.ReportDataType);
            var reportType =
                Enum.TryParse(reportTypeContext.ToString(), out SD.ReportTransaType result)
                    ? result
                    : SD.ReportTransaType.Operation;

            // Veficar que hay datos del reporte guardados
            var reportDataVmJson = HttpContext.Session.GetString(AC.FilterReportData);
            if (reportDataVmJson is null)
            {
                return Content($"{AC.ReportDataErrorLoad}");
            }

            var reportDataVmTrans = JsonSerializer.Deserialize<TransactionReportVM>(reportDataVmJson);

            switch (reportType)
            {
                case SD.ReportTransaType.Operation:
                    report = await SetReportDataForOperation(reportDataVmTrans);
                    break;

                case SD.ReportTransaType.Deposit:
                    report = await SetReportDataForDeposit(reportDataVmTrans);
                    break;

                case SD.ReportTransaType.Transfer:
                    report = await SetReportDataForTransfer(reportDataVmTrans);
                    break;
            }


            // Obtener datos de compañia
            company = _uow.Company.Get(filter: x => x.Id == _companyId);
            if (company is null)
            {
                return Content("Compañia no encontrada");
            }

            _companyName = company.CommercialName;
            _companyImageUrl = company.ImageLogoUrl ?? string.Empty;

            report.Dictionary.Variables[AC.ParFileImagePath].ValueObject = _companyImageUrl;
            report.Dictionary.Variables[AC.ParNameCompany].ValueObject = _companyName;
            // Decimales
            report.Dictionary.Variables[AC.ParDecimalExchangeRate].ValueObject = _decimalExchange;
            report.Dictionary.Variables[AC.ParDecimalTransaction].ValueObject = _decimalTransa;
            report.ReportName = SD.SystemInformationReportTypeName[(short)reportType];
            report.RegBusinessObject("datrep", _parametersReport[ParametersReport.ListData]);
            return StiNetCoreViewer.GetReportResult(this, report);
        }
        catch (Exception ex)
        {
            //TempData[AC.Error] = ex.Message;
            return Content($"{AC.ReportDataErrorLoad}: {ex.Message}");
        }
    }

    public IActionResult ViewerEvent()
    {
        return StiNetCoreViewer.ViewerEventResult(this);
    }

    #endregion ImplementacionReporte


    // FUNCIONES PARA CONFIGURAR LOS REPORTES A UTILIZAR =====>
    private async Task<StiReport> SetReportDataForOperation(TransactionReportVM modelVm)
    {
        StiReport reportResult = new();

        _parametersReport.Add(ParametersReport.FileName, SD.SystemInformationReportTypeFileName[(short)modelVm.ReportType]);
        _parametersReport.Add(ParametersReport.FilePath,
            $"{Path.Combine(_hostEnvironment.ContentRootPath, "Areas", "Exchange", "Reports", $"{SD.SystemInformationReportTypeFileName[(short)modelVm.ReportType]}")}");

        // Verificar que existe el archivo del reporte
        if (!System.IO.File.Exists(_parametersReport[ParametersReport.FilePath]?.ToString() ?? string.Empty))
        {
            throw new Exception($"{AC.ReportFileNotFound}");
        }

        if (Path.GetExtension(_parametersReport[ParametersReport.FileName]?.ToString() ?? string.Empty).ToUpper() != ".MRT")
        {
            throw new Exception($"{AC.ReportFileInvalid}");
        }

        reportResult.Load(StiNetCoreHelper.MapPath(this, _parametersReport[ParametersReport.FilePath]?.ToString() ?? string.Empty));

        // Obtener datos
        // Veficar que hay datos del reporte guardados
        var reportDataListJson = HttpContext.Session.GetString(AC.ReportListData);
        if (reportDataListJson is null)
        {
            throw new Exception($"{AC.ReportDataErrorLoad}");
        }
        var reportDataList = JsonSerializer.Deserialize<List<TransaODTVM>>(reportDataListJson);
        // Guardar los datos
        _parametersReport.Add(ParametersReport.ListData, reportDataList);

        // Setear parametros
        reportResult.Dictionary.Variables[AC.ParNameReport].ValueObject = SD.SystemInformationReportTypeName[(short)modelVm.ReportType];

        return reportResult;
    }

    private async Task<StiReport> SetReportDataForDeposit(TransactionReportVM modelVm)
    {
        StiReport reportResult = new();

        _parametersReport.Add(ParametersReport.FileName, SD.SystemInformationReportTypeFileName[(short)modelVm.ReportType]);
        _parametersReport.Add(ParametersReport.FilePath,
            $"{Path.Combine(_hostEnvironment.ContentRootPath, "Areas", "Exchange", "Reports", $"{SD.SystemInformationReportTypeFileName[(short)modelVm.ReportType]}")}");

        // Verificar que existe el archivo del reporte
        if (!System.IO.File.Exists(_parametersReport[ParametersReport.FilePath]?.ToString() ?? string.Empty))
        {
            throw new Exception($"{AC.ReportFileNotFound}");
        }

        if (Path.GetExtension(_parametersReport[ParametersReport.FileName]?.ToString() ?? string.Empty).ToUpper() != ".MRT")
        {
            throw new Exception($"{AC.ReportFileInvalid}");
        }

        reportResult.Load(StiNetCoreHelper.MapPath(this, _parametersReport[ParametersReport.FilePath]?.ToString() ?? string.Empty));

        // Obtener datos
        // Veficar que hay datos del reporte guardados
        var reportDataListJson = HttpContext.Session.GetString(AC.ReportListData);
        if (reportDataListJson is null)
        {
            throw new Exception($"{AC.ReportDataErrorLoad}");
        }
        var reportDataList = JsonSerializer.Deserialize<List<TransaODTVM>>(reportDataListJson);
        // Guardar los datos
        _parametersReport.Add(ParametersReport.ListData, reportDataList);

        // Setear parametros
        reportResult.Dictionary.Variables[AC.ParNameReport].ValueObject = SD.SystemInformationReportTypeName[(short)modelVm.ReportType];

        return reportResult;
    }

    private async Task<StiReport> SetReportDataForTransfer(TransactionReportVM modelVm)
    {
        StiReport reportResult = new();

        _parametersReport.Add(ParametersReport.FileName, SD.SystemInformationReportTypeFileName[(short)modelVm.ReportType]);
        _parametersReport.Add(ParametersReport.FilePath,
            $"{Path.Combine(_hostEnvironment.ContentRootPath, "Areas", "Exchange", "Reports", $"{SD.SystemInformationReportTypeFileName[(short)modelVm.ReportType]}")}");

        // Verificar que existe el archivo del reporte
        if (!System.IO.File.Exists(_parametersReport[ParametersReport.FilePath]?.ToString() ?? string.Empty))
        {
            throw new Exception($"{AC.ReportFileNotFound}");
        }

        if (Path.GetExtension(_parametersReport[ParametersReport.FileName]?.ToString() ?? string.Empty).ToUpper() != ".MRT")
        {
            throw new Exception($"{AC.ReportFileInvalid}");
        }

        reportResult.Load(StiNetCoreHelper.MapPath(this, _parametersReport[ParametersReport.FilePath]?.ToString() ?? string.Empty));

        // Obtener datos
        // Veficar que hay datos del reporte guardados
        var reportDataListJson = HttpContext.Session.GetString(AC.ReportListData);
        if (reportDataListJson is null)
        {
            throw new Exception($"{AC.ReportDataErrorLoad}");
        }
        var reportDataList = JsonSerializer.Deserialize<List<TransaODTVM>>(reportDataListJson);
        // Guardar los datos
        _parametersReport.Add(ParametersReport.ListData, reportDataList);

        // Setear parametros
        reportResult.Dictionary.Variables[AC.ParNameReport].ValueObject = SD.SystemInformationReportTypeName[(short)modelVm.ReportType];

        return reportResult;
    }


    // Obtener las vistas parciales para renderizarlas
    [HttpPost]
    public async Task<IActionResult> GetPartialView(SD.ReportTransaType reportType)
    {
        JsonResultResponse? resultResponse;
        var partialViewName = SD.SystemInformationReportPartialViewName[(short)reportType];
        var transaVM = new TransactionReportVM();
        switch (reportType)
        {
            case SD.ReportTransaType.Operation:
                resultResponse = await SetDataPartialViewOperation(reportType);
                if (!resultResponse.IsSuccess || (resultResponse.Data == null))
                {
                    return BadRequest(resultResponse.ErrorMessages);
                }
                transaVM = resultResponse.Data as TransactionReportVM;

                return PartialView(partialViewName, transaVM);

            case SD.ReportTransaType.Deposit:
                resultResponse = await SetDataPartialViewDeposit(reportType);
                if (!resultResponse.IsSuccess || (resultResponse.Data == null))
                {
                    return BadRequest(resultResponse.ErrorMessages);
                }
                transaVM = resultResponse.Data as TransactionReportVM;

                return PartialView(partialViewName, transaVM);

            case SD.ReportTransaType.Transfer:
                resultResponse = await SetDataPartialViewTransfer(reportType);
                if (!resultResponse.IsSuccess || (resultResponse.Data == null))
                {
                    return BadRequest(resultResponse.ErrorMessages);
                }
                transaVM = resultResponse.Data as TransactionReportVM;

                return PartialView(partialViewName, transaVM);
        }

        return PartialView(partialViewName);
    }

    //Configurar modelo para cada vista parcial (si lo requiere)
    // Reporte => Listado de Operaciones
    private async Task<JsonResultResponse> SetDataPartialViewOperation(SD.ReportTransaType typeReport)
    {
        JsonResultResponse? resultResponse = new() { IsSuccess = true };

        try
        {
            var modelVm = new TransactionReportVM()
            {
                ReportType = typeReport,
                DateTransaInitial = DateOnly.FromDateTime(DateTime.UtcNow),
                DateTransaFinal = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            resultResponse.Data = modelVm;
            return resultResponse;
        }
        catch (Exception e)
        {
            resultResponse.IsSuccess = false;
            resultResponse.ErrorMessages = e.Message;
            return resultResponse;
        }
    }

    // Reporte => Listado de Depositos
    private async Task<JsonResultResponse> SetDataPartialViewDeposit(SD.ReportTransaType typeReport)
    {
        JsonResultResponse? resultResponse = new() { IsSuccess = true };

        try
        {
            var modelVm = new TransactionReportVM()
            {
                ReportType = typeReport,
                DateTransaInitial = DateOnly.FromDateTime(DateTime.UtcNow),
                DateTransaFinal = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            resultResponse.Data = modelVm;
            return resultResponse;
        }
        catch (Exception e)
        {
            resultResponse.IsSuccess = false;
            resultResponse.ErrorMessages = e.Message;
            return resultResponse;
        }
    }

    // Reporte => Listado de Depositos
    private async Task<JsonResultResponse> SetDataPartialViewTransfer(SD.ReportTransaType typeReport)
    {
        JsonResultResponse? resultResponse = new() { IsSuccess = true };

        try
        {
            var modelVm = new TransactionReportVM()
            {
                ReportType = typeReport,
                DateTransaInitial = DateOnly.FromDateTime(DateTime.UtcNow),
                DateTransaFinal = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            resultResponse.Data = modelVm;
            return resultResponse;
        }
        catch (Exception e)
        {
            resultResponse.IsSuccess = false;
            resultResponse.ErrorMessages = e.Message;
            return resultResponse;
        }
    }

    // VALIDAR QUE EXISTAN DATOS PARA EL REPORTE Y GUARDARLOS ======>
    [HttpPost]
    public async Task<JsonResult> VerificationDataForOperation([FromBody] TransactionReportVM reportData)
    {
        JsonResultResponse? jsonResponse = new();
        try
        {
            // Obtener la cotización
            var transactionList = _uow.Quotation.GetAll(filter: x => x.CompanyId == _companyId,
                includeProperties: "TypeTrx,CustomerTrx,CurrencyDepositTrx,CurrencyTransferTrx,CurrencyTransaTrx").ToList();

            if (transactionList is null || transactionList.Count() == 0)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Cotización invalida";
                return Json(jsonResponse);
            }

            List<TransaODTVM> transaListVM = new();

            foreach (var transaction in transactionList)
            {
                var transa = new TransaODTVM
                {
                    Id = transaction.Id,
                    CompanyId = transaction.CompanyId,
                    NumberTransa = $"{Enum.GetName(typeof(SD.QuotationTypeNameAbrv), (int)transaction.TypeNumeral)}-{transaction.Numeral}",
                    CustomerFullName = transaction.CustomerTrx.CommercialName,
                    ExchangeRateTransa = transaction.TypeNumeral == SD.QuotationType.Buy ? transaction.ExchangeRateBuyTransa : transaction.ExchangeRateSellTransa,
                    ExchangeRateOfficialTransa = transaction.ExchangeRateOfficialTransa,
                    AmountTransaction = transaction.AmountTransaction,
                    AmountRevenue = transaction.AmountRevenue,
                    AmountCost = transaction.AmountCost,
                    TotalDeposit = transaction.TotalDeposit,
                    TotalTransfer = transaction.TotalTransfer,
                    IsClosed = transaction.IsClosed,
                    CreatedBy = transaction.CreatedBy,
                    ClosedBy = transaction.ClosedBy,
                    DateTransa = transaction.DateTransa
                };

                transaListVM.Add(transa);
            }

            // Guardar los datos en el contexto
            var reportListData = JsonSerializer.Serialize(transaListVM);
            HttpContext.Session.SetString(AC.ReportListData, reportListData);
            jsonResponse.IsSuccess = true;
            return Json(jsonResponse);
        }
        catch (Exception e)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = $"{e.Message}";
            return Json(jsonResponse);
        }
    }

    [HttpPost]
    public async Task<JsonResult> VerificationDataForDeposit([FromBody] TransactionReportVM reportData)
    {
        JsonResultResponse? jsonResponse = new();
        try
        {
            // Obtener la cotización
            var transactionDetailList = _uow.QuotationDetail.GetAll(filter: x => x.CompanyId == _companyId && x.QuotationDetailType == QuotationDetailType.Deposit,
                includeProperties: "ParentTrx,CurrencyDetailTrx,BankSourceTrx,BankTargetTrx").ToList();

            if (transactionDetailList is null || transactionDetailList.Count() == 0)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Detalle de cotización invalida";
                return Json(jsonResponse);
            }

            var customerIds = transactionDetailList.Select(x => x.ParentTrx.CustomerId).Distinct().ToList();

            var customerList = _uow.Customer.GetAll(filter: x => x.CompanyId == _companyId && customerIds.Contains(x.Id)).ToList();

            if (customerList is null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Cliente invalido";
                return Json(jsonResponse);
            }

            List<TransaODTVM> transaListVM = new();
            foreach (var transaction in transactionDetailList)
            {
                decimal amountMB = 0, amountMF = 0, amountMA = 0;
                var customer = customerList.FirstOrDefault(c => c.Id == transaction.ParentTrx.CustomerId);

                switch (transaction.ParentTrx.CurrencyTransferType)
                {
                    case CurrencyType.Base:
                        amountMB = transaction.AmountDetail;
                        break;
                    case CurrencyType.Foreign:
                        amountMF = transaction.AmountDetail;
                        break;
                    case CurrencyType.Additional:
                        amountMA = transaction.AmountDetail;
                        break;
                }
                var transa = new TransaODTVM
                {
                    Id = transaction.ParentId,
                    CompanyId = transaction.CompanyId,
                    NumberTransa = $"{Enum.GetName(typeof(SD.QuotationTypeNameAbrv), (int)transaction.ParentTrx.TypeNumeral)}-{transaction.ParentTrx.Numeral}",
                    CustomerFullName = customer != null ? customer.CommercialName : "",
                    ExchangeRateTransa = transaction.ParentTrx.TypeNumeral == SD.QuotationType.Buy ? transaction.ParentTrx.ExchangeRateBuyTransa : transaction.ParentTrx.ExchangeRateSellTransa,
                    ExchangeRateOfficialTransa = transaction.ParentTrx.ExchangeRateOfficialTransa,
                    AmountTransactionBase = amountMB,
                    AmountTransactionForeign = amountMF,
                    AmountTransactionAdditional = amountMA,
                    IsClosed = transaction.ParentTrx.IsClosed,
                    DateTransa = transaction.ParentTrx.DateTransa,
                    BankSourceCode = transaction.BankSourceTrx.Code,
                    IdDetail = transaction.Id
                };

                transaListVM.Add(transa);
            }

            // Guardar los datos en el contexto
            var reportListData = JsonSerializer.Serialize(transaListVM);
            HttpContext.Session.SetString(AC.ReportListData, reportListData);
            jsonResponse.IsSuccess = true;
            return Json(jsonResponse);
        }
        catch (Exception e)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = $"{e.Message}";
            return Json(jsonResponse);
        }
    }

    [HttpPost]
    public async Task<JsonResult> VerificationDataForTransfer([FromBody] TransactionReportVM reportData)
    {
        JsonResultResponse? jsonResponse = new();
        try
        {
            // Obtener la cotización
            var transactionDetailList = _uow.QuotationDetail.GetAll(filter: x => x.CompanyId == _companyId && x.QuotationDetailType == QuotationDetailType.Transfer,
                includeProperties: "ParentTrx,CurrencyDetailTrx,BankSourceTrx,BankTargetTrx").ToList();

            if (transactionDetailList is null || transactionDetailList.Count() == 0)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Detalle de cotización invalida";
                return Json(jsonResponse);
            }

            var customerIds = transactionDetailList.Select(x => x.ParentTrx.CustomerId).Distinct().ToList();

            var customerList = _uow.Customer.GetAll(filter: x => x.CompanyId == _companyId && customerIds.Contains(x.Id)).ToList();

            if (customerList is null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Cliente invalido";
                return Json(jsonResponse);
            }

            List<TransaODTVM> transaListVM = new();

            foreach (var transaction in transactionDetailList)
            {
                decimal amountMB = 0, amountMF = 0, amountMA = 0;

                var customer = customerList.FirstOrDefault(c => c.Id == transaction.ParentTrx.CustomerId);

                switch (transaction.ParentTrx.CurrencyTransferType)
                {
                    case CurrencyType.Base:
                        amountMB = transaction.AmountDetail;
                        break;
                    case CurrencyType.Foreign:
                        amountMF = transaction.AmountDetail;
                        break;
                    case CurrencyType.Additional:
                        amountMA = transaction.AmountDetail;
                        break;
                }

                var transa = new TransaODTVM
                {
                    Id = transaction.ParentId,
                    CompanyId = transaction.CompanyId,
                    NumberTransa = $"{Enum.GetName(typeof(SD.QuotationTypeNameAbrv), (int)transaction.ParentTrx.TypeNumeral)}-{transaction.ParentTrx.Numeral}",
                    CustomerFullName = customer != null ? customer.CommercialName : "",
                    ExchangeRateTransa = transaction.ParentTrx.TypeNumeral == SD.QuotationType.Buy ? transaction.ParentTrx.ExchangeRateBuyTransa : transaction.ParentTrx.ExchangeRateSellTransa,
                    ExchangeRateOfficialTransa = transaction.ParentTrx.ExchangeRateOfficialTransa,
                    AmountTransactionBase = amountMB,
                    AmountTransactionForeign = amountMF,
                    AmountTransactionAdditional = amountMA,
                    IsClosed = transaction.ParentTrx.IsClosed,
                    DateTransa = transaction.ParentTrx.DateTransa,
                    BankSourceCode = transaction.BankSourceTrx.Code,
                    BankTargetCode = transaction.BankTargetTrx.Code,
                    IdDetail = transaction.Id
                };

                transaListVM.Add(transa);
            }

            // Guardar los datos en el contexto
            var reportListData = JsonSerializer.Serialize(transaListVM);
            HttpContext.Session.SetString(AC.ReportListData, reportListData);
            jsonResponse.IsSuccess = true;
            return Json(jsonResponse);
        }
        catch (Exception e)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = $"{e.Message}";
            return Json(jsonResponse);
        }
    }
}

