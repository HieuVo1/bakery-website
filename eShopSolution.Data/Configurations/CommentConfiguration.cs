using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
    class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Content).IsRequired(true);
            builder.Property(x => x.Created_At)
            .IsRequired()
            .HasColumnType("Date")
            .HasDefaultValueSql("GetDate()");
            builder.HasOne(x => x.UserApp).WithMany(u => u.Comments).HasForeignKey(x => x.UserId);
        }
    }
}
