using Location.Tracking.Application.Devices.Dtos;
using Location.Tracking.Application.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Devices
{
    public interface IDeviceService
    {
        public Task<Result<DeviceDetails>> GetDeviceByIdAsync(Guid deviceId);
        public Task<Result<DeviceDetails>> GetDeviceByImeiAsync(string imei);
        public Task<Result<IEnumerable<DeviceDetails>>> GetAllDevicesAsync();
        public Task<Result> CreateNewDeviceAsync(CreateDeviceRequest createDevice, Guid userId);
        public Task<Result> DeleteDeviceAsync(Guid deviceId);
        public Task<Result> UpdateDeviceAsync(Guid deviceId, UpdateDeviceRequest updateDevice);

    }
}
