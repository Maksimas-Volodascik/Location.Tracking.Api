using Location.Tracking.Application.DTOs;
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
        Task<IEnumerable<Device>> GetAllDevicesAsync();
        Task<Device> GetDeviceByIdAsync(Guid deviceId);
        Task CreateNewDeviceAsync(DeviceConfigurationDto deviceConfigurationDto);
        Task<Device?> UpdateDeviceAsync(DeviceConfigurationDto deviceConfigurationDto, Guid deviceId);
        Task DeleteDeviceAsync(Guid deviceId);
    }
}
