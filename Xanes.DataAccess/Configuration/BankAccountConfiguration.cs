using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xanes.Models;
using Xanes.Utility;

namespace Xanes.DataAccess.Configuration;

public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
{
    public void Configure(EntityTypeBuilder<BankAccount> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CompanyId).ValueGeneratedOnAdd().HasDefaultValue(1);
        builder.Property(x => x.IsLocal).ValueGeneratedOnAdd().HasDefaultValue(true);
        builder.Property(b => b.CurrencyType).HasDefaultValue(SD.CurrencyType.Base)
            .HasSentinel(default);
        builder.HasIndex(x => new { x.CompanyId, x.ParentId, x.Code }
                , "bankaccounts_idx_2010")
            .IsUnique();

        builder.HasOne(x => x.ParentTrx)
            .WithMany()
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(true);

        builder.HasOne(x => x.CurrencyTrx)
            .WithMany()
            .HasForeignKey(x => x.CurrencyId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(true);

    }
}