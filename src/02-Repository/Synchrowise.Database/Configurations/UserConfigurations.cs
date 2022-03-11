using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Synchrowise.Core.Models;

namespace Synchrowise.Database.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Firebase_Id).IsRequired();
            builder.HasIndex(x=> x.Firebase_Id).IsUnique();
            builder.Property(x => x.Username).IsRequired().HasMaxLength(20);
            builder.HasIndex(x=> x.Username).IsUnique();
            builder.Property(x => x.DisplayName).IsRequired().HasMaxLength(30);
            builder.Property(x => x.Email).IsRequired();

        }
    }
}