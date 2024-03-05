using Xanes.DataAccess.Data;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models;

namespace Xanes.DataAccess.Repository;

public class CurrencyRepository : Repository<Currency>, ICurrencyRepository
{
    private readonly ApplicationDbContext _db;

    public CurrencyRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(Currency obj)
    {
        _db.Currencies.Update(obj);
    }
}