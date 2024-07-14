
namespace Xanes.DataAccess.ServicesApi.Interface;
public interface ICustomerLegacyService
{
    Task<T> GetAllLegacyAsync<T>(string token, int pageSize, int pageNumber);
}

