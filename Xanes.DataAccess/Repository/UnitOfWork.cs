using Xanes.DataAccess.Data;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models;

namespace Xanes.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;
    public IBankRepository Bank { get; private set; }
    public IBankAccountRepository BankAccount { get; private set; }

    public ICurrencyRepository Currency { get; private set; }

    public ICustomerCategoryRepository CustomerCategory { get; private set; }

    public IIdentificationTypeRepository IdentificationType { get; private set; }
    public ICustomerRepository Customer { get; private set; }
    public ICompanyRepository Company { get; }

    public IPersonTypeRepository PersonType { get; private set; }
    public ICustomerSectorRepository CustomerSector { get; private set; }
    public ICurrencyExchangeRateRepository CurrencyExchangeRate { get; private set; }
    public IQuotationTypeRepository QuotationType { get; private set; }
    public IQuotationRepository Quotation { get; private set; }
    public IQuotationDetailRepository QuotationDetail { get; private set; }
    public IConfigCxcRepository ConfigCxc { get; private set; }
    public IConfigFacRepository ConfigFac { get; private set; }

    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        Bank = new BankRepository(_db);
        BankAccount = new BankAccountRepository(_db);
        Currency = new CurrencyRepository(_db);
        CustomerCategory = new CustomerCategoryRepository(_db);
        IdentificationType = new IdentificationTypeRepository(_db);
        PersonType = new PersonTypeRepository(_db);
        Customer = new CustomerRepository(_db);
        Company = new CompanyRepository(_db);
        CustomerSector = new CustomerSectorRepository(_db);
        CurrencyExchangeRate = new CurrencyExchangeRateRepository(_db);
        QuotationType = new QuotationTypeRepository(_db);
        Quotation = new QuotationRepository(_db);
        QuotationDetail = new QuotationDetailRepository(_db);
        ConfigCxc = new ConfigCxcRepository(_db);
        ConfigFac = new ConfigFacRepository(_db);
    }

    public void Save()
    {
        _db.SaveChanges();
    }
}