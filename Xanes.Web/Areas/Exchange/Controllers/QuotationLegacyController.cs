using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.DataAccess.ServicesApi.Interface;
using Xanes.LoggerService;
using Xanes.Models;
using Xanes.Models.Dtos;
using Xanes.Models.ViewModels;
using Xanes.Utility;
#pragma warning disable CS8604

namespace Xanes.Web.Areas.Exchange.Controllers;

[Area("Exchange")]
public class QuotationLegacyController : Controller
{
    private readonly IQuotationLegacyService _srv;
    private readonly IQuotationDetailLegacyService _srvDetail;
    private readonly ILoggerManager _logger;
    private readonly IConfiguration _cfg;
    private readonly IUnitOfWork _uow;
    private readonly int _companyId;
    private string _sessionToken;

    public QuotationLegacyController(IQuotationLegacyService service
        , ILoggerManager logger
        , IConfiguration cfg
        , IUnitOfWork uow
        , IQuotationDetailLegacyService srvDetail
        , IHttpContextAccessor httpCtxAcc)
    {
        _logger = logger;
        _srv = service;
        _cfg = cfg;
        _uow = uow;
        _srvDetail = srvDetail;
        _companyId = _cfg.GetValue<int>("ApplicationSettings:CompanyId");
        _sessionToken = httpCtxAcc.HttpContext.Session.GetString(SD.SessionToken) ?? string.Empty;

    }

    [HttpGet]
    public IActionResult Export()
    {
        // Titulo de la pagina
        ViewData[AC.Title] = $"Cotizaciones Legacy - Exportar";
        TransactionReportVM modelVm = new();
        modelVm.DateTransaInitial = DateOnly.FromDateTime(DateTime.Now);
        modelVm.DateTransaFinal = modelVm.DateTransaInitial;
        return View(modelVm);
    }


    [HttpGet]
    public async Task<IActionResult> ExportExcel(string dateInitial, string dateFinal, bool includeVoid = true)
    {
        DateOnly dateTransaInitial = DateOnly.Parse(dateInitial);
        DateOnly dateTransaFinal = DateOnly.Parse(dateFinal);
        var objQuotationList = new List<Quotation>();
        var apiResponse = string.Empty;

        apiResponse = await _srv.GetAllLegacyAsync(_sessionToken, 0, 1, dateTransaInitial, dateTransaFinal, "");

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var objLegacyList = JsonSerializer.Deserialize<List<QuotationLegacyDto>>(apiResponse, options)!;

        if (objLegacyList == null || objLegacyList.Count == 0)
        {
            return NoContent();
        }

        var objTypeList = _uow.QuotationType.GetAll(filter: x =>
          x.CompanyId == _companyId).ToList();

        if (objTypeList == null || objTypeList.Count == 0)
        {
            TempData[AC.Error] = "Tipos de cotizaciones no encontradas";
            return BadRequest();
        }

        var objCustomerList = _uow.Customer.GetAll(filter: x =>
          x.CompanyId == _companyId).ToList();

        if (objCustomerList == null || objCustomerList.Count == 0)
        {
            TempData[AC.Error] = "Clientes no encontrados";
            return BadRequest();
        }

        var objBankAccountList = _uow.BankAccount.GetAll(filter: x =>
            x.CompanyId == _companyId).ToList();

        if (objBankAccountList == null || objBankAccountList.Count == 0)
        {
            TempData[AC.Error] = "Cuentas bancarias no encontradas";
            return BadRequest();
        }

        var objBusinessList = _uow.BusinessExecutive.GetAll(filter: x =>
            x.CompanyId == _companyId).ToList();

        if (objBusinessList == null || objBusinessList.Count == 0)
        {
            TempData[AC.Error] = "Ejecutivos no encontrados";
            return BadRequest();
        }

        var objCurrencyList = _uow.Currency.GetAll(filter: x =>
            x.CompanyId == _companyId).ToList();

        if (objCurrencyList == null || objCurrencyList.Count == 0)
        {
            TempData[AC.Error] = "Monedas no encontradas";
            return BadRequest();
        }

        var objBankList = _uow.Bank.GetAll(filter: x =>
            x.CompanyId == _companyId).ToList();

        if (objBankList == null || objBankList.Count == 0)
        {
            TempData[AC.Error] = "Bancos no encontrados";
            return BadRequest();
        }

        foreach (var quotationLegacy in objLegacyList)
        {
            var quotation = new Quotation();
            quotation.CompanyId = _companyId;
            quotation.Id = quotationLegacy.Id;
            var objType = objTypeList.FirstOrDefault(x => x.Code == quotationLegacy.TypeCode);
            if (objType == null)
            {
                TempData[AC.Error] = $"Tipo de cotización: {quotationLegacy.TypeCode} no fue encontrado";
                return BadRequest();
            }

            quotation.TypeId = objType.Id;
            quotation.TypeNumeral = (SD.QuotationType)objType.Numeral;
            quotation.TypeTrx = objType;
            quotation.Numeral = quotationLegacy.NumeralTrx;
            var date = quotationLegacy.DateTrx.Split("T")[0];
            quotation.DateTransa = DateOnly.Parse(date);

            var objCustomer = objCustomerList.FirstOrDefault(x => x.Code == quotationLegacy.CustomerCode);
            if (objCustomer == null)
            {
                TempData[AC.Error] = $"El cliente con el código: {quotationLegacy.CustomerCode} no fue encontrado";
                return BadRequest();
            }

            quotation.CustomerId = objCustomer.Id;
            quotation.CustomerTrx = objCustomer;

            var objCurrencyTransa = objCurrencyList.FirstOrDefault(x => x.Numeral == quotationLegacy.CurrencyTransaction);
            if (objCurrencyTransa == null)
            {
                TempData[AC.Error] = $"La moneda de la transacción: {quotationLegacy.CurrencyTransaction} no fue encontrado";
                return BadRequest();
            }

            quotation.CurrencyTransaId = objCurrencyTransa.Id;
            quotation.CurrencyTransaType = (SD.CurrencyType)objCurrencyTransa.Numeral;
            quotation.CurrencyTransaTrx = objCurrencyTransa;

            var objCurrencyDeposit = objCurrencyList.FirstOrDefault(x => x.Numeral == quotationLegacy.CurrencyDeposit);
            if (objCurrencyDeposit == null)
            {
                TempData[AC.Error] = $"La moneda del deposito: {quotationLegacy.CurrencyDeposit} no fue encontrado";
                return BadRequest();
            }

            quotation.CurrencyDepositId = objCurrencyDeposit.Id;
            quotation.CurrencyDepositType = (SD.CurrencyType)objCurrencyDeposit.Numeral;
            quotation.CurrencyDepositTrx = objCurrencyDeposit;

            var objCurrencyTransfer = objCurrencyList.FirstOrDefault(x => x.Numeral == quotationLegacy.CurrencyTransfer);
            if (objCurrencyTransfer == null)
            {
                TempData[AC.Error] = $"La moneda de la transferencia: {quotationLegacy.CurrencyTransfer} no fue encontrado";
                return BadRequest();
            }

            quotation.CurrencyTransferId = objCurrencyTransfer.Id;
            quotation.CurrencyTransferType = (SD.CurrencyType)objCurrencyTransfer.Numeral;
            quotation.CurrencyTransferTrx = objCurrencyTransfer;
            quotation.ExchangeRateOfficialTransa = quotationLegacy.ExchangeRateOfficial;
            quotation.ExchangeRateBuyTransa = quotationLegacy.ExchangeRateBuy;
            quotation.ExchangeRateSellTransa = quotationLegacy.ExchangeRateSell;
            quotation.AmountTransaction = quotationLegacy.AmountTrx;
            quotation.AmountExchange = quotationLegacy.AmountExchange;
            quotation.AmountCost = quotationLegacy.AmountCost;
            quotation.AmountRevenue = quotationLegacy.AmountRevenue;

            if (quotation.TypeNumeral == SD.QuotationType.Transfer)
            {
                var objBankAccountSource =
                    objBankAccountList.FirstOrDefault(x => x.Id == quotationLegacy.BankAccountSourceId);

                if (objBankAccountSource == null)
                {
                    TempData[AC.Error] = $"La cuenta bancaria origen: {quotationLegacy.BankAccountSourceId} no fue encontrada";
                    return BadRequest();
                }

                quotation.BankAccountSourceId = objBankAccountSource.Id;
                quotation.BankAccountSourceTrx = objBankAccountSource;

                var objBankAccountTarget =
                    objBankAccountList.FirstOrDefault(x => x.Id == quotationLegacy.BankAccountTargetId);

                if (objBankAccountTarget == null)
                {
                    TempData[AC.Error] = $"La cuenta bancaria destino: {quotationLegacy.BankAccountTargetId} no fue encontrada";
                    return BadRequest();
                }

                quotation.BankAccountTargetId = objBankAccountTarget.Id;
                quotation.BankAccountTargetTrx = objBankAccountTarget;

            }

            var objBusiness = objBusinessList.FirstOrDefault(x => x.Code == quotationLegacy.BusinessExecutiveCode);
            if (objBusiness == null)
            {
                TempData[AC.Error] = $"El ejecutivo: {quotationLegacy.BusinessExecutiveCode} no fue encontrado";
                return BadRequest();
            }

            quotation.BusinessExecutiveId = objBusiness.Id;
            quotation.BusinessExecutiveCode = objBusiness.Code;
            quotation.BusinessExecutiveTrx = objBusiness;

            quotation.IsVoid = quotationLegacy.IsVoid;
            quotation.IsClosed = quotationLegacy.IsClosed;
            quotation.IsPosted = quotationLegacy.IsJournalPost;

            objQuotationList.Add(quotation);
        }

        if (objQuotationList.Count == 0)
        {
            return NoContent();
        }

        var objQuotationDetailList = new List<QuotationDetail>();

        apiResponse = await _srvDetail.GetAllLegacyAsync(_sessionToken, 0, 1, dateTransaInitial, dateTransaFinal, "");

        var objDetailLegacyList = JsonSerializer.Deserialize<List<QuotationDetailLegacyDto>>(apiResponse, options)!;

        foreach (var detailLegacy in objDetailLegacyList)
        {
            var detail = new QuotationDetail();
            detail.CompanyId = _companyId;
            detail.Id = detailLegacy.Id;
            detail.ParentId = detailLegacy.ParentId;
            detail.AmountDetail = detailLegacy.AmountDetail;
            detail.LineNumber = detailLegacy.DetailNumeral;
            detail.QuotationDetailType = (SD.QuotationDetailType)detailLegacy.TypeDetail;
            detail.CurrencyDetailId = objQuotationList.First(x => x.Id == detailLegacy.ParentId).CurrencyTransaId;
            detail.CurrencyDetailTrx = objQuotationList.First(x => x.Id == detailLegacy.ParentId).CurrencyTransaTrx;
            detail.BankTransactionId = detailLegacy.TransactionRelateUId;
            detail.JournalEntryId = detailLegacy.JournalEntryUId;
            detail.JournalEntryTransferFeeId = detailLegacy.JournalEntryTransferFeeId;
            detail.BankTransactionTransferFeeId = detailLegacy.BankTransactionTransferFeeId;

            if (detailLegacy.TypeNumeral == (int)SD.QuotationType.Transfer)
            {
                if (detail.QuotationDetailType == SD.QuotationDetailType.CreditTransfer)
                {
                    detail.BankSourceId = objBankList.First(x => x.Code == detailLegacy.BankTargetCode).Id;
                    detail.BankSourceTrx = objBankList.First(x => x.Code == detailLegacy.BankTargetCode);
                    detail.BankTargetId = objBankList.First(x => x.Code == detailLegacy.BankTargetCode).Id;
                    detail.BankTargetTrx = objBankList.First(x => x.Code == detailLegacy.BankTargetCode);
                }
                else
                {
                    detail.BankSourceId = objBankList.First(x => x.Code == detailLegacy.BankSourceCode).Id;
                    detail.BankSourceTrx = objBankList.First(x => x.Code == detailLegacy.BankSourceCode);
                    detail.BankTargetId = objBankList.First(x => x.Code == detailLegacy.BankSourceCode).Id;
                    detail.BankTargetTrx = objBankList.First(x => x.Code == detailLegacy.BankSourceCode);
                }
            }
            else
            {
                if (detail.QuotationDetailType == SD.QuotationDetailType.Deposit)
                {
                    detail.BankSourceId = objBankList.First(x => x.Code == detailLegacy.BankTargetCode).Id;
                    detail.BankSourceTrx = objBankList.First(x => x.Code == detailLegacy.BankTargetCode);
                    detail.BankTargetId = objBankList.First(x => x.Code == detailLegacy.BankTargetCode).Id;
                    detail.BankTargetTrx = objBankList.First(x => x.Code == detailLegacy.BankTargetCode);
                }
                else
                {
                    detail.BankSourceId = objBankList.First(x => x.Code == detailLegacy.BankSourceCode).Id;
                    detail.BankSourceTrx = objBankList.First(x => x.Code == detailLegacy.BankSourceCode);
                    detail.BankTargetId = objBankList.First(x => x.Code == detailLegacy.BankTargetCode).Id;
                    detail.BankTargetTrx = objBankList.First(x => x.Code == detailLegacy.BankTargetCode);
                }
            }

            objQuotationDetailList.Add(detail);
        }

        return GenerarExcel("Cotizaciones.xlsx", objQuotationList, objQuotationDetailList);
    }

    private FileResult GenerarExcel(string nombreArchivo, List<Models.Quotation> listEntities, List<Models.QuotationDetail> listEntityDetails)
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
                worksheet.Cell(4, 12).Value = "S";

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

                var children = listEntityDetails.Where(x => x.ParentId == header.Id).ToList();

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



}

