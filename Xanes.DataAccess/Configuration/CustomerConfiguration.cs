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
        builder.Property(x => x.InternalSerial).ValueGeneratedOnAdd().HasDefaultValue(AC.InternalSerialOfficial);
        builder.Property(x => x.BusinessExecutiveId).IsRequired(false);

        builder.HasIndex(x => new { x.CompanyId, x.InternalSerial, x.Code }
                , "customers_idx_2010")
            .IsUnique();

        builder.HasIndex(x => new { x.CompanyId, x.TypeId, x.IdentificationNumber }
                , "customers_idx_2020")
            .IsUnique();

        builder.HasOne(x => x.IdentificationTypeTrx)
            .WithMany()
            .HasForeignKey(x => x.IdentificationTypeId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(true);

        builder.HasOne(x => x.TypeTrx)
            .WithMany()
            .HasForeignKey(x => x.TypeId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(true);

        builder.HasOne(x => x.SectorTrx)
            .WithMany()
            .HasForeignKey(x => x.SectorId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(true);

        builder.HasOne(x => x.BusinessExecutiveTrx)
            .WithMany()
            .HasForeignKey(x => x.BusinessExecutiveId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(false);
    }
}