using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Reviews");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Content).IsRequired(true);
            builder.Property(c => c.Name).IsRequired(true);
            builder.Property(c => c.Rating).IsRequired(true);
            builder.Property(c => c.Email).IsRequired(true);
            builder.Property(x => x.Created_At)
            .IsRequired()
            .HasColumnType("Date")
            .HasDefaultValueSql("GetDate()");
        }
    }
}
