using Xanes.Models.Dtos.eSiafN4;

namespace Xanes.DataAccess.ServicesApi.Interface.eSiafN4;
public interface ICuentaBancariaService
{
    Task<T> GetAllAsync<T>(string token, int pageSize, int pageNumber);
    Task<T> GetAsync<T>(string token, Guid id);
    Task<T> GetAllByBankAsync<T>(string token, string codigo);

}

