
namespace Xanes.DataAccess.ServicesApi.Interface;
public interface ICustomerLegacyService
{
    Task<string> GetAllLegacyAsync(string token, int pageSize, int pageNumber);
}

