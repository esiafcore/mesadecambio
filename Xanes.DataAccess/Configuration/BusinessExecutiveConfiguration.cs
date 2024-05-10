using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xanes.Models;

namespace Xanes.DataAccess.Configuration;

public class BusinessExecutiveConfiguration : IEntityTypeConfiguration<BusinessExecutive>
{
    public void Configure(EntityTypeBuilder<BusinessExecutive> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CompanyId).ValueGeneratedOnAdd().HasDefaultValue(1);

        builder.HasIndex(x => new { x.CompanyId, x.Code }
                , "businessexecutives_idx_2010")
            .IsUnique();


    }
}