namespace ProductManagement.Domain.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;        // Who performed the action
        public string UserName { get; set; } = null!;      // Optional: human-readable name

        public string EntityName { get; set; } = null!;    // e.g., "Product", "ProjectItem"
        public string EntityId { get; set; } = null!;      // The ID of the affected entity

        public string Action { get; set; } = null!;        // e.g., "Created", "Updated", "Deleted"

        public string? Changes { get; set; }                // JSON string describing changes

        public DateTime Timestamp { get; set; }
    }

}
