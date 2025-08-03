using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = null!;
        public string Version { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal? Price { get; set; }

        public List<Project> Projects { get; set; } = new();
    }
}
