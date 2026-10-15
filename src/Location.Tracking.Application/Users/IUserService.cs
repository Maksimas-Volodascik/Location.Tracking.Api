using Location.Tracking.Application.Shared.Results;
using Location.Tracking.Application.Users.Dtos;

namespace Location.Tracking.Application.Users
{
    public interface IUserService
    {
        public Task<Result> DeleteUser(Guid userId);
        public Task<Result> UpdateUser(Guid userId, UserConfiguration userConfiguration);
        public Task<Result<IEnumerable<UserData>>> GetAllUsers(UserFilterSettings userFilterSettings);
    }
}
