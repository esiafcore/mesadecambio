using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Xanes.DataAccess.Repository.IRepository;
using System.Text.Json;
using Xanes.Models;
using Xanes.Models.Shared;
using Xanes.Models.ViewModels;
using Xanes.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;

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
        ViewBag.DecimalTransa = JsonSerializer.Serialize(_decimalTransa);
        ViewBag.DecimalExchange = JsonSerializer.Serialize(_decimalExchange);
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
            CurrencyDepositType = SD.CurrencyType.Base,
            CompanyId = _companyId
        };

        model.CurrencyTransaList = objCurrencyList
            .Where(x => (x.IsActive && (x.Numeral != (int)SD.CurrencyType.Base)))
            .ToList();
        model.CurrencyDepositList = objCurrencyList.Where(x => x.IsActive).ToList();
        model.CurrencyTransferList = objCurrencyList.Where(x => x.IsActive).ToList();
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

            //Verificamos si existe la moneda de deposito
            objCurrency = _uow.Currency.Get(filter: x =>
                x.CompanyId == obj.CompanyId && x.Numeral == (int)obj.CurrencyDepositType);

            if (objCurrency == null)
            {
                ModelState.AddModelError("", $"Moneda de deposito no encontrada");
            }
            else
            {
                obj.CurrencyDepositId = objCurrency.Id;
            }

            //Verificamos si existe la moneda de transferencia
            objCurrency = _uow.Currency.Get(filter: x =>
                x.CompanyId == obj.CompanyId && x.Numeral == (int)obj.CurrencyTransferType);

            if (objCurrency == null)
            {
                ModelState.AddModelError("", $"Moneda de transferencia no encontrada");
            }
            else
            {
                obj.CurrencyTransferId = objCurrency.Id;
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

            if (obj.TypeNumeral == SD.QuotationType.Buy)
            {
                if (obj.ExchangeRateBuyTransa < obj.ExchangeRateOfficialTransa)
                {
                    obj.AmountRevenue = (obj.ExchangeRateOfficialTransa - obj.ExchangeRateBuyTransa) * obj.AmountTransaction;
                    obj.AmountCost = 0;
                }
                else
                {
                    obj.AmountCost = (obj.ExchangeRateBuyTransa - obj.ExchangeRateOfficialTransa) * obj.AmountTransaction;
                    obj.AmountRevenue = 0;
                }
            }else if (obj.TypeNumeral == SD.QuotationType.Sell)
            {
                if (obj.ExchangeRateSellTransa < obj.ExchangeRateOfficialTransa)
                {
                    obj.AmountCost = (obj.ExchangeRateOfficialTransa - obj.ExchangeRateSellTransa) * obj.AmountTransaction;
                    obj.AmountRevenue = 0;
                }
                else
                {
                    obj.AmountRevenue = (obj.ExchangeRateSellTransa - obj.ExchangeRateOfficialTransa) * obj.AmountTransaction;
                    obj.AmountCost = 0;
                }
            }


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
            TempData["success"] = "Cotización creada exitosamente";
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

        objViewModel.CurrencyTransaList = objCurrencyList
            .Where(x => (x.IsActive && (x.Numeral != (int)SD.CurrencyType.Base)))
            .ToList();
        objViewModel.CurrencyDepositList = objCurrencyList.Where(x => x.IsActive).ToList();
        objViewModel.CurrencyTransferList = objCurrencyList.Where(x => x.IsActive).ToList();
        objViewModel.QuotationTypeList = objTypeList;
        objViewModel.CustomerList = objCustomerList.Select(x => new SelectListItem { Text = x.CommercialName, Value = x.Id.ToString() });

        return View(objViewModel);
    }

    [HttpPost]
    public IActionResult Update(Models.ViewModels.QuotationCreateVM objViewModel)
    {
        Models.Quotation obj = objViewModel.DataModel;
        //Datos son validos
        if (ModelState.IsValid)
        {
            if (obj.CompanyId != _companyId)
            {
                ModelState.AddModelError("", $"Id de la compañía no puede ser distinto de {_companyId}");
            }

            //Verificamos si existe el cliente
            var objCustomer = _uow.Customer.Get(filter: x => x.CompanyId == obj.CompanyId && x.Id == obj.CustomerId);
            if (objCustomer == null)
            {
                ModelState.AddModelError("", $"Cliente no encontrado");
            }

            if (!ModelState.IsValid) return RedirectToAction("CreateDetail", "Quotation", new { id = obj.Id });

            if (obj.TypeNumeral == SD.QuotationType.Buy)
            {
                if (obj.ExchangeRateBuyTransa < obj.ExchangeRateOfficialTransa)
                {
                    obj.AmountRevenue = (obj.ExchangeRateOfficialTransa - obj.ExchangeRateBuyTransa) * obj.AmountTransaction;
                    obj.AmountCost = 0;
                }
                else
                {
                    obj.AmountCost = (obj.ExchangeRateBuyTransa - obj.ExchangeRateOfficialTransa) * obj.AmountTransaction;
                    obj.AmountRevenue = 0;
                }
            }
            else if (obj.TypeNumeral == SD.QuotationType.Sell)
            {
                if (obj.ExchangeRateSellTransa < obj.ExchangeRateOfficialTransa)
                {
                    obj.AmountCost = (obj.ExchangeRateOfficialTransa - obj.ExchangeRateSellTransa) * obj.AmountTransaction;
                    obj.AmountRevenue = 0;
                }
                else
                {
                    obj.AmountRevenue = (obj.ExchangeRateSellTransa - obj.ExchangeRateOfficialTransa) * obj.AmountTransaction;
                    obj.AmountCost = 0;
                }
            }

            //Seteamos campos de auditoria
            obj.UpdatedBy = "LOCALHOSTME";
            obj.UpdatedDate = DateTime.Now;
            obj.UpdatedHostName = "LOCALHOSTPC";
            obj.UpdatedIpv4 = "127.0.0.1";
            obj.IsPosted = false;
            obj.IsClosed = false;
            obj.IsLoan = false;
            obj.IsPayment = false;
            _uow.Quotation.Update(obj);
            _uow.Save();
            TempData["success"] = "Cotización actualizada exitosamente";
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

        return RedirectToAction("CreateDetail", "Quotation", new { id = obj.Id });
    }

    public IActionResult CreateDetail(int id)
    {
        QuotationDetailVM model = new();
        ViewBag.DecimalTransa = JsonSerializer.Serialize(_decimalTransa);
        ViewBag.DecimalExchange = JsonSerializer.Serialize(_decimalExchange);

        var objHeader = _uow.Quotation.Get(filter: x => x.CompanyId == _companyId && x.Id == id,
            includeProperties: "TypeTrx,CustomerTrx", isTracking: false);
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
        model.ModelCreateVM.CurrencyTransaList = objCurrencyList
            .Where(x => (x.IsActive && (x.Numeral != (int)SD.CurrencyType.Base)))
            .ToList();
        model.ModelCreateVM.CurrencyDepositList = objCurrencyList.Where(x => x.IsActive).ToList();
        model.ModelCreateVM.CurrencyTransferList = objCurrencyList.Where(x => x.IsActive).ToList();
        model.ModelCreateVM.QuotationTypeList = objTypeList;
        model.ModelCreateVM.CustomerList = objCustomerList.Select(x => new SelectListItem { Text = x.CommercialName, Value = x.Id.ToString() });

        //model.BankList = objBankList.Select(x => new SelectListItem { Text = $"{x.Code}", Value = x.Id.ToString() });
        model.BankList = objBankList;
        model.ModelCreateVM.DataModel = objHeader;
        model.CustomerFullName = $"{objHeader.CustomerTrx.CommercialName}";
        model.NumberTransa = $"COT-{objHeader.TypeTrx.Code}-{objHeader.Numeral}";
        model.DataModel = new();
        return View(model);
    }

    [HttpPost]
    public IActionResult CreateDetail(Models.ViewModels.QuotationDetailVM objViewModel)
    {
        Models.QuotationDetail obj = objViewModel.DataModel;
        //Datos son validos
        //if (ModelState.IsValid)
        //{
        if (obj.CompanyId != _companyId)
        {
            ModelState.AddModelError("", $"Id de la compañía no puede ser distinto de {_companyId}");
        }

        //Verificamos si existe el padre
        var objHeader = _uow.Quotation.Get(filter: x =>
            x.CompanyId == obj.CompanyId && x.Id == obj.ParentId, isTracking: false);

        if (objHeader == null)
        {
            ModelState.AddModelError("", $"Registro padre no encontrada");
        }

        //Obtenemos los hijos
        var objDetails = _uow.QuotationDetail.GetAll(filter: x =>
            x.CompanyId == obj.CompanyId && x.ParentId == objHeader.Id && x.QuotationDetailType == objViewModel.DataModel.QuotationDetailType
            , includeProperties: "ParentTrx,CurrencyDetailTrx,BankSourceTrx,BankTargetTrx");

        if (objDetails == null)
        {
            ModelState.AddModelError("", $"Registros hijos son invalidos");
        }

        //Verificamos si existe la moneda
        var objCurrency = _uow.Currency.Get(filter: x =>
            x.CompanyId == obj.CompanyId && x.Numeral == (int)obj.CurrencyDetailId, isTracking: false);

        if (objCurrency == null)
        {
            ModelState.AddModelError("", $"Moneda no encontrada");
        }

        //Verificamos si existe el banco origen
        var objBankSource = _uow.Bank.Get(filter: x =>
            x.CompanyId == obj.CompanyId && x.Id == obj.BankSourceId, isTracking: false);

        if (objBankSource == null)
        {
            ModelState.AddModelError("", $"Banco Origen no encontrado");
        }

        //Verificamos si tiene banco destino y verificamos si existe
        if (obj.BankTargetId != 0)
        {
            var objBankTarget = _uow.Bank.Get(filter: x =>
                x.CompanyId == obj.CompanyId && x.Id == obj.BankTargetId, isTracking: false);

            if (objBankTarget == null)
            {
                ModelState.AddModelError("", $"Banco Destino no encontrado");
            }
        }
        else
        {
            obj.BankTargetId = obj.BankSourceId;
        }

        if (!ModelState.IsValid)
        {
            var objHeaderQt = _uow.Quotation.Get(filter: x => x.CompanyId == _companyId && x.Id == obj.ParentId,
                includeProperties: "TypeTrx,CustomerTrx", isTracking: false);
            if (objHeaderQt == null)
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

            objViewModel.BankList = objBankList;
            objViewModel.ModelCreateVM.DataModel = objHeaderQt;
            objViewModel.CustomerFullName = $"{objHeaderQt.CustomerTrx.CommercialName}";
            objViewModel.NumberTransa = $"COT-{objHeaderQt.TypeTrx.Code}-{objHeaderQt.Numeral}";
            ViewBag.DecimalTransa = JsonSerializer.Serialize(_decimalTransa);
            return View(objViewModel);
        }


        if (obj.Id == 0)
        {
            //Seteamos campos de auditoria
            obj.LineNumber = objDetails.Count() + 1;
            obj.CreatedBy = "LOCALHOSTME";
            obj.CreatedDate = DateTime.Now;
            obj.CreatedHostName = "LOCALHOSTPC";
            obj.CreatedIpv4 = "127.0.0.1";
            _uow.QuotationDetail.Add(obj);
            _uow.Save();
            TempData["success"] = "Cotización creada exitosamente";
        }
        else
        {
            var objDetail = objDetails.First(x => x.Id == obj.Id);

            //Seteamos campos de auditoria
            objDetail.AmountDetail = obj.AmountDetail;
            objDetail.BankSourceId = obj.BankSourceId;
            objDetail.BankTargetId = obj.BankTargetId;
            objDetail.QuotationDetailType = obj.QuotationDetailType;
            objDetail.UpdatedBy = "LOCALHOSTME";
            objDetail.UpdatedDate = DateTime.Now;
            objDetail.UpdatedHostName = "LOCALHOSTPC";
            objDetail.UpdatedIpv4 = "127.0.0.1";
            _uow.QuotationDetail.Update(objDetail);
            _uow.Save();
            TempData["success"] = "Cotización actualizada exitosamente";
        }

        return RedirectToAction("CreateDetail", "Quotation", new { id = obj.ParentId });
        //}
        //else
        //{
        //    StringBuilder errorsMessagesBuilder = new();

        //    List<string> listErrorMessages = ModelState.Values
        //        .SelectMany(v => v.Errors)
        //        .Select(x => x.ErrorMessage)
        //        .ToList();
        //    foreach (var item in listErrorMessages)
        //    {
        //        errorsMessagesBuilder.Append(item);
        //    }

        //    ModelState.AddModelError("", errorsMessagesBuilder.ToString());
        //}


    }

    public IActionResult Delete(int id)
    {
        QuotationDetailVM model = new();
        ViewBag.DecimalTransa = JsonSerializer.Serialize(_decimalTransa);

        var objHeader = _uow.Quotation.Get(filter: x => x.CompanyId == _companyId && x.Id == id,
            includeProperties: "TypeTrx,CustomerTrx", isTracking: false);
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

        model.BankList = objBankList;
        model.ModelCreateVM.DataModel = objHeader;
        model.CustomerFullName = $"{objHeader.CustomerTrx.CommercialName}";
        model.NumberTransa = $"COT-{objHeader.TypeTrx.Code}-{objHeader.Numeral}";
        return View(model);
    }

    public IActionResult Detail(int id)
    {
        QuotationDetailVM model = new();
        ViewBag.DecimalTransa = JsonSerializer.Serialize(_decimalTransa);

        var objHeader = _uow.Quotation.Get(filter: x => x.CompanyId == _companyId && x.Id == id,
            includeProperties: "TypeTrx,CustomerTrx", isTracking: false);
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

        model.BankList = objBankList;
        model.ModelCreateVM.DataModel = objHeader;
        model.CustomerFullName = $"{objHeader.CustomerTrx.CommercialName}";
        model.NumberTransa = $"COT-{objHeader.TypeTrx.Code}-{objHeader.Numeral}";
        return View(model);
    }

    #region API_CALL
    public JsonResult GetAll()
    {
        JsonResultResponse? jsonResponse = new();

        var objList = _uow.Quotation
            .GetAll(x => (x.CompanyId == _companyId)
            , includeProperties: "TypeTrx,CustomerTrx,CurrencyTransaTrx,CurrencyTransferTrx,CurrencyDepositTrx").ToList();
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

    [HttpPost, ActionName("Delete")]
    public async Task<JsonResult> DeletePost(int id)
    {
        JsonResultResponse? jsonResponse = new();
        StringBuilder errorsMessagesBuilder = new();
        if (id == 0)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = $"El id es requerido";
            return Json(jsonResponse);
        }
        try
        {
            var objHeader = _uow.Quotation
                .Get(filter: x => x.CompanyId == _companyId && x.Id == id, isTracking: false);
            if (objHeader == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Cotización no encontrada";
                return Json(jsonResponse);
            }


            if (!(await _uow.Quotation.RemoveWithChildren(objHeader.Id)))
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Cotización no encontrada";
                return Json(jsonResponse);
            }
            jsonResponse.IsSuccess = true;
            TempData["success"] = $"Cotización eliminada correctamente";
            jsonResponse.UrlRedirect = Url.Action(action: "Index", controller: "Quotation");

            return Json(jsonResponse);
        }
        catch (Exception ex)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = ex.Message.ToString();
            return Json(jsonResponse);
        }
    }

    [HttpPost, ActionName("DeleteDetail")]
    public JsonResult DeleteDetailPost(int id)
    {
        JsonResultResponse? jsonResponse = new();
        StringBuilder errorsMessagesBuilder = new();
        if (id == 0)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = $"El id es requerido";
            return Json(jsonResponse);
        }
        try
        {
            var objDetail = _uow.QuotationDetail
                .Get(filter: x => x.CompanyId == _companyId && x.Id == id, isTracking: false);
            if (objDetail == null)
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Detalle de cotización no encontrado";
                return Json(jsonResponse);
            }

            _uow.QuotationDetail.Remove(objDetail);
            _uow.Save();
            jsonResponse.IsSuccess = true;
            jsonResponse.SuccessMessages = $"Detalle eliminado correctamente";
            return Json(jsonResponse);
        }
        catch (Exception ex)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = ex.Message.ToString();
            return Json(jsonResponse);
        }
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
