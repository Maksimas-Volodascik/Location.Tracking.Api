using AutoMapper;
using Location.Tracking.Application.Shared.Interface;
using Location.Tracking.Application.Shared.Results;
using Location.Tracking.Application.Users.Dtos;
using Location.Tracking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Location.Tracking.Application.Users
{
    public class UserService : IUserService
    {
        private readonly ITrackingDbContext _context;
        private readonly IMapper _mapper;
        public UserService(ITrackingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> DeleteUser(Guid userId)
        {
            User? user = await _context.Users.FindAsync(userId);
            if (user is null)
            {
                return Result.Failure(Errors.UserErrors.UserNotFound);
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> UpdateUser(Guid userId, UserConfiguration userConfiguration)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user is null) return Result.Failure(Errors.UserErrors.UserNotFound);

            user = _mapper.Map(userConfiguration, user);

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<IEnumerable<UserData>>> GetAllUsers(UserFilterSettings userFilterSettings)
        {
            IQueryable<User> userQuery = _context.Users;

            if (!String.IsNullOrWhiteSpace(userFilterSettings.FirstName))
                userQuery = userQuery.Where(u => u.FirstName == userFilterSettings.FirstName);

            if (!String.IsNullOrWhiteSpace(userFilterSettings.LastName))
                userQuery = userQuery.Where(u => u.LastName == userFilterSettings.LastName);

            if (!String.IsNullOrWhiteSpace(userFilterSettings.Email))
                userQuery = userQuery.Where(u => u.Email == userFilterSettings.Email);

            if (userFilterSettings.Role != null)
                userQuery = userQuery.Where(u => u.Role == userFilterSettings.Role);

            var paginatedResult = userQuery
                .Skip((userFilterSettings.PageNumber - 1) * userFilterSettings.PageSize)
                .Take(userFilterSettings.PageSize);

            var result = await paginatedResult.Select(u => new UserData
            {
                UserGuid = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Role = u.Role.ToString(),
                WorkspaceId = u.WorkspaceId
            }).ToListAsync();

            return Result<IEnumerable<UserData>>.Success(result);
        }

    }
}
