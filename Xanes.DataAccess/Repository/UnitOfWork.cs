using Xanes.DataAccess.Data;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models;

namespace Xanes.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;
    public IBankRepository Bank { get; private set; }
    public ICurrencyRepository Currency { get; private set; }

    public IQuotationTypeRepository QuotationType { get; private set; }
    public ICustomerCategoryRepository CustomerCategory { get; private set; }

    public IIdentificationTypeRepository IdentificationType { get; private set; }

    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        Bank = new BankRepository(_db);
        Currency = new CurrencyRepository(_db);
        QuotationType = new QuotationTypeRepository(_db);
        CustomerCategory = new CustomerCategoryRepository(_db);
        IdentificationType = new IdentificationTypeRepository(_db);
    }

    public void Save()
    {
        _db.SaveChanges();
    }
}