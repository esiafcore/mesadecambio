using System.Linq.Expressions;
using Xanes.DataAccess.Data;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models;

namespace Xanes.DataAccess.Repository;

public class QuotationDetailRepository : Repository<QuotationDetail>, IQuotationDetailRepository
{
    private readonly ApplicationDbContext _db;
    public QuotationDetailRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(QuotationDetail obj)
    {
        _db.QuotationsDetails.Update(obj);
    }

    public async Task<int> NextLineNumber(Expression<Func<QuotationDetail, bool>>? filter = null)
    {
        IQueryable<QuotationDetail> query = _db.Set<QuotationDetail>();
        if (filter != null)
        {
            query = query.Where(filter);
        }

        // Si no hay elementos, asigna 0 como valor predeterminado
        int nextNumeral = query.Any() ? query.Max(x => x.LineNumber) : 0;

        return await Task.FromResult(nextNumeral + 1);
    }
}