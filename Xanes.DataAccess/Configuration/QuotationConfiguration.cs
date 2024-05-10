using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xanes.Models;
using Xanes.Utility;

namespace Xanes.DataAccess.Configuration;

public class QuotationConfiguration : IEntityTypeConfiguration<Quotation>
{
    public void Configure(EntityTypeBuilder<Quotation> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.BankAccountSourceId).IsRequired(false);
        builder.Property(x => x.BankAccountTargetId).IsRequired(false);

        builder.Property(x => x.CompanyId).ValueGeneratedOnAdd().HasDefaultValue(1);
        builder.Property(x => x.InternalSerial).ValueGeneratedOnAdd().HasDefaultValue(AC.InternalSerialOfficial);

        builder.Property(b => b.ExchangeRateOfficialTransa).HasPrecision(18, 8);
        builder.Property(b => b.ExchangeRateBuyTransa).HasPrecision(18, 8);
        builder.Property(b => b.ExchangeRateSellTransa).HasPrecision(18, 8);
        builder.Property(b => b.ExchangeRateOfficialReal).HasPrecision(18, 8);
        builder.Property(b => b.ExchangeRateBuyReal).HasPrecision(18, 8);
        builder.Property(b => b.ExchangeRateSellReal).HasPrecision(18, 8);

        builder.Property(b => b.AmountTransaction).HasPrecision(18, 4).HasDefaultValue(0m);
        builder.Property(b => b.AmountCommission).HasPrecision(18, 4).HasDefaultValue(0m);
        builder.Property(b => b.AmountExchange).HasPrecision(18, 4).HasDefaultValue(0m);
        builder.Property(b => b.AmountRevenue).HasPrecision(18, 4).HasDefaultValue(0m);
        builder.Property(b => b.AmountCost).HasPrecision(18, 4).HasDefaultValue(0m);
        builder.Property(b => b.TotalDeposit).HasPrecision(18, 4).HasDefaultValue(0m);
        builder.Property(b => b.TotalTransfer).HasPrecision(18, 4).HasDefaultValue(0m);


        builder.Property(b => b.IsPosted).HasDefaultValue(false);
        builder.Property(b => b.IsLoan).HasDefaultValue(false);
        builder.Property(b => b.IsPayment).HasDefaultValue(false);
        builder.Property(b => b.IsClosed).HasDefaultValue(false);

        builder.Property(b => b.TypeNumeral).HasDefaultValue(SD.QuotationType.Buy)
            .HasSentinel(default);
        builder.Property(b => b.CurrencyDepositType).HasDefaultValue(SD.CurrencyType.Base)
            .HasSentinel(default);
        builder.Property(b => b.CurrencyTransaType).HasDefaultValue(SD.CurrencyType.Base)
            .HasSentinel(default);
        builder.Property(b => b.ExchangeRateSourceType).HasDefaultValue(SD.ExchangeRateSourceType.BaseForeign)
            .HasSentinel(default);

        builder.HasIndex(x => new { x.CompanyId, x.TypeId, x.DateTransa, x.InternalSerial, x.Numeral }
                , "quotations_idx_2010")
            .IsUnique();

        builder.HasOne(x => x.TypeTrx)
            .WithMany()
            .HasForeignKey(x => x.TypeId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(true);

        builder.HasOne(x => x.CustomerTrx)
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(true);

        builder.HasOne(x => x.BankAccountSourceTrx)
            .WithMany()
            .HasForeignKey(x => x.BankAccountSourceId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(false);

        builder.HasOne(x => x.BankAccountTargetTrx)
            .WithMany()
            .HasForeignKey(x => x.BankAccountTargetId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(false);

        builder.HasOne(x => x.CurrencyDepositTrx)
            .WithMany()
            .HasForeignKey(x => x.CurrencyDepositId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(true);

        builder.HasOne(x => x.CurrencyTransferTrx)
            .WithMany()
            .HasForeignKey(x => x.CurrencyTransferId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(true);

        builder.HasOne(x => x.CurrencyTransaTrx)
            .WithMany()
            .HasForeignKey(x => x.CurrencyTransaId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(true);

        builder.HasOne(x => x.BusinessExecutiveTrx)
            .WithMany()
            .HasForeignKey(x => x.BusinessExecutiveId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(true);

    }
}