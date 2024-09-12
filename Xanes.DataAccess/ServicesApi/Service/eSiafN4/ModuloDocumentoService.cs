using Microsoft.Extensions.Configuration;
using Xanes.DataAccess.ServicesApi.Interface.eSiafN4;
using Xanes.Models.Dtos.eSiafN4;
using Xanes.Models.Shared;
using Xanes.Utility;

namespace Xanes.DataAccess.ServicesApi.Service.eSiafN4;

public class ModuloDocumentoService : BaseService, IModuloDocumentoService
{
    private string _actionUrl;
    private IConfiguration _cfg;
    public string _companyId;
    public ModuloDocumentoService(IHttpClientFactory httpClient,
        IConfiguration configuration
    ) : base(httpClient, configuration)
    {
        // configuration.GetValue<string>("ServicesUrl:Version")
        _actionUrl = string.Format("{0}modulosdocumentos"
            , configuration.GetValue<string>("ServicesUrl:UrlApi"));
        _cfg = configuration;
        _companyId = _cfg.GetValue<string>(AC.SecreteSiafN4CompanyUid) ?? string.Empty;
    }


    public Task<T> GetAsync<T>(string token, Guid id)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = HttpMethod.Get,
            Url = string.Format("{0}/{1}", _actionUrl, id.ToString()),
            Token = token
        });
    }

    public Task<T> GetByCodeAsync<T>(string token, Guid parentId, string codigo)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = HttpMethod.Get,
            Url = string.Format("{0}/getbycode?companyId={1}&parentId={2}&codigo={3}", _actionUrl, _companyId, parentId, codigo),
            Token = token
        });
    }

    public Task<T> GetByNumberAsync<T>(string token, Guid parentId, int numero)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = HttpMethod.Get,
            Url = string.Format("{0}/getbynumber?companyId={1}&parentId={2}&numero={3}", _actionUrl, _companyId, parentId, numero),
            Token = token
        });
    }

}

