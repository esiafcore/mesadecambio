using Xanes.DataAccess.Data;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models;

namespace Xanes.DataAccess.Repository;

public class CustomerCategoryRepository : Repository<CustomerCategory>, ICustomerCategoryRepository
{
    private readonly ApplicationDbContext _db;

    public CustomerCategoryRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(CustomerCategory obj)
    {
        _db.CustomersCategories.Update(obj);
    }
}