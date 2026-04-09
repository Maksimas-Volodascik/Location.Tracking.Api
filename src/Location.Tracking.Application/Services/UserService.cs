using Location.Tracking.Application.DTOs;
using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.Interfaces.Services;
using Location.Tracking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _baseRepository;
        public UserService(IBaseRepository<User> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public Task<User> LoginAsync(LoginDto credentials)
        {
            throw new NotImplementedException();
        }

        public async Task<User> RegisterAsync(LoginDto credentials)
        {
            //var doesExist = _baseRepository.GetByIdAsync(Guid.NewGuid());

            User newUser = new User
            {
                Email = credentials.Email,
                PasswordHash = credentials.Password
            };

            await _baseRepository.AddAsync(newUser);
            await _baseRepository.SaveChangesAsync();
            return newUser;
            // check if email exists
            // check if email is email
            // Add user
            // return OK
        }
    }
}
