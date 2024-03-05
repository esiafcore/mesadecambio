using Xanes.DataAccess.Data;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models;

namespace Xanes.DataAccess.Repository;

public class BankRepository : Repository<Bank>, IBankRepository
{
    private readonly ApplicationDbContext _db;

    public BankRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(Bank obj)
    {
        _db.Banks.Update(obj);
    }

    public void Save()
    {
        _db.SaveChanges();
    }
}