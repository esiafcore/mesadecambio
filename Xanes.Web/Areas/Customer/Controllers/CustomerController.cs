using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models;
using Xanes.Utility;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Azure;
using System.Net;
using Xanes.Models.ViewModels;
using System.Text;
using Xanes.Models.Shared;


namespace Xanes.Web.Areas.Customer.Controllers;
[Area("Customer")]
public class CustomerController : Controller
{
    private readonly IUnitOfWork _uow;
    private readonly IConfiguration _configuration;
    private readonly int _companyId;
    private readonly ConfigCxc _cfgCxc;

    //Usada en el Upsert
    private List<CustomerSector> sectorList;
    private IEnumerable<SelectListItem> sectorSelectList;
    private List<PersonType> typeSelectList;

    public CustomerController(IUnitOfWork uow, IConfiguration configuration)
    {
        _uow = uow;
        _configuration = configuration;
        _companyId = _configuration.GetValue<int>("ApplicationSettings:CompanyId");
        _cfgCxc = _uow.ConfigCxc
            .Get(filter: x => (x.CompanyId == _companyId));
    }

    public IActionResult Index()
    {
        var objList = _uow.Customer.GetAll(filter: x => (x.CompanyId == _companyId)
        , includeProperties: "TypeTrx,SectorTrx").ToList();
        return View(objList);
    }

    public IActionResult Detail(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var obj = _uow.Customer.Get(filter: x => (x.Id == id)
        , includeProperties: "TypeTrx,SectorTrx"
        , isTracking: false);

        if (obj == null)
        {
            return NotFound();
        }

        return View(obj);
    }

    public async Task<IActionResult> Upsert(int? id)
    {
        Models.Customer obj;

        if (id == null || id == 0)
        {
            //create
            //Setear valor por defecto
            obj = new Models.Customer()
            {
                CompanyId = _companyId,
                FirstName = string.Empty,
                SecondName = string.Empty,
                LastName = string.Empty,
                SecondSurname = string.Empty,
                BusinessName = string.Empty,
                CommercialName = string.Empty,
                InternalSerial = AC.InternalSerialDraft,
                Code = new string(AC.CharDefaultEmpty, AC.RepeatCharTimes),
                IsActive = true
            };

            if (_cfgCxc.IsAutomaticallyCustomerCode)
            {
                var nextCode = await _uow
                    .ConfigCxc
                    .NextSequentialNumber(filter: x => (x.CompanyId == _companyId)
                        , typeSequential: SD.TypeSequential.Draft
                        , mustUpdate: true);

                obj.Code = nextCode.ToString()
                    .PadLeft(AC.RepeatCharTimes, AC.CharDefaultEmpty);
            }

        }
        else
        {
            //update
            obj = _uow.Customer
                .Get(filter: x => (x.Id == id), isTracking: false);

            if (obj == null)
            {
                return NotFound();
            }
        }

        fnFillDataUpsertLists();

        if (id is null or 0)
        {
            var legalPerson = typeSelectList
                .FirstOrDefault(x => x.Numeral == (int)SD.PersonType.LegalPerson);

            obj.TypeNumeral = (int)SD.PersonType.LegalPerson;
            if (legalPerson != null) { obj.TypeId = legalPerson.Id; }
        }

        var dataVM = new Models.ViewModels.CustomerCreateVM()
        {
            DataModel = obj,
            //CategoryList = categorySelectList,
            TypeList = typeSelectList,
            SectorList = sectorList
        };

        return View(dataVM);
    }

    [HttpPost]
    public async Task<IActionResult> Upsert([FromForm] Models.Customer obj)
    {
        StringBuilder errorsMessagesBuilder = new();
        JsonResultResponse? jsonResponse = new();

        //if (obj.TypeNumeral == (int)SD.PersonType.NaturalPerson)
        //{
        //    obj.BusinessName = string.Empty;
        //    obj.CommercialName = string.Empty;
        //}
        //else
        //{
        //    obj.FirstName = string.Empty;
        //    obj.SecondName = string.Empty;
        //    obj.LastName = string.Empty;
        //    obj.CommercialName = string.Empty;

        //}

        //Datos son validos
        if (ModelState.IsValid)
        {
            if (obj.CompanyId != _companyId)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Id de la compañía no puede ser distinto de {_companyId}";
                return Json(jsonResponse);
            }

            if (obj.BusinessName.Trim().ToLower() == ".")
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Razón Social no puede ser .";
                return Json(jsonResponse);
            }

            if (obj.CommercialName.Trim().ToLower() == ".")
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Nombre Comercial no puede ser .";
                return Json(jsonResponse);
            }

            if (obj.Code.Trim().ToLower() == ".")
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Código no puede ser .";
                return Json(jsonResponse);
            }


            //Verificamos si existe el tipo
            var objType = _uow.PersonType.Get(filter: x =>
                x.CompanyId == obj.CompanyId && x.Numeral == (int)obj.TypeNumeral);

            if (objType == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Tipo de persona no encontrada";
                return Json(jsonResponse);
            }

            obj.TypeId = objType.Id;

            //Creando
            if (obj.Id == 0)
            {
                if (!_cfgCxc.IsAutomaticallyCustomerCode)
                {
                    //Validar si codigo no existe
                    bool isExist = await _uow
                        .Customer.IsExists(x => (x.CompanyId == obj.CompanyId)
                                                && (x.Code.Trim() == obj.Code.Trim()));
                    if (isExist)
                    {
                        jsonResponse.IsSuccess = false;
                        jsonResponse.ErrorMessages = $"Código {obj.Code} ya existe";
                        return Json(jsonResponse);
                    }
                }

                //Validar si identificación ya existe
                bool isIdentificationExist = await _uow
                    .Customer.IsExists(x => (x.CompanyId == obj.CompanyId)
                                            && (x.IdentificationNumber.Trim() == obj.IdentificationNumber.Trim())
                                            && (x.TypeNumeral == obj.TypeNumeral));

                if (isIdentificationExist)
                {
                    if (isIdentificationExist)
                    {
                        jsonResponse.IsSuccess = false;
                        jsonResponse.ErrorMessages = $"# Identificación {obj.Code} ya existe";
                        return Json(jsonResponse);
                    }
                }

                if (_cfgCxc.IsAutomaticallyCustomerCode)
                {
                    var nextSequential = await _uow
                        .Customer
                        .NextSequentialNumber(filter: x => (x.CompanyId == _companyId)
                                                           && (x.InternalSerial == AC.InternalSerialOfficial));

                    obj.Code = nextSequential.ToString()
                        .PadLeft(AC.RepeatCharTimes, AC.CharDefaultEmpty);
                }

                _uow.Customer.Add(obj);
                _uow.Save();
                TempData["success"] = "Customer created successfully";
            }
            else
            {
                //Validar que codigo no está repetido
                var objExists = _uow.Customer
                    .Get(filter: x => (x.CompanyId == _companyId)
                                      & (x.Code.Trim().ToLower() == obj.Code.Trim().ToLower()), isTracking: false);

                if (objExists != null && objExists.Id != obj.Id)
                {
                    jsonResponse.IsSuccess = false;
                    jsonResponse.ErrorMessages = $"Código {obj.Code} ya existe";
                    return Json(jsonResponse);
                }

                //Validar que identificación no está repetido
                objExists = _uow.Customer
                    .Get(filter: x => (x.CompanyId == _companyId)
                                      & (x.TypeId == obj.TypeId)
                                      & (x.IdentificationNumber.Trim().ToLower() == obj.IdentificationNumber.Trim().ToLower()), isTracking: false);

                if (objExists != null && objExists.Id != obj.Id)
                {
                    jsonResponse.IsSuccess = false;
                    jsonResponse.ErrorMessages = $"Identificación {obj.IdentificationNumber} ya existe";
                    return Json(jsonResponse);
                }

                _uow.Customer.Update(obj);
                _uow.Save();
                TempData["success"] = "Customer updated successfully";

            }

            jsonResponse.UrlRedirect = Url.Action(action: "Index", controller: "Customer");
            jsonResponse.IsSuccess = true;
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

        return Json(jsonResponse);
    }

    private void fnFillDataUpsertLists()
    {
        //categorySelectList = _uow.CustomerCategory
        //    .GetAll(filter: x => (x.CompanyId == _companyId))
        //    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });

        sectorSelectList = _uow.CustomerSector
            .GetAll(filter: x => (x.CompanyId == _companyId))
            .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });

        sectorList = _uow.CustomerSector
            .GetAll(filter: x => (x.CompanyId == _companyId)).ToList();

        typeSelectList = _uow.PersonType
            .GetAll(filter: x => (x.CompanyId == _companyId))
            .ToList();
    }

    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var obj = _uow.Customer.Get(filter: x => (x.Id == id)
            , includeProperties: "TypeTrx"
            , isTracking: false);

        if (obj == null)
        {
            return NotFound();
        }

        return View(obj);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeletePost(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        if (!await _uow.Customer.IsExists(filter: x => x.Id == id.Value))
        {
            return NotFound();
        }

        if (!_uow.Customer.RemoveByFilter(filter: x => x.Id == id))
        {
            return NotFound();
        }
        TempData["success"] = "Customer deleted successfully";
        return RedirectToAction("Index", "Customer");
    }

    [HttpGet]
    public IActionResult ExportExcel()
    {
        var objCustomerList = _uow.Customer.GetAll(filter: x => (x.CompanyId == _companyId), includeProperties: "TypeTrx,SectorTrx").ToList();

        if (objCustomerList == null || objCustomerList.Count == 0)
        {
            return NoContent();
        }

        return GenerarExcel("Clientes.xlsx", objCustomerList);
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
            worksheet.Cell(4, 4).Value = "# Ident.";
            worksheet.Cell(4, 5).Value = "Razón Social";
            worksheet.Cell(4, 6).Value = "Nombre Comercial";
            worksheet.Cell(4, 7).Value = "Primer Nombre";
            worksheet.Cell(4, 8).Value = "Segundo Nombre";
            worksheet.Cell(4, 9).Value = "Primer Apellido";
            worksheet.Cell(4, 10).Value = "Segundo Apellido";
            worksheet.Cell(4, 11).Value = "Dirección";
            worksheet.Cell(4, 12).Value = "Es Banco";
            worksheet.Cell(4, 13).Value = "Es del Sistema";

            int rowNum = 5;
            foreach (var item in listEntities)
            {
                worksheet.Cell(rowNum, 1).Value = item.TypeTrx.Code;
                worksheet.Cell(rowNum, 2).Value = item.SectorTrx.Code;
                worksheet.Cell(rowNum, 3).Value = item.Code;
                worksheet.Cell(rowNum, 4).Value = item.IdentificationNumber;
                worksheet.Cell(rowNum, 5).Value = item.BusinessName;
                worksheet.Cell(rowNum, 6).Value = item.CommercialName;
                worksheet.Cell(rowNum, 7).Value = item.FirstName;
                worksheet.Cell(rowNum, 8).Value = item.SecondName;
                worksheet.Cell(rowNum, 9).Value = item.LastName;
                worksheet.Cell(rowNum, 10).Value = item.SecondSurname;
                worksheet.Cell(rowNum, 11).Value = item.AddressPrimary;
                worksheet.Cell(rowNum, 12).Value = item.IsBank ? "S" : "N";
                worksheet.Cell(rowNum, 13).Value = item.IsSystemRow ? "S" : "N";

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
        ViewData[AC.Title] = $"Clientes - Importar";
        ImportVM modelVm = new();
        return View(modelVm);
    }


    [HttpPost]
    public async Task<JsonResult> Import([FromForm] ImportVM objImportViewModel)
    {
        JsonResultResponse? jsonResponse = new();
        try
        {
            List<string> ErrorListMessages = new List<string>();
            var errorsMessagesBuilder = new StringBuilder();
            List<Models.Customer> objCustomerList = new();

            if (objImportViewModel.FileExcel is null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"No hay registros para importar";
                return Json(jsonResponse);
            }

            var objTypeList = _uow.PersonType
                .GetAll(filter: x => x.CompanyId == _companyId)
                .ToList();

            if (objTypeList == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Tipo de persona no encontrado";
                return Json(jsonResponse);
            }

            var objSectorList = _uow.CustomerSector
                .GetAll(filter: x => x.CompanyId == _companyId)
                .ToList();
            if (objSectorList == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Sector de cliente no encontrado";
                return Json(jsonResponse);
            }

            var workbook = new XLWorkbook(objImportViewModel.FileExcel.OpenReadStream());

            var hoja = workbook.Worksheet(1);

            var primerFilaUsada = hoja.FirstRowUsed().RangeAddress.FirstAddress.RowNumber;
            var ultimaFilaUsada = hoja.LastRowUsed().RangeAddress.FirstAddress.RowNumber;

            for (int i = primerFilaUsada + 4; i <= ultimaFilaUsada; i++)
            {

                var objCustomer = new Models.Customer();
                objCustomer.CompanyId = _companyId;

                var fila = hoja.Row(i);

                string businessName = fila.Cell(5).IsEmpty() ? null : fila.Cell(5).GetString();
                if (string.IsNullOrWhiteSpace(businessName))
                {
                    ErrorListMessages.Add($"La razon social está vacia en la fila:{i}. |");
                }

                string commercialName = fila.Cell(6).IsEmpty() ? null : fila.Cell(6).GetString();
                //if (string.IsNullOrWhiteSpace(commercialName))
                //{
                //    ErrorListMessages.Add($"El nombre comercial está vacio en la fila:{i}. ");
                //}

                string address = fila.Cell(11).IsEmpty() ? null : fila.Cell(11).GetString();
                //if (string.IsNullOrWhiteSpace(address))
                //{
                //    ErrorListMessages.Add($"La dirección está vacia en la fila:{i}. ");
                //}

                string isBank = fila.Cell(12).IsEmpty() ? null : fila.Cell(12).GetString();
                if (string.IsNullOrWhiteSpace(isBank))
                {
                    ErrorListMessages.Add($"Es banco está vacio en la fila:{i}. |");
                }
                else
                {
                    if (isBank == "S")
                    {
                        objCustomer.IsBank = true;
                    }
                    else if (isBank == "N")
                    {
                        objCustomer.IsBank = false;
                    }
                    else
                    {
                        ErrorListMessages.Add($"Es banco es invalido en la fila:{i}. |");
                    }
                }

                string isSystem = fila.Cell(13).IsEmpty() ? null : fila.Cell(13).GetString();
                if (string.IsNullOrWhiteSpace(isSystem))
                {
                    ErrorListMessages.Add($"Es del sistema está vacio en la fila:{i}. |");
                }
                else
                {
                    if (isSystem == "S")
                    {
                        objCustomer.IsSystemRow = true;
                    }
                    else if (isSystem == "N")
                    {
                        objCustomer.IsSystemRow = false;
                    }
                    else
                    {
                        ErrorListMessages.Add($"Es del sistema es invalido en la fila:{i}. |");
                    }
                }

                string type = fila.Cell(1).IsEmpty() ? null : fila.Cell(1).GetString();
                if (string.IsNullOrWhiteSpace(type))
                {
                    ErrorListMessages.Add($"El tipo está vacio en la fila:{i}. |");
                }
                else
                {
                    var objType = objTypeList.FirstOrDefault(x => x.Code == type);
                    if (objType == null)
                    {
                        ErrorListMessages.Add($"El tipo no fue encontrado en la fila:{i}. |");
                    }
                    else
                    {
                        objCustomer.TypeId = objType.Id;
                        objCustomer.TypeNumeral = objType.Numeral;

                        if (objType.Numeral == (int)SD.PersonType.NaturalPerson)
                        {
                            if (businessName != null)
                            {
                                var nameSplit = businessName.Split(" ");
                                objCustomer.FirstName = nameSplit[0];
                                objCustomer.SecondName = nameSplit.Length > 1 && nameSplit[1] != "." ? nameSplit[1] : "";
                                objCustomer.LastName = nameSplit.Length > 2 ? nameSplit[2] : "";
                                objCustomer.SecondSurname = nameSplit.Length > 3 && nameSplit[3] != "." ? nameSplit[3] : "";
                                businessName = businessName.Replace(".", "");
                                objCustomer.BusinessName = businessName;
                                objCustomer.CommercialName = businessName;
                            }
                        }
                        else
                        {
                            objCustomer.FirstName = string.Empty;
                            objCustomer.LastName = string.Empty;

                            if (string.IsNullOrWhiteSpace(commercialName))
                            {
                                ErrorListMessages.Add($"El nombre comercial está vacio en la fila:{i}. |");
                            }
                            else
                            {
                                if (businessName != null) objCustomer.BusinessName = businessName;
                                objCustomer.CommercialName = commercialName;
                            }
                        }
                    }
                }

                string sector = fila.Cell(2).IsEmpty() ? null : fila.Cell(2).GetString();
                if (string.IsNullOrWhiteSpace(sector))
                {
                    ErrorListMessages.Add($"El sector está vacio en la fila:{i}. |");
                }
                else
                {
                    var objSector = objSectorList.FirstOrDefault(x => x.Code == sector);
                    if (objSector == null)
                    {
                        ErrorListMessages.Add($"El sector no fue encontrado en la fila:{i}. |");
                    }
                    else
                    {
                        objCustomer.SectorId = objSector.Id;
                    }

                }

                string code = fila.Cell(3).IsEmpty() ? null : fila.Cell(3).GetString();
                if (string.IsNullOrWhiteSpace(code))
                {
                    ErrorListMessages.Add($"El código está vacio en la fila:{i}. |");
                }
                else
                {
                    var customerExist = await _uow.Customer.IsExists(x => x.CompanyId == _companyId && x.Code == code);
                    if (customerExist)
                    {
                        ErrorListMessages.Add($"El código: {code} ya existe en la fila:{i}. |");
                    }
                    else
                    {
                        objCustomer.Code = code;
                    }
                }

                string numberIdent = fila.Cell(4).IsEmpty() ? null : fila.Cell(4).GetString();
                if (string.IsNullOrWhiteSpace(numberIdent))
                {
                    ErrorListMessages.Add($"El número de identificación está vacio en la fila:{i}. |");
                }
                else
                {
                    var customerExist = await _uow.Customer.IsExists(x => x.CompanyId == _companyId && x.IdentificationNumber == numberIdent);
                    if (customerExist)
                    {
                        ErrorListMessages.Add($"El número de identificación: {numberIdent} ya existe en la fila:{i}. |");
                    }
                    else
                    {
                        objCustomer.IdentificationNumber = numberIdent;
                    }
                }

                objCustomer.AddressPrimary = address;
                objCustomer.CreatedBy = AC.LOCALHOSTME;
                objCustomer.CreatedDate = DateTime.UtcNow;
                objCustomer.CreatedHostName = AC.LOCALHOSTPC;
                objCustomer.CreatedIpv4 = AC.Ipv4Default;
                objCustomerList.Add(objCustomer);
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

            await _uow.Customer.ImportRangeAsync(objCustomerList);

            jsonResponse.SuccessMessages = "Importación exitosamente";
            jsonResponse.IsSuccess = true;
            jsonResponse.UrlRedirect = Url.Action(action: "Index", controller: "Customer");
            return Json(jsonResponse);

        }
        catch (Exception ex)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = ex.Message.ToString();
            return Json(jsonResponse);
        }
    }

    public JsonResult GetAll()
    {
        JsonResultResponse? jsonResponse = new();
        var objList = _uow.Customer
            .GetAll(x => (x.CompanyId == _companyId), includeProperties: "SectorTrx,TypeTrx").ToList();

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
}
