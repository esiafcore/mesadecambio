using Microsoft.AspNetCore.Mvc;
using Stimulsoft.Svg.ExCSS;
using System.Text;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models.Shared;
using Xanes.Utility;

namespace Xanes.Web.Areas.Exchange.Controllers;
[Area("Exchange")]
public class BusinessExecutiveController : Controller
{
    private readonly IUnitOfWork _uow;
    private readonly IConfiguration _configuration;
    private readonly int _companyId;

    public BusinessExecutiveController(IUnitOfWork uow, IConfiguration configuration)
    {
        _uow = uow;
        _configuration = configuration;
        _companyId = _configuration.GetValue<int>("ApplicationSettings:CompanyId");
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Upsert(int? id)
    {
        Models.BusinessExecutive obj;

        if (id == null || id == 0)
        {
            //create
            //Setear valor por defecto
            obj = new Models.BusinessExecutive()
            {
                Id = 0,
                CompanyId = _companyId,
                FirstName = string.Empty,
                SecondName = string.Empty,
                LastName = string.Empty,
                SecondSurname = string.Empty,
                Code = string.Empty,
                IsActive = true
            };
        }
        else
        {
            //update
            obj = _uow.BusinessExecutive
                .Get(filter: x => (x.Id == id), isTracking: false);

            if (obj == null)
            {
                return NotFound();
            }
        }

        var dataVM = new Models.ViewModels.BusinessExecutiveVM()
        {
            DataModel = obj
        };

        return View(dataVM);
    }

    [HttpPost]
    public async Task<IActionResult> Upsert(Models.ViewModels.BusinessExecutiveVM objViewModel)
    {
        Models.BusinessExecutive obj = objViewModel.DataModel;

        //Datos son validos
        if (ModelState.IsValid)
        {
            if (obj.CompanyId != _companyId)
            {
                ModelState.AddModelError("", $"Id de la compañía no puede ser distinto de {_companyId}");
            }

            if (obj.Code.Trim().ToLower() == ".")
            {
                ModelState.AddModelError("code", "Código no puede ser .");
            }

            if (!ModelState.IsValid) return View(objViewModel);


            //Creando
            if (obj.Id == 0)
            {
                //Validar si codigo no existe
                bool isExist = await _uow
                    .BusinessExecutive.IsExists(x => (x.CompanyId == obj.CompanyId)
                                            && (x.Code.Trim() == obj.Code.Trim()));
                if (isExist)
                {
                    ModelState.AddModelError("code", $"Código {obj.Code} ya existe");
                }


                if (ModelState.IsValid)
                {
                    //Seteamos campos de auditoria
                    obj.CreatedBy = AC.LOCALHOSTME;
                    obj.CreatedDate = DateTime.UtcNow;
                    obj.CreatedHostName = AC.LOCALHOSTPC;
                    obj.CreatedIpv4 = AC.Ipv4Default;
                    obj.IsActive = true;
                    _uow.BusinessExecutive.Add(obj);
                    _uow.Save();
                    TempData["success"] = "Ejecutivo creado exitosamente";
                    return RedirectToAction("Index", "BusinessExecutive");
                }
            }
            else
            {
                //Validar que codigo no está repetido
                var objExists = _uow.Customer
                    .Get(filter: x => (x.CompanyId == _companyId)
                                      & (x.Code.Trim().ToLower() == obj.Code.Trim().ToLower()), isTracking: false);

                if (objExists != null && objExists.Id != obj.Id)
                {
                    ModelState.AddModelError("code", $"Código {obj.Code} ya existe");
                }

                //Datos son validos
                if (ModelState.IsValid)
                {
                    obj.UpdatedBy = AC.LOCALHOSTME;
                    obj.UpdatedDate = DateTime.UtcNow;
                    obj.UpdatedHostName = AC.LOCALHOSTPC;
                    obj.UpdatedIpv4 = AC.Ipv4Default;
                    _uow.BusinessExecutive.Update(obj);
                    _uow.Save();
                    TempData["success"] = "Ejecutivo actualizado exitosamente";
                    return RedirectToAction("Index", "BusinessExecutive");
                }

                return View(objViewModel);
            }
        }

        return View(objViewModel);
    }

    [HttpGet]
    public IActionResult Detail(int id)
    {
        var obj = _uow.BusinessExecutive
            .Get(filter: x => (x.Id == id), isTracking: false);

        if (obj == null)
        {
            return NotFound();
        }

        var dataVM = new Models.ViewModels.BusinessExecutiveVM()
        {
            DataModel = obj
        };

        return View(dataVM);
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var obj = _uow.BusinessExecutive
            .Get(filter: x => (x.Id == id), isTracking: false);

        if (obj == null)
        {
            return NotFound();
        }

        var dataVM = new Models.ViewModels.BusinessExecutiveVM()
        {
            DataModel = obj
        };

        return View(dataVM);
    }


    #region API_CALL

    public JsonResult GetAll()
    {
        JsonResultResponse? jsonResponse = new();
        var objList = _uow.BusinessExecutive
            .GetAll(x => (x.CompanyId == _companyId)).ToList();

        if (objList == null)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = "Error al cargar los datos";
            return Json(jsonResponse);
        }

        if (objList.Count <= 0)
        {
            jsonResponse.IsInfo = true;
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = "No hay registros que mostrar";
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
            if (!await _uow.BusinessExecutive.IsExists(filter: x => x.Id == id))
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = $"Ejecutivo no encontrado";
                return Json(jsonResponse);
            }

            if (!_uow.BusinessExecutive.RemoveByFilter(filter: x => x.Id == id))
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.ErrorMessages = "Error Desconocido";
                return Json(jsonResponse);
            }

            jsonResponse.IsSuccess = true;
            TempData["success"] = $"Ejecutivo eliminado correctamente";
            jsonResponse.UrlRedirect = Url.Action(action: "Index", controller: "BusinessExecutive");

            return Json(jsonResponse);
        }
        catch (Exception ex)
        {
            jsonResponse.IsSuccess = false;
            jsonResponse.ErrorMessages = ex.Message.ToString();
            return Json(jsonResponse);
        }
    }
    #endregion
}
