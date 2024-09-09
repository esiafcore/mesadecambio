using Microsoft.Extensions.Configuration;
using Xanes.DataAccess.ServicesApi.Interface.XanesN8;
using Xanes.Models.Dtos.XanesN8;
using Xanes.Models.Shared;
using Xanes.Utility;

namespace Xanes.DataAccess.ServicesApi.Service.XanesN8;

public class AuthService : BaseService, IAuthService
{
    private string _actionUrl;
    private IConfiguration _cfg;

    public AuthService(IHttpClientFactory httpClient,
        IConfiguration configuration
    ) : base(httpClient, configuration)
    {
        // configuration.GetValue<string>("ServicesUrl:Version")
        _actionUrl = string.Format("{0}usuarios"
            , configuration.GetValue<string>(AC.ServicesUrlApiPath));
        _cfg = configuration;
    }

    public Task<T> LoginAsync<T>(CredencialesUsuarioDto obj)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = HttpMethod.Post,
            Data = obj,
            Url = $"{_actionUrl}/login"
        });
    }

    public Task<T> RegisterAsync<T>(CredencialesUsuarioDto obj)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = HttpMethod.Post,
            Data = obj,
            Url = $"{_actionUrl}/registrar"
        });
    }
}

