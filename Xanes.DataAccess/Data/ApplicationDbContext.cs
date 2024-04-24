using Microsoft.EntityFrameworkCore;
using Xanes.DataAccess.Configuration;
using Xanes.Models;

namespace Xanes.DataAccess.Data;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLowerCaseNamingConvention();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration<Bank>(new BankConfiguration());
        modelBuilder.ApplyConfiguration<Currency>(new CurrencyConfiguration());
        modelBuilder.ApplyConfiguration<QuotationType>(new QuotationTypeConfiguration());
        modelBuilder.ApplyConfiguration<PersonType>(new PersonTypeConfiguration());
        modelBuilder.ApplyConfiguration<CustomerSector>(new CustomerSectorConfiguration());
        modelBuilder.ApplyConfiguration<CustomerCategory>(new CustomerCategoryConfiguration());
        modelBuilder.ApplyConfiguration<Customer>(new CustomerConfiguration());
        modelBuilder.ApplyConfiguration<IdentificationType>(new IdentificationTypeConfiguration());
        modelBuilder.ApplyConfiguration<Company>(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration<CurrencyExchangeRate>(new CurrencyExchangeRateConfiguration());
        modelBuilder.ApplyConfiguration<Quotation>(new QuotationConfiguration());
        modelBuilder.ApplyConfiguration<QuotationDetail>(new QuotationDetailConfiguration());
        modelBuilder.ApplyConfiguration<ConfigCxc>(new ConfigCxcConfiguration());
        modelBuilder.ApplyConfiguration<ConfigFac>(new ConfigFacConfiguration());

    }

    public virtual DbSet<Bank> Banks {get;set;}
    public virtual DbSet<Currency> Currencies { get; set; }
    public virtual DbSet<PersonType> PersonsTypes { get; set; }
    public virtual DbSet<CustomerSector> CustomersSectors { get; set; }
    public virtual DbSet<CustomerCategory> CustomersCategories { get; set; }
    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<IdentificationType> IdentificationsTypes { get; set; }
    public virtual DbSet<Company> Companies { get; set; }
    public virtual DbSet<CurrencyExchangeRate> CurrenciesExchangeRates { get; set; }
    public virtual DbSet<QuotationType> QuotationsTypes { get; set; }
    public virtual DbSet<Quotation> Quotations { get; set; }
    public virtual DbSet<QuotationDetail> QuotationsDetails { get; set; }
    public virtual DbSet<ConfigCxc> ConfigsCxc { get; set; }
    public virtual DbSet<ConfigFac> ConfigsFac { get; set; }

}