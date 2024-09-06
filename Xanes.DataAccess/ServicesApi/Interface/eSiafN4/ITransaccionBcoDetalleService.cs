using Xanes.Models.Dtos.eSiafN4;

namespace Xanes.DataAccess.ServicesApi.Interface.eSiafN4;
public interface ITransaccionBcoDetalleService
{
    Task<T> GetAllAsync<T>(string token, int pageSize, int pageNumber, int fiscalYear, int fiscalMonth);
    Task<T> GetAllByParentAsync<T>(string token, int pageSize, int pageNumber, Guid id, int fiscalYear, int fiscalMonth);
    Task<T> GetAsync<T>(string token, Guid id);
    Task<T> CreateAsync<T>(string token, TransaccionesBcoDetalleDtoCreate model);
    Task<T> UpdateAsync<T>(string token, TransaccionesBcoDetalleDtoUpdate model);
    Task<T> DeleteAsync<T>(string token, Guid id);
    Task<T> DeleteByParentAsync<T>(string token, Guid id);
}

