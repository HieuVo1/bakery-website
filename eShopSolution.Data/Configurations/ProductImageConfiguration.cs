using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImages");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.ImagePath).IsRequired();

            builder.Property(p => p.IsDefault).IsRequired().HasDefaultValue(0);

            builder.HasOne(p => p.Product)
            .WithMany(c => c.ProductImages)
            .HasForeignKey(p => p.ProductId);
        }
    }
}
