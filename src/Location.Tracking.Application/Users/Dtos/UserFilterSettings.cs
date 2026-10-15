using Location.Tracking.Application.Shared.Paging;
using Location.Tracking.Domain.Enums;

namespace Location.Tracking.Application.Users.Dtos
{
    public class UserFilterSettings : PaginationParams
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public UserRole? Role { get; set; }
    }
}
