using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xanes.Models;

namespace Xanes.DataAccess.Configuration;
public class PersonTypeConfiguration : IEntityTypeConfiguration<PersonType>
{
    public void Configure(EntityTypeBuilder<PersonType> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CompanyId).ValueGeneratedOnAdd().HasDefaultValue(1);
        builder.Property(x => x.Numeral).ValueGeneratedOnAdd().HasDefaultValue(0);

        builder.HasIndex(x => new { x.CompanyId, x.Code }
                , "customerstypes_idx_2010")
            .IsUnique();

        builder.HasIndex(x => new { x.CompanyId, x.Numeral }
                , "customerstypes_idx_2020")
            .IsUnique();

        builder.HasData(
            new PersonType
            {
                Id = 1,
                CompanyId = 1,
                Numeral = 1,
                Code = "NAT",
                Name = "Natural"
            },
            new PersonType
            {
                Id = 2,
                CompanyId = 1,
                Numeral = 2,
                Code = "JUR",
                Name = "Jurídico"
            }
            );
    }
}