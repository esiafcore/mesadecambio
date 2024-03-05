using Xanes.DataAccess.Data;
using Xanes.DataAccess.Repository.IRepository;

namespace Xanes.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;
    public IBankRepository Bank { get; private set; }

    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        Bank = new BankRepository(_db);
    }

    public void Save()
    {
        _db.SaveChanges();
    }
}