﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xanes.DataAccess.Data;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models;

namespace Xanes.Web.Controllers;

public class BankController : Controller
{
    private readonly IBankRepository _repo;
    private readonly IConfiguration _configuration;
    private readonly int _companyId;

    public BankController(IBankRepository repo,IConfiguration configuration)
    {
        _repo = repo;
        _configuration = configuration;
        _companyId = _configuration.GetValue<int>("ApplicationSettings:CompanyId");
    }
    // GET
    public IActionResult Index()
    {
        var objList = _repo.GetAll().ToList();
        return View(objList);
    }

    public IActionResult Create()
    {
        //Setear valor por defecto
        var obj = new Bank()
        {
            OrderPriority = 1,
            BankingCommissionPercentage = 0,
            CompanyId = _companyId
        };

        return View(obj);
    }

    [HttpPost]
    public IActionResult Create(Bank obj)
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

        //Datos son validos
        if (ModelState.IsValid) {
            _repo.Add(obj);
            _repo.Save();
            TempData["success"] = "Bank created successfully";
            return RedirectToAction("Index", "Bank");
        }

        return View(obj);
    }

    public IActionResult Edit(int? id)
    {
        if ((id == null) || (id == 0))
        {
            return NotFound();
        }

        var obj = _repo.Get(x => x.Id == id,isTracking:false);

        if (obj == null)
        {
            return NotFound();
        }

        return View(obj);
    }

    [HttpPost]
    public IActionResult Edit(Bank obj)
    {
        //Validar que codigo no está repetido
        var objExists = _repo
            .Get(x => x.Code.Trim().ToLower() == obj.Code.Trim().ToLower(),isTracking: false);

        if ((objExists != null) && (objExists.Id != obj.Id))
        {
            ModelState.AddModelError("", $"Código {obj.Code} ya existe");
        }
        //Datos son validos
        if (ModelState.IsValid)
        {
            _repo.Update(obj);
            _repo.Save();
            TempData["success"] = "Bank updated successfully";
            return RedirectToAction("Index", "Bank");
        }

        return View(obj);
    }

    public IActionResult Delete(int? id)
    {
        if ((id == null) || (id == 0))
        {
            return NotFound();
        }

        var obj = _repo.Get(x => x.Id == id, isTracking: false);

        if (obj == null)
        {
            return NotFound();
        }

        return View(obj);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePost(int? id)
    {
        var obj = _repo.Get(x => x.Id == id, isTracking: false);

        if (obj == null)
        {
            return NotFound();
        }
        _repo.Remove(obj);
        _repo.Save();
        TempData["success"] = "Bank deleted successfully";
        return RedirectToAction("Index", "Bank");
    }

}