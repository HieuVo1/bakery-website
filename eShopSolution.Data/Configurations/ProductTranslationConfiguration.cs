using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
    public class ProductTranslationConfiguration : IEntityTypeConfiguration<ProductTranslation>
    {
        public void Configure(EntityTypeBuilder<ProductTranslation> builder)
        {
            builder.ToTable("ProductTranslations");
            builder.HasKey(t => new { t.ProductId, t.LanguageId });

            builder.HasOne(pt => pt.Language)
            .WithMany(l => l.ProductTranslations)
            .HasForeignKey(pt => pt.LanguageId);

            builder.HasOne(pt => pt.Product)
            .WithMany(p => p.ProductTranslations)
            .HasForeignKey(pt => pt.ProductId);
        }
    }
}
