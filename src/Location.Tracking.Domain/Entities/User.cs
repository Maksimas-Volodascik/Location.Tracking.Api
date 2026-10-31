using Location.Tracking.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Location.Tracking.Domain.Entities
{
    public class User
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Demo;
        public bool IsActive { get; set; } = true;
        public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
      
        public Guid WorkspaceId { get; set; }
        public Workspace Workspace { get; set; } = null!;
        public User() { } 

        public User(string email, string passwordHash) {
            Email = email;
            PasswordHash = passwordHash;
        }
    }
}
