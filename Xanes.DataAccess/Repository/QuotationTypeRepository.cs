using Xanes.DataAccess.Data;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models;

namespace Xanes.DataAccess.Repository;

public class QuotationTypeRepository : Repository<QuotationType>, IQuotationTypeRepository
{
    private readonly ApplicationDbContext _db;

    public QuotationTypeRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(QuotationType obj)
    {
        _db.QuotationsTypes.Update(obj);
    }
}