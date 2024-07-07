using Xanes.DataAccess.Data;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models;

namespace Xanes.DataAccess.Repository;

public class BankAccountRepository : Repository<BankAccount>, IBankAccountRepository
{
    private readonly ApplicationDbContext _db;

    public BankAccountRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(BankAccount obj)
    {
        _db.BankAccounts.Update(obj);
    }

}