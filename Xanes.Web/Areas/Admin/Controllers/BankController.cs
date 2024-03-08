﻿using Microsoft.AspNetCore.Mvc;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models;

namespace Xanes.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class BankController : Controller
{
    private readonly IUnitOfWork _uow;
    private readonly IConfiguration _configuration;
    private readonly int _companyId;

    public BankController(IUnitOfWork uow, IConfiguration configuration)
    {
        _uow = uow;
        _configuration = configuration;
        _companyId = _configuration.GetValue<int>("ApplicationSettings:CompanyId");
    }
    // GET
    public IActionResult Index()
    {
        var objList = _uow.Bank.GetAll().ToList();
        return View(objList);
    }

    public IActionResult Upsert(int? id)
    {
        if (id == null || id == 0)
        {
            //create
            //Setear valor por defecto
            var obj = new Bank()
            {
                OrderPriority = 1,
                BankingCommissionPercentage = 0,
                CompanyId = _companyId
            };
            return View(obj);
        }
        else
        {
            //update
            var obj = _uow.Bank.Get(x => x.Id == id, isTracking: false);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
    }

    [HttpPost]
    public IActionResult Upsert(Bank obj)
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

            if (!ModelState.IsValid) return View(obj);

            //Creando
            if (obj.Id == 0)
            {
                _uow.Bank.Add(obj);
                _uow.Save();
                TempData["success"] = "Bank created successfully";
                return RedirectToAction("Index", "Bank");
            }
            else
            {
                //Validar que codigo no está repetido
                var objExists = _uow.Bank
                    .Get(x => x.Code.Trim().ToLower() == obj.Code.Trim().ToLower(), isTracking: false);

                if (objExists != null && objExists.Id != obj.Id)
                {
                    ModelState.AddModelError("", $"Código {obj.Code} ya existe");
                }
                //Datos son validos
                if (ModelState.IsValid)
                {
                    _uow.Bank.Update(obj);
                    _uow.Save();
                    TempData["success"] = "Bank updated successfully";
                    return RedirectToAction("Index", "Bank");
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

        var obj = _uow.Bank.Get(x => x.Id == id, isTracking: false);

        if (obj == null)
        {
            return NotFound();
        }

        return View(obj);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePost(int? id)
    {
        var obj = _uow.Bank.Get(x => x.Id == id, isTracking: false);

        if (obj == null)
        {
            return NotFound();
        }
        _uow.Bank.Remove(obj);
        _uow.Save();
        TempData["success"] = "Bank deleted successfully";
        return RedirectToAction("Index", "Bank");
    }

}