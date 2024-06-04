using Microsoft.AspNetCore.Mvc;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models;
using Xanes.Utility;

namespace Xanes.Web.Areas.Customer.Controllers;
[Area("Customer")]
public class CustomerCategoryController : Controller
{
    private readonly IUnitOfWork _uow;
    private readonly IConfiguration _configuration;
    private readonly int _companyId;

    public CustomerCategoryController(IUnitOfWork uow, IConfiguration configuration)
    {
        _uow = uow;
        _configuration = configuration;
        _companyId = _configuration.GetValue<int>("ApplicationSettings:CompanyId");
    }
    public IActionResult Index()
    {
        ViewData[AC.Title] = "Categorias de Clientes";

        var objList = _uow.CustomerCategory
            .GetAll(filter:x => (x.CompanyId == _companyId)).ToList();
        return View(objList);
    }

    public IActionResult Detail(int? id)
    {
        ViewData[AC.Title] = "Visualizar - Categoria de Cliente";

        if (id == null || id == 0)
        {
            return NotFound();
        }

        var obj = _uow.CustomerCategory
            .Get(filter: x => (x.Id == id), isTracking: false);

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
            ViewData[AC.Title] = "Crear - Categoria de Cliente";

            //create
            //Setear valor por defecto
            var obj = new CustomerCategory()
            {
                CompanyId = _companyId,
                IsActive = true
            };
            return View(obj);
        }
        else
        {
            ViewData[AC.Title] = "Actualizar - Categoria de Cliente";

            //update
            var obj = _uow.CustomerCategory
                .Get(filter: x => (x.Id == id), isTracking: false);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
    }

    [HttpPost]
    public IActionResult Upsert(CustomerCategory obj)
    {

        //Datos son validos
        if (ModelState.IsValid)
        {
            if (obj.CompanyId != _companyId)
            {
                ModelState.AddModelError("", $"Id de la compañía no puede ser distinto de {_companyId}");
            }

            if (obj.Name.Trim().ToLower() == ".")
            {
                ModelState.AddModelError("name", "Nombre no puede ser .");
            }

            if (obj.Code.Trim().ToLower() == ".")
            {
                ModelState.AddModelError("code", "Código no puede ser .");
            }

            if (obj.Numeral <= 0)
            {
                ModelState.AddModelError("numeral", "No puede ser cero o menor a cero");
            }

            if (!ModelState.IsValid) return View(obj);

            //Creando
            if (obj.Id == 0)
            {
                _uow.CustomerCategory.Add(obj);
                _uow.Save();
                TempData["success"] = "Customer Category created successfully";
                return RedirectToAction("Index", "CustomerCategory");
            }
            else
            {
                //Validar que codigo no está repetido
                var objExists = _uow.CustomerCategory
                    .Get(filter: x => (x.CompanyId == _companyId)
                                      & (x.Code.Trim().ToLower() == obj.Code.Trim().ToLower()), isTracking: false);

                if (objExists != null && objExists.Id != obj.Id)
                {
                    ModelState.AddModelError("", $"Código {obj.Code} ya existe");
                }

                //Validar que numeral no está repetido
                objExists = _uow.CustomerCategory
                    .Get(filter: x => (x.CompanyId == _companyId)
                                      & (x.Numeral == obj.Numeral), isTracking: false);

                if (objExists != null && objExists.Id != obj.Id)
                {
                    ModelState.AddModelError("", $"Número {obj.Numeral} ya existe");
                }

                //Datos son validos
                if (ModelState.IsValid)
                {
                    _uow.CustomerCategory.Update(obj);
                    _uow.Save();
                    TempData["success"] = "Customer Category updated successfully";
                    return RedirectToAction("Index", "CustomerCategory");
                }
                return View(obj);
            }
        }
        return View(obj);
    }

    public IActionResult Delete(int? id)
    {
        ViewData[AC.Title] = "Eliminar - Categoria de Cliente";

        if (id == null || id == 0)
        {
            return NotFound();
        }

        var obj = _uow.CustomerCategory
            .Get(filter: x => (x.Id == id), isTracking: false);

        if (obj == null)
        {
            return NotFound();
        }

        return View(obj);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePost(int? id)
    {
        var obj = _uow.CustomerCategory
            .Get(filter: x => (x.Id == id), isTracking: false);

        if (obj == null)
        {
            return NotFound();
        }
        _uow.CustomerCategory.Remove(obj);
        _uow.Save();
        TempData["success"] = "Customer Category deleted successfully";
        return RedirectToAction("Index", "CustomerCategory");
    }
}
