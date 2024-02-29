using Microsoft.AspNetCore.Mvc;

namespace Xanes.Web.Controllers;

public class BankController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}