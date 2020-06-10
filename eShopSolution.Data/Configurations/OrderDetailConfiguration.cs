using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetails");

            builder.HasKey(o => new { o.ProductId, o.OrderId });

            builder.HasOne(p => p.Order)
            .WithMany(b => b.OrderDetails)
            .HasForeignKey(p => p.OrderId);

            builder.HasOne(o => o.Product)
            .WithOne(p => p.OrderDetail)
            .HasForeignKey<OrderDetail>(o => o.ProductId);
        }
    }
}
