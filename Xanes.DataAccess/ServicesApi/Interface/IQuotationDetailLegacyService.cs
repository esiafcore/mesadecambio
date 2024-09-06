
namespace Xanes.DataAccess.ServicesApi.Interface;
public interface IQuotationDetailLegacyService
{
    Task<T> GetAllLegacyAsync<T>(string token, int pageSize, int pageNumber, DateOnly beginDate, DateOnly endDate, string? identificationNumber);
}

