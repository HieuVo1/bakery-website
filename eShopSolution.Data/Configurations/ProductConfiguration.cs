using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using eShopSolution.Data.Entities;

namespace eShopSolution.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Price).IsRequired();

            builder.Property(p => p.Name).IsRequired();

            builder.Property(p => p.Stock).IsRequired().HasDefaultValue(0);

            builder.Property(p => p.Created_At)
            .IsRequired()
            .HasColumnType("Date")
            .HasDefaultValueSql("GetDate()");

            builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId);

    
        }
    }

}

