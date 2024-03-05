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
    }
}