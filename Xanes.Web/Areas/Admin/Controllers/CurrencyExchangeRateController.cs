using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models.ViewModels;
using Xanes.Models;
using Xanes.Utility;
using Microsoft.DotNet.MSIdentity.Shared;
using Newtonsoft.Json;
using System.Text;
using Xanes.Models.Shared;

namespace Xanes.Web.Areas.Admin.Controllers;
[Area("Admin")]
public class CurrencyExchangeRateController : Controller
{
    private readonly IUnitOfWork _uow;
    private readonly IConfiguration _configuration;
    private readonly int _companyId;

    public CurrencyExchangeRateController(IUnitOfWork uow, IConfiguration configuration)
    {
        _uow = uow;
        _configuration = configuration;
        _companyId = _configuration.GetValue<int>("ApplicationSettings:CompanyId");
    }

    public IActionResult Index(SD.CurrencyType currencyType = SD.CurrencyType.Foreign)
    {
        var objList = _uow.CurrencyExchangeRate
            .GetAll(filter: x => (x.CompanyId == _companyId) && (x.CurrencyType == currencyType),
                includeProperties: "CurrencyTrx")
            .OrderBy(x => x.DateTransa)
            .ToList();

        var currencyList = _uow.Currency
            .GetAll(filter: x => (x.CompanyId == _companyId)
                && (x.Numeral != (short)SD.CurrencyType.Base)
                && (x.IsActive))
            .OrderBy(x => x.Numeral)
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString()
                ,
                Text = x.Abbreviation
                ,
                Selected = (x.Numeral == (short)currencyType)
            });

        var objViewModel = new CurrencyExchangeRateIndexVM()
        {
            DataModelList = objList,
            CurrencyList = currencyList,
            CurrencySelected = currencyType
        };

        return View(objViewModel);
    }

    // DETALLE
    public IActionResult Detail(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var obj = _uow.CurrencyExchangeRate
            .Get(filter: x => (x.Id == id), includeProperties: "CurrencyTrx", isTracking: false);

        if (obj == null)
        {
            return NotFound();
        }

        return View(obj);
    }

    // UPSERT
    public IActionResult Upsert(int? id, SD.CurrencyType currencyType)
    {
        CurrencyExchangeRate obj;

        if (id == null || id == 0)
        {
            //Obtener moneda
            var currencyCurrent = _uow.Currency
                .Get(filter: x => (x.CompanyId == _companyId) && (x.Numeral == (short)currencyType));
            if (currencyCurrent == null)
            {
                return NotFound();
            }

            //Setear valor por defecto
            obj = new CurrencyExchangeRate()
            {
                CompanyId = _companyId,
                DateTransa = DateOnly.FromDateTime(DateTime.UtcNow),
                CurrencyType = currencyType,
                CurrencyTrx = currencyCurrent
            };
        }
        else
        {
            obj = _uow.CurrencyExchangeRate
                .Get(x => (x.Id == id)
                    , includeProperties: "CurrencyTrx", isTracking: false);

            if (obj == null)
            {
                return NotFound();
            }

        }

        var currencyList = _uow.Currency
            .GetAll(filter: x => (x.CompanyId == _companyId)
                && (x.Numeral != (short)SD.CurrencyType.Base))
            .OrderBy(x => x.Numeral)
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString()
                ,
                Text = x.Abbreviation
                ,
                Selected = (x.Numeral == (short)obj.CurrencyType)
            });

        var dataVM = new CurrencyExchangeRateVM()
        {
            DataModel = obj,
            CurrencyList = currencyList
        };

        return View(dataVM);
    }

    [HttpPost]
    public async Task<IActionResult> Upsert(CurrencyExchangeRateVM objViewModel)
    {
        CurrencyExchangeRate obj = objViewModel.DataModel;
        if (ModelState.IsValid)
        {
            if (obj.CompanyId != _companyId)
            {
                ModelState.AddModelError("", $"Id de la compañía no puede ser distinto de {_companyId}");
            }

            if (obj.OfficialRate < 0)
            {
                ModelState.AddModelError("", $"TC Oficial no puede ser {obj.OfficialRate}");
            }

            if (obj.BuyRate < 0)
            {
                ModelState.AddModelError("", $"TC Compra no puede ser {obj.BuyRate}");
            }

            if (obj.SellRate < 0)
            {
                ModelState.AddModelError("", $"TC Venta no puede ser {obj.SellRate}");
            }

            // Verificar que exista la moneda
            var objCurrency = _uow.Currency
                .Get(filter: x => (x.Numeral == (short)obj.CurrencyType));
            if (objCurrency == null)
            {
                return NotFound();
            }

            // Asignar id moneda
            obj.CurrencyId = objCurrency.Id;

            // Verificar si ya existe moneda - fecha
            var objExist = await _uow.CurrencyExchangeRate
                .IsExists(filter: x => (x.CompanyId == _companyId)
                                       && (x.DateTransa == obj.DateTransa)
                                       && (x.CurrencyType == obj.CurrencyType)
                                       && (x.Id != obj.Id));
            if (objExist)
            {
                ModelState.AddModelError("", $"Tipo de cambio para {objCurrency.Abbreviation} - {obj.DateTransa} ya existe");
            }


            if (!ModelState.IsValid)
            {
                var currencyList = _uow.Currency
                    .GetAll(filter: x => (x.CompanyId == _companyId)
                        && (x.Numeral != (short)SD.CurrencyType.Base))
                    .OrderBy(x => x.Numeral)
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString()
                        ,
                        Text = x.Abbreviation
                        ,
                        Selected = (x.Numeral == (short)obj.CurrencyType)
                    });

                objViewModel.CurrencyList = currencyList;
                objViewModel.DataModel.CurrencyTrx = objCurrency;
                return View(objViewModel);
            }

            // Creando
            if (obj.Id == 0)
            {
                _uow.CurrencyExchangeRate.Add(obj);
                _uow.Save();
                TempData["success"] = "Exchange Rate created successfully";
            }
            else
            {
                // Verificar que exista
                if (!(await _uow.CurrencyExchangeRate
                        .IsExists(filter: x => x.Id == obj.Id)))
                {
                    return NotFound();
                }

                _uow.CurrencyExchangeRate.Update(obj);
                _uow.Save();
                TempData["success"] = "Exchange Rate updated successfully";
            }

            return RedirectToAction("Index", "CurrencyExchangeRate", new { currencyType = obj.CurrencyType });
        }
        else
        {

            var currencyList = _uow.Currency
                .GetAll(filter: x => (x.CompanyId == _companyId)
                && (x.Numeral != (short)SD.CurrencyType.Base))
                .OrderBy(x => x.Numeral)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString()
                ,
                    Text = x.Abbreviation
                ,
                    Selected = (x.Numeral == (short)obj.CurrencyType)
                });

            objViewModel.CurrencyList = currencyList;
            return View(objViewModel);
        }

    }

    // DELETE
    public IActionResult Delete(int? id)
    {
        if ((id == null) || (id == 0))
        {
            return NotFound();
        }

        var obj = _uow.CurrencyExchangeRate
            .Get(filter: x => (x.Id == id), includeProperties: "CurrencyTrx", isTracking: false);

        if (obj == null)
        {
            return NotFound();
        }

        return View(obj);
    }

    [HttpPost]
    public async Task<JsonResult> GetCurrencyExchangeRate(string date)
    {
        JsonResultResponse? jsonResponse = new();
        DateOnly dateTransa = DateOnly.Parse(date);

        var objList = _uow.CurrencyExchangeRate.GetAll
            (x => (x.CompanyId == _companyId) && (x.DateTransa == dateTransa)
                , includeProperties: "CurrencyTrx").ToList();

        if (objList is null || objList.Count == 0)
        {
            jsonResponse.IsSuccess = false;
            return Json(jsonResponse);
        }

        var currencyForeign = objList
            .FirstOrDefault(t => (t.CurrencyType == SD.CurrencyType.Foreign));
        var currencyAdditional = objList
            .FirstOrDefault(t => (t.CurrencyType == SD.CurrencyType.Additional));

        jsonResponse.IsSuccess = true;
        jsonResponse.Data = new
        {
            CurrencyForeign = currencyForeign,
            CurrencyAdditional = currencyAdditional
        };

        return Json(jsonResponse);
    }


    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePost(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var objCurrency = _uow.CurrencyExchangeRate
            .Get(filter: x => (x.Id == id), includeProperties: "CurrencyTrx", isTracking: false);

        if (objCurrency == null)
        {
            return NotFound();
        }

        var currencyType = objCurrency.CurrencyType;

        if (!_uow.CurrencyExchangeRate
                .RemoveByFilter(filter: x => (x.Id == id)))
        {
            return NotFound();
        }
        TempData["success"] = "Exchange Rate deleted successfully";
        return RedirectToAction("Index", "CurrencyExchangeRate", new { currencyType = currencyType });
    }
}
