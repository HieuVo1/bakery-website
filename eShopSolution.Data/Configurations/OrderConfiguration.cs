using eShopSolution.Data.Entities;
using eShopSolution.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ShipName).IsRequired();
            builder.Property(x => x.ShipAddress).IsRequired();
            builder.Property(x => x.ShipPhone).IsRequired();
            builder.Property(x => x.ShipEmail).IsRequired().IsUnicode(false).HasMaxLength(50);
            builder.Property(x => x.Status).IsRequired().HasDefaultValue(OrderStatus.InStock);
            builder.Property(x => x.Created_At)
            .IsRequired()
            .HasColumnType("Date")
            .HasDefaultValueSql("GetDate()");
            builder.HasOne(o => o.Promotion)
               .WithOne(p => p.Order)
               .HasForeignKey<Order>(o => o.PromotionId);
        }
    }
}
