using AutoMapper;
using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.Shared.Interface;
using Location.Tracking.Application.Shared.Results;
using Location.Tracking.Application.Users.Dtos;
using Location.Tracking.Application.Users.Query.GetUsers;
using Location.Tracking.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<Result<IEnumerable<UserData>>> GetAllUsers()
        {
            var users = await _context.Users
                                .Select(u => new UserData
                                {
                                    UserGuid = u.Id,
                                    FirstName = u.FirstName,
                                    LastName = u.LastName,
                                    Email = u.Email,
                                    Role = u.Role
                                }).ToListAsync();

            return Result<IEnumerable<UserData>>.Success(users);
        }

    }
}
