using Microsoft.AspNetCore.Mvc;
using Xanes.DataAccess.Repository.IRepository;
using System.Text.Json;
using Xanes.Models;
using Xanes.Models.Shared;
using Xanes.Models.ViewModels;
using Xanes.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using Xanes.DataAccess.Repository;

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
        QuotationCreateVM model = new();
        Quotation objData = new();

        var objCurrencyList = _uow.Currency
            .GetAll(x => (x.CompanyId == _companyId))
            .ToList();

        if (objCurrencyList == null)
        {
            return NotFound();
        }

        var objTypeList = _uow.QuotationType
            .GetAll(x => (x.CompanyId == _companyId))
            .ToList();

        if (objTypeList == null)
        {
            return NotFound();
        }

        var objCustomerList = _uow.Customer
            .GetAll(x => (x.CompanyId == _companyId))
            .ToList();

        if (objTypeList == null)
        {
            return NotFound();
        }

        if (id == null || id == 0)
        {
            objData = new Quotation
            {
                DateTransa = DateOnly.FromDateTime(DateTime.UtcNow),
                TypeNumeral = SD.QuotationType.Buy,
                CurrencyTransaType = SD.CurrencyType.Foreign,
                CurrencyOriginExchangeType = SD.CurrencyType.Base

            };

        }
        else
        {
            objData = _uow.Quotation
                .Get(x => (x.Id == id)
                    , isTracking: false);
            //, includeProperties: "CurrencyTrx"

            if (objData == null)
            {
                return NotFound();
            }
        }


        model.CurrencyOriginExchangeList = objCurrencyList.Where(x => x.IsActive).ToList();
        model.CurrencyTransaList = objCurrencyList
                .Where(x => (x.IsActive && (x.Numeral != (int)SD.CurrencyType.Base)))
                .ToList();

        model.QuotationTypeList = objTypeList;
        model.CustomerList = objCustomerList.Select(x => new SelectListItem { Text = x.CommercialName, Value = x.Id.ToString() });
        model.DataModel = objData;

        return View(model);
    }

    #region API_CALL
    public JsonResult GetAll()
    {
        JsonResultResponse? jsonResponse = new();

        var objList = _uow.Quotation
            .GetAll(x => (x.CompanyId == _companyId)
            , includeProperties: "TypeTrx,CustomerTrx,CurrencyOriginExchangeTrx,CurrencyTransaTrx").ToList();
        if (objList.Count <= 0)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = "No hay registros que mostrar";
            return Json(jsonResponse);
        }

        jsonResponse.IsSuccess = true;
        jsonResponse.Data = objList;
        return Json(jsonResponse);
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
