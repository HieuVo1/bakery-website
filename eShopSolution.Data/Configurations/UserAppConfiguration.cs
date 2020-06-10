using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
    public class UserAppConfiguration : IEntityTypeConfiguration<UserApp>
    {
        public void Configure(EntityTypeBuilder<UserApp> builder)
        {
            builder.ToTable("Users");
            builder.Property(c => c.Dob).IsRequired(true);
            builder.HasOne(o => o.RoleApp).WithMany(u => u.Users).HasForeignKey(o => o.RoleID);
        }
    }
}
