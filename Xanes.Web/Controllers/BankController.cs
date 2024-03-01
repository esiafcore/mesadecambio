using Microsoft.AspNetCore.Mvc;
using Xanes.Web.Data;
using Xanes.Web.Models;

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
            CompanyId = _companyId,
        };

        return View(obj);
    }

    [HttpPost]
    public IActionResult Create(Bank obj)
    {
        //Datos son validos
        if (ModelState.IsValid) {
            _db.Banks.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index", "Bank");
        }

        return View(obj);
    }
}