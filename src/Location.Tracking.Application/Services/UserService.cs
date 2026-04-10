using Location.Tracking.Application.DTOs;
using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.Interfaces.Services;
using Location.Tracking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

namespace Location.Tracking.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<User> LoginAsync(LoginDto credentials)
        {
            throw new NotImplementedException();
        }

        public async Task<User> RegisterAsync(RegisterDto credentials)
        {
            if (await _userRepository.GetUserByEmailAsync(credentials.Email) != null) return null;

            User newUser = new User
            {
                FirstName = credentials.FirstName,
                LastName = credentials.LastName,
                Email = credentials.Email,
            };

            var hasher = new PasswordHasher<User>();
            var hashedPassword = hasher.HashPassword(newUser, credentials.Password);

            newUser.PasswordHash = hashedPassword;

            await _userRepository.AddAsync(newUser);
            await _userRepository.SaveChangesAsync();

            return newUser;
        }
    }
}
