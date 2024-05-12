using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models;
using Xanes.Utility;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;


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
        , includeProperties: "TypeTrx,CategoryTrx,SectorTrx"
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
    public async Task<IActionResult> Upsert(Models.ViewModels.CustomerCreateVM objViewModel)
    {

        Models.Customer obj = objViewModel.DataModel;

        //Datos son validos
        if (ModelState.IsValid)
        {
            if (obj.CompanyId != _companyId)
            {
                ModelState.AddModelError("", $"Id de la compañía no puede ser distinto de {_companyId}");
            }

            if (obj.BusinessName.Trim().ToLower() == ".")
            {
                ModelState.AddModelError("businessname", "Razón Social no puede ser .");
            }

            if (obj.CommercialName.Trim().ToLower() == ".")
            {
                ModelState.AddModelError("commercialname", "Nombre Comercial no puede ser .");
            }

            if (obj.Code.Trim().ToLower() == ".")
            {
                ModelState.AddModelError("code", "Código no puede ser .");
            }

            if (!ModelState.IsValid) return View(objViewModel);

            //Verificamos si existe el tipo
            var objType = _uow.PersonType.Get(filter: x =>
                x.CompanyId == obj.CompanyId && x.Numeral == (int)obj.TypeNumeral);

            if (objType == null)
            {
                ModelState.AddModelError("", $"Tipo de persona no encontrada");
            }
            else
            {
                obj.TypeId = objType.Id;
            }


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
                        ModelState.AddModelError("code", $"Código {obj.Code} ya existe");
                    }
                }


                //Validar si identificación ya existe
                bool isIdentificationExist = await _uow
                    .Customer.IsExists(x => (x.CompanyId == obj.CompanyId)
                                            && (x.Identificationnumber.Trim() == obj.Identificationnumber.Trim())
                                            && (x.TypeNumeral == obj.TypeNumeral));

                if (isIdentificationExist)
                {
                    if (isIdentificationExist)
                    {
                        ModelState.AddModelError("Identificationnumber", $"# Identificación {obj.Code} ya existe");
                    }
                }

                if (ModelState.IsValid)
                {
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
                    return RedirectToAction("Index", "Customer");
                }
            }
            else
            {
                //Validar que codigo no está repetido
                var objExists = _uow.Customer
                    .Get(filter: x => (x.CompanyId == _companyId)
                                      & (x.Code.Trim().ToLower() == obj.Code.Trim().ToLower()), isTracking: false);

                if (objExists != null && objExists.Id != obj.Id)
                {
                    ModelState.AddModelError("", $"Código {obj.Code} ya existe");
                }

                //Validar que identificación no está repetido
                objExists = _uow.Customer
                    .Get(filter: x => (x.CompanyId == _companyId)
                                      & (x.TypeId == obj.TypeId)
                                      & (x.Identificationnumber.Trim().ToLower() == obj.Identificationnumber.Trim().ToLower()), isTracking: false);

                if (objExists != null && objExists.Id != obj.Id)
                {
                    ModelState.AddModelError("", $"Identificación {obj.Identificationnumber} ya existe");
                }

                //Datos son validos
                if (ModelState.IsValid)
                {
                    _uow.Customer.Update(obj);
                    _uow.Save();
                    TempData["success"] = "Customer updated successfully";
                    return RedirectToAction("Index", "Customer");
                }

                fnFillDataUpsertLists();

                //objViewModel.CategoryList = categorySelectList;
                objViewModel.TypeList = typeSelectList;
                objViewModel.SectorSelectList = sectorSelectList;
                return View(objViewModel);
            }
        }

        fnFillDataUpsertLists();

        //objViewModel.CategoryList = categorySelectList;
        objViewModel.SectorSelectList = sectorSelectList;
        objViewModel.TypeList = typeSelectList;
        return View(objViewModel);

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
            , includeProperties: "TypeTrx,CategoryTrx"
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
    public FileResult ExportExcel()
    {
        var objCustomerList = _uow.Customer.GetAll(filter:x=>(x.CompanyId == _companyId), includeProperties: "TypeTrx,SectorTrx").ToList();

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
            worksheet.Cell(4, 5).Value = "Nombre Comercial";
            worksheet.Cell(4, 6).Value = "Razón Social";
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
                worksheet.Cell(rowNum, 4).Value = item.Identificationnumber;
                worksheet.Cell(rowNum, 5).Value = item.CommercialName;
                worksheet.Cell(rowNum, 6).Value = item.BusinessName;
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
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    nombreArchivo);
            }
        }
    }

}
