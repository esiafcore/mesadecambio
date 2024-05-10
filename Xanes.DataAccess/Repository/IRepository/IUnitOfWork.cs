namespace Xanes.DataAccess.Repository.IRepository;

public interface IUnitOfWork
{
    IBankRepository Bank { get; }
    IBankAccountRepository BankAccount { get; }
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
    IBusinessExecutiveRepository BusinessExecutive { get; }
    IQuotationDetailRepository QuotationDetail { get; }
    IConfigCxcRepository ConfigCxc { get; }
    IConfigFacRepository ConfigFac { get; }
    void Save();
}