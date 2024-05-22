using Xanes.Models.Shared;

namespace Xanes.DataAccess.ServicesApi;

public interface IBaseService
{
    APIResponse ResponseModel { get; set; }
    Task<T> SendAsync<T>(APIRequest apiRequest, Pagination? pagination);
}
