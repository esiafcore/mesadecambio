using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xanes.Models;

namespace Xanes.DataAccess.Configuration;

public class ConfigCxcConfiguration : IEntityTypeConfiguration<ConfigCxc>
{
    public void Configure(EntityTypeBuilder<ConfigCxc> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CompanyId).ValueGeneratedOnAdd().HasDefaultValue(1);
        builder.Property(x => x.SequentialNumberCustomer).ValueGeneratedOnAdd().HasDefaultValue(0);
        builder.Property(x => x.SequentialNumberDraftCustomer).ValueGeneratedOnAdd().HasDefaultValue(0);
        builder.Property(x => x.IsAutomaticallyCustomerCode).ValueGeneratedOnAdd().HasDefaultValue(false);

        builder.HasIndex(x => new { x.CompanyId }
                , "configscxc_idx_2010")
            .IsUnique();

        builder.HasData(
            new ConfigCxc
            {
                Id = 1,
                CompanyId = 1,
                SequentialNumberCustomer = 0,
                SequentialNumberDraftCustomer = 0,
                IsAutomaticallyCustomerCode = true
            });
    }
}