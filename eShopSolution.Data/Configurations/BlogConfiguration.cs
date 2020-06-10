using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
    public class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.ToTable("Blogs");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Content).IsRequired(true);
            builder.Property(x => x.Created_At)
            .IsRequired()
            .HasColumnType("Date")
            .HasDefaultValueSql("GetDate()");
            builder.HasOne(x => x.UserApp).WithMany(u => u.Blogs).HasForeignKey(x => x.UserId);
            builder.HasOne(x => x.Categories).WithMany(u => u.Blogs).HasForeignKey(x => x.CategoryId);
        }
    }
}
