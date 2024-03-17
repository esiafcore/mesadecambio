using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xanes.Models;

namespace Xanes.DataAccess.Configuration;

public class CustomerSectorConfiguration:IEntityTypeConfiguration<CustomerSector>
{
    public void Configure(EntityTypeBuilder<CustomerSector> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CompanyId).ValueGeneratedOnAdd().HasDefaultValue(1);
        builder.Property(x => x.ParentId).IsRequired(false);

        builder.HasIndex(x => new { x.CompanyId, x.Code }
                , "customerssectors_idx_2010")
            .IsUnique();
    }
}