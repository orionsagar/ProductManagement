using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Domain.Entities;
using IdentityRole = Microsoft.AspNetCore.Identity.IdentityRole;

namespace ProductManagement.Infrastructure
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAndAdmin(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { "Admin", "ProductManager", "ProjectManager", "ProductionEngineer" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Seed admin user
            var adminEmail = "admin@system.com";
            var adminPassword = "Admin123!";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create admin user: {errors}");
                }
            }
        }
    }


    //public static class DbInitializer
    //{
    //    public static async Task SeedRolesAndAdmin(IServiceProvider serviceProvider)
    //    {
    //        using var scope = serviceProvider.CreateScope();
    //        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    //        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    //        string[] roles = { "Admin", "ProductManager", "ProjectManager", "ProductionEngineer" };
    //        foreach (var role in roles)
    //        {
    //            if (!await roleManager.RoleExistsAsync(role))
    //                await roleManager.CreateAsync(new IdentityRole(role));
    //        }

    //        // Seed admin user
    //        var adminEmail = "admin@system.com";
    //        var adminPassword = "Admin123!";

    //        var adminUser = await userManager.FindByEmailAsync(adminEmail);
    //        if (adminUser == null)
    //        {
    //            adminUser = new ApplicationUser { 
    //                UserName = adminEmail, 
    //                Email = adminEmail, 
    //                EmailConfirmed = true 
    //            };
    //            var result = await userManager.CreateAsync(adminUser, adminPassword);
    //            if (result.Succeeded)
    //            {
    //                await userManager.AddToRoleAsync(adminUser.Id, "Admin");
    //            }
    //            else
    //            {
    //                var errors = string.Join(", ", result.Errors.Select(e => e.FirstOrDefault()));
    //                throw new Exception($"Failed to create admin user: {errors}");
    //            }
    //        }
    //    }
    //}
}
