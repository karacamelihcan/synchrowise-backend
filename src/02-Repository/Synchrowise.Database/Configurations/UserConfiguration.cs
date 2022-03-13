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
            builder.Property(x => x.Firebase_uid).IsRequired();
            builder.HasIndex(x=> x.Firebase_uid).IsUnique();
            builder.Property(x => x.Username).HasMaxLength(50);
            builder.HasIndex(x=> x.Username).IsUnique();
            builder.Property(x => x.Email).IsRequired();
        }
    }
}