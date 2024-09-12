using Xanes.Models.Dtos.eSiafN4;

namespace Xanes.DataAccess.ServicesApi.Interface.eSiafN4;
public interface IModuloService
{
    Task<T> GetAsync<T>(string token, Guid id);
    Task<T> GetByCodeAsync<T>(string token, string codigo);
    Task<T> GetByNumberAsync<T>(string token, int numero);
}

