using Location.Tracking.Application.Dashboard.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Dashboard
{
    public interface IDashboardService
    {
        public Task<SystemMetrics> GetDashboardMetricsAsync();
    }
}
