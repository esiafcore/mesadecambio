using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xanes.Models;

namespace Xanes.DataAccess.Configuration;

public class QuotationConfiguration : IEntityTypeConfiguration<Quotation>
{
    public void Configure(EntityTypeBuilder<Quotation> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CompanyId).ValueGeneratedOnAdd().HasDefaultValue(1);

        builder.HasIndex(x => new { x.CompanyId, x.TypeId, x.DateTransa ,x.Numeral }
                , "quotations_idx_2010")
            .IsUnique();

        builder.Property(b => b.ExchangeRateOfficialTransa).HasPrecision(18, 8);
        builder.Property(b => b.ExchangeRateBuyTransa).HasPrecision(18, 8);
        builder.Property(b => b.ExchangeRateSellTransa).HasPrecision(18, 8);
        builder.Property(b => b.ExchangeRateOfficialReal).HasPrecision(18, 8);
        builder.Property(b => b.ExchangeRateBuyReal).HasPrecision(18, 8);
        builder.Property(b => b.ExchangeRateSellReal).HasPrecision(18, 8);

        builder.Property(b => b.AmountTransa).HasPrecision(18, 4);
        builder.Property(b => b.AmountRevenue).HasPrecision(18, 4);
        builder.Property(b => b.AmountCost).HasPrecision(18, 4);
        builder.Property(b => b.IsPosted).HasDefaultValue(false);
        builder.Property(b => b.IsLoan).HasDefaultValue(false);
        builder.Property(b => b.IsPayment).HasDefaultValue(false);

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

        builder.HasOne(x => x.CurrencyOriginExchangeTrx)
            .WithMany()
            .HasForeignKey(x => x.CurrencyOriginExchangeId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(true);

        builder.HasOne(x => x.CurrencyTransaTrx)
            .WithMany()
            .HasForeignKey(x => x.CurrencyTransaId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(true);
    }
}