using Xanes.Models.Dtos.eSiafN4;

namespace Xanes.DataAccess.ServicesApi.Interface.eSiafN4;
public interface IModuloDocumentoService
{
    Task<T> GetAsync<T>(string token, Guid id);
    Task<T> GetByCodeAsync<T>(string token, Guid parentId, string codigo);
    Task<T> GetByNumberAsync<T>(string token, Guid parentId, int numero);
}

