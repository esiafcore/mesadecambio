using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xanes.Models;
using Xanes.Utility;

namespace Xanes.DataAccess.Configuration;

public class QuotationDetailConfiguration : IEntityTypeConfiguration<QuotationDetail>
{
    public void Configure(EntityTypeBuilder<QuotationDetail> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CompanyId).ValueGeneratedOnAdd().HasDefaultValue(1);
        builder.Property(x => x.LineNumber).ValueGeneratedOnAdd().HasDefaultValue(1);
        builder.Property(b => b.AmountDetail).HasPrecision(18, 4);
        builder.Property(b => b.IsBankTransactionPosted).HasDefaultValue(false);
        builder.Property(b => b.IsBankTransactionVoidPosted).HasDefaultValue(false);
        builder.Property(b => b.IsJournalEntryPosted).HasDefaultValue(false);
        builder.Property(b => b.IsJournalEntryVoidPosted).HasDefaultValue(false);
        builder.Property(b => b.QuotationDetailType).HasDefaultValue(SD.QuotationDetailType.Deposit)
            .HasSentinel(default);

        builder.HasIndex(x => new { x.CompanyId, x.ParentId , x.QuotationDetailType, x.LineNumber}
                , "quotationsdetails_idx_2010")
            .IsUnique();

        builder.HasOne(x => x.ParentTrx)
            .WithMany()
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(true);

        builder.HasOne(x => x.CurrencyDetailTrx)
            .WithMany()
            .HasForeignKey(x => x.CurrencyDetailId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(true);

        builder.HasOne(x => x.BankSourceTrx)
            .WithMany()
            .HasForeignKey(x => x.BankSourceId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(true);

        builder.HasOne(x => x.BankTargetTrx)
            .WithMany()
            .HasForeignKey(x => x.BankTargetId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(false);

    }
}