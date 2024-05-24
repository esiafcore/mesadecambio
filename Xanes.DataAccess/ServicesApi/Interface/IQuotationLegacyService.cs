
namespace Xanes.DataAccess.ServicesApi.Interface;
public interface IQuotationLegacyService
{
    Task<string> GetAllLegacyAsync(string token, int pageSize, int pageNumber, DateOnly beginDate, DateOnly endDate);
}

