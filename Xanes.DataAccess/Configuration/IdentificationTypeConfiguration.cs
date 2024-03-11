using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xanes.Models;
using Xanes.Utility;

namespace Xanes.DataAccess.Configuration;

public class IdentificationTypeConfiguration : IEntityTypeConfiguration<IdentificationType>
{
    public void Configure(EntityTypeBuilder<IdentificationType> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CompanyId).ValueGeneratedOnAdd().HasDefaultValue(1);

        builder.HasIndex(x => new { x.CompanyId, x.Code }
                , "identificationstypes_idx_2010")
            .IsUnique();

        builder.HasIndex(x => new { x.CompanyId, Numeral = x.Numeral }
                , "identificationstypes_idx_2020")
            .IsUnique();

        builder.HasData(
            new IdentificationType
            {
                Id = 1,
                CompanyId = 1,
                Numeral = (int)SD.IdentificationTypeNumber.RUC,
                Code = "RUC",
                Name = "Registro Único Cotnribuyente",
                IsLegal = true,
                IsForeign = false,
                RegularExpressionNumber = "([J0-9]\\\\d{12}[a-zA-Z0-9])",
                FormatExpressionNumber = "$1",
                SubstitutionExpressionNumber = "$1",
                IdentificationMaxLength = 14,
                IsActive = true,
            },
            new IdentificationType
            {
                Id = 2,
                CompanyId = 1,
                Numeral = (int)SD.IdentificationTypeNumber.CEDU,
                Code = "CEDU",
                Name = "Cédula de Identificación",
                IsLegal = false,
                IsForeign = false,
                RegularExpressionNumber = "(\\d{3})-*?(\\d{6})-*?(\\d{4}\\w{1})",
                FormatExpressionNumber = "$1-$2-$3",
                SubstitutionExpressionNumber = "$1$2$3",
                IdentificationMaxLength = 14,
                IsActive = true,
            },
            new IdentificationType
            {
                Id = 3,
                CompanyId = 1,
                Numeral = (int)SD.IdentificationTypeNumber.DIMEX,
                Code = "DIMEX",
                Name = "Documento de Identidad Migratorio para Extranjeros",
                IsLegal = false,
                IsForeign = true,
                RegularExpressionNumber = string.Empty,
                FormatExpressionNumber = string.Empty,
                SubstitutionExpressionNumber = string.Empty,
                IdentificationMaxLength = 0,
                IsActive = false,
            },
            new IdentificationType
            {
                Id = 4,
                CompanyId = 1,
                Numeral = (int)SD.IdentificationTypeNumber.NITE,
                Code = "NITE",
                Name = "Número de Identificación Tributaria Especial",
                IsLegal = false,
                IsForeign = false,
                RegularExpressionNumber = string.Empty,
                FormatExpressionNumber = string.Empty,
                SubstitutionExpressionNumber = string.Empty,
                IdentificationMaxLength = 0,
                IsActive = false,
            },
            new IdentificationType
            {
                Id = 5,
                CompanyId = 1,
                Numeral = (int)SD.IdentificationTypeNumber.DIDI,
                Code = "DIDI",
                Name = "Documento de Identificación para Diplomático",
                IsLegal = false,
                IsForeign = true,
                RegularExpressionNumber = string.Empty,
                FormatExpressionNumber = string.Empty,
                SubstitutionExpressionNumber = string.Empty,
                IdentificationMaxLength = 0,
                IsActive = false,
            },
            new IdentificationType
            {
                Id = 6,
                CompanyId = 1,
                Numeral = (int)SD.IdentificationTypeNumber.PASS,
                Code = "PASS",
                Name = "Pasaporte",
                IsLegal = false,
                IsForeign = true,
                RegularExpressionNumber = string.Empty,
                FormatExpressionNumber = string.Empty,
                SubstitutionExpressionNumber = string.Empty,
                IdentificationMaxLength = 0,
                IsActive = true,
            }
            );
    }
}