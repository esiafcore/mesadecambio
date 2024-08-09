using Microsoft.AspNetCore.Mvc;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System.Text;
using Newtonsoft.Json;
using static Xanes.Utility.SD;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Utility;
using Xanes.Models.ViewModels;
using Xanes.Models;
using Xanes.Models.Shared;

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

        var path = Path.Combine(hostEnvironment.ContentRootPath, "License\\license.key");
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
        try
        {
            string? processingDateString = HttpContext.Session.GetString(AC.ProcessingDate)
                ?? DateOnly.FromDateTime(DateTime.Now).ToString();
            DateOnly dateFilter = DateOnly.Parse(processingDateString);
            ViewBag.ProcessingDate = JsonConvert.SerializeObject(dateFilter.ToString(AC.DefaultDateFormatWeb));

            // TITULO DE LA PAGINA
            ViewData[AC.Title] = "Sistema de Información";

            return View();
        }
        catch (Exception ex)
        {
            TempData[AC.Success] = ex.Message;
            return RedirectToAction("Index", "Home", new { area = "Exchange" });
        }
    }

    // TRANSACCIONES ====>
    // Transport List
    [HttpPost]
    public IActionResult PrintTransaTransportList(TransactionReportVM reportDataVm)
    {
        return PrintReport(reportDataVm, reportDataVm.ReportType);
    }

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
        var reportDtoJson = JsonConvert.SerializeObject(reportDataVm);
        HttpContext.Session.SetString(AC.FilterReportData, reportDtoJson);
        HttpContext.Session.SetInt32(AC.ReportDataType, (short)reportTypeParam);

        // Titulo pestaña del reporte
        ViewData[AC.Title] = $"Rpt - {SD.SystemInformationReportTypeName[(short)reportTypeParam]}";

        return View("~/Views/Shared/IndexReport.cshtml");
    }

    //! RENDERIZACION DE REPORTE ===>
    #region ImplementacionReporte

    public IActionResult GetReport()
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

            var reportDataVmTrans = JsonConvert.DeserializeObject<TransactionReportVM>(reportDataVmJson);

            switch (reportType)
            {
                case SD.ReportTransaType.Operation:
                    report = SetReportDataForOperation(reportDataVmTrans);
                    break;

                case SD.ReportTransaType.Deposit:
                    report = SetReportDataForDeposit(reportDataVmTrans);
                    break;

                case SD.ReportTransaType.Transfer:
                    report = SetReportDataForTransfer(reportDataVmTrans);
                    break;

                case SD.ReportTransaType.Transport:
                    report = SetReportDataForTransport(reportDataVmTrans);
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

            var DateTransaInitial = HttpContext.Session.GetString("DateTransaInitial");
            var DateTransaFinal = HttpContext.Session.GetString("DateTransaFinal");

            report.Dictionary.Variables[AC.ParFilterDescription].ValueObject = $"Fecha Inicial: {DateTransaInitial} Fecha Final: {DateTransaFinal}";

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
    private StiReport SetReportDataForTransport(TransactionReportVM modelVm)
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
        var reportDataList = JsonConvert.DeserializeObject<List<TransaODTVM>>(reportDataListJson);
        // Guardar los datos
        _parametersReport.Add(ParametersReport.ListData, reportDataList);

        //int countBuy = 0, countSell = 0;
        //decimal amountNetBuy = 0, amountNetSell = 0, amountNetDepositBuy = 0, amountNetDepositSell = 0, amountNetTransferBuy = 0, amountNetTransferSell = 0;
        //decimal amountNetCostBuy = 0, amountNetCostSell = 0, amountNetRevenueBuy = 0, amountNetRevenueSell = 0;

        //countBuy = reportDataList.Where(x => x.TypeNumeral == SD.QuotationType.Buy).Count();
        //countSell = reportDataList.Where(x => x.TypeNumeral == SD.QuotationType.Sell).Count();
        //amountNetBuy = reportDataList.Where(x => x.TypeNumeral == SD.QuotationType.Buy).Sum(x => x.AmountTransaction);
        //amountNetCostBuy = reportDataList.Where(x => x.TypeNumeral == SD.QuotationType.Buy).Sum(x => x.AmountCost);
        //amountNetDepositBuy = reportDataList.Where(x => x.TypeNumeral == SD.QuotationType.Buy).Sum(x => x.TotalDeposit);
        //amountNetTransferBuy = reportDataList.Where(x => x.TypeNumeral == SD.QuotationType.Buy).Sum(x => x.TotalTransfer);
        //amountNetRevenueBuy = reportDataList.Where(x => x.TypeNumeral == SD.QuotationType.Buy).Sum(x => x.AmountRevenue);
        //amountNetSell = reportDataList.Where(x => x.TypeNumeral == SD.QuotationType.Sell).Sum(x => x.AmountTransaction);
        //amountNetCostSell = reportDataList.Where(x => x.TypeNumeral == SD.QuotationType.Sell).Sum(x => x.AmountCost);
        //amountNetDepositSell = reportDataList.Where(x => x.TypeNumeral == SD.QuotationType.Sell).Sum(x => x.TotalDeposit);
        //amountNetTransferSell = reportDataList.Where(x => x.TypeNumeral == SD.QuotationType.Sell).Sum(x => x.TotalTransfer);
        //amountNetRevenueSell = reportDataList.Where(x => x.TypeNumeral == SD.QuotationType.Sell).Sum(x => x.AmountRevenue);

        // Setear parametros
        //reportResult.Dictionary.Variables["parCountBuy"].ValueObject = countBuy;
        //reportResult.Dictionary.Variables["parCountSell"].ValueObject = countSell;
        //reportResult.Dictionary.Variables["parAmountNetBuy"].ValueObject = amountNetBuy;
        //reportResult.Dictionary.Variables["parAmountNetCostBuy"].ValueObject = amountNetCostBuy;
        //reportResult.Dictionary.Variables["parAmountNetDepositBuy"].ValueObject = amountNetDepositBuy;
        //reportResult.Dictionary.Variables["parAmountNetTransferBuy"].ValueObject = amountNetTransferBuy;
        //reportResult.Dictionary.Variables["parAmountNetRevenueBuy"].ValueObject = amountNetRevenueBuy;
        //reportResult.Dictionary.Variables["parAmountNetSell"].ValueObject = amountNetSell;
        //reportResult.Dictionary.Variables["parAmountNetCostSell"].ValueObject = amountNetCostSell;
        //reportResult.Dictionary.Variables["parAmountNetDepositSell"].ValueObject = amountNetDepositSell;
        //reportResult.Dictionary.Variables["parAmountNetTransferSell"].ValueObject = amountNetTransferSell;
        //reportResult.Dictionary.Variables["parAmountNetRevenueSell"].ValueObject = amountNetRevenueSell;

        reportResult.Dictionary.Variables[AC.ParNameReport].ValueObject = SD.SystemInformationReportTypeName[(short)modelVm.ReportType];

        return reportResult;
    }

    private StiReport SetReportDataForOperation(TransactionReportVM modelVm)
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
        var reportDataList = JsonConvert.DeserializeObject<List<TransaODTVM>>(reportDataListJson);
        // Guardar los datos
        _parametersReport.Add(ParametersReport.ListData, reportDataList);

        int countBuy = 0, countSell = 0;
        decimal amountNetBuy = 0, amountNetSell = 0, amountNetDepositBuy = 0, amountNetDepositSell = 0, amountNetTransferBuy = 0, amountNetTransferSell = 0;
        decimal amountNetCostBuy = 0, amountNetCostSell = 0, amountNetRevenueBuy = 0, amountNetRevenueSell = 0;

        countBuy = reportDataList.Where(x => x.TypeNumeral == SD.QuotationType.Buy).Count();
        countSell = reportDataList.Where(x => x.TypeNumeral == SD.QuotationType.Sell).Count();
        amountNetBuy = reportDataList.Where(x => x.TypeNumeral == SD.QuotationType.Buy).Sum(x => x.AmountTransaction);
        amountNetCostBuy = reportDataList.Where(x => x.TypeNumeral == SD.QuotationType.Buy).Sum(x => x.AmountCost);
        amountNetDepositBuy = reportDataList.Where(x => x.TypeNumeral == SD.QuotationType.Buy).Sum(x => x.TotalDeposit);
        amountNetTransferBuy = reportDataList.Where(x => x.TypeNumeral == SD.QuotationType.Buy).Sum(x => x.TotalTransfer);
        amountNetRevenueBuy = reportDataList.Where(x => x.TypeNumeral == SD.QuotationType.Buy).Sum(x => x.AmountRevenue);
        amountNetSell = reportDataList.Where(x => x.TypeNumeral == SD.QuotationType.Sell).Sum(x => x.AmountTransaction);
        amountNetCostSell = reportDataList.Where(x => x.TypeNumeral == SD.QuotationType.Sell).Sum(x => x.AmountCost);
        amountNetDepositSell = reportDataList.Where(x => x.TypeNumeral == SD.QuotationType.Sell).Sum(x => x.TotalDeposit);
        amountNetTransferSell = reportDataList.Where(x => x.TypeNumeral == SD.QuotationType.Sell).Sum(x => x.TotalTransfer);
        amountNetRevenueSell = reportDataList.Where(x => x.TypeNumeral == SD.QuotationType.Sell).Sum(x => x.AmountRevenue);


        // Setear parametros
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
        reportResult.Dictionary.Variables[AC.ParNameReport].ValueObject = SD.SystemInformationReportTypeName[(short)modelVm.ReportType];

        return reportResult;
    }

    private StiReport SetReportDataForDeposit(TransactionReportVM modelVm)
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
        var reportDataList = JsonConvert.DeserializeObject<List<TransaODTVM>>(reportDataListJson);
        // Guardar los datos
        _parametersReport.Add(ParametersReport.ListData, reportDataList);

        int countClosed = 0, countOpen = 0;
        decimal amountNetMBC = 0, amountNetMFC = 0, amountNetMAC = 0, amountNetMBO = 0, amountNetMFO = 0, amountNetMAO = 0;
        countClosed = reportDataList.Where(x => x.IsClosed).Count();
        countOpen = reportDataList.Where(x => x.IsClosed == false).Count();
        amountNetMBC = reportDataList.Where(x => x.IsClosed).Sum(x => x.AmountTransactionBase);
        amountNetMFC = reportDataList.Where(x => x.IsClosed).Sum(x => x.AmountTransactionForeign);
        amountNetMAC = reportDataList.Where(x => x.IsClosed).Sum(x => x.AmountTransactionAdditional);
        amountNetMBO = reportDataList.Where(x => x.IsClosed == false).Sum(x => x.AmountTransactionBase);
        amountNetMFO = reportDataList.Where(x => x.IsClosed == false).Sum(x => x.AmountTransactionForeign);
        amountNetMAO = reportDataList.Where(x => x.IsClosed == false).Sum(x => x.AmountTransactionAdditional);


        // Setear parametros
        reportResult.Dictionary.Variables["parCountClosed"].ValueObject = countClosed;
        reportResult.Dictionary.Variables["parCountOpen"].ValueObject = countOpen;
        reportResult.Dictionary.Variables["parAmountNetMBC"].ValueObject = amountNetMBC;
        reportResult.Dictionary.Variables["parAmountNetMFC"].ValueObject = amountNetMFC;
        reportResult.Dictionary.Variables["parAmountNetMAC"].ValueObject = amountNetMAC;
        reportResult.Dictionary.Variables["parAmountNetMBO"].ValueObject = amountNetMBO;
        reportResult.Dictionary.Variables["parAmountNetMFO"].ValueObject = amountNetMFO;
        reportResult.Dictionary.Variables["parAmountNetMAO"].ValueObject = amountNetMAO;

        reportResult.Dictionary.Variables[AC.ParNameReport].ValueObject = SD.SystemInformationReportTypeName[(short)modelVm.ReportType];

        return reportResult;
    }

    private StiReport SetReportDataForTransfer(TransactionReportVM modelVm)
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
        var reportDataList = JsonConvert.DeserializeObject<List<TransaODTVM>>(reportDataListJson);
        // Guardar los datos
        _parametersReport.Add(ParametersReport.ListData, reportDataList);

        // Setear parametros
        int countClosed = 0, countOpen = 0;
        decimal amountNetMBC = 0, amountNetMFC = 0, amountNetMAC = 0, amountNetMBO = 0, amountNetMFO = 0, amountNetMAO = 0;
        countClosed = reportDataList.Where(x => x.IsClosed).Count();
        countOpen = reportDataList.Where(x => x.IsClosed == false).Count();
        amountNetMBC = reportDataList.Where(x => x.IsClosed).Sum(x => x.AmountTransactionBase);
        amountNetMFC = reportDataList.Where(x => x.IsClosed).Sum(x => x.AmountTransactionForeign);
        amountNetMAC = reportDataList.Where(x => x.IsClosed).Sum(x => x.AmountTransactionAdditional);
        amountNetMBO = reportDataList.Where(x => x.IsClosed == false).Sum(x => x.AmountTransactionBase);
        amountNetMFO = reportDataList.Where(x => x.IsClosed == false).Sum(x => x.AmountTransactionForeign);
        amountNetMAO = reportDataList.Where(x => x.IsClosed == false).Sum(x => x.AmountTransactionAdditional);


        // Setear parametros
        reportResult.Dictionary.Variables["parCountClosed"].ValueObject = countClosed;
        reportResult.Dictionary.Variables["parCountOpen"].ValueObject = countOpen;
        reportResult.Dictionary.Variables["parAmountNetMBC"].ValueObject = amountNetMBC;
        reportResult.Dictionary.Variables["parAmountNetMFC"].ValueObject = amountNetMFC;
        reportResult.Dictionary.Variables["parAmountNetMAC"].ValueObject = amountNetMAC;
        reportResult.Dictionary.Variables["parAmountNetMBO"].ValueObject = amountNetMBO;
        reportResult.Dictionary.Variables["parAmountNetMFO"].ValueObject = amountNetMFO;
        reportResult.Dictionary.Variables["parAmountNetMAO"].ValueObject = amountNetMAO;
        reportResult.Dictionary.Variables[AC.ParNameReport].ValueObject = SD.SystemInformationReportTypeName[(short)modelVm.ReportType];

        return reportResult;
    }
    
    // Obtener las vistas parciales para renderizarlas
    [HttpPost]
    public IActionResult GetPartialView(SD.ReportTransaType reportType)
    {
        JsonResultResponse? resultResponse;
        var partialViewName = SD.SystemInformationReportPartialViewName[(short)reportType];
        var transaVM = new TransactionReportVM();
        switch (reportType)
        {
            case SD.ReportTransaType.Operation:
                resultResponse = SetDataPartialViewOperation(reportType);
                if (!resultResponse.IsSuccess || (resultResponse.Data == null))
                {
                    return BadRequest(resultResponse.ErrorMessages);
                }
                transaVM = resultResponse.Data as TransactionReportVM;

                return PartialView(partialViewName, transaVM);

            case SD.ReportTransaType.Deposit:
                resultResponse = SetDataPartialViewDeposit(reportType);
                if (!resultResponse.IsSuccess || (resultResponse.Data == null))
                {
                    return BadRequest(resultResponse.ErrorMessages);
                }
                transaVM = resultResponse.Data as TransactionReportVM;

                return PartialView(partialViewName, transaVM);

            case SD.ReportTransaType.Transfer:
                resultResponse = SetDataPartialViewTransfer(reportType);
                if (!resultResponse.IsSuccess || (resultResponse.Data == null))
                {
                    return BadRequest(resultResponse.ErrorMessages);
                }
                transaVM = resultResponse.Data as TransactionReportVM;

                return PartialView(partialViewName, transaVM);

            case SD.ReportTransaType.Transport:
                resultResponse = SetDataPartialViewTransport(reportType);
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
    private JsonResultResponse SetDataPartialViewTransport(SD.ReportTransaType typeReport)
    {
        JsonResultResponse? resultResponse = new() { IsSuccess = true };

        try
        {
            var modelVm = new TransactionReportVM()
            {
                ReportType = typeReport,
                DateTransaInitial = DateOnly.FromDateTime(DateTime.Now),
                DateTransaFinal = DateOnly.FromDateTime(DateTime.Now)
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

    // Reporte => Listado de Operaciones
    private JsonResultResponse SetDataPartialViewOperation(SD.ReportTransaType typeReport)
    {
        JsonResultResponse? resultResponse = new() { IsSuccess = true };

        try
        {
            var modelVm = new TransactionReportVM()
            {
                ReportType = typeReport,
                DateTransaInitial = DateOnly.FromDateTime(DateTime.Now),
                DateTransaFinal = DateOnly.FromDateTime(DateTime.Now)
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
    private JsonResultResponse SetDataPartialViewDeposit(SD.ReportTransaType typeReport)
    {
        JsonResultResponse? resultResponse = new() { IsSuccess = true };

        try
        {
            var modelVm = new TransactionReportVM()
            {
                ReportType = typeReport,
                DateTransaInitial = DateOnly.FromDateTime(DateTime.Now),
                DateTransaFinal = DateOnly.FromDateTime(DateTime.Now)
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
    private JsonResultResponse SetDataPartialViewTransfer(SD.ReportTransaType typeReport)
    {
        JsonResultResponse? resultResponse = new() { IsSuccess = true };

        try
        {
            var modelVm = new TransactionReportVM()
            {
                ReportType = typeReport,
                DateTransaInitial = DateOnly.FromDateTime(DateTime.Now),
                DateTransaFinal = DateOnly.FromDateTime(DateTime.Now)
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
    public JsonResult VerificationDataForTransport([FromBody] TransactionReportVM reportData)
    {
        JsonResultResponse? jsonResponse = new();
        StiReport reportResult = new();

        try
        {
            // Obtener la cotización
            var transactionList = _uow.Quotation
                .GetAll(filter: x => (x.CompanyId == _companyId)
                                         && (x.DateTransa >= reportData.DateTransaInitial)
                                         && (x.DateTransa <= reportData.DateTransaFinal)
                                         && (x.TypeNumeral == SD.QuotationType.Transfer),
                includeProperties: "TypeTrx,CustomerTrx,CurrencyDepositTrx,CurrencyTransferTrx,CurrencyTransaTrx,BankAccountSourceTrx,BankAccountTargetTrx")
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

            foreach (var transaction in transactionList)
            {
                var transa = new TransaODTVM
                {
                    Id = transaction.Id,
                    CompanyId = transaction.CompanyId,
                    TypeNumeral = transaction.TypeNumeral,
                    NumberTransa = $"{Enum.GetName(typeof(SD.QuotationTypeNameAbrv), (int)transaction.TypeNumeral)}-{transaction.Numeral.ToString().PadLeft(3, AC.CharDefaultEmpty)}",
                    CustomerFullName = transaction.CustomerTrx.BusinessName,
                    CurrencySourceTarget = transaction.CurrencyTransaTrx.Code,
                    ExchangeRateOfficialTransa = transaction.ExchangeRateOfficialTransa,
                    AmountTransaction = transaction.AmountTransactionRpt,
                    AmountCommission = transaction.AmountCommissionRpt,
                    BankAccountSourceName = transaction.BankAccountSourceTrx.Name,
                    BankAccountTargetName = transaction.BankAccountTargetTrx.Name,
                    ExecutiveCode = transaction.BusinessExecutiveCode,
                    IsClosed = transaction.IsClosed,
                    IsVoid = transaction.IsVoid,
                    CreatedBy = transaction.CreatedBy,
                    ClosedBy = transaction.ClosedBy,
                    DateTransa = transaction.DateTransa
                };

                transaListVM.Add(transa);
            }

            // Guardar los datos en el contexto
            var reportListData = JsonConvert.SerializeObject(transaListVM);
            HttpContext.Session.SetString(AC.ReportListData, reportListData);
            HttpContext.Session.SetString("DateTransaInitial", reportData.DateTransaInitial.ToString(AC.DefaultDateFormatView));
            HttpContext.Session.SetString("DateTransaFinal", reportData.DateTransaFinal.ToString(AC.DefaultDateFormatView));
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
    public JsonResult VerificationDataForOperation([FromBody] TransactionReportVM reportData)
    {
        JsonResultResponse? jsonResponse = new();
        StiReport reportResult = new();
        try
        {
            // Obtener la cotización
            var transactionList = _uow.Quotation.GetAll(filter: x => (x.CompanyId == _companyId)
                                                                     && (x.DateTransa >= reportData.DateTransaInitial)
                                                                     && (x.DateTransa <= reportData.DateTransaFinal)
                                                                        // No se incluyen TRA
                                                                        && (x.TypeNumeral != SD.QuotationType.Transfer),
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

            foreach (var transaction in transactionList)
            {
                var currency = "";

                if (transaction.TypeNumeral == SD.QuotationType.Buy ||
                    transaction.TypeNumeral == SD.QuotationType.Transfer)
                {
                    currency =
                        $"{transaction.CurrencyTransaTrx.Code}-{transaction.CurrencyTransferTrx.Code}";
                }
                else
                {
                    currency =
                        $"{transaction.CurrencyTransaTrx.Code}-{transaction.CurrencyDepositTrx.Code}";
                }


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

            // Guardar los datos en el contexto
            var reportListData = JsonConvert.SerializeObject(transaListVM);
            HttpContext.Session.SetString(AC.ReportListData, reportListData);
            HttpContext.Session.SetString("DateTransaInitial", reportData.DateTransaInitial.ToString(AC.DefaultDateFormatView));
            HttpContext.Session.SetString("DateTransaFinal", reportData.DateTransaFinal.ToString(AC.DefaultDateFormatView));
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
    public JsonResult VerificationDataForDeposit([FromBody] TransactionReportVM reportData)
    {
        JsonResultResponse? jsonResponse = new();
        try
        {
            // Obtener la cotización
            var transactionDetailList = _uow.QuotationDetail.GetAll(filter: x => x.CompanyId == _companyId && x.QuotationDetailType == QuotationDetailType.Deposit
                    && x.ParentTrx.DateTransa >= reportData.DateTransaInitial &&
                    x.ParentTrx.DateTransa <= reportData.DateTransaFinal,
                includeProperties: "ParentTrx,CurrencyDetailTrx,BankSourceTrx,BankTargetTrx").ToList();

            if (transactionDetailList is null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Detalle cotización invalida";
                return Json(jsonResponse);
            }

            if (transactionDetailList.Count() == 0)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.IsInfo = true;
                jsonResponse.ErrorMessages = $"No hay depositos";
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

                //switch (transaction.CurrencyDetailTrx.Numeral .ParentTrx.CurrencyTransferType)
                switch (transaction.CurrencyDetailTrx.Numeral)
                {
                    case (int)CurrencyType.Base:
                        amountMB = transaction.AmountDetail;
                        break;
                    case (int)CurrencyType.Foreign:
                        amountMF = transaction.AmountDetail;
                        break;
                    case (int)CurrencyType.Additional:
                        amountMA = transaction.AmountDetail;
                        break;
                }
                var transa = new TransaODTVM
                {
                    Id = transaction.ParentId,
                    QuotationDetailType = transaction.QuotationDetailType,
                    CompanyId = transaction.CompanyId,
                    NumberTransa = $"{Enum.GetName(typeof(SD.QuotationTypeNameAbrv), (int)transaction.ParentTrx.TypeNumeral)}-{transaction.ParentTrx.Numeral.ToString().PadLeft(3, AC.CharDefaultEmpty)}",
                    CustomerFullName = customer != null ? customer.CommercialName : "",
                    ExchangeRateTransa = transaction.ParentTrx.TypeNumeral == SD.QuotationType.Buy ? transaction.ParentTrx.ExchangeRateBuyTransa : transaction.ParentTrx.ExchangeRateSellTransa,
                    ExchangeRateOfficialTransa = transaction.ParentTrx.ExchangeRateOfficialTransa,
                    AmountTransactionBase = amountMB,
                    AmountTransactionForeign = amountMF,
                    AmountTransactionAdditional = amountMA,
                    IsClosed = transaction.ParentTrx.IsClosed,
                    ExecutiveCode = transaction.ParentTrx.BusinessExecutiveCode,
                    DateTransa = transaction.ParentTrx.DateTransa,
                    BankSourceCode = transaction.BankSourceTrx.Code,
                    IdDetail = transaction.Id
                };

                transaListVM.Add(transa);
            }

            // Guardar los datos en el contexto
            var reportListData = JsonConvert.SerializeObject(transaListVM);
            HttpContext.Session.SetString(AC.ReportListData, reportListData);
            HttpContext.Session.SetString("DateTransaInitial", reportData.DateTransaInitial.ToString(AC.DefaultDateFormatView));
            HttpContext.Session.SetString("DateTransaFinal", reportData.DateTransaFinal.ToString(AC.DefaultDateFormatView));

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
    public JsonResult VerificationDataForTransfer([FromBody] TransactionReportVM reportData)
    {
        JsonResultResponse? jsonResponse = new();
        try
        {
            // Obtener la cotización
            var transactionDetailList = _uow.QuotationDetail.GetAll(filter: x => x.CompanyId == _companyId && x.QuotationDetailType == QuotationDetailType.Transfer && x.ParentTrx.DateTransa >= reportData.DateTransaInitial &&
                    x.ParentTrx.DateTransa <= reportData.DateTransaFinal,
                includeProperties: "ParentTrx,CurrencyDetailTrx,BankSourceTrx,BankTargetTrx").ToList();

            if (transactionDetailList is null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Detalle cotización invalida";
                return Json(jsonResponse);
            }

            if (transactionDetailList.Count() == 0)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.IsInfo = true;
                jsonResponse.ErrorMessages = $"No hay transferencias";
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
                    NumberTransa = $"{Enum.GetName(typeof(SD.QuotationTypeNameAbrv), (int)transaction.ParentTrx.TypeNumeral)}-{transaction.ParentTrx.Numeral.ToString().PadLeft(3, AC.CharDefaultEmpty)}",
                    CustomerFullName = customer != null ? customer.CommercialName : "",
                    ExchangeRateTransa = transaction.ParentTrx.TypeNumeral == SD.QuotationType.Buy ? transaction.ParentTrx.ExchangeRateBuyTransa : transaction.ParentTrx.ExchangeRateSellTransa,
                    ExchangeRateOfficialTransa = transaction.ParentTrx.ExchangeRateOfficialTransa,
                    AmountTransactionBase = amountMB,
                    AmountTransactionForeign = amountMF,
                    AmountTransactionAdditional = amountMA,
                    IsClosed = transaction.ParentTrx.IsClosed,
                    ExecutiveCode = transaction.ParentTrx.BusinessExecutiveCode,
                    DateTransa = transaction.ParentTrx.DateTransa,
                    BankSourceCode = transaction.BankSourceTrx.Code,
                    BankTargetCode = transaction.BankTargetTrx.Code,
                    IdDetail = transaction.Id
                };

                transaListVM.Add(transa);
            }

            // Guardar los datos en el contexto
            var reportListData = JsonConvert.SerializeObject(transaListVM);
            HttpContext.Session.SetString(AC.ReportListData, reportListData);
            HttpContext.Session.SetString("DateTransaInitial", reportData.DateTransaInitial.ToString());
            HttpContext.Session.SetString("DateTransaFinal", reportData.DateTransaFinal.ToString());
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

