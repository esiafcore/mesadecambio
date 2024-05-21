using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using Xanes.DataAccess.Data;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models;
using Xanes.Utility;

namespace Xanes.DataAccess.Repository;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    private readonly ApplicationDbContext _db;

    public CustomerRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(Customer obj)
    {
        _db.Customers.Update(obj);
    }

    public async Task<long> NextSequentialNumber(Expression<Func<Customer, bool>>? filter = null)
    {
        IQueryable<Customer> query = _db.Set<Customer>();
        if (filter != null)
        {
            query = query.Where(filter);

        }
        string? nextCode = query.Max(x => x.Code);
        if (String.IsNullOrEmpty(nextCode))
        {
            nextCode = AC.CharDefaultEmpty.ToString();
        }
        return await Task.FromResult(long.Parse(nextCode) + 1);
    }


    public async Task ImportRangeAsync(List<Customer> entityList)
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
}