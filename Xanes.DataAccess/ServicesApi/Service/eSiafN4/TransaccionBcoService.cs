using Microsoft.Extensions.Configuration;
using Xanes.DataAccess.ServicesApi.Interface.eSiafN4;
using Xanes.Models.Dtos.eSiafN4;
using Xanes.Models.Shared;
using Xanes.Utility;

namespace Xanes.DataAccess.ServicesApi.Service.eSiafN4;

public class TransaccionBcoService : BaseService, ITransaccionBcoService
{
    private string _actionUrl;
    private IConfiguration _cfg;
    public string _companyId;
    public TransaccionBcoService(IHttpClientFactory httpClient,
        IConfiguration configuration
    ) : base(httpClient, configuration)
    {
        // configuration.GetValue<string>("ServicesUrl:Version")
        _actionUrl = string.Format("{0}transaccionesbco"
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

    public Task<T> GetIsAprovalAsync<T>(string token, Guid id)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = HttpMethod.Get,
            Url = string.Format("{0}/isaproval?id={1}", _actionUrl, id.ToString()),
            Token = token
        });
    }

    public Task<T> GetNextSecuentialNumberAsync<T>(
        string token, Guid bankAccountId, int fiscalYear, int fiscalMonth,
        short tipo, short subtipo, Enumeradores.ConsecutivoTipo consecutivo, bool isSave)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = HttpMethod.Get,
            Url = string.Format("{0}/getnextsecuentialnumber?companyId={1}&bankAccountId={2}&fiscalYear={3}&fiscalMonth={4}&tipo={5}&subtipo={6}&consecutivo={7}&isSave={8}",
                _actionUrl, _companyId, bankAccountId.ToString(), fiscalYear, fiscalMonth, tipo, subtipo, consecutivo, isSave),
            Token = token
        });
    }

    public Task<T> CreateRelationAsync<T>(string token, Guid transaBcoDebitId, Guid transaBcoCreditId, Guid? transaBcoCommisionId)
    {
        transaBcoCommisionId = (transaBcoCommisionId == null ? Guid.Empty : transaBcoCommisionId);

        return SendAsync<T>(new APIRequest()
        {
            ApiType = HttpMethod.Post,
            Url = string.Format("{0}/relation?transaBcoDebitId={1}&transaBcoCreditId={2}&transaBcoCommisionId={3}", _actionUrl, transaBcoDebitId.ToString(), transaBcoCreditId.ToString(), transaBcoCommisionId.ToString()),
            Token = token
        });
    }

    public Task<T> CreateAsync<T>(string token, TransaccionesBcoDtoCreate body)
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

    public Task<T> UpdateAsync<T>(string token, TransaccionesBcoDtoUpdate body)
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

