using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xanes.Models;

namespace Xanes.DataAccess.Configuration;

public class CurrencyExchangeRateConfiguration : IEntityTypeConfiguration<CurrencyExchangeRate>
{
    public void Configure(EntityTypeBuilder<CurrencyExchangeRate> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CompanyId).ValueGeneratedOnAdd().HasDefaultValue(1);

        builder.HasIndex(x => new { x.CompanyId, x.CurrencyId, x.DateTransa }
                , "currenciesexchangerates_idx_2010")
            .IsUnique();

        builder.HasIndex(x => new { x.CompanyId, x.CurrencyType, x.DateTransa }
                , "currenciesexchangerates_idx_2020")
            .IsUnique();

        builder.Property(b => b.OfficialRate).HasPrecision(18, 8);
        builder.Property(b => b.BuyRate).HasPrecision(18, 8);
        builder.Property(b => b.SellRate).HasPrecision(18, 8);

    }
}