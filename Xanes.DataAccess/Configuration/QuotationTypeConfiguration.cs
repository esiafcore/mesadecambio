using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xanes.Models;

namespace Xanes.DataAccess.Configuration;

public class QuotationTypeConfiguration : IEntityTypeConfiguration<QuotationType>
{
    public void Configure(EntityTypeBuilder<QuotationType> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CompanyId).ValueGeneratedOnAdd().HasDefaultValue(1);

        builder.HasIndex(x => new { x.CompanyId, x.Code }
                , "quotationstypes_idx_2010")
            .IsUnique();

        builder.HasIndex(x => new { x.CompanyId, x.Numeral}
                , "quotationstypes_idx_2020")
            .IsUnique();

        builder.HasData(
            new QuotationType
            {
                Id = 1,
                CompanyId = 1,
                Numeral = 1,
                Code = "COM",
                Name = "COMPRA",
                OrderSequence = 10
            },
            new QuotationType
            {
                Id = 2,
                CompanyId = 1,
                Numeral = 2,
                Code = "VTA",
                Name = "VENTA",
                OrderSequence = 20
            },
            new QuotationType
            {
                Id = 3,
                CompanyId = 1,
                Numeral = 4,
                Code = "TRF",
                Name = "TRANSFERENCIA",
                OrderSequence = 30
            }
            );
    }
}