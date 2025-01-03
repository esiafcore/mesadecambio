﻿using Microsoft.Extensions.Configuration;
using Xanes.DataAccess.ServicesApi.Interface.eSiafN4;
using Xanes.Models.Dtos.eSiafN4;
using Xanes.Models.Shared;
using Xanes.Utility;

namespace Xanes.DataAccess.ServicesApi.Service.eSiafN4;

public class AsientoContableService : BaseService, IAsientoContableService
{
    private string _actionUrl;
    private IConfiguration _cfg;
    public string _companyId;
    public AsientoContableService(IHttpClientFactory httpClient,
        IConfiguration configuration
    ) : base(httpClient, configuration)
    {
        // configuration.GetValue<string>("ServicesUrl:Version")
        _actionUrl = string.Format("{0}asientoscontables"
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

    public Task<T> GetNextSequentialNumberAsync<T>(
        string token, int fiscalYear, int fiscalMonth,
        short tipo, short subtipo, Enumeradores.ConsecutivoTipo consecutivo, bool isSave)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = HttpMethod.Get,
            Url = string.Format("{0}/getnextsequentialnumber?companyId={1}&fiscalYear={2}&fiscalMonth={3}&tipo={4}&subtipo={5}&consecutivo={6}&isSave={7}",
                _actionUrl, _companyId, fiscalYear, fiscalMonth, tipo, subtipo, consecutivo, isSave),
            Token = token
        });
    }

    public Task<T> CreateAsync<T>(string token, AsientosContablesDtoCreate body)
    {
        body.UidCia = Guid.Parse(_companyId);

        return SendAsync<T>(new APIRequest()
        {
            ApiType = HttpMethod.Post,
            Url = string.Format("{0}", _actionUrl),
            Data = body,
            Token = token
        });
    }

    public Task<T> UpdateAsync<T>(string token, AsientosContablesDtoUpdate body)
    {
        body.UidCia = Guid.Parse(_companyId);

        return SendAsync<T>(new APIRequest()
        {
            ApiType = HttpMethod.Put,
            Url = string.Format("{0}/{1}", _actionUrl, body.UidRegist.ToString()),
            Data = body,
            Token = token
        });
    }

    public Task<T> DeleteAsync<T>(string token, Guid id)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = HttpMethod.Delete,
            Url = string.Format("{0}/{1}", _actionUrl, id.ToString()),
            Token = token
        });
    }
}

