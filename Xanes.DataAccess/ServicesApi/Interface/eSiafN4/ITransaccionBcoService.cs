using Xanes.Models.Dtos.eSiafN4;

namespace Xanes.DataAccess.ServicesApi.Interface.eSiafN4;
public interface ITransaccionBcoService
{
    Task<T> GetAllAsync<T>(string token, int pageSize, int pageNumber, int fiscalYear, int fiscalMonth);
    Task<T> GetAsync<T>(string token, Guid id);
    Task<T> CreateAsync<T>(string token, TransaccionesBcoDtoCreate model);
    Task<T> UpdateAsync<T>(string token, TransaccionesBcoDtoUpdate model);
    Task<T> DeleteAsync<T>(string token, Guid id);
}

