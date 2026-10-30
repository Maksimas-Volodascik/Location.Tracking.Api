using Location.Tracking.Domain.Enums;

namespace Location.Tracking.Application.Auth.Dtos
{
    public class InviteUserRequest
    {
        public UserRole Role { get; set; } = UserRole.Demo;
        public required Guid WorkspaceId { get; set; }
    }
}
