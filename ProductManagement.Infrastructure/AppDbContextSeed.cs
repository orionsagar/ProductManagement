using ProductManagement.Domain.Entities;

namespace ProductManagement.Infrastructure
{
    public static class AppDbContextSeed
    {
        public static async Task SeedSampleDataAsync(AppDbContext context)
        {
            if (!context.Products.Any())
            {
                var product = new Product
                {
                    Name = "WidgetX",
                    Version = "1.0",
                    Description = "Mainline Widget"
                };
                context.Products.Add(product);

                var project = new Project
                {
                    Name = "Project Alpha",
                    Description = "Initial production",
                    Product = product
                };
                context.Projects.Add(project);

                context.ProductItems.AddRange(
                    new ProductItem { Name = "Product Item 1", Description = "Product Item 1 Description", Status = ProductItemStatus.Pending, Project = project },
                    new ProductItem { Name = "Product Item 2", Description = "Product Item 2 Description", Status = ProductItemStatus.InProgress, Project = project },
                    new ProductItem { Name = "Product Item 3", Description = "Product Item 3 Description", Status = ProductItemStatus.Completed, Project = project }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
