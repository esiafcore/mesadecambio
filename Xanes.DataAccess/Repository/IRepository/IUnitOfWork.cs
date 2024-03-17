namespace Xanes.DataAccess.Repository.IRepository;

public interface IUnitOfWork
{
    IBankRepository Bank { get; }
    ICurrencyRepository Currency { get; } 
    IQuotationTypeRepository QuotationType { get; }
    ICustomerCategoryRepository CustomerCategory { get; }
    IIdentificationTypeRepository IdentificationType { get; }
    IPersonTypeRepository PersonType { get; }
    ICustomerRepository Customer { get; }
    ICompanyRepository Company { get; }
    ICustomerSectorRepository CustomerSector { get; }
    void Save();
}