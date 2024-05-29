﻿using Microsoft.Extensions.Configuration;
using Xanes.DataAccess.ServicesApi.Interface;
using Xanes.Models.Shared;
using Xanes.Utility;

namespace Xanes.DataAccess.ServicesApi.Service;

public class QuotationDetailLegacyService : BaseService, IQuotationDetailLegacyService
{
    private string _actionUrl;
    private IConfiguration _configuration;

    public QuotationDetailLegacyService(IHttpClientFactory httpClient,
        IConfiguration configuration
    ) : base(httpClient, configuration)
    {
        // configuration.GetValue<string>("ServicesUrl:Version")
        _actionUrl = string.Format("{0}quotationsdetaillegacy"
            , configuration.GetValue<string>("ServicesUrl:UrlApi"));
        _configuration = configuration;
    }

    public Task<string> GetAllLegacyAsync(string token, int pageSize, int pageNumber, DateOnly beginDate, DateOnly endDate, string? identificationNumber = null)
    {
        return SendAsync(new APIRequest()
        {
            ApiType = HttpMethod.Get,
            Url = string.Format("{0}?beginDate={1}&endDate={2}&identificationNumber={3}&pagina={4}&recordsPorPagina={5}",
                _actionUrl, beginDate.ToString(AC.DefaultDateFormatWeb), endDate.ToString(AC.DefaultDateFormatWeb), identificationNumber, pageNumber, pageSize),
            Token = token
        });
    }
}

