
using Microsoft.Extensions.Configuration;
using Xanes.DataAccess.ServicesApi.Interface.eSiafN4;
using Xanes.Models.Dtos.eSiafN4;
using Xanes.Models.Shared;
using Xanes.Utility;

namespace Xanes.DataAccess.ServicesApi.Service.eSiafN4;

public class ConsecutivoCntDetalleService : BaseService, IConsecutivoCntDetalleService
{
    private string _actionUrl;
    private IConfiguration _cfg;
    public string _companyId;
    public ConsecutivoCntDetalleService(IHttpClientFactory httpClient,
        IConfiguration configuration
    ) : base(httpClient, configuration)
    {
        // configuration.GetValue<string>("ServicesUrl:Version")
        _actionUrl = string.Format("{0}consecutivoscntdetalle"
            , configuration.GetValue<string>("ServicesUrl:UrlApi"));
        _cfg = configuration;
        _companyId = _cfg.GetValue<string>(AC.SecreteSiafN4CompanyUid) ?? string.Empty;
    }

    public Task<T> GetAllAsync<T>(string token, int pageSize, int pageNumber, int fiscalYear, int fiscalMonth)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = HttpMethod.Get,
            Url = string.Format("{0}?companyId={1}&yearfiscal={2}&mesfiscal={3}&pagina={4}&recordsPorPagina={5}",
                _actionUrl, _companyId, fiscalYear, fiscalMonth, pageNumber, pageSize),
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

    public Task<T> UpdateAsync<T>(string token, ConsecutivosCntDetalleDtoUpdate body)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = HttpMethod.Put,
            Url = string.Format("{0}/{1}", _actionUrl, body.UidCia.ToString()),
            Data = body,
            Token = token
        });
    }
}

