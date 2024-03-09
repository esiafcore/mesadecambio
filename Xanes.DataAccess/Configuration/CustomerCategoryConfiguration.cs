using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xanes.Models;

namespace Xanes.DataAccess.Configuration;

public class CustomerCategoryConfiguration : IEntityTypeConfiguration<CustomerCategory>
{
    public void Configure(EntityTypeBuilder<CustomerCategory> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CompanyId).ValueGeneratedOnAdd().HasDefaultValue(1);
        builder.Property(x => x.IsBank).ValueGeneratedOnAdd().HasDefaultValue(false);


        builder.HasIndex(x => new { x.CompanyId, x.Code }
                , "customerscategories_idx_2010")
            .IsUnique();

        builder.HasIndex(x => new { x.CompanyId, x.Numeral }
                , "customerscategories_idx_2020")
            .IsUnique();
    }
}