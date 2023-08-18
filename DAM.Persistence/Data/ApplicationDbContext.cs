using DAM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAM.Persistence.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Organization> Organization { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Organization>().ToTable("Organizations");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Asset>().ToTable("Assets");
            modelBuilder.Entity<Role>().ToTable("Roles");
            base.OnModelCreating(modelBuilder);
        }
    }
}
