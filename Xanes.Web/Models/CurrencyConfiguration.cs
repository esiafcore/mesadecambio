using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Xanes.Web.Models;

public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new {x.CompanyId, x.CodeIso }
            ,"currencies_idx_2010")
            .IsUnique();

        builder.HasIndex(x => new { x.CompanyId, x.Code}
                , "currencies_idx_2020")
            .IsUnique();

        builder.HasIndex(x => new { x.CompanyId, x.Abbreviation}
                , "currencies_idx_2030")
            .IsUnique();

    }

}