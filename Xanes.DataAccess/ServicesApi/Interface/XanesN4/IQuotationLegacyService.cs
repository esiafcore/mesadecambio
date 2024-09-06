namespace Xanes.DataAccess.ServicesApi.Interface.XanesN4;
public interface IQuotationLegacyService
{
    Task<T> GetAllLegacyAsync<T>(string token, int pageSize, int pageNumber, DateOnly beginDate, DateOnly endDate, string? identificationNumber);
}

