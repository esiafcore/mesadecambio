namespace Xanes.DataAccess.ServicesApi.Interface.XanesN4;
public interface ICustomerLegacyService
{
    Task<T> GetAllLegacyAsync<T>(string token, int pageSize, int pageNumber);
}

