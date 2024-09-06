using Xanes.Models.Dtos.eSiafN4;

namespace Xanes.DataAccess.ServicesApi.Interface.eSiafN4;
public interface IConsecutivoBcoDetalleService
{
    Task<T> GetAllAsync<T>(string token, int pageSize, int pageNumber, int fiscalYear, int fiscalMonth);
    Task<T> GetAsync<T>(string token, Guid id);
    Task<T> UpdateAsync<T>(string token, ConsecutivosBcoDetalleDtoUpdate model);
}

