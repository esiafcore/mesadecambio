using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Xanes.Web.Models;

public class QuotationTypeConfiguration : IEntityTypeConfiguration<QuotationType>
{
    public void Configure(EntityTypeBuilder<QuotationType> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.CompanyId, x.Code }
                , "quotationstypes_idx_2010")
            .IsUnique();

        builder.HasIndex(x => new { x.CompanyId, x.Numeral}
                , "quotationstypes_idx_2020")
            .IsUnique();
    }
}