using Microsoft.Extensions.Configuration;
using Xanes.DataAccess.ServicesApi.Interface.XanesN4;
using Xanes.Models.Shared;

namespace Xanes.DataAccess.ServicesApi.Service.XanesN4;

public class CustomerLegacyService : BaseService, ICustomerLegacyService
{
    private string _actionUrl;
    private IConfiguration _cfg;

    public CustomerLegacyService(IHttpClientFactory httpClient,
        IConfiguration configuration
    ) : base(httpClient, configuration)
    {
        // configuration.GetValue<string>("ServicesUrl:Version")
        _actionUrl = string.Format("{0}customerslegacy"
            , configuration.GetValue<string>("ServicesUrl:UrlApi"));
        _cfg = configuration;
    }

    public Task<T> GetAllLegacyAsync<T>(string token, int pageSize, int pageNumber)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = HttpMethod.Get,
            Url = string.Format("{0}?pagina={1}&recordsPorPagina={2}",
                _actionUrl, pageNumber, pageSize),
            Token = token
        });
    }
}

