using Xanes.Models.Dtos.eSiafN4;

namespace Xanes.DataAccess.ServicesApi.Interface.eSiafN4;
public interface IBancoService
{
    Task<T> GetAllAsync<T>(string token, int pageSize, int pageNumber);
    Task<T> GetAsync<T>(string token, Guid id);
    Task<T> GetByCodeAsync<T>(string token, string codigo);

}

