using Location.Tracking.Application.DTOs.Dashboard;
using Location.Tracking.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUserService _userService;
        private readonly IRecordService _recordService;
        private readonly IDeviceService _deviceService;
        public DashboardService(IUserService userService, IRecordService recordService, IDeviceService deviceService)
        {
            _userService = userService;
            _recordService = recordService;
            _deviceService = deviceService;
        }

        public async Task<SystemMetrics> GetSystemMetricsAsync()
        {
            SystemMetrics systemMetrics = new SystemMetrics
            {
                Users = await _userService.GetUsersMetricsAsync(),
                Records = await _recordService.GetRecordsMetricsAsync(),
                Devices = await _deviceService.GetDeviceMetricsAsync()
            };

            return systemMetrics;
        }
    }
}
