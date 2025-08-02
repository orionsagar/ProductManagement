namespace ProductManagement.Domain.Entities
{
    public class Project
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public List<ProductItem> ProductItems { get; set; } = new();
    }
}
