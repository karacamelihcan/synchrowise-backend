using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Synchrowise.Core.Models;

namespace Synchrowise.Database.Configurations
{
    public class UserAvatarConfiguration : IEntityTypeConfiguration<UserAvatar>
    {
        public void Configure(EntityTypeBuilder<UserAvatar> builder)
        {
            builder.HasKey(img => img.Id);
            builder.Property(img => img.Id).UseIdentityColumn();
            builder.Property(img => img.Path).IsRequired();
            builder.HasOne(img => img.Owner).WithOne(usr => usr.Avatar).HasForeignKey<UserAvatar>(img => img.OwnerID);
        }
    }
}