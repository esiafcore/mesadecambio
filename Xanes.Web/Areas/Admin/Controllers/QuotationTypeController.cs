using Microsoft.AspNetCore.Mvc;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Utility;

namespace Xanes.Web.Areas.Admin.Controllers;
[Area("Admin")]
public class QuotationTypeController : Controller
{
    private readonly IUnitOfWork _uow;
    private readonly IConfiguration _configuration;
    private readonly int _companyId;

    public QuotationTypeController(IUnitOfWork uow, IConfiguration configuration)
    {
        _uow = uow;
        _configuration = configuration;
        _companyId = _configuration.GetValue<int>("ApplicationSettings:CompanyId");
    }

    public IActionResult Index()
    {
        ViewData[AC.Title] = "Tipos de Transacción";

        var objList = _uow.QuotationType
            .GetAll(x => (x.CompanyId == _companyId)).ToList();
        return View(objList);
    }

    public IActionResult Detail(int? id)
    {
        ViewData[AC.Title] = "Visualizar - Tipo de Transacción";

        if (id == null || id == 0)
        {
            return NotFound();
        }

        var obj = _uow.QuotationType.Get(x => (x.Id == id), isTracking: false);

        if (obj == null)
        {
            return NotFound();
        }

        return View(obj);
    }

}
