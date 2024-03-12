namespace Xanes.DataAccess.Repository.IRepository;

public interface IUnitOfWork
{
    IBankRepository Bank { get; }
    ICurrencyRepository Currency { get; } 
    IQuotationTypeRepository QuotationType { get; }
    ICustomerCategoryRepository CustomerCategory { get; }
    IIdentificationTypeRepository IdentificationType { get; }
    ICustomerRepository Customer { get; }

    void Save();
}