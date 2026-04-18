using Location.Tracking.Application.DTOs;
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
        private readonly IDeviceModelService _deviceModelService;
        public DeviceService(IDeviceRepository deviceRepository, IDeviceModelService deviceModelService)
        {
            _deviceRepository = deviceRepository;
            _deviceModelService = deviceModelService;
        }

        public async Task CreateNewDeviceAsync(DeviceConfigurationDto deviceConfigurationDto)
        {
            var deviceModel = await _deviceModelService.GetDeviceModelByName(deviceConfigurationDto.DeviceModelName);

            if (deviceModel == null) return; // return bad response

            Device device = new Device()
            {
                Imei = deviceConfigurationDto.Imei,
                IsEnabled = deviceConfigurationDto.IsEnabled,
                DeviceModelId = deviceModel.Id,
                UserId = new Guid("019d971c-510c-7d7f-ac02-c7d5456dfa2c")//temporary
            };

            await _deviceRepository.AddAsync(device);
            await _deviceRepository.SaveChangesAsync();
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
