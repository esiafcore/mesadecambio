using Xanes.DataAccess.Data;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models;

namespace Xanes.DataAccess.Repository;

public class CurrencyExchangeRateRepository : Repository<CurrencyExchangeRate>, ICurrencyExchangeRateRepository
{
    private readonly ApplicationDbContext _db;

    public CurrencyExchangeRateRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(CurrencyExchangeRate obj)
    {
        _db.CurrenciesExchangeRates.Update(obj);
    }
}