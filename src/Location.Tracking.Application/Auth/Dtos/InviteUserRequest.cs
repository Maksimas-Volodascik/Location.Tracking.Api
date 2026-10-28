
using Location.Tracking.Domain.Enums;

namespace Location.Tracking.Application.Auth.Dtos
{
    public class InviteUserRequest
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public UserRole Role { get; set; }
    }
}
