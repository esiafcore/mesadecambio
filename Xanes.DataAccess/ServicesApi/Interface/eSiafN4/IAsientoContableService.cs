using Xanes.Models.Dtos.eSiafN4;
using static Xanes.Utility.Enumeradores;

namespace Xanes.DataAccess.ServicesApi.Interface.eSiafN4;
public interface IAsientoContableService
{
    Task<T> GetAllAsync<T>(string token, int pageSize, int pageNumber, int fiscalYear, int fiscalMonth);
    Task<T> GetAsync<T>(string token, Guid id);
    Task<T> GetNextSecuentialNumberAsync<T>(
        string token, int fiscalYear,
        int fiscalMonth, short tipo, short subtipo,
        ConsecutivoTipo consecutivo, bool isSave);
    Task<T> CreateAsync<T>(string token, AsientosContablesDtoCreate model);
    Task<T> UpdateAsync<T>(string token, AsientosContablesDtoUpdate model);
    Task<T> DeleteAsync<T>(string token, Guid id);
}

