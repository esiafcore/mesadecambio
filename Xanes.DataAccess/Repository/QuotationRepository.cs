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

    public async Task<int> NextSequentialNumber(Expression<Func<Quotation, bool>>? filter = null)
    {
        IQueryable<Quotation> query = _db.Set<Quotation>();
        if (filter != null)
        {
            query = query.Where(filter);
        }
        int nextNumeral = query.Max(x => x.Numeral);

        return await Task.FromResult(nextNumeral + 1);
    }
}