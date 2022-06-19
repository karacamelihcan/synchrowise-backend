using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Synchrowise.Core.Models;

namespace Synchrowise.Database.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.UserId);
            builder.Property(x => x.UserId).UseIdentityColumn();
            builder.Property(x => x.Firebase_uid).IsRequired();
            builder.HasIndex(x=> x.Firebase_uid).IsUnique();
            builder.Property(x => x.Username).HasMaxLength(50);
            builder.HasIndex(x=> x.Username).IsUnique();
            builder.Property(x => x.Email).IsRequired();

            builder.HasOne( usr => usr.Avatar).WithOne(img => img.Owner).HasForeignKey<User>(usr => usr.AvatarID);
            builder.HasOne( usr => usr.Notifications).WithOne(ntf => ntf.Owner).HasForeignKey<User>(usr => usr.UserId);
            builder.HasMany(usr => usr.Messages).WithOne(msg => msg.Sender);
        }
    }
}