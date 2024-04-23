using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xanes.Models;

namespace Xanes.DataAccess.Configuration;

public class ConfigFacConfiguration : IEntityTypeConfiguration<ConfigFac>
{
    public void Configure(EntityTypeBuilder<ConfigFac> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CompanyId).ValueGeneratedOnAdd().HasDefaultValue(1);
        builder.Property(x => x.SequentialNumberQuotation).ValueGeneratedOnAdd().HasDefaultValue(0);
        builder.Property(x => x.SequentialNumberDraftQuotation).ValueGeneratedOnAdd().HasDefaultValue(0);
        builder.Property(x => x.IsAutomaticallyQuotationCode).ValueGeneratedOnAdd().HasDefaultValue(true);

        builder.HasIndex(x => new { x.CompanyId }
                , "configsfac_idx_2010")
            .IsUnique();

        builder.HasData(
            new ConfigFac
            {
                Id = 1,
                CompanyId = 1,
                SequentialNumberQuotation = 0,
                SequentialNumberDraftQuotation = 0,
                IsAutomaticallyQuotationCode = true
            });
    }
}