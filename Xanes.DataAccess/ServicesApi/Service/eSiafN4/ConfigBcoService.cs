using Microsoft.Extensions.Configuration;
using Xanes.DataAccess.ServicesApi.Interface.eSiafN4;
using Xanes.Models.Dtos.eSiafN4;
using Xanes.Models.Shared;
using Xanes.Utility;

namespace Xanes.DataAccess.ServicesApi.Service.eSiafN4;

public class ConfigBcoService : BaseService, IConfigBcoService
{
    private string _actionUrl;
    private IConfiguration _cfg;
    public string _companyId;
    public ConfigBcoService(IHttpClientFactory httpClient,
        IConfiguration configuration
    ) : base(httpClient, configuration)
    {
        // configuration.GetValue<string>("ServicesUrl:Version")
        _actionUrl = string.Format("{0}configbco"
            , configuration.GetValue<string>("ServicesUrl:UrlApi"));
        _cfg = configuration;
        _companyId = _cfg.GetValue<string>(AC.SecreteSiafN4CompanyUid) ?? string.Empty;
    }

    public Task<T> GetAllAsync<T>(string token, int pageSize, int pageNumber)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = HttpMethod.Get,
            Url = string.Format("{0}?pagina={1}&recordsPorPagina={2}",
                _actionUrl, pageNumber, pageSize),
            Token = token
        });
    }

    public Task<T> GetAsync<T>(string token)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = HttpMethod.Get,
            Url = string.Format("{0}/{1}", _actionUrl, _companyId),
            Token = token
        });
    }
}

