﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Xanes.Web.Models;

public class BankConfiguration:IEntityTypeConfiguration<Bank>
{
    public void Configure(EntityTypeBuilder<Bank> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.CompanyId, x.Code }
                , "banks_idx_2010")
            .IsUnique();

    }
}