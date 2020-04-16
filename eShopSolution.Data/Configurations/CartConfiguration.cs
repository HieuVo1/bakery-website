using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Configurations
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Carts");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Price).IsRequired(true);
            builder.Property(x => x.Created_At)
            .IsRequired()
            .HasColumnType("Date")
            .HasDefaultValueSql("GetDate()");
            builder.HasOne(x => x.UserApp).WithMany(u => u.Carts).HasForeignKey(x => x.UserId);
        }
    }
}
