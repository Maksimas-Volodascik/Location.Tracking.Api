using Location.Tracking.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Location.Tracking.Domain.Entities
{
    public class Invitation
    {
        [Key]
        public Guid Id { get; set; } = Guid.CreateVersion7();
        public Guid CreatedByUserId { get; set; }
        public UserRole Role { get; set; } = UserRole.Demo;
        public string TokenHash { get; set; } = null!;
        public DateTimeOffset ExpiresAt { get; set; }
        public DateTimeOffset? UsedAt { get; set; } 
        public Guid WorkspaceId { get; set; }
        public Workspace Workspace { get; set; } = null!;
    }
}
