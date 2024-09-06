using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.DataAccess.ServicesApi.Interface;
using Xanes.LoggerService;
using Xanes.Models.Dtos;
using Xanes.Models.Shared;
using Xanes.Utility;
using Xanes.Models.Dtos.XanesN8;

namespace Xanes.Web.Areas.Auth.Controllers;
[Area("Auth")]
public class AuthController : Controller
{
    private readonly IAuthService _srv;
    private readonly ILoggerManager _logger;
    private readonly IConfiguration _cfg;
    private readonly int _companyId;
    private readonly IUnitOfWork _uow;

    public AuthController(IAuthService service
        , ILoggerManager logger
        , IConfiguration cfg
        , IUnitOfWork uow)
    {
        _logger = logger;
        _srv = service;
        _cfg = cfg;
        _uow = uow;
        _companyId = _cfg.GetValue<int>("ApplicationSettings:CompanyId");
    }

    [HttpGet]
    public IActionResult Login()
    {
        ViewData[AC.Title] = "Login";
        CredencialesUsuarioDto obj = new();
        return View(obj);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(CredencialesUsuarioDto obj)
    {
        var errorsMessagesBuilder = new StringBuilder();
        ViewData[AC.Title] = "Login";
        try
        {
            var apiResponse = await _srv.LoginAsync<APIResponse>(obj);
            if (apiResponse is null)
            {
                TempData[AC.Error] = "No se pudo inciar sesion";
                return RedirectToAction("Index", "Home", new { Area = "exchange" });
            }

            if (!apiResponse.isSuccess)
            {
                errorsMessagesBuilder.AppendJoin("", apiResponse.errorMessages);
                TempData[AC.Error] = $"{errorsMessagesBuilder}";
                return RedirectToAction("Login", "Auth", new { Area = "auth" });
            }

            var model = JsonConvert.DeserializeObject<RespuestaAutenticacionDto>(Convert.ToString(apiResponse.result))!;
            if (model != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(model.Token);

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                if (jwtToken != null)
                {
                    //Leer Roles
                    var rolesClaims = jwtToken.Claims.Where(u => u.Type == "role").ToList();
                    if (rolesClaims.Count > 0)
                    {
                        identity.AddClaims(rolesClaims
                            .Select(x => new Claim(ClaimTypes.Role, x.Value)));
                    }
                    //Leer usuario
                    identity.AddClaim(new Claim(ClaimTypes.Email, jwtToken.Claims.FirstOrDefault(u => u.Type == "email")?.Value));
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                }

                //Grabar el TOKEN en la sessión
                HttpContext.Session.SetString(SD.SessionToken, model.Token);

                return RedirectToAction("Index", "Quotation", new { Area = "exchange" });
            }
            else
            {
                return View(obj);
            }
        }
        catch (Exception e)
        {
            TempData[AC.Error] = e.Message;
            return RedirectToAction("Login", "Auth", new { Area = "auth" });
        }
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(CredencialesUsuarioDto obj)
    {
        try
        {
            var apiResponse = await _srv.RegisterAsync<APIResponse>(obj);

            var model = JsonConvert.DeserializeObject<RespuestaAutenticacionDto>(Convert.ToString(apiResponse.result))!;

            if (model != null)
            {
                TempData[AC.Success] = "Usuario creado exitosamente";
                return RedirectToAction("Login");
            }
            else
            {
                TempData[AC.Error] = "Error al crear el usuario";
                return RedirectToAction("Register");
            }
        }
        catch (Exception e)
        {
            TempData[AC.Error] = e.Message;
            return RedirectToAction("Login", "Auth", new { Area = "auth" });
        }
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        //Limpiar el TOKEN de la sesión
        HttpContext.Session.SetString(SD.SessionToken, "");
        HttpContext.Session.Remove(AC.ProcessingDate);
        //HttpContext.Session.SetString(AC.UsernameLogged, "");

        return RedirectToAction("Login", "Auth", new { Area = "auth" });
    }
}

