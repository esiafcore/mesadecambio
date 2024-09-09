using Microsoft.Extensions.Configuration;
using Xanes.DataAccess.ServicesApi.Interface.eSiafN4;
using Xanes.Models.Dtos.eSiafN4;
using Xanes.Models.Shared;
using Xanes.Utility;

namespace Xanes.DataAccess.ServicesApi.Service.eSiafN4;

public class CuentaBancariaService : BaseService, ICuentaBancariaService
{
    private string _actionUrl;
    private IConfiguration _cfg;
    public string _companyId;
    public CuentaBancariaService(IHttpClientFactory httpClient,
        IConfiguration configuration
    ) : base(httpClient, configuration)
    {
        // configuration.GetValue<string>("ServicesUrl:Version")
        _actionUrl = string.Format("{0}cuentasbancarias"
            , configuration.GetValue<string>("ServicesUrl:UrlApi"));
        _cfg = configuration;
        _companyId = _cfg.GetValue<string>(AC.SecreteSiafN4CompanyUid) ?? string.Empty;
    }

    public Task<T> GetAllAsync<T>(string token, int pageSize, int pageNumber)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = HttpMethod.Get,
            Url = string.Format("{0}?companyId={1}&pagina={2}&recordsPorPagina={3}",
                _actionUrl, _companyId, pageNumber, pageSize),
            Token = token
        });
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

    public Task<T> GetAllByBankAsync<T>(string token, string codigo)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = HttpMethod.Get,
            Url = string.Format("{0}/getallbybank?companyId={1}&codigo={2}", _actionUrl, _companyId, codigo),
            Token = token
        });
    }

}

