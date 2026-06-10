using Location.Tracking.Application.DTOs.Users;
using Location.Tracking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<UsersMetrics> GetUsersMetricsAsync();
    }
}
