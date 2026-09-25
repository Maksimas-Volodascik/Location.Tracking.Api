using Location.Tracking.Application.Dashboard.Dtos;
using Location.Tracking.Application.Shared.Interface;
using Microsoft.EntityFrameworkCore;

namespace Location.Tracking.Application.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly ITrackingDbContext _context;
        public DashboardService(ITrackingDbContext context)
        {
            _context = context;
        }

        public async Task<SystemMetrics> GetDashboardMetricsAsync()
        {
            UsersMetrics usersMetrics = await GetUserMetricsAsync();
            RecordsMetrics recordsMetrics = await GetRecordsMetrics();
            DevicesMetrics devicesMetrics = await GetDeviceMetricsAsync();
            ErrorMetrics errorMetrics = await GetErrorMetricsAsync();

            SystemMetrics systemMetrics = new SystemMetrics
            {
                Users = usersMetrics,
                Records = recordsMetrics,
                Devices = devicesMetrics,
                Errors = errorMetrics,
            };

            return systemMetrics;
        }

        private async Task<UsersMetrics> GetUserMetricsAsync()
        {
            var usersMetrics = await _context.Users
                .GroupBy(_ => 1)
                .Select(user => new UsersMetrics
                {
                    Total = user.Count(),
                    Users = user.Count(usr => usr.Role == "User"),
                    Admin = user.Count(usr => usr.Role == "Admin")
                }).SingleOrDefaultAsync();

            return usersMetrics ?? new UsersMetrics();
        }

        private async Task<RecordsMetrics> GetRecordsMetrics()
        {
            var dateStart = DateTimeOffset.Now.Date;
            var dateEnd = dateStart.AddDays(1);

            var recordsMetrics = await _context.RawRecords
                .Select(r => r.ReceivedAt)
                .GroupBy(_ => 1)
                .Select(r => new RecordsMetrics
                {
                    Daily = r.Count(r => r.Date >= dateStart && r.Date < dateEnd),
                    Total = r.Count()
                }).SingleOrDefaultAsync();

            return recordsMetrics ?? new RecordsMetrics();
        }

        private async Task<DevicesMetrics> GetDeviceMetricsAsync()
        {
            var weekly = DateTimeOffset.UtcNow.AddDays(-7);

            var devicesMetrics = await _context.Devices
                .GroupBy(_ => 1)
                .Select(device => new DevicesMetrics
                {
                    Total = device.Count(),
                    Weekly = device.Count(g => g.DateAdded >= weekly)
                }).SingleOrDefaultAsync();

            return devicesMetrics ?? new DevicesMetrics();
        }

        private async Task<ErrorMetrics> GetErrorMetricsAsync()
        {
            //tba
            return new ErrorMetrics();
        }
    }
}
