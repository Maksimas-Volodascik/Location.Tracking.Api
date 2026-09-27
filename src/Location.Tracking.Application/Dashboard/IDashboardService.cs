using Location.Tracking.Application.Dashboard.Dtos;
using Location.Tracking.Application.Shared.Results;

namespace Location.Tracking.Application.Dashboard
{
    public interface IDashboardService
    {
        public Task<Result<SystemMetrics>> GetDashboardMetricsAsync();
    }
}
