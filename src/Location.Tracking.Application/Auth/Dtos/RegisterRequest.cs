
namespace Location.Tracking.Application.Auth.Dtos
{
    public class RegisterRequest
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string WorkspaceName { get; set; }
    }
}
