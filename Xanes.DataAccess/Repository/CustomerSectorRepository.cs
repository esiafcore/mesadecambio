using Xanes.DataAccess.Data;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models;

namespace Xanes.DataAccess.Repository;

public class CustomerSectorRepository : Repository<CustomerSector>, ICustomerSectorRepository
{
    private readonly ApplicationDbContext _db;

    public CustomerSectorRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(CustomerSector obj)
    {
        _db.CustomersSectors.Update(obj);
    }
}