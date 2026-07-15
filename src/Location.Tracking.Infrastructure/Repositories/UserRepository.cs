using Location.Tracking.Application.Dashboard.Query.GetDashboardMetrics;
using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.RawRecords.Query;
using Location.Tracking.Application.Users.Query.GetUsers;
using Location.Tracking.Domain.Entities;
using Location.Tracking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly TrackingDbContext _context;
        public UserRepository(TrackingDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Set<User>().FirstOrDefaultAsync(e => e.Email.Equals(email));
        }

        public async Task<IEnumerable<UserData>> GetUserData()
        {
            var query = from user in _context.Users
                        select new UserData
                        {
                            UserGuid = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            Role = user.Role
                        };

            return await query.ToListAsync();
        }

        public async Task<UsersMetrics> GetUsersMetrics()
        {
            UsersMetrics? query = await _context.Users
                .GroupBy(_ => 1)
                .Select(u => new UsersMetrics
                {
                    Total = u.Count(),
                    Users = u.Count(u => u.Role == "User"),
                    Admin = u.Count(u => u.Role == "Admin")
                }).FirstOrDefaultAsync();

            return query;
        }
    }
}
