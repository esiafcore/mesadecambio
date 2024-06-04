using Microsoft.AspNetCore.Mvc;
using Xanes.DataAccess.Data;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models;
using Xanes.Utility;

namespace Xanes.Web.Areas.Admin.Controllers;
[Area("Admin")]
public class IdentificationTypeController : Controller
{
    private readonly IUnitOfWork _uow;
    private readonly IConfiguration _configuration;
    private readonly int _companyId;

    public IdentificationTypeController(IUnitOfWork uow, IConfiguration configuration)
    {
        _uow = uow;
        _configuration = configuration;
        _companyId = _configuration.GetValue<int>("ApplicationSettings:CompanyId");
    }
    public IActionResult Index()
    {
        ViewData[AC.Title] = "Tipos de Idenficación";

        var objList = _uow.IdentificationType
            .GetAll(filter: x => (x.CompanyId == _companyId)).ToList();
        return View(objList);
    }

    public IActionResult Detail(int? id)
    {
        ViewData[AC.Title] = "Visualizar - Tipo de Idenficación";


        if (id == null || id == 0)
        {
            return NotFound();
        }

        var obj = _uow.IdentificationType
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
            ViewData[AC.Title] = "Crear - Tipo de Idenficación";

            //create
            //Setear valor por defecto
            var obj = new IdentificationType()
            {
                CompanyId = _companyId,
                IsActive = true,
                RegularExpressionNumber = string.Empty,
                FormatExpressionNumber = string.Empty,
                SubstitutionExpressionNumber = string.Empty,
                IdentificationMaxLength = 0
            };
            return View(obj);
        }
        else
        {
            ViewData[AC.Title] = "Actualizar - Tipo de Idenficación";

            //update
            var obj = _uow.IdentificationType
                .Get(filter: x => (x.Id == id), isTracking: false);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
    }

    [HttpPost]
    public IActionResult Upsert(IdentificationType obj)
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

            if (!Enum.IsDefined(typeof(SD.IdentificationTypeNumber),obj.Numeral))
            {
                ModelState.AddModelError("numeral", "No está en los valores permitidos");
            }

            if (!ModelState.IsValid) return View(obj);

            //Creando
            if (obj.Id == 0)
            {
                _uow.IdentificationType.Add(obj);
                _uow.Save();
                TempData["success"] = "Identification Type created successfully";
                return RedirectToAction("Index", "IdentificationType");
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
                    _uow.IdentificationType.Update(obj);
                    _uow.Save();
                    TempData["success"] = "Identification Type updated successfully";
                    return RedirectToAction("Index", "IdentificationType");
                }
                return View(obj);
            }
        }
        return View(obj);
    }

    public IActionResult Delete(int? id)
    {
        ViewData[AC.Title] = "Eliminar - Tipo de Idenficación";

        if (id == null || id == 0)
        {
            return NotFound();
        }

        var obj = _uow.IdentificationType
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
        var obj = _uow.IdentificationType
            .Get(filter: x => (x.Id == id), isTracking: false);

        if (obj == null)
        {
            return NotFound();
        }
        _uow.IdentificationType.Remove(obj);
        _uow.Save();
        TempData["success"] = "Identification Type deleted successfully";
        return RedirectToAction("Index", "IdentificationType");
    }


}
