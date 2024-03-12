using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xanes.Models;
using Xanes.Utility;

namespace Xanes.DataAccess.Configuration;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CompanyId).ValueGeneratedOnAdd().HasDefaultValue(1);

        builder.HasIndex(x => new { x.CompanyId, x.Code }
                , "customers_idx_2010")
            .IsUnique();

        builder.HasIndex(x => new { x.CompanyId, x.TypeId, x.Identificationnumber }
                , "customers_idx_2020")
            .IsUnique();

        builder.HasIndex(x => new { x.CompanyId, x.Code }
                , "customers_idx_2030")
            .IsUnique();

        builder.HasOne(x => x.CategoryTrx)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(true);

        builder.HasOne(x => x.TypeTrx)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(true);

        builder.HasData(
            new Customer
            {
                Id = 5809,
                CompanyId = 1,
                Code = "00803",
                CategoryId = 6, //COMERCIAL
                CategoryNumeral = 6,
                TypeId = 1, //JURIDICO
                TypeNumeral = 1,
                Identificationnumber = "J0310000122865",
                FirstName = string.Empty,
                SecondName = string.Empty,
                LastName = string.Empty,
                SecondSurname = string.Empty,
                BusinessName = "AMERICAN PHARMA",
                CommercialName = "AMERICAN PHARMA",
                IsBank = false,
                IsSystemRow = false,
                IsActive = true
            },
            new Customer
            {
                Id = 5808,
                CompanyId = 1,
                Code = "00802",
                CategoryId = 6,//COMERCIAL
                CategoryNumeral = 6,
                TypeId = 2, //NATURAL
                TypeNumeral = 2,
                Identificationnumber = "0013009870051Y",
                FirstName = "MIGUEL",
                SecondName = "FERNANDO",
                LastName = "RAMIREZ",
                SecondSurname = "OCON",
                BusinessName = "MIGUEL FERNANDO RAMIREZ OCON",
                CommercialName = "MIGUEL FERNANDO RAMIREZ OCON",
                IsBank = false,
                IsSystemRow = false,
                IsActive = true
            },
            new Customer
            {
                Id = 5807,
                CompanyId = 1,
                Code = "00801",
                CategoryId = 6,//COMERCIAL
                CategoryNumeral = 6,
                TypeId = 2, //NATURAL
                TypeNumeral = 2,
                Identificationnumber = "244686858",
                FirstName = "JIMMY",
                SecondName = "ALEXANDER",
                LastName = "SANDOVAL",
                SecondSurname = "FRANCO",
                BusinessName = "JIMMY ALEXANDER SANDOVAL FRANCO",
                CommercialName = "JIMMY ALEXANDER SANDOVAL FRANCO",
                IsBank = false,
                IsSystemRow = false,
                IsActive = true
            },
            new Customer
            {
                Id = 5806,
                CompanyId = 1,
                Code = "00800",
                CategoryId = 6, //COMERCIAL
                CategoryNumeral = 6,
                TypeId = 1, //JURIDICO
                TypeNumeral = 1,
                Identificationnumber = "J0310000441430",
                FirstName = string.Empty,
                SecondName = string.Empty,
                LastName = string.Empty,
                SecondSurname = string.Empty,
                BusinessName = "INSUMOS SMART NICARAGUA SOCIEDAD ANONIMA",
                CommercialName = "INSUMOS SMART NICARAGUA SOCIEDAD ANONIMA",
                IsBank = false,
                IsSystemRow = false,
                IsActive = true
            },
            new Customer
            {
                Id = 5805,
                CompanyId = 1,
                Code = "00799",
                CategoryId = 6,//COMERCIAL
                CategoryNumeral = 6,
                TypeId = 2, //NATURAL
                TypeNumeral = 2,
                Identificationnumber = "0012206860039E",
                FirstName = "MEYLING",
                SecondName = "RAQUEL",
                LastName = "SANCHEZ",
                SecondSurname = "ORTIZ",
                BusinessName = "MEYLING RAQUEL SANCHEZ ORTIZ",
                CommercialName = "MEYLING RAQUEL SANCHEZ ORTIZ",
                IsBank = false,
                IsSystemRow = false,
                IsActive = true
            }
            );
    }
}