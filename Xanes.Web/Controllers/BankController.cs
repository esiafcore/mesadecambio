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
        //var dataList = _db.Banks.ToList();
        List<Bank> dataList = _db.Banks.ToList();
        return View(dataList);
    }

    public IActionResult Create()
    {
        return View();
    }
}