using Microsoft.Extensions.Configuration;
using Xanes.DataAccess.ServicesApi.Interface;
using Xanes.Models.Shared;

namespace Xanes.DataAccess.ServicesApi.Service;

public class QuotationLegacyService : BaseService, IQuotationLegacyService
{
    private string _actionUrl;
    private IConfiguration _configuration;

    public QuotationLegacyService(IHttpClientFactory httpClient,
        IConfiguration configuration
    ) : base(httpClient, configuration)
    {
        // configuration.GetValue<string>("ServicesUrl:Version")
        _actionUrl = string.Format("{0}{1}/quotationslegacy"
            , configuration.GetValue<string>("ServicesUrl:UrlApi")
            , configuration.GetValue<string>("ServicesUrl:UrlSuffix"));
        _configuration = configuration;
    }

    public Task<T> GetAllLegacyAsync<T>(string token, int pageSize, int pageNumber, DateOnly beginDate, DateOnly endDate)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = HttpMethod.Get,
            Url = string.Format("{0}?beginDate={1}&endDate={2}&pagina={3}&recordsPorPagina={4}",
                _actionUrl, beginDate.ToString(), endDate.ToString(), pageNumber, pageSize),
            Token = token
        });
    }
}

