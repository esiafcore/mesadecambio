using Microsoft.AspNetCore.Mvc;
using Xanes.DataAccess.Repository.IRepository;

namespace Xanes.Web.Areas.Customer.Controllers;
[Area("Customer")]
public class CustomerController : Controller
{
    private readonly IUnitOfWork _uow;
    private readonly IConfiguration _configuration;
    private readonly int _companyId;

    public CustomerController(IUnitOfWork uow, IConfiguration configuration)
    {
        _uow = uow;
        _configuration = configuration;
        _companyId = _configuration.GetValue<int>("ApplicationSettings:CompanyId");
    }
    public IActionResult Index()
    {
        var objList = _uow.Customer.GetAll(filter:x => (x.CompanyId == _companyId)
        ,includeProperties: "TypeTrx,CategoryTrx").ToList();
        return View(objList);
    }

    public IActionResult Detail(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var obj = _uow.Customer.Get(x => (x.Id == id), isTracking: false);

        if (obj == null)
        {
            return NotFound();
        }

        return View(obj);
    }

    public IActionResult Upsert(int? id)
    {
        if (id == null || id == 0)
        {
            //create
            //Setear valor por defecto
            var obj = new Models.Customer()
            {
                CompanyId = _companyId,
                IsActive = true
            };
            return View(obj);
        }
        else
        {
            //update
            var obj = _uow.Customer
                .Get(x => (x.Id == id), isTracking: false);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
    }

    [HttpPost]
    public IActionResult Upsert(Models.Customer obj)
    {

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

            if (!ModelState.IsValid) return View(obj);

            //Creando
            if (obj.Id == 0)
            {
                _uow.Customer.Add(obj);
                _uow.Save();
                TempData["success"] = "Customer created successfully";
                return RedirectToAction("Index", "Customer");
            }
            else
            {
                //Validar que codigo no está repetido
                var objExists = _uow.Customer
                    .Get(x => (x.CompanyId == _companyId)
                    & (x.Code.Trim().ToLower() == obj.Code.Trim().ToLower()), isTracking: false);

                if (objExists != null && objExists.Id != obj.Id)
                {
                    ModelState.AddModelError("", $"Código {obj.Code} ya existe");
                }

                //Validar que identificación no está repetido
                objExists = _uow.Customer
                    .Get(x => (x.CompanyId == _companyId)
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
                return View(obj);
            }
        }
        return View(obj);
    }

    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var obj = _uow.Customer.Get(x => (x.Id == id), isTracking: false);

        if (obj == null)
        {
            return NotFound();
        }

        return View(obj);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePost(int? id)
    {
        var obj = _uow.Customer.Get(x => (x.Id == id), isTracking: false);

        if (obj == null)
        {
            return NotFound();
        }
        _uow.Customer.Remove(obj);
        _uow.Save();
        TempData["success"] = "Customer deleted successfully";
        return RedirectToAction("Index", "Customer");
    }

}
