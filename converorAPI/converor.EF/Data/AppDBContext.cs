using converor.Core.Models;
using converor.EF.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace converor.EF.Data
{
    public class AppDBContext : IdentityDbContext<ApplicationUser>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options): base(options)
        {
                
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // apply FolderConfiguration
            builder.ApplyConfiguration(new FolderConfiguration());
            builder.ApplyConfiguration(new FileConfiguration());
        }
        public virtual DbSet<FileContent> Files { get; set; }
        public virtual DbSet<FileDescription> FileDescriptions { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<Folder> Folders {get; set;}
        public virtual DbSet<AuthenticationToken> AuthenticationTokens { get; set; }
    }
}
