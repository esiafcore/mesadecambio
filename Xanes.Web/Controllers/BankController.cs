using Microsoft.AspNetCore.Mvc;
using Xanes.Web.Data;
using Xanes.Web.Models;

namespace Xanes.Web.Controllers;

public class BankController : Controller
{
    private readonly ApplicationDbContext _db;

    public BankController(ApplicationDbContext db)
    {
        _db = db;
    }
    // GET
    public IActionResult Index()
    {
        var objList = _db.Banks.ToList();
        return View(objList);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Bank obj)
    {
        _db.Banks.Add(obj);
        _db.SaveChanges();
        return RedirectToAction("Index","Bank");
    }
}