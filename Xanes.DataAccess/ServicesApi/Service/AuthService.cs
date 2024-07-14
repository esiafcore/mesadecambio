using Microsoft.Extensions.Configuration;
using Xanes.DataAccess.ServicesApi.Interface;
using Xanes.Models.Dtos;
using Xanes.Models.Shared;
using Xanes.Utility;

namespace Xanes.DataAccess.ServicesApi.Service;

public class AuthService : BaseService, IAuthService
{
    private string _actionUrl;
    private IConfiguration _configuration;

    public AuthService(IHttpClientFactory httpClient,
        IConfiguration configuration
    ) : base(httpClient, configuration)
    {
        // configuration.GetValue<string>("ServicesUrl:Version")
        _actionUrl = string.Format("{0}usuarios"
            , configuration.GetValue<string>(AC.ServicesUrlApiPath));
        _configuration = configuration;
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

