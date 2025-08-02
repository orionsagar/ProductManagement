using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ProductManagement.Infrastructure.Helpers;

namespace ProductManagement.Infrastructure
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // Build configuration from appsettings.json in startup project
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // points to startup-project during EF CLI runs
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = "Host=localhost;Port=5432;Database=ProductManagementDb;Username=postgres;Password=123";
            //AppConfig.ConnectionString; //configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(connectionString); // or UseSqlServer, etc.

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
