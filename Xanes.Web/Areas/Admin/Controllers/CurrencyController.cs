using Microsoft.AspNetCore.Mvc;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models;
using Xanes.Utility;

namespace Xanes.Web.Areas.Admin.Controllers;
[Area("Admin")]
public class CurrencyController : Controller
{
    private readonly IUnitOfWork _uow;
    private readonly IConfiguration _cfg;
    private readonly int _companyId;

    public CurrencyController(IUnitOfWork uow, IConfiguration configuration)
    {
        _uow = uow;
        _cfg = configuration;
        _companyId = _cfg.GetValue<int>("ApplicationSettings:CompanyId");

    }
    public IActionResult Index()
    {
        ViewData[AC.Title] = "Monedas";

        var objList = _uow.Currency.GetAll(x => (x.CompanyId == _companyId)).ToList();
        return View(objList);
    }

    public IActionResult Upsert(int? id)
    {
        if (id == null || id == 0)
        {
            ViewData[AC.Title] = "Crear - Moneda";

            //Setear valor por defecto
            var obj = new Currency()
            {
                Numeral = 0,
                CompanyId = _companyId
            };
            return View(obj);
        }
        else
        {
            ViewData[AC.Title] = "Actualizar - Moneda";

            var obj = _uow.Currency.Get(x => (x.Id == id), isTracking: false);

            if (obj == null)
            {
                TempData[AC.Error] = $"Moneda no encontrada";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }
    }

    [HttpPost]
    public IActionResult Upsert(Currency obj)
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

            if (obj.CodeIso.Trim().ToLower() == ".")
            {
                ModelState.AddModelError("code", "Código no puede ser .");
            }

            if (!ModelState.IsValid) return View(obj);

            //Creando
            if (obj.Id == 0)
            {
                _uow.Currency.Add(obj);
                _uow.Save();
                TempData["success"] = "Currency created successfully";
                return RedirectToAction("Index", "Currency");
            }
            else
            {
                //Validar que codigo no está repetido
                var objExists = _uow.Currency
                    .Get(x => (x.CompanyId == _companyId)
                              & (x.Code.Trim().ToLower() == obj.Code.Trim().ToLower()), isTracking: false);

                if (objExists != null && objExists.Id != obj.Id)
                {
                    ModelState.AddModelError("", $"Código {obj.Code} ya existe");
                }

                //Validar que codigo ISO no está repetido
                objExists = _uow.Currency
                    .Get(x => (x.CompanyId == _companyId)
                              & (x.CodeIso.Trim().ToLower() == obj.CodeIso.Trim().ToLower()), isTracking: false);

                if (objExists != null && objExists.Id != obj.Id)
                {
                    ModelState.AddModelError("", $"Código ISO {obj.CodeIso} ya existe");
                }

                //Datos son validos
                if (!ModelState.IsValid) return View(obj);

                _uow.Currency.Update(obj);
                _uow.Save();
                TempData["success"] = "Currency updated successfully";
                return RedirectToAction("Index", "Currency");

            }
        }

        return View(obj);
    }


    public IActionResult Delete(int? id)
    {
        ViewData[AC.Title] = "Eliminar - Moneda";


        if (id == null || id == 0)
        {
            TempData[AC.Error] = $"El id es invalido";
            return RedirectToAction(nameof(Index));
        }

        var obj = _uow.Currency.Get(x => (x.Id == id), isTracking: false);

        if (obj == null)
        {
            TempData[AC.Error] = $"Moneda no encontrada";
            return RedirectToAction(nameof(Index));
        }

        return View(obj);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePost(int? id)
    {
        var obj = _uow.Currency.Get(x => (x.Id == id), isTracking: false);

        if (obj == null)
        {
            TempData[AC.Error] = $"Moneda no encontrada";
            return RedirectToAction(nameof(Index));
        }
        _uow.Currency.Remove(obj);
        _uow.Save();
        TempData["success"] = "Currency deleted successfully";
        return RedirectToAction("Index", "Currency");
    }
}
