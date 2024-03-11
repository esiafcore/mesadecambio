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

        builder.HasData(
                    new CustomerCategory
                    {
                        Id = 1,
                        CompanyId = 1,
                        Numeral = 1,
                        Code = "BAN",
                        Name = "Bancos",
                        IsBank = true,
                        IsActive = true,
                    },
                    new CustomerCategory
                    {
                        Id = 2,
                        CompanyId = 1,
                        Numeral = 2,
                        Code = "FIN",
                        Name = "Financieras",
                        IsBank = true,
                        IsActive = true,
                    },
                    new CustomerCategory
                    {
                        Id = 3,
                        CompanyId = 1,
                        Numeral = 3,
                        Code = "IND",
                        Name = "Industrias",
                        IsBank = false,
                        IsActive = true,
                    },
                    new CustomerCategory
                    {
                        Id = 4,
                        CompanyId = 1,
                        Numeral = 4,
                        Code = "ONG",
                        Name = "ONG",
                        IsBank = false,
                        IsActive = true,
                    },
                    new CustomerCategory
                    {
                        Id = 5,
                        CompanyId = 1,
                        Numeral = 5,
                        Code = "UNI",
                        Name = "Universidades",
                        IsBank = false,
                        IsActive = true,
                    },
                    new CustomerCategory
                    {
                        Id = 6,
                        CompanyId = 1,
                        Numeral = 6,
                        Code = "COM",
                        Name = "Comercial",
                        IsBank = false,
                        IsActive = true,
                    },
                    new CustomerCategory
                    {
                        Id = 7,
                        CompanyId = 1,
                        Numeral = 7,
                        Code = "FAM",
                        Name = "Farmacias",
                        IsBank = false,
                        IsActive = true,
                    },
                    new CustomerCategory
                    {
                        Id = 8,
                        CompanyId = 1,
                        Numeral = 8,
                        Code = "TEC",
                        Name = "Tecnológicos",
                        IsBank = false,
                        IsActive = true,
                    },
                    new CustomerCategory
                    {
                        Id = 9,
                        CompanyId = 1,
                        Numeral = 9,
                        Code = "OTR",
                        Name = "Otros",
                        IsBank = false,
                        IsActive = true,
                    },
                    new CustomerCategory
                    {
                        Id = 10,
                        CompanyId = 1,
                        Numeral = 10,
                        Code = "SER",
                        Name = "Servicios",
                        IsBank = false,
                        IsActive = true,
                    });
    }
}