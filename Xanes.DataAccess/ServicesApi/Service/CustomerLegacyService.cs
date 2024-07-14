using Microsoft.Extensions.Configuration;
using Xanes.DataAccess.ServicesApi.Interface;
using Xanes.Models.Shared;

namespace Xanes.DataAccess.ServicesApi.Service;

public class CustomerLegacyService : BaseService, ICustomerLegacyService
{
    private string _actionUrl;
    private IConfiguration _configuration;

    public CustomerLegacyService(IHttpClientFactory httpClient,
        IConfiguration configuration
    ) : base(httpClient, configuration)
    {
        // configuration.GetValue<string>("ServicesUrl:Version")
        _actionUrl = string.Format("{0}customerslegacy"
            , configuration.GetValue<string>("ServicesUrl:UrlApi"));
        _configuration = configuration;
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

