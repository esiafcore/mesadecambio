﻿using Microsoft.EntityFrameworkCore;
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

    }
}