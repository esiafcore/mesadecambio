using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xanes.Models;

namespace Xanes.DataAccess.Configuration;

public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CompanyId).ValueGeneratedOnAdd().HasDefaultValue(1);

        builder.HasIndex(x => new {x.CompanyId, x.CodeIso }
            ,"currencies_idx_2010")
            .IsUnique();

        builder.HasIndex(x => new { x.CompanyId, x.Code}
                , "currencies_idx_2020")
            .IsUnique();

        builder.HasIndex(x => new { x.CompanyId, x.Abbreviation}
                , "currencies_idx_2030")
            .IsUnique();

        builder.HasIndex(x => new { x.CompanyId, x.Numeral }
                , "currencies_idx_2040")
            .IsUnique();

        builder.HasData(
            new Currency
            {
                Id = 1,
                CompanyId = 1,
                Numeral = 1,
                CodeIso = "NIO",
                Code = "COR",
                Abbreviation = "C$",
                Name = "CORDOBAS",
                NameSingular = "CORDOBA",
                NameForeign = "CORDOBAS",
                NameForeignSingular = "CORDOBA",
                IsActive = true
            },
            new Currency
            {
                Id = 2,
                CompanyId = 1,
                Numeral = 2,
                CodeIso = "USD",
                Code = "USD",
                Abbreviation = "U$",
                Name = "DOLARES",
                NameSingular = "DOLAR",
                NameForeign = "DOLLARS",
                NameForeignSingular = "DOLLAR",
                IsActive = true
            },
            new Currency
            {
                Id = 4,
                CompanyId = 1,
                Numeral = 4,
                CodeIso = "EUR",
                Code = "EUR",
                Abbreviation ="\u20ac",
                Name = "EUROS",
                NameSingular = "EURO",
                NameForeign = "EUROS",
                NameForeignSingular = "EURO",
                IsActive = true
            }
            );
    }

}