using Xanes.DataAccess.Data;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models;

namespace Xanes.DataAccess.Repository;

public class BusinessExecutiveRepository : Repository<BusinessExecutive>, IBusinessExecutiveRepository
{
    private readonly ApplicationDbContext _db;

    public BusinessExecutiveRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(BusinessExecutive obj)
    {
        _db.BusinessExecutives.Update(obj);
    }
}