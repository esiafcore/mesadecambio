using Xanes.DataAccess.Data;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models;

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
}