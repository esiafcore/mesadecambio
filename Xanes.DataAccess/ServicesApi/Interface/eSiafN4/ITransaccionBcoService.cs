using Xanes.Models.Dtos.eSiafN4;
using static Xanes.Utility.Enumeradores;

namespace Xanes.DataAccess.ServicesApi.Interface.eSiafN4;
public interface ITransaccionBcoService
{
    Task<T> GetAllAsync<T>(string token, int pageSize, int pageNumber, int fiscalYear, int fiscalMonth);
    Task<T> GetAsync<T>(string token, Guid id);
    Task<T> GetStatusAsync<T>(string token, Guid id);
    Task<T> GetIsAprovalAsync<T>(string token, Guid id);
    Task<T> GetNextSecuentialNumberAsync<T>(
        string token, Guid bankAccountId, int fiscalYear, 
        int fiscalMonth, short tipo, short subtipo,
        ConsecutivoTipo consecutivo, bool isSave);
    Task<T> CreateAsync<T>(string token, TransaccionesBcoDtoCreate model);
    Task<T> CreateRelationAsync<T>(string token, Guid transaBcoDebitId, Guid transaBcoCreditId, Guid? transaBcoCommisionId);
    Task<T> UpdateAsync<T>(string token, TransaccionesBcoDtoUpdate model);
    Task<T> DeleteAsync<T>(string token, Guid id);
}

