using System.ComponentModel.DataAnnotations;

namespace Location.Tracking.Domain.Entities
{
    public class Workspace
    {
        [Key]
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public List<User> Users { get; } = new();
    }
}
