using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models.ViewModels;
using Xanes.Models;
using Xanes.Utility;
using Xanes.Models.Shared;
using System.Text.Json;
using System.Text;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using static Xanes.Utility.SD;


namespace Xanes.Web.Areas.Admin.Controllers;
[Area("Admin")]
public class CurrencyExchangeRateController : Controller
{
    private readonly IUnitOfWork _uow;
    private readonly IConfiguration _configuration;
    private readonly int _companyId;
    private readonly int _decimalTransa;
    private readonly int _decimalExchange;
    public CurrencyExchangeRateController(IUnitOfWork uow, IConfiguration configuration)
    {
        _uow = uow;
        _configuration = configuration;
        _companyId = _configuration.GetValue<int>("ApplicationSettings:CompanyId");
        _decimalTransa = _configuration.GetValue<int>("ApplicationSettings:DecimalTransa");
        _decimalExchange = _configuration.GetValue<int>("ApplicationSettings:DecimalExchange");
    }

    public IActionResult Index(SD.CurrencyType currencyType = SD.CurrencyType.Foreign)
    {
        ViewBag.DecimalTransa = JsonSerializer.Serialize(_decimalTransa);
        ViewBag.DecimalExchange = JsonSerializer.Serialize(_decimalExchange);

        var currencyList = _uow.Currency
            .GetAll(filter: x => (x.CompanyId == _companyId)
                && (x.Numeral != (short)SD.CurrencyType.Base)
                && (x.IsActive))
            .OrderBy(x => x.Numeral)
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString()
                ,
                Text = x.Abbreviation
                ,
                Selected = (x.Numeral == (short)currencyType)
            });

        var objViewModel = new CurrencyExchangeRateIndexVM()
        {
            CurrencyList = currencyList,
            CurrencySelected = currencyType
        };

        return View(objViewModel);
    }

    public JsonResult GetAll(SD.CurrencyType currencyType = SD.CurrencyType.Foreign)
    {
        JsonResultResponse? jsonResponse = new();
        var objList = _uow.CurrencyExchangeRate
            .GetAll(filter: x => (x.CompanyId == _companyId) && (x.CurrencyType == currencyType),
                includeProperties: "CurrencyTrx")
            .OrderByDescending(x => x.DateTransa)
            .ToList();

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

    // DETALLE
    public IActionResult Detail(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var obj = _uow.CurrencyExchangeRate
            .Get(filter: x => (x.Id == id), includeProperties: "CurrencyTrx", isTracking: false);

        if (obj == null)
        {
            return NotFound();
        }

        return View(obj);
    }

    // UPSERT
    public IActionResult Upsert(int? id, SD.CurrencyType currencyType)
    {
        CurrencyExchangeRate obj;

        if (id == null || id == 0)
        {
            //Obtener moneda
            var currencyCurrent = _uow.Currency
                .Get(filter: x => (x.CompanyId == _companyId) && (x.Numeral == (short)currencyType));
            if (currencyCurrent == null)
            {
                return NotFound();
            }

            //Setear valor por defecto
            obj = new CurrencyExchangeRate()
            {
                CompanyId = _companyId,
                DateTransa = DateOnly.FromDateTime(DateTime.Now),
                CurrencyType = currencyType,
                CurrencyTrx = currencyCurrent
            };
        }
        else
        {
            obj = _uow.CurrencyExchangeRate
                .Get(x => (x.Id == id)
                    , includeProperties: "CurrencyTrx", isTracking: false);

            if (obj == null)
            {
                return NotFound();
            }

        }

        var currencyList = _uow.Currency
            .GetAll(filter: x => (x.CompanyId == _companyId)
                && (x.Numeral != (short)SD.CurrencyType.Base))
            .OrderBy(x => x.Numeral)
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString()
                ,
                Text = x.Abbreviation
                ,
                Selected = (x.Numeral == (short)obj.CurrencyType)
            });

        var dataVM = new CurrencyExchangeRateVM()
        {
            DataModel = obj,
            CurrencyList = currencyList
        };

        return View(dataVM);
    }

    [HttpPost]
    public async Task<IActionResult> Upsert(CurrencyExchangeRateVM objViewModel)
    {
        CurrencyExchangeRate obj = objViewModel.DataModel;
        if (ModelState.IsValid)
        {
            if (obj.CompanyId != _companyId)
            {
                ModelState.AddModelError("", $"Id de la compañía no puede ser distinto de {_companyId}");
            }

            if (obj.OfficialRate < 0)
            {
                ModelState.AddModelError("", $"TC Oficial no puede ser {obj.OfficialRate}");
            }

            if (obj.BuyRate < 0)
            {
                ModelState.AddModelError("", $"TC Compra no puede ser {obj.BuyRate}");
            }

            if (obj.SellRate < 0)
            {
                ModelState.AddModelError("", $"TC Venta no puede ser {obj.SellRate}");
            }

            // Verificar que exista la moneda
            var objCurrency = _uow.Currency
                .Get(filter: x => (x.Numeral == (short)obj.CurrencyType));
            if (objCurrency == null)
            {
                return NotFound();
            }

            // Asignar id moneda
            obj.CurrencyId = objCurrency.Id;

            // Verificar si ya existe moneda - fecha
            var objExist = await _uow.CurrencyExchangeRate
                .IsExists(filter: x => (x.CompanyId == _companyId)
                                       && (x.DateTransa == obj.DateTransa)
                                       && (x.CurrencyType == obj.CurrencyType)
                                       && (x.Id != obj.Id));
            if (objExist)
            {
                ModelState.AddModelError("", $"Tipo de cambio para {objCurrency.Abbreviation} - {obj.DateTransa} ya existe");
            }


            if (!ModelState.IsValid)
            {
                var currencyList = _uow.Currency
                    .GetAll(filter: x => (x.CompanyId == _companyId)
                        && (x.Numeral != (short)SD.CurrencyType.Base))
                    .OrderBy(x => x.Numeral)
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString()
                        ,
                        Text = x.Abbreviation
                        ,
                        Selected = (x.Numeral == (short)obj.CurrencyType)
                    });

                objViewModel.CurrencyList = currencyList;
                objViewModel.DataModel.CurrencyTrx = objCurrency;
                return View(objViewModel);
            }

            // Creando
            if (obj.Id == 0)
            {
                obj.CreatedBy = AC.LOCALHOSTME;
                obj.CreatedDate = DateTime.UtcNow;
                obj.CreatedHostName = AC.LOCALHOSTPC;
                obj.CreatedIpv4 = AC.Ipv4Default;
                _uow.CurrencyExchangeRate.Add(obj);
                _uow.Save();
                TempData["success"] = "Exchange Rate created successfully";
            }
            else
            {
                // Verificar que exista
                if (!(await _uow.CurrencyExchangeRate
                        .IsExists(filter: x => x.Id == obj.Id)))
                {
                    return NotFound();
                }
                obj.UpdatedBy = AC.LOCALHOSTME;
                obj.UpdatedDate = DateTime.UtcNow;
                obj.UpdatedHostName = AC.LOCALHOSTPC;
                obj.UpdatedIpv4 = AC.Ipv4Default;
                _uow.CurrencyExchangeRate.Update(obj);
                _uow.Save();
                TempData["success"] = "Exchange Rate updated successfully";
            }

            return RedirectToAction("Index", "CurrencyExchangeRate", new { currencyType = obj.CurrencyType });
        }
        else
        {

            var currencyList = _uow.Currency
                .GetAll(filter: x => (x.CompanyId == _companyId)
                && (x.Numeral != (short)SD.CurrencyType.Base))
                .OrderBy(x => x.Numeral)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString()
                ,
                    Text = x.Abbreviation
                ,
                    Selected = (x.Numeral == (short)obj.CurrencyType)
                });

            objViewModel.CurrencyList = currencyList;
            return View(objViewModel);
        }

    }

    // DELETE
    public IActionResult Delete(int? id)
    {
        if ((id == null) || (id == 0))
        {
            return NotFound();
        }

        var obj = _uow.CurrencyExchangeRate
            .Get(filter: x => (x.Id == id), includeProperties: "CurrencyTrx", isTracking: false);

        if (obj == null)
        {
            return NotFound();
        }

        return View(obj);
    }

    [HttpPost]
    public JsonResult GetCurrencyExchangeRate(string date)
    {
        JsonResultResponse? jsonResponse = new();
        DateOnly dateTransa = DateOnly.Parse(date);

        var objList = _uow.CurrencyExchangeRate.GetAll
            (x => (x.CompanyId == _companyId) && (x.DateTransa == dateTransa)
                , includeProperties: "CurrencyTrx").ToList();

        if (objList is null || objList.Count == 0)
        {
            jsonResponse.IsSuccess = false;
            return Json(jsonResponse);
        }

        var currencyForeign = objList
            .FirstOrDefault(t => (t.CurrencyType == SD.CurrencyType.Foreign));
        var currencyAdditional = objList
            .FirstOrDefault(t => (t.CurrencyType == SD.CurrencyType.Additional));

        jsonResponse.IsSuccess = true;
        jsonResponse.Data = new
        {
            CurrencyForeign = currencyForeign,
            CurrencyAdditional = currencyAdditional
        };

        return Json(jsonResponse);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePost(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var objCurrency = _uow.CurrencyExchangeRate
            .Get(filter: x => (x.Id == id), includeProperties: "CurrencyTrx", isTracking: false);

        if (objCurrency == null)
        {
            return NotFound();
        }

        var currencyType = objCurrency.CurrencyType;

        if (!_uow.CurrencyExchangeRate
                .RemoveByFilter(filter: x => (x.Id == id)))
        {
            return NotFound();
        }
        TempData["success"] = "Exchange Rate deleted successfully";
        return RedirectToAction("Index", "CurrencyExchangeRate", new { currencyType = currencyType });
    }

    #region EXPORT - IMPORT

    [HttpGet]
    public IActionResult ExportExcel(SD.CurrencyType currencyType = SD.CurrencyType.Foreign)
    {
        var objList = _uow.CurrencyExchangeRate
            .GetAll(filter: x => (x.CompanyId == _companyId) && (x.CurrencyType == currencyType),
                includeProperties: "CurrencyTrx")
            .OrderByDescending(x => x.DateTransa)
            .ToList();

        if (objList == null || objList.Count == 0)
        {
            return NoContent();
        }

        return GenerarExcel("TiposDeCambio.xlsx", objList);
    }

    private FileResult GenerarExcel(string nombreArchivo, List<Models.CurrencyExchangeRate> listEntities)
    {
        using (XLWorkbook wb = new XLWorkbook())
        {
            var worksheet = wb.Worksheets.Add("TiposDeCambio");

            var objCompany = _uow.Company.Get(filter: x => x.Id == _companyId);

            // Escribir el nombre de la compañía en la primera fila
            worksheet.Cell(1, 1).Value = objCompany.Name;
            worksheet.Range(1, 1, 1, 7).Merge().Style.Font.Bold = true;
            worksheet.Range(1, 1, 1, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Range(1, 1, 1, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            // Escribir el título del excel en la segunda fila
            worksheet.Cell(2, 1).Value = "Listado de Tipos de Cambio";
            worksheet.Range(2, 1, 2, 7).Merge().Style.Font.Bold = true;
            worksheet.Range(2, 1, 2, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Range(2, 1, 2, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


            var headerRow = worksheet.Row(4);
            headerRow.Style.Font.Bold = true;
            headerRow.Style.Fill.BackgroundColor = XLColor.PastelGray;

            worksheet.Cell(4, 1).Value = "Moneda";
            worksheet.Cell(4, 2).Value = "Fecha";
            worksheet.Cell(4, 3).Value = "T/C Oficial";
            worksheet.Cell(4, 4).Value = "T/C Compra";
            worksheet.Cell(4, 5).Value = "T/C Venta";

            int rowNum = 5;
            foreach (var item in listEntities)
            {
                worksheet.Cell(rowNum, 1).Value = (short)item.CurrencyType;
                worksheet.Cell(rowNum, 2).SetValue(item.DateTransa.ToDateTimeConvert());
                worksheet.Cell(rowNum, 2).Style.NumberFormat.SetFormat(AC.DefaultDateFormatView);
                worksheet.Cell(rowNum, 3).Value = item.OfficialRate;
                worksheet.Cell(rowNum, 3).Style.NumberFormat.Format = AC.XlsFormatRateExchange;
                worksheet.Cell(rowNum, 4).Value = item.BuyRate;
                worksheet.Cell(rowNum, 4).Style.NumberFormat.Format = AC.XlsFormatRateExchange;
                worksheet.Cell(rowNum, 5).Value = item.SellRate;
                worksheet.Cell(rowNum, 5).Style.NumberFormat.Format = AC.XlsFormatRateExchange;
                rowNum++;
            }

            worksheet.Column(1).AdjustToContents();
            worksheet.Column(2).AdjustToContents();
            worksheet.Column(3).AdjustToContents();
            worksheet.Column(4).AdjustToContents();
            worksheet.Column(5).AdjustToContents();


            // Asignar un nombre a la página del excel
            wb.Worksheet(1).Name = "Data";

            using (MemoryStream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                return File(stream.ToArray(), AC.ContentTypeExcel,
                    nombreArchivo);
            }
        }
    }

    [HttpGet]
    public IActionResult Import()
    {
        // Titulo de la pagina
        ViewData[AC.Title] = $"Tipos de Cambio - Importar";
        ImportVM modelVm = new();
        return View(modelVm);
    }

    [HttpPost]
    public async Task<JsonResult> Import([FromForm] ImportVM objImportViewModel)
    {
        List<string> ErrorListMessages = new List<string>();
        var errorsMessagesBuilder = new StringBuilder();
        JsonResultResponse? jsonResponse = new();
        List<CurrencyExchangeRate>? objList = new();
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

            var workbook = new XLWorkbook(objImportViewModel.FileExcel.OpenReadStream());

            var hoja = workbook.Worksheet(1);

            var primerFilaUsada = hoja.FirstRowUsed().RangeAddress.FirstAddress.RowNumber;
            var ultimaFilaUsada = hoja.LastRowUsed().RangeAddress.FirstAddress.RowNumber;


            for (int i = primerFilaUsada + 4; i <= ultimaFilaUsada; i++)
            {
                var fila = hoja.Row(i);
                var obj = new Models.CurrencyExchangeRate();
                obj.CompanyId = _companyId;
                obj.CreatedBy = AC.LOCALHOSTME;
                obj.CreatedDate = DateTime.UtcNow;
                obj.CreatedHostName = AC.LOCALHOSTPC;
                obj.CreatedIpv4 = AC.Ipv4Default;
                var currency = fila.Cell(1).GetString();
                if (string.IsNullOrWhiteSpace(currency))
                {
                    ErrorListMessages.Add($"La moneda está vacia - En la fila:{i}. |");
                }
                else
                {
                    int currencyType = int.Parse(currency);

                    if (Enum.IsDefined(typeof(SD.CurrencyType), currencyType))
                    {
                        obj.CurrencyId = objCurrencyList
                            .First(x => x.Numeral == currencyType).Id;

                        obj.CurrencyType = (SD.CurrencyType)currencyType;
                    }
                    else
                    {
                        ErrorListMessages.Add($"La moneda  es invalida - En la fila:{i}. |");
                    }
                }


                var date = fila.Cell(2).GetString();
                if (string.IsNullOrWhiteSpace(date))
                {
                    ErrorListMessages.Add($"La fecha está vacia - En la fila:{i}. |");
                }
                else
                {
                    DateOnly dateTransa = DateOnly.Parse(date.Split(" ")[0]);
                    obj.DateTransa = dateTransa;
                }

                var exchangeOfficial = fila.Cell(3).GetString();
                if (string.IsNullOrWhiteSpace(exchangeOfficial))
                {
                    ErrorListMessages.Add($"El tipo de cambio oficial está vacio - En la fila:{i}. |");
                }
                else
                {
                    obj.OfficialRate = decimal.Parse(exchangeOfficial);
                    if (obj.OfficialRate < 0)
                    {
                        ErrorListMessages.Add(
                            $"El tipo de cambio oficial no puede ser menor  a cero - En la fila:{i}. |");
                    }
                }

                var exchangeBuy = fila.Cell(4).GetString();
                if (string.IsNullOrWhiteSpace(exchangeBuy))
                {
                    ErrorListMessages.Add($"El tipo de cambio compra está vacio - En la fila:{i}. |");
                }
                else
                {
                    obj.BuyRate = decimal.Parse(exchangeBuy);
                    if (obj.BuyRate < 0)
                    {
                        ErrorListMessages.Add(
                            $"El tipo de cambio compra no puede ser menor a cero - En la fila:{i}. |");
                    }
                }

                var exchangeSell = fila.Cell(5).GetString();
                if (string.IsNullOrWhiteSpace(exchangeSell))
                {
                    ErrorListMessages.Add($"El tipo de cambio venta está vacio - En la fila:{i}. |");
                }
                else
                {
                    obj.SellRate = decimal.Parse(exchangeSell);
                    if (obj.SellRate < 0)
                    {
                        ErrorListMessages.Add(
                            $"El tipo de cambio venta no puede ser menor a cero - En la fila:{i}. |");
                    }
                }

                // Verificar si ya existe moneda - fecha
                var objExist = await _uow.CurrencyExchangeRate
                    .IsExists(filter: x => (x.CompanyId == _companyId)
                                           && (x.DateTransa == obj.DateTransa)
                                           && (x.CurrencyType == obj.CurrencyType)
                                           && (x.Id != obj.Id));
                if (objExist)
                {
                    ErrorListMessages.Add(
                        $"El tipo de cambio para {objCurrencyList
                            .First(x => x.Id == obj.CurrencyId).Abbreviation} _ {obj.DateTransa} ya existe - En la fila:{i}. |");
                }

                objList.Add(obj);
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


            await _uow.CurrencyExchangeRate.ImportRangeAsync(objList);

            jsonResponse.SuccessMessages = "Importación exitosamente";
            jsonResponse.IsSuccess = true;
            jsonResponse.UrlRedirect = Url.Action(action: "Index", controller: "CurrencyExchangeRate");
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
