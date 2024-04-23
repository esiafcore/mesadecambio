using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models;
using Xanes.Utility;

namespace Xanes.Web.Areas.Customer.Controllers;
[Area("Customer")]
public class CustomerController : Controller
{
    private readonly IUnitOfWork _uow;
    private readonly IConfiguration _configuration;
    private readonly int _companyId;
    private readonly ConfigCxc _cfgCxc;

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
        var objList = _uow.Customer.GetAll(filter:x => (x.CompanyId == _companyId)
        ,includeProperties: "TypeTrx,CategoryTrx,SectorTrx").ToList();
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
                var nextCode= await _uow
                    .ConfigCxc
                    .NextSequentialNumber(filter: x => (x.CompanyId == _companyId)
                        , typeSequential: SD.TypeSequential.Draft
                        , mustUpdate: true);

                obj.Code = nextCode.ToString()
                    .PadLeft(AC.RepeatCharTimes,AC.CharDefaultEmpty);
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

        var categorySelectList = _uow.CustomerCategory
            .GetAll(filter: x => (x.CompanyId == _companyId))
            .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });

        var sectorSelectList = _uow.CustomerSector
            .GetAll(filter: x => (x.CompanyId == _companyId))
            .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });

        var typeSelectList = _uow.PersonType
            .GetAll(filter: x => (x.CompanyId == _companyId))
            .ToList();

        if (typeSelectList == null)
        {
            return NotFound();
        }

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
            CategoryList = categorySelectList,
            TypeList = typeSelectList,
            SectorList = sectorSelectList
        };

        return View(dataVM);
    }

    [HttpPost]
    public async Task<IActionResult> Upsert(Models.ViewModels.CustomerCreateVM objViewModel)
    {
        IEnumerable<SelectListItem> categorySelectList;
        IEnumerable<SelectListItem> sectorSelectList;
        List<PersonType> typeSelectList;

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

            //Creando
            if (obj.Id == 0)
            {
                //Validar si no existe
                bool isExist = await _uow
                    .Customer.IsExists(x => (x.CompanyId == obj.CompanyId)
                     && (x.Code.Trim() == obj.Code.Trim()));

                if (!isExist)
                {
                    _uow.Customer.Add(obj);
                    _uow.Save();
                    TempData["success"] = "Customer created successfully";
                    return RedirectToAction("Index", "Customer");
                }

                ModelState.AddModelError("code", $"Código {obj.Code} ya existe");
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
                categorySelectList = _uow.CustomerCategory
                    .GetAll(filter: x => (x.CompanyId == _companyId))
                    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });

                sectorSelectList = _uow.CustomerSector
                    .GetAll(filter: x => (x.CompanyId == _companyId))
                    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });

                typeSelectList = _uow.PersonType
                    .GetAll(filter: x => (x.CompanyId == _companyId))
                    .ToList();

                objViewModel.CategoryList = categorySelectList;
                objViewModel.TypeList = typeSelectList;
                objViewModel.SectorList = sectorSelectList;
                return View(objViewModel);
            }
        }

        categorySelectList = _uow.CustomerCategory
            .GetAll(filter: x => (x.CompanyId == _companyId))
            .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });

        sectorSelectList = _uow.CustomerSector
            .GetAll(filter: x => (x.CompanyId == _companyId))
            .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });

        typeSelectList = _uow.PersonType
            .GetAll(filter: x => (x.CompanyId == _companyId))
            .ToList();

        objViewModel.CategoryList = categorySelectList;
        objViewModel.SectorList = sectorSelectList;
        objViewModel.TypeList = typeSelectList;
        return View(objViewModel);
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

}
