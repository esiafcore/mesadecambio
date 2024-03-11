using Microsoft.EntityFrameworkCore;
using Xanes.DataAccess.Configuration;
using Xanes.DataAccess.Shared;
using Xanes.Models;

namespace Xanes.DataAccess.Data;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
    }

    //protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    //{
    //    base.ConfigureConventions(configurationBuilder);
    //    configurationBuilder.Properties<DateTime>()
    //        .HaveConversion(typeof(UtcVAlueConverter));
    //}
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
        modelBuilder.ApplyConfiguration<CustomerCategory>(new CustomerCategoryConfiguration());
        modelBuilder.ApplyConfiguration<Customer>(new CustomerConfiguration());
        modelBuilder.ApplyConfiguration<IdentificationType>(new IdentificationTypeConfiguration());

    }

    public virtual DbSet<Bank> Banks {get;set;}
    public virtual DbSet<Currency> Currencies { get; set; }
    public virtual DbSet<QuotationType> QuotationsTypes { get; set; }
    public virtual DbSet<PersonType> PersonsTypes { get; set; }
    public virtual DbSet<CustomerCategory> CustomersCategories { get; set; }
    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<IdentificationType> IdentificationsTypes { get; set; }
}