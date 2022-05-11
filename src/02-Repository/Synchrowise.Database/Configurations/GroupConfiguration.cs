using System;
using System.Collections.Generic;
using System.Linq;
using Synchrowise.Core.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Synchrowise.Database.Configurations
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.GroupName).IsRequired();

            builder.HasMany(x => x.Users).WithOne(x => x.Group);
            builder.HasMany(grp => grp.GroupFiles).WithOne(file => file.Group);

        }
    }
}