using Microsoft.AspNetCore.Mvc;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models;
using Xanes.Utility;

namespace Xanes.Web.Areas.Customer.Controllers;
[Area("Customer")]
public class CustomerSectorController : Controller
{
    private readonly IUnitOfWork _uow;
    private readonly IConfiguration _configuration;
    private readonly int _companyId;

    public CustomerSectorController(IUnitOfWork uow,
        IConfiguration configuration)
    {
        _uow = uow;
        _configuration = configuration;
        _companyId = _configuration.GetValue<int>("ApplicationSettings:CompanyId");
    }

    public IActionResult Index()
    {
        ViewData[AC.Title] = "Sectores";

        var objList = _uow.CustomerSector
            .GetAll(filter: x => (x.CompanyId == _companyId)).OrderBy(x => x.Code)
            .ToList();
        return View(objList);
    }

    // Detalle
    public IActionResult Detail(int? id)
    {
        ViewData[AC.Title] = "Visualizar - Sector";

        if (id == null || id == 0)
        {
            return NotFound();
        }

        var obj = _uow.CustomerSector
            .Get(filter: x => (x.Id == id), includeProperties: "ParentTrx", isTracking: false);

        if (obj == null)
        {
            return NotFound();
        }

        return View(obj);
    }

    // UPSERT
    public IActionResult Upsert(int? id, int? parentId)
    {
        if (id == null || id == 0)
        {
            ViewData[AC.Title] = "Crear - Sector";

            //create
            //Setear valor por defecto
            var obj = new CustomerSector()
            {
                CompanyId = _companyId,
                IsActive = true,
                CodePath = AC.CodeEmpty,
                IdPath = AC.CodeEmpty,
                TypeLevel = SD.TypeLevel.Root,
                ParentId = null
            };


            if ((parentId != null) && (parentId != 0))
            {
                obj.ParentId = parentId;
                obj.TypeLevel = SD.TypeLevel.Detail;

                // Obtener padre
                var modelParent = _uow.CustomerSector
                    .Get(filter: x => (x.Id == obj.ParentId), isTracking: false);
                if (modelParent == null)
                {
                    return NotFound();
                }

                obj.ParentTrx = modelParent;

            }

            return View(obj);
        }
        else
        {
            ViewData[AC.Title] = "Actualizar - Sector";

            //update
            var obj = _uow.CustomerSector
                .Get(filter: x => (x.Id == id), includeProperties: "ParentTrx", isTracking: false);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Upsert(CustomerSector obj)
    {
        bool isParentData = false;
        CustomerSector? modelParent = new();

        // Obtener padre
        if ((obj.ParentId != null) && (obj.ParentId != 0))
        {
            modelParent = _uow.CustomerSector
                .Get(filter: x => (x.Id == obj.ParentId), isTracking: false);
        }

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

            if (!ModelState.IsValid)
            {
                obj.ParentTrx = modelParent;
                return View(obj);
            }

            // Creando
            if (obj.Id == 0)
            {
                if (obj.TypeLevel == SD.TypeLevel.Root)
                {
                    obj.Depthnumber = 1;
                    obj.ParentId = null;
                    obj.SequentialNumber = 0;
                    obj.SequentialDraftNumber = 0;
                    obj.CodePath = obj.Code.Trim();
                }
                else
                {
                    if (modelParent == null)
                    {
                        return NotFound();
                    }

                    if (modelParent.TypeLevel == SD.TypeLevel.Detail)
                    {
                        modelParent.TypeLevel = SD.TypeLevel.SubLevel;
                    }

                    obj.Depthnumber = (short)(modelParent.Depthnumber + 1);

                    // Asignar codigo
                    obj.CodePath = $"{modelParent.CodePath.Trim()}{AC.SeparationCharCode}{obj.Code.Trim()}";

                    isParentData = true;
                }

                //Validar que codigo no está repetido
                var objExists = _uow.CustomerSector
                    .Get(filter: x => (x.CompanyId == _companyId)
                                      & (x.Code.Trim().ToLower() == obj.Code.Trim().ToLower()), isTracking: false);

                if ((objExists != null) && (objExists.Id != obj.Id))
                {
                    ModelState.AddModelError("", $"Código {obj.Code} ya existe");
                }

                if (!ModelState.IsValid)
                {
                    obj.ParentTrx = modelParent;
                    return View(obj);
                }

                _uow.CustomerSector.Add(obj);
                _uow.Save();

                // Si es un root actualizar idPath
                if (!isParentData)
                {
                    obj.IdPath = $"{obj.Id}";
                }
                else if (modelParent != null)
                {
                    obj.IdPath = $"{modelParent.IdPath.Trim()}{AC.SeparationCharCode}{obj.Id}";
                }

                _uow.CustomerSector.Update(obj);
                _uow.Save();

                TempData["success"] = "Customer Sector created successfully";
                return RedirectToAction("Index", "CustomerSector");
            }
            else
            {
                // Verificar que exista
                if (! (await _uow.CustomerSector
                        .IsExists(filter:x => x.Id == obj.Id)))
                {
                    return NotFound();
                }

                _uow.CustomerSector.Update(obj);
                _uow.Save();

                TempData["success"] = "Customer Sector updated successfully";
                return RedirectToAction("Index", "CustomerSector");
            }

        }
        obj.ParentTrx = modelParent;
        return View(obj);
    }

    // DELETE
    public IActionResult Delete(int? id)
    {
        ViewData[AC.Title] = "Eliminar - Sector";

        if (id == null || id == 0)
        {
            return NotFound();
        }

        var obj = _uow.CustomerSector
            .Get(filter: x => (x.Id == id), includeProperties: "ParentTrx", isTracking: false);

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

        if (!(await _uow.CustomerSector
                .IsExists(filter: x => (x.Id == id))))
        {
            return NotFound();
        }

        // Verificar si tiene hijos
        if (await _uow.CustomerSector.IsExists(filter: x => (x.ParentId == id)))
        {
            return BadRequest();
        }

        if (!_uow.CustomerSector
                .RemoveByFilter(filter: x => (x.Id == id)))
        {
            return NotFound();
        }
        TempData["success"] = "Customer Sector deleted successfully";
        return RedirectToAction("Index", "CustomerSector");
    }

}
