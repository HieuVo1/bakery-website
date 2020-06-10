using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
    public class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
    {
        public void Configure(EntityTypeBuilder<Promotion> builder)
        {
            builder.ToTable("Promotions");
            builder.HasKey(p => p.Id);
            builder.HasAlternateKey(p => p.Code);
            builder.Property(p => p.FromDate).IsRequired(true);
            builder.Property(p => p.ToDate).IsRequired(true);
           
        }
    }
}
