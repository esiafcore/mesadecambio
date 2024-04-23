namespace Xanes.DataAccess.Repository.IRepository;

public interface IUnitOfWork
{
    IBankRepository Bank { get; }
    ICurrencyRepository Currency { get; } 
    ICustomerCategoryRepository CustomerCategory { get; }
    IIdentificationTypeRepository IdentificationType { get; }
    IPersonTypeRepository PersonType { get; }
    ICustomerRepository Customer { get; }
    ICompanyRepository Company { get; }
    ICustomerSectorRepository CustomerSector { get; }
    ICurrencyExchangeRateRepository CurrencyExchangeRate { get; }
    IQuotationTypeRepository QuotationType { get; }
    IQuotationRepository Quotation{ get; }
    IConfigCxcRepository ConfigCxc { get; }

    void Save();
}