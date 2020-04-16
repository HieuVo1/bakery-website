using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Configurations
{
    public class RoleAppConfiguration : IEntityTypeConfiguration<RoleApp>
    {

        public void Configure(EntityTypeBuilder<RoleApp> builder)
        {
            builder.ToTable("Roles");
            builder.Property(c => c.Description).IsRequired(true);
        }

    }
}
