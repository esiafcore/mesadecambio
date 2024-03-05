using Microsoft.EntityFrameworkCore;
using Xanes.DataAccess.Configuration;
using Xanes.Models;

namespace Xanes.DataAccess.Data;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration<Bank>(new BankConfiguration());
        modelBuilder.ApplyConfiguration<Currency>(new CurrencyConfiguration());
        modelBuilder.ApplyConfiguration<QuotationType>(new QuotationTypeConfiguration());
        modelBuilder.ApplyConfiguration<CustomerType>(new CustomerTypeConfiguration());


    }

    public virtual DbSet<Bank> Banks {get;set;}
    public virtual DbSet<Currency> Currencies { get; set; }
    public virtual DbSet<QuotationType> QuotationsTypes { get; set; }
    public virtual DbSet<CustomerType> CustomersTypes { get; set; }

}