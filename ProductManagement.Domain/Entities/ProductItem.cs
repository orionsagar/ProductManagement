namespace ProductManagement.Domain.Entities
{
    public class ProductItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public ProductItemStatus Status { get; set; } = ProductItemStatus.Pending;

        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!;
    }
}
