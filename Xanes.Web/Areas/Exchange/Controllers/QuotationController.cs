using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Xanes.DataAccess.Repository.IRepository;
using System.Text.Json;
using Xanes.Models;
using Xanes.Models.Shared;
using Xanes.Models.ViewModels;
using Xanes.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using Xanes.DataAccess.Repository;
using Microsoft.DotNet.MSIdentity.Shared;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Runtime.CompilerServices;

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

    public IActionResult Create()
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

        objData = new Quotation
        {
            DateTransa = DateOnly.FromDateTime(DateTime.UtcNow),
            TypeNumeral = SD.QuotationType.Buy,
            CurrencyTransaType = SD.CurrencyType.Foreign,
            CurrencyOriginExchangeType = SD.CurrencyType.Base,
            CompanyId = _companyId
        };

        model.CurrencyOriginExchangeList = objCurrencyList
            .Where(x => (x.IsActive && (x.Numeral != (int)SD.CurrencyType.Base)))
            .ToList();
        model.CurrencyTransaList = objCurrencyList.Where(x => x.IsActive).ToList();

        model.QuotationTypeList = objTypeList;
        model.CustomerList = objCustomerList.Select(x => new SelectListItem { Text = x.CommercialName, Value = x.Id.ToString() });
        model.DataModel = objData;

        return View(model);
    }

    [HttpPost]
    public IActionResult Create(Models.ViewModels.QuotationCreateVM objViewModel)
    {
        Models.Quotation obj = objViewModel.DataModel;
        //Datos son validos
        if (ModelState.IsValid)
        {
            if (obj.CompanyId != _companyId)
            {
                ModelState.AddModelError("", $"Id de la compañía no puede ser distinto de {_companyId}");
            }

            //Verificamos si existe la moneda de la Transaccion
            var objCurrency = _uow.Currency.Get(filter: x =>
                x.CompanyId == obj.CompanyId && x.Numeral == (int)obj.CurrencyTransaType);

            if (objCurrency == null)
            {
                ModelState.AddModelError("", $"Moneda de la transacción no encontrada");
            }
            else
            {
                obj.CurrencyTransaId = objCurrency.Id;
            }

            //Verificamos si existe la moneda de origen
            objCurrency = _uow.Currency.Get(filter: x =>
                x.CompanyId == obj.CompanyId && x.Numeral == (int)obj.CurrencyOriginExchangeType);

            if (objCurrency == null)
            {
                ModelState.AddModelError("", $"Moneda origen no encontrada");
            }
            else
            {
                obj.CurrencyOriginExchangeId = objCurrency.Id;
            }

            //Verificamos si existe el tipo
            var objQuotationType = _uow.QuotationType.Get(filter: x =>
                x.CompanyId == obj.CompanyId && x.Numeral == (int)obj.TypeNumeral);

            if (objQuotationType == null)
            {
                ModelState.AddModelError("", $"Tipo de transacción no encontrado");
            }
            else
            {
                obj.TypeId = objQuotationType.Id;
            }

            //Verificamos si existe el cliente
            var objCustomer = _uow.Customer.Get(filter: x => x.CompanyId == obj.CompanyId && x.Id == obj.CustomerId);
            if (objCustomer == null)
            {
                ModelState.AddModelError("", $"Cliente no encontrado");
            }

            if (!ModelState.IsValid) return View(objViewModel);

            //Obtenemos el secuencial en borrador
            var numberTransa = _uow.ConfigFac.NextSequentialNumber(filter: x => x.CompanyId == obj.CompanyId,
                SD.TypeSequential.Draft, true);

            obj.Numeral = Convert.ToInt32(numberTransa.Result.ToString());
            obj.InternalSerial = AC.InternalSerialDraft;

            //Seteamos campos de auditoria
            obj.CreatedBy = "LOCALHOSTME";
            obj.CreatedDate = DateTime.Now;
            obj.CreatedHostName = "LOCALHOSTPC";
            obj.CreatedIpv4 = "127.0.0.1";
            obj.IsPosted = false;
            obj.IsClosed = false;
            obj.IsLoan = false;
            obj.IsPayment = false;
            _uow.Quotation.Add(obj);
            _uow.Save();
            TempData["success"] = "Quotation created successfully";
            return RedirectToAction("CreateDetail", "Quotation", new { id = obj.Id });
        }
        else
        {
            StringBuilder errorsMessagesBuilder = new();

            List<string> listErrorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(x => x.ErrorMessage)
                .ToList();
            foreach (var item in listErrorMessages)
            {
                errorsMessagesBuilder.Append(item);
            }

            ModelState.AddModelError("", errorsMessagesBuilder.ToString());
        }

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

        objViewModel.CurrencyOriginExchangeList = objCurrencyList
            .Where(x => (x.IsActive && (x.Numeral != (int)SD.CurrencyType.Base)))
            .ToList();
        objViewModel.CurrencyTransaList = objCurrencyList.Where(x => x.IsActive).ToList();
        objViewModel.QuotationTypeList = objTypeList;
        objViewModel.CustomerList = objCustomerList.Select(x => new SelectListItem { Text = x.CommercialName, Value = x.Id.ToString() });

        return View(objViewModel);
    }

    public async Task<IActionResult> CreateDetail(int id)
    {
        QuotationDetailVM model = new();
        ViewBag.DecimalTransa = JsonSerializer.Serialize(_decimalTransa);

        var objHeader = _uow.Quotation.Get(filter: x => x.CompanyId == _companyId && x.Id == id,
            includeProperties: "TypeTrx,CustomerTrx");
        if (objHeader == null)
        {
            return NotFound();
        }

        var objBankList = _uow.Bank
            .GetAll(x => (x.CompanyId == _companyId))
            .ToList();

        if (objBankList == null)
        {
            return NotFound();
        }

        model.BankList = objBankList.Select(x => new SelectListItem { Text = $"{x.Code} {x.Name}", Value = x.Id.ToString() });
        model.ModelCreateVM.DataModel = objHeader;
        model.CustomerFullName = $"{objHeader.CustomerTrx.FirstName} {objHeader.CustomerTrx.SecondName} {objHeader.CustomerTrx.LastName} {objHeader.CustomerTrx.SecondSurname}";
        model.NumberTransa = $"COT-{objHeader.TypeTrx.Code}-{objHeader.Numeral}";
        return View(model);
    }

    [HttpPost]
    public IActionResult CreateDetail(Models.ViewModels.QuotationDetailVM objViewModel)
    {
        Models.QuotationDetail obj = objViewModel.DataModel;
        //Datos son validos
        if (ModelState.IsValid)
        {
            if (obj.CompanyId != _companyId)
            {
                ModelState.AddModelError("", $"Id de la compañía no puede ser distinto de {_companyId}");
            }

            ////Verificamos si existe la moneda de la Transaccion
            //var objCurrency = _uow.Currency.Get(filter: x =>
            //    x.CompanyId == obj.CompanyId && x.Numeral == (int)obj.CurrencyTransaType);

            //if (objCurrency == null)
            //{
            //    ModelState.AddModelError("", $"Moneda de la transacción no encontrada");
            //}
            //else
            //{
            //    obj.CurrencyTransaId = objCurrency.Id;
            //}

            ////Verificamos si existe la moneda de origen
            //objCurrency = _uow.Currency.Get(filter: x =>
            //    x.CompanyId == obj.CompanyId && x.Numeral == (int)obj.CurrencyOriginExchangeType);

            //if (objCurrency == null)
            //{
            //    ModelState.AddModelError("", $"Moneda origen no encontrada");
            //}
            //else
            //{
            //    obj.CurrencyOriginExchangeId = objCurrency.Id;
            //}

            ////Verificamos si existe el tipo
            //var objQuotationType = _uow.QuotationType.Get(filter: x =>
            //    x.CompanyId == obj.CompanyId && x.Numeral == (int)obj.TypeNumeral);

            //if (objQuotationType == null)
            //{
            //    ModelState.AddModelError("", $"Tipo de transacción no encontrado");
            //}
            //else
            //{
            //    obj.TypeId = objQuotationType.Id;
            //}

            ////Verificamos si existe el cliente
            //var objCustomer = _uow.Customer.Get(filter: x => x.CompanyId == obj.CompanyId && x.Id == obj.CustomerId);
            //if (objCustomer == null)
            //{
            //    ModelState.AddModelError("", $"Cliente no encontrado");
            //}

            if (!ModelState.IsValid) return View(objViewModel);

            //Obtenemos el secuencial en borrador
            var numberTransa = _uow.ConfigFac.NextSequentialNumber(filter: x => x.CompanyId == obj.CompanyId,
                SD.TypeSequential.Draft, true);

            //obj.Numeral = Convert.ToInt32(numberTransa.Result.ToString());
            //obj.InternalSerial = AC.InternalSerialDraft;

            ////Seteamos campos de auditoria
            //obj.CreatedBy = "LOCALHOSTME";
            //obj.CreatedDate = DateTime.Now;
            //obj.CreatedHostName = "LOCALHOSTPC";
            //obj.CreatedIpv4 = "127.0.0.1";
            //obj.IsPosted = false;
            //obj.IsClosed = false;
            //obj.IsLoan = false;
            //obj.IsPayment = false;
            //_uow.Quotation.Add(obj);
            _uow.Save();
            //TempData["success"] = "Quotation created successfully";
            //return RedirectToAction("CreateDetail", "Quotation", new { id = obj.Id });
        }
        else
        {
            StringBuilder errorsMessagesBuilder = new();

            List<string> listErrorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(x => x.ErrorMessage)
                .ToList();
            foreach (var item in listErrorMessages)
            {
                errorsMessagesBuilder.Append(item);
            }

            ModelState.AddModelError("", errorsMessagesBuilder.ToString());
        }

        return View(objViewModel);
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

    public JsonResult GetAllDepositByParent(int parentId)
    {
        JsonResultResponse? jsonResponse = new();

        var objList = _uow.QuotationDetail
            .GetAll(x => (x.CompanyId == _companyId &&
                          x.ParentId == parentId &&
                          x.QuotationDetailType == SD.QuotationDetailType.Deposit)
                , includeProperties: "ParentTrx,CurrencyDetailTrx,BankSourceTrx,BankTargetTrx").ToList();
        if (objList == null)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = "Error al cargar los datos";
            return Json(jsonResponse);
        }

        jsonResponse.IsSuccess = true;
        jsonResponse.Data = objList;
        return Json(jsonResponse);
    }

    public JsonResult GetAllTransferByParent(int parentId)
    {
        JsonResultResponse? jsonResponse = new();

        var objList = _uow.QuotationDetail
            .GetAll(x => (x.CompanyId == _companyId &&
                          x.ParentId == parentId &&
                          x.QuotationDetailType == SD.QuotationDetailType.Transfer)
                , includeProperties: "ParentTrx,CurrencyDetailTrx,BankSourceTrx,BankTargetTrx").ToList();
        if (objList == null)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = "Error al cargar los datos";
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
