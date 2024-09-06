using System.Text;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.DataAccess.ServicesApi.Interface;
using Xanes.LoggerService;
using Xanes.Models.Dtos.XanesN4;
using Xanes.Models.Shared;
using Xanes.Utility;
#pragma warning disable CS8604

namespace Xanes.Web.Areas.Customer.Controllers;

[Area("Customer")]
[Authorize()]

public class CustomerLegacyController : Controller
{
    private readonly ICustomerLegacyService _srv;
    private readonly ILoggerManager _logger;
    private readonly IConfiguration _cfg;
    private readonly int _companyId;
    private readonly IUnitOfWork _uow;
    private string _sessionToken;

    public CustomerLegacyController(ICustomerLegacyService service
        , ILoggerManager logger
        , IConfiguration cfg
        , IUnitOfWork uow
        , IHttpContextAccessor httpCtxAcc)
    {
        _logger = logger;
        _srv = service;
        _cfg = cfg;
        _uow = uow;
        _companyId = _cfg.GetValue<int>("ApplicationSettings:CompanyId");
        _sessionToken = httpCtxAcc.HttpContext.Session.GetString(SD.SessionToken) ?? string.Empty;
    }

    [HttpGet]
    public async Task<IActionResult> Export()
    {
        var errorsMessagesBuilder = new StringBuilder();

        var apiResponse = await _srv.GetAllLegacyAsync<APIResponse>(_sessionToken, 0, 1);
        if (apiResponse is null)
        {
            TempData[AC.Error] = "No se pudo obtener la respuesta";
            return RedirectToAction("Index", "Home", new { Area = "exchange" });
        }

        if (apiResponse is { isSuccess: false })
        {
            errorsMessagesBuilder.AppendJoin("", apiResponse.errorMessages);
            TempData[AC.Error] = errorsMessagesBuilder.ToString();
            return RedirectToAction("Index", "Home", new { Area = "exchange" });
        }

        var objList = JsonConvert.DeserializeObject<List<CustomerLegacyDto>>(Convert.ToString(apiResponse.result))!;
        var objCustomerList = new List<Models.Customer>();

        var objTypeList = _uow.PersonType.GetAll(filter: x =>
            x.CompanyId == _companyId).ToList();

        if (objTypeList == null)
        {
            TempData[AC.Error] = "Tipos de personas no encontradas";
            return RedirectToAction("Index", "Home", new { Area = "exchange" });
        }

        var objIdentificationTypeList = _uow.IdentificationType.GetAll(filter: x =>
            x.CompanyId == _companyId).ToList();

        if (objIdentificationTypeList == null)
        {
            TempData[AC.Error] = "Tipos de identificación no encontrados";
            return RedirectToAction("Index", "Home", new { Area = "exchange" });
        }

        var objSectorList = _uow.CustomerSector.GetAll(filter: x =>
            x.CompanyId == _companyId).ToList();

        if (objSectorList == null)
        {
            TempData[AC.Error] = "Sectores no encontrados";
            return RedirectToAction("Index", "Home", new { Area = "exchange" });
        }

        //Armar los clientes a exportar

        foreach (var customerLegacy in objList)
        {
            var customer = new Models.Customer();
            customer.Id = customerLegacy.Id;
            customer.CompanyId = _companyId;
            customer.IdentificationNumber = customerLegacy.IdentificationNumber;
            customer.AddressPrimary = customerLegacy.BusinessAddress;
            customer.BusinessName = customerLegacy.BusinessName;
            customer.CommercialName = customerLegacy.CommercialName;
            customer.FirstName = customerLegacy.FirstName;
            customer.LastName = customerLegacy.LastName;
            customer.SecondName = customerLegacy.SecondName;
            customer.SecondSurname = customerLegacy.SecondSurname;
            customer.Code = customerLegacy.Code;
            customer.IsBank = customerLegacy.IsBank;
            customer.IsSystemRow = customerLegacy.IsSystemRow;
            customer.TotalQuotations = customerLegacy.TotalQuotations;

            if (customerLegacy.PersonType == Enum.GetName(typeof(SD.PersonTypeCode), SD.PersonTypeCode.JUR))
            {
                customer.TypeNumeral = (int)SD.PersonType.LegalPerson;
                customer.TypeId = objTypeList.First(x => x.Numeral == (int)SD.PersonType.LegalPerson).Id;
                customer.TypeTrx = objTypeList.First(x => x.Numeral == (int)SD.PersonType.LegalPerson);
            }
            else
            {
                customer.TypeNumeral = (int)SD.PersonType.NaturalPerson;
                customer.TypeId = objTypeList.First(x => x.Numeral == (int)SD.PersonType.NaturalPerson).Id;
                customer.TypeTrx = objTypeList.First(x => x.Numeral == (int)SD.PersonType.NaturalPerson);
            }

            var sector = objSectorList.FirstOrDefault(x => x.Code == customerLegacy.SectorCategoryCode);

            if(sector == null)
            {
                TempData[AC.Error] = $"Sector: {customerLegacy.SectorCategoryCode} no encontrado";
                return RedirectToAction("Index", "Home", new { Area = "exchange" });
            }
            
            customer.SectorId = sector.Id;
            customer.SectorTrx = sector;
            
            var identificationType =
                objIdentificationTypeList.FirstOrDefault(x => x.Code == customerLegacy.IdentificationTypeCode);

            if (identificationType == null)
            {
                TempData[AC.Error] = $"Tipo de Identificación: {customerLegacy.IdentificationTypeCode} no encontrado";
                return RedirectToAction("Index", "Home", new { Area = "exchange" });
            }
            
            customer.IdentificationTypeCode = identificationType.Code;
            customer.IdentificationTypeId = identificationType.Id;
            customer.IdentificationTypeTrx = identificationType;

            objCustomerList.Add(customer);
        }

        if (objCustomerList == null || objCustomerList.Count == 0)
        {
            TempData[AC.Error] = $"Clientes no encontrados";
            return RedirectToAction("Index", "Home", new { Area = "exchange" });
        }

        return GenerarExcel("ClientesLegacy.xlsx", objCustomerList);
    }

    private FileResult GenerarExcel(string nombreArchivo, List<Models.Customer> listEntities)
    {
        using (XLWorkbook wb = new XLWorkbook())
        {
            var worksheet = wb.Worksheets.Add("Clientes");

            var objCompany = _uow.Company.Get(filter: x => x.Id == _companyId);

            // Escribir el nombre de la compañía en la primera fila
            worksheet.Cell(1, 1).Value = objCompany.Name;
            worksheet.Range(1, 1, 1, 7).Merge().Style.Font.Bold = true;
            worksheet.Range(1, 1, 1, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Range(1, 1, 1, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            // Escribir el título del excel en la segunda fila
            worksheet.Cell(2, 1).Value = "Listado de Clientes";
            worksheet.Range(2, 1, 2, 7).Merge().Style.Font.Bold = true;
            worksheet.Range(2, 1, 2, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Range(2, 1, 2, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            var headerRow = worksheet.Row(4);
            headerRow.Style.Font.Bold = true;
            headerRow.Style.Fill.BackgroundColor = XLColor.PastelGray;

            worksheet.Cell(4, 1).Value = "Tipo";
            worksheet.Cell(4, 2).Value = "Sector";
            worksheet.Cell(4, 3).Value = "Código";
            worksheet.Cell(4, 4).Value = "Código Tipo Ident.";
            worksheet.Cell(4, 5).Value = "# Ident.";
            worksheet.Cell(4, 6).Value = "Razón Social";
            worksheet.Cell(4, 7).Value = "Nombre Comercial";
            worksheet.Cell(4, 8).Value = "Primer Nombre";
            worksheet.Cell(4, 9).Value = "Segundo Nombre";
            worksheet.Cell(4, 10).Value = "Primer Apellido";
            worksheet.Cell(4, 11).Value = "Segundo Apellido";
            worksheet.Cell(4, 12).Value = "Dirección";
            worksheet.Cell(4, 13).Value = "Es Banco";
            worksheet.Cell(4, 14).Value = "Es del Sistema";
            worksheet.Cell(4, 15).Value = "Total Cotizaciones";
            worksheet.Cell(4, 16).Value = "Id";

            int rowNum = 5;
            foreach (var item in listEntities)
            {
                worksheet.Cell(rowNum, 1).Value = item.TypeTrx.Code;
                worksheet.Cell(rowNum, 2).Value = item.SectorTrx.Code;
                worksheet.Cell(rowNum, 3).Value = item.Code;
                worksheet.Cell(rowNum, 4).Value = item.IdentificationTypeCode;
                worksheet.Cell(rowNum, 5).Value = item.IdentificationNumber;
                worksheet.Cell(rowNum, 6).Value = item.BusinessName;
                worksheet.Cell(rowNum, 7).Value = item.CommercialName;
                worksheet.Cell(rowNum, 8).Value = item.FirstName;
                worksheet.Cell(rowNum, 9).Value = item.SecondName;
                worksheet.Cell(rowNum, 10).Value = item.LastName;
                worksheet.Cell(rowNum, 11).Value = item.SecondSurname;
                worksheet.Cell(rowNum, 12).Value = item.AddressPrimary;
                worksheet.Cell(rowNum, 13).Value = item.IsBank ? "S" : "N";
                worksheet.Cell(rowNum, 14).Value = item.IsSystemRow ? "S" : "N";
                worksheet.Cell(rowNum, 15).Value = item.TotalQuotations;
                worksheet.Cell(rowNum, 16).Value = item.Id;

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
            worksheet.Column(13).AdjustToContents();
            worksheet.Column(14).AdjustToContents();
            worksheet.Column(15).AdjustToContents();
            worksheet.Column(16).AdjustToContents();

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

}

