using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Synchrowise.Core.Models;

namespace Synchrowise.Database.Configurations
{
    public class GroupMessageConfigurations : IEntityTypeConfiguration<GroupMessage>
    {
        public void Configure(EntityTypeBuilder<GroupMessage> builder)
        {
            builder.HasKey(msg => msg.Id);
            builder.Property(msg => msg.Id).UseIdentityColumn();
            builder.Property(msg => msg.Message).IsRequired();
            builder.HasOne(msg => msg.Group).WithMany(grp => grp.Messages);
            builder.HasOne(msg => msg.Sender).WithMany(snd => snd.Messages);
        }
    }
}