namespace Location.Tracking.Application.Users.Dtos
{
    public class UserData
    {
        public Guid UserGuid { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public Guid WorkspaceId { get; set; }
    }
}
