using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.Interfaces.Services;
using Location.Tracking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _deviceRepository;
        public DeviceService(IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

        public Task CreateNewDeviceAsync()
        {
            throw new NotImplementedException();
        }

        public async Task DeleteDeviceAsync(Guid guid)
        {
            var device = await _deviceRepository.GetByIdAsync(guid);

            if (device == null) return;

            _deviceRepository.Delete(device);
            await _deviceRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Device>> GetAllDevicesAsync()
        {
            var devices = await _deviceRepository.GetAllDevicesAsync();

            return devices;
        }

        public async Task<Device> GetDeviceByIdAsync(Guid uuid)
        {
            var device = await _deviceRepository.GetByIdAsync(uuid);

            return device;
        }

        public Task UpdateDeviceAsync()
        {
            throw new NotImplementedException();
        }
    }
}
