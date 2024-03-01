using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Xanes.Web.Models;

public class BankConfiguration : IEntityTypeConfiguration<Bank>
{
    public void Configure(EntityTypeBuilder<Bank> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CompanyId).ValueGeneratedOnAdd().HasDefaultValue(1);
        builder.Property(x => x.IsCompany).ValueGeneratedOnAdd().HasDefaultValue(false);
        builder.Property(x => x.OrderPriority).ValueGeneratedOnAdd().HasDefaultValue(1);
        builder.Property(x => x.BankingCommissionPercentage).ValueGeneratedOnAdd().HasDefaultValue(0);


        builder.HasIndex(x => new { x.CompanyId, x.Code }
                , "banks_idx_2010")
            .IsUnique();

        builder.HasData(
            new Bank
            {
                Id = 1,
                CompanyId = 1,
                Code = "BAC",
                Name = "Banco de America Central",
                BankingCommissionPercentage = 0,
                BankAccountExcludeUId = new Guid("234F2AD8-2A98-E911-B070-4CCC6A8AD00B"),
                IsCompany = false,
                OrderPriority = 0,
                LogoBank = "/Content/images/Bank/BacLogo.png"
            },

            new Bank
            {
                Id = 5,
                CompanyId = 1,
                Code = "FICOHSA",
                Name = "FICOHSA",
                BankingCommissionPercentage = 0,
                BankAccountExcludeUId = new Guid("530E22A8-2C98-E911-B070-4CCC6A8AD00B"),
                IsCompany = false,
                OrderPriority = 0,
                LogoBank = "/Content/images/Bank/FicohsaLogo.png"
            },
            new Bank
            {
                Id = 2,
                CompanyId = 1,
                Code = "BDF",
                Name = "Banco de Finanza",
                BankingCommissionPercentage = 0,
                BankAccountExcludeUId = null,
                IsCompany = false,
                OrderPriority = 0,
                LogoBank = "/Content/images/Bank/BdfLogo.png"
            },
            new Bank
            {
                Id = 3,
                CompanyId = 1,
                Code = "LAFISE",
                Name = "Bancentro",
                BankingCommissionPercentage = 0,
                BankAccountExcludeUId = null,
                IsCompany = false,
                OrderPriority = 0,
                LogoBank = "/Content/images/Bank/LafiseLogo.png"
            },
            new Bank
            {
                Id = 4,
                CompanyId = 1,
                Code = "ATLANT",
                Name = "ATLANTIDA",
                BankingCommissionPercentage = 0,
                BankAccountExcludeUId = null,
                IsCompany = false,
                OrderPriority = 0,
                LogoBank = "/Content/images/Bank/AtlantidaLogo.png"
            },
            new Bank
            {
                Id = 6,
                CompanyId = 1,
                Code = "BANPRO",
                Name = "BANPRO",
                BankingCommissionPercentage = 0,
                BankAccountExcludeUId = null,
                IsCompany = false,
                OrderPriority = 0,
                LogoBank = "/Content/images/Bank/BanproLogo.png"
            },
            new Bank
            {
                Id = 7,
                CompanyId = 1,
                Code = "AVANZ",
                Name = "AVANZ",
                BankingCommissionPercentage = 0,
                BankAccountExcludeUId = null,
                IsCompany = false,
                OrderPriority = 0,
                LogoBank = "/Content/images/Bank/AvanzLogo.png"
            }
            );
    }
}