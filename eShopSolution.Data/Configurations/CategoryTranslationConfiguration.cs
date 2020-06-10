using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
    public class CategoryTranslationConfiguration : IEntityTypeConfiguration<CategoryTranslation>
    {
        public void Configure(EntityTypeBuilder<CategoryTranslation> builder)
        {
            builder.ToTable("CategoryTranslations");
            builder.HasKey(t => new { t.CategoryId, t.LanguageId });

            builder.HasOne(ct => ct.Language)
            .WithMany(l => l.CategoryTranslations)
            .HasForeignKey(ct => ct.LanguageId);

            builder.HasOne(ct => ct.Category)
            .WithMany(c => c.CategoryTranslations)
            .HasForeignKey(ct => ct.CategoryId);
        }
    }
}
