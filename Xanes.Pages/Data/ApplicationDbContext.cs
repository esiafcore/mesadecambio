using Microsoft.EntityFrameworkCore;
using Xanes.Pages.Models;

namespace Xanes.Pages.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration<Bank>(new BankConfiguration());

    }

    public virtual DbSet<Bank> Banks { get; set; }

}