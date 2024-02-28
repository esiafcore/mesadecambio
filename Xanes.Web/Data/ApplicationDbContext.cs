using Microsoft.EntityFrameworkCore;
using Xanes.Web.Models;

namespace Xanes.Web.Data;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opptons): base(opptons)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration<Bank>(new BankConfiguration());
        modelBuilder.ApplyConfiguration<Currency>(new CurrencyConfiguration());
        modelBuilder.ApplyConfiguration<QuotationType>(new QuotationTypeConfiguration());
    }

    public virtual DbSet<Bank> Banks {get;set;}
    public virtual DbSet<Currency> Currencies { get; set; }
    public virtual DbSet<QuotationType> QuotationsTypes { get; set; }
}