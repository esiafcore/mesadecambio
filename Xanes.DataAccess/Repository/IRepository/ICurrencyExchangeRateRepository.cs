using Xanes.Models;

namespace Xanes.DataAccess.Repository.IRepository;

public interface ICurrencyExchangeRateRepository : IRepository<CurrencyExchangeRate>
{
    void Update(CurrencyExchangeRate obj);

    Task ImportRangeAsync(List<CurrencyExchangeRate> entityList);

}