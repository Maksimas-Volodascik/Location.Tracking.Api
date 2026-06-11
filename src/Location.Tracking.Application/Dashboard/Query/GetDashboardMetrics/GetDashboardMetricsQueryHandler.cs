using Location.Tracking.Application.DTOs.Devices;
using Location.Tracking.Application.DTOs.Records;
using Location.Tracking.Application.DTOs.Users;
using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Dashboard.Query.GetDashboardMetrics
{
    public class GetDashboardMetricsQueryHandler : IRequestHandler<GetDashboardMetricsQuery, SystemMetrics>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRecordRepository _recordRepository;
        private readonly IDeviceRepository _deviceRepository;
        public GetDashboardMetricsQueryHandler(IUserRepository userRepository, IRecordRepository recordRepository, IDeviceRepository deviceRepository)
        {
            _userRepository = userRepository;
            _recordRepository = recordRepository;
            _deviceRepository = deviceRepository;
        }
        public async Task<SystemMetrics> Handle(GetDashboardMetricsQuery request, CancellationToken cancellationToken)
        {
            UsersMetrics usersMetrics = await _userRepository.GetUsersMetrics();
            RecordsMetrics recordsMetrics = await _recordRepository.GetRecordsMetrics();
            DevicesMetrics devicesMetrics = await _deviceRepository.GetDeviceMetricsAsync();

            SystemMetrics systemMetrics = new SystemMetrics
            {
                Users = usersMetrics,
                Records = recordsMetrics,
                Devices = devicesMetrics,
            };

            return systemMetrics;
        }
    }
}
