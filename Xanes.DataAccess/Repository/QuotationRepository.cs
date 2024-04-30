using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using Xanes.DataAccess.Data;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models;
using Xanes.Utility;

namespace Xanes.DataAccess.Repository;

public class QuotationRepository : Repository<Quotation>, IQuotationRepository
{
    private readonly ApplicationDbContext _db;
    public QuotationRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(Quotation obj)
    {
        _db.Quotations.Update(obj);
    }

    public async Task<bool> RemoveWithChildren(int id)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            await _db.QuotationsDetails
                .Where(x => x.ParentId == id)
                .ExecuteDeleteAsync();

            await _db.Quotations
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
            
            await transaction.CommitAsync();
            return await Task.FromResult(true);
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

        return await Task.FromResult(false);
    }

    public async Task<int> NextSequentialNumber(Expression<Func<Quotation, bool>>? filter = null)
    {
        IQueryable<Quotation> query = _db.Set<Quotation>();
        if (filter != null)
        {
            query = query.Where(filter);
        }

        // Si no hay elementos, asigna 0 como valor predeterminado
        int nextNumeral = query.Any() ? query.Max(x => x.Numeral) : 0;

        return await Task.FromResult(nextNumeral + 1);
    }
}