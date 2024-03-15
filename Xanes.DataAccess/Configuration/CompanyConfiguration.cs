using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xanes.Models;
using Xanes.Utility;

namespace Xanes.DataAccess.Configuration;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.CountryNumber, x.IdentificationNumber}
                , "companies_idx_2010")
            .IsUnique();

        builder.HasIndex(x => new { x.CountryCode, x.IdentificationNumber }
                , "companies_idx_2020")
            .IsUnique();

        builder.HasData(
            new Company
            {
                Id = 1,
                Name = "Factoring S.A.",
                CommercialName = "Factoring S.A.",
                BusinessName = "Factoring S.A.",
                CountryNumber = SD.CountryAlpha03.NIC,
                CountryCode = "NIC",
                Website = "https://factoring.com.ni",
                IdentificationNumber = "J0310000031339",
                PhoneNumber = "+505 22782272",
                AddressPrimary = "Portón Principal del Colegio Teresiano 1/2c.al este. Managua, Nicaragua",
                IsActive = true,
                ImageLogoUrl = "https://localhost:7102/CompanyImagesLogo/8071a404-950c-4df1-9875-27db4f4a4c26.jpg",
                ImageLogoLocalPath = "wwwroot/CompanyImagesLogo/8071a404-950c-4df1-9875-27db4f4a4c26.jpg"
            });

    }
}