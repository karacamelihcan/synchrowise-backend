using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Synchrowise.Core.Models;

namespace Synchrowise.Database
{   
    public class SynchrowiseDbContext : DbContext
    {
        public SynchrowiseDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserAvatar> UserAvatars { get; set; }
        public DbSet<GroupFile> GroupFiles { get; set; }
        public DbSet<NotificationSettings> Notifications { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}