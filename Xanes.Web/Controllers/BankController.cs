using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xanes.DataAccess.Data;
using Xanes.Models;

namespace Xanes.Web.Controllers;

public class BankController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IConfiguration _configuration;
    private readonly int _companyId;

    public BankController(ApplicationDbContext db,IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
        _companyId = _configuration.GetValue<int>("ApplicationSettings:CompanyId");
    }
    // GET
    public IActionResult Index()
    {
        var objList = _db.Banks.ToList();
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
            _db.Banks.Add(obj);
            _db.SaveChanges();
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

        //Setear valor por defecto
        //var obj = _db.Banks
        //    .FirstOrDefault(x => x.Id == id);

        var obj = _db.Banks
            .Find(id);

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
        var objExists = _db.Banks
            .AsNoTracking()
            .FirstOrDefault(x => x.Code.Trim().ToLower() == obj.Code.Trim().ToLower());
        if ((objExists != null) && (objExists.Id != obj.Id))
        {
            ModelState.AddModelError("", $"Código {obj.Code} ya existe");
        }
        //Datos son validos
        if (ModelState.IsValid)
        {
            _db.Banks.Update(obj);
            _db.SaveChanges();
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

        var obj = _db.Banks
            .Find(id);

        if (obj == null)
        {
            return NotFound();
        }

        return View(obj);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePost(int? id)
    {
        //Validar que codigo no está repetido
        bool isrowExist = _db.Banks.Any(x => x.Id == id);

        if (!isrowExist)
        {
            return NotFound();
        }
        _db.Banks.Where(x => x.Id == id).ExecuteDelete();
        TempData["success"] = "Bank deleted successfully";
        return RedirectToAction("Index", "Bank");
    }

}