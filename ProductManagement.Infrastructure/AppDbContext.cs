using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Entities;

namespace ProductManagement.Infrastructure
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<ProductItem> ProductItems => Set<ProductItem>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Needed for Identity schema
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            modelBuilder.Entity<Product>()
           .HasMany(p => p.Projects)
           .WithOne(p => p.Product)
           .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.ProductItems)
                .WithOne(p => p.Project)
                .HasForeignKey(p => p.ProjectId);
        }
    }
}
