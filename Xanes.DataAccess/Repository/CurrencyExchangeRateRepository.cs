using Microsoft.EntityFrameworkCore;
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

    public async Task ImportRangeAsync(List<CurrencyExchangeRate> entityList)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            await dbSet.AddRangeAsync(entityList);
            await _db.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (DbUpdateException ex)
        {
            await transaction.RollbackAsync();
            throw new Exception(ex.InnerException?.Message);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception(ex.Message);
        }
    }

    public void Update(CurrencyExchangeRate obj)
    {
        _db.CurrenciesExchangeRates.Update(obj);
    }
}