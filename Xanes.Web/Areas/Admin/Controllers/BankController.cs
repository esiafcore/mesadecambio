﻿using Microsoft.AspNetCore.Mvc;
using Xanes.DataAccess.Repository;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models;
using Xanes.Utility;

namespace Xanes.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class BankController : Controller
{
    private readonly IUnitOfWork _uow;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _webHostEnvironment;

    private readonly int _companyId;

    public BankController(IUnitOfWork uow, IConfiguration configuration
        , IWebHostEnvironment webHostEnvironment)
    {
        _uow = uow;
        _configuration = configuration;
        _webHostEnvironment = webHostEnvironment;
        _companyId = _configuration.GetValue<int>("ApplicationSettings:CompanyId");
    }
    // GET
    public IActionResult Index()
    {
        var objList = _uow.Bank.GetAll(x => (x.CompanyId == _companyId)).ToList();
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
            var obj = _uow.Bank.Get(x => (x.Id == id), isTracking: false);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Upsert(Bank obj, IFormFile? filelogo)
    {
        //Datos son validos
        if (ModelState.IsValid)
        {
            //Trabajar con Logo del Banco
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (filelogo != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(filelogo.FileName);
                string imagePath = Path.Combine(wwwRootPath, AC.ImagesBankFolder);

                if (!string.IsNullOrEmpty(obj.LogoUrl))
                {
                    //delete the old image
                    var oldImagePath =
                        Path.Combine(wwwRootPath, obj.LogoUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                await using var fileStream = new FileStream(Path.Combine(imagePath, fileName)
                    , FileMode.Create);
                await filelogo.CopyToAsync(fileStream);
                obj.LogoUrl = $"\\{AC.ImagesBankFolder}{fileName}";
            }


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
                    .Get(filter: x => (x.CompanyId == _companyId)
                                      & (x.Code.Trim().ToLower() == obj.Code.Trim().ToLower()),
                                      isTracking: false);

                if ((objExists != null) && (objExists.Id != obj.Id))
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

    //public IActionResult Delete(int? id)
    //{
    //    if (id == null || id == 0)
    //    {
    //        return NotFound();
    //    }

    //    var obj = _uow.Bank.Get(x => (x.Id == id), isTracking: false);

    //    if (obj == null)
    //    {
    //        return NotFound();
    //    }

    //    return View(obj);
    //}

    //[HttpPost, ActionName("Delete")]
    //public IActionResult DeletePost(int? id)
    //{
    //    var obj = _uow.Bank.Get(x => (x.Id == id), isTracking: false);

    //    if (obj == null)
    //    {
    //        return NotFound();
    //    }
    //    _uow.Bank.Remove(obj);
    //    _uow.Save();
    //    TempData["success"] = "Bank deleted successfully";
    //    return RedirectToAction("Index", "Bank");
    //}

    #region API CALLS
    public IActionResult GetAll()
    {
        var objList = _uow.Bank
            .GetAll(x => (x.CompanyId == _companyId)).ToList();
        return Json(new { data = objList });
    }

    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        var rowToBeDeleted = _uow.Bank.Get(filter:u => (u.Id == id)
        ,isTracking:false);

        if (rowToBeDeleted == null)
        {
            return Json(new { success = false, message = "Bank don´t exist" });
        }

        if (!string.IsNullOrEmpty(rowToBeDeleted.LogoUrl))
        {
            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                rowToBeDeleted.LogoUrl.TrimStart('\\'));

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

        _uow.Bank.RemoveByFilter(filter: u => (u.Id == rowToBeDeleted.Id));

        return Json(new { isSuccess = true
            , successMessages = "Bank Successfully Removed"
            , errorMessages = string.Empty});
    }

    #endregion

}