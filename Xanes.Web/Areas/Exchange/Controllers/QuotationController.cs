using Microsoft.AspNetCore.Mvc;
using Xanes.DataAccess.Repository.IRepository;
using System.Text.Json;

namespace Xanes.Web.Areas.Exchange.Controllers;

[Area("Exchange")]
public class QuotationController : Controller
{
    private readonly IUnitOfWork _uow;
    private readonly IConfiguration _configuration;
    private readonly int _companyId;
    private readonly int _decimalTransa;
    private readonly int _decimalExchange;

    public QuotationController(IUnitOfWork uow, IConfiguration configuration)
    {
        _uow = uow;
        _configuration = configuration;
        _companyId = _configuration.GetValue<int>("ApplicationSettings:CompanyId");
        _decimalTransa = _configuration.GetValue<int>("ApplicationSettings:DecimalTransa");
        _decimalExchange = _configuration.GetValue<int>("ApplicationSettings:DecimalExchange");

    }
    public IActionResult Index()
    {
        ViewBag.DecimalTransa = JsonSerializer.Serialize(_decimalTransa);
        ViewBag.DecimalExchange = JsonSerializer.Serialize(_decimalExchange);
        return View();
    }

    public IActionResult Upsert(int? id)
    {
        return View();
    }

    #region API CALLS
    public IActionResult GetAll()
    {
        var objList = _uow.Quotation
            .GetAll(x => (x.CompanyId == _companyId)
            ,includeProperties: "TypeTrx,CustomerTrx,CurrencyOriginExchangeTrx,CurrencyTransaTrx").ToList();
        return Json(new { data = objList });
    }

    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        var rowToBeDeleted = _uow.Quotation.Get(filter: u => (u.Id == id)
            , isTracking: false);

        if (rowToBeDeleted == null)
        {
            return Json(new { success = false, message = "Quotation don´t exist" });
        }

        _uow.Bank.RemoveByFilter(filter: u => (u.Id == rowToBeDeleted.Id));

        return Json(new
        {
            isSuccess = true
            ,
            successMessages = "Quotation Successfully Removed"
            ,
            errorMessages = string.Empty
        });
    }

    #endregion
}
