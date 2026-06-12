using Location.Tracking.Application.Dashboard.Query.GetDashboardMetrics;
using Location.Tracking.Application.DTOs.Device;
using Location.Tracking.Application.Shared;
using Location.Tracking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Interfaces.Services
{
    public interface IDeviceService
    {
        Task<Result<IEnumerable<Device>>> GetAllDevicesAsync();
        Task<Result<Device>> GetDeviceByIdAsync(Guid deviceId);
        Task<Result<Device>> GetDeviceByImeiAsync(string deviceImei);
        Task<Result> CreateNewDeviceAsync(DeviceConfigurationDto deviceConfigurationDto, string userId);
        Task<Result> UpdateDeviceAsync(DeviceConfigurationDto deviceConfigurationDto, Guid deviceId);
        Task<Result> DeleteDeviceAsync(Guid deviceId);
        Task<DevicesMetrics> GetDeviceMetricsAsync();
    }
}
