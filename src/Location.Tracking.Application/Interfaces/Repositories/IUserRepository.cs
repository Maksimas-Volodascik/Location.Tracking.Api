using Location.Tracking.Application.Dashboard.Query.GetDashboardMetrics;
using Location.Tracking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Interfaces.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<UsersMetrics> GetUsersMetrics();
        Task<User?> GetUserByEmailAsync(string email);
    }
}
