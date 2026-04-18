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
        Task<Device> GetDeviceByIdAsync(Guid uuid);
        Task CreateNewDeviceAsync(DeviceConfigurationDto deviceConfigurationDto);
        Task UpdateDeviceAsync();
        Task DeleteDeviceAsync(Guid guid);
    }
}
