using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.Interfaces.Services;
using Location.Tracking.Application.Shared;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Location.Tracking.Application.DTOs.Users;
using Location.Tracking.Application.Users.Commands.Register;

namespace Location.Tracking.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtSettings _jwtSettings;
        public UserService(IUserRepository userRepository, IOptions<JwtSettings> options)
        {
            _userRepository = userRepository;
            _jwtSettings = options.Value;
        }
        
        public async Task<UsersMetrics> GetUsersMetricsAsync()
        {
            UsersMetrics usersMetrics = await _userRepository.GetUsersMetrics();

            return usersMetrics;
        }
    }
}
