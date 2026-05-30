using Location.Tracking.Application.DTOs.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<SystemMetrics> GetSystemMetricsAsync();
    }
}
