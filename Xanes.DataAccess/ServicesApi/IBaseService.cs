using Xanes.Models.Shared;

namespace Xanes.DataAccess.ServicesApi;

public interface IBaseService
{
    APIResponse ResponseModel { get; set; }
    Task<string> SendAsync(APIRequest apiRequest, Pagination? pagination);
}
