using AutoMapper;
using Location.Tracking.Application.DTOs.Device;
using Location.Tracking.Application.DTOs.Devices;
using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.Interfaces.Services;
using Location.Tracking.Application.Shared;
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
        private readonly IMapper _mapper;
        public DeviceService(IDeviceRepository deviceRepository, IDeviceModelService deviceModelService, IMapper mapper)
        {
            _deviceRepository = deviceRepository;
            _deviceModelService = deviceModelService;
            _mapper = mapper;
        }

        public async Task<Result> CreateNewDeviceAsync(DeviceConfigurationDto deviceConfigurationDto, string deviceId)
        {
            Guid userId = new Guid(deviceId);

            //todo: check if user exists

            var deviceModel = await _deviceModelService.GetDeviceModelByNameAsync(deviceConfigurationDto.DeviceModelName);

            if (!deviceModel.IsSuccess) return Result<Device>.Failure(deviceModel.Error);
            
            var existingDevice = await _deviceRepository.GetDeviceByImeiAsync(deviceConfigurationDto.Imei);

            if(existingDevice != null) return Result<Device>.Failure(Errors.DeviceErrors.DeviceNotFound);

            Device device = new Device()
            {
                Imei = deviceConfigurationDto.Imei,
                IsEnabled = deviceConfigurationDto.IsEnabled,
                Name = deviceConfigurationDto.Name,
                DeviceModelId = deviceModel.Data.Id,
                UserId = userId
            };

            await _deviceRepository.AddAsync(device);
            await _deviceRepository.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> DeleteDeviceAsync(Guid deviceId)
        {
            var device = await _deviceRepository.GetByIdAsync(deviceId);

            if (device == null) return Result<Device>.Failure(Errors.DeviceErrors.DeviceExists);

            _deviceRepository.Delete(device);
            await _deviceRepository.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<IEnumerable<Device>>> GetAllDevicesAsync()
        {
            var devices = await _deviceRepository.GetAllDevicesAsync();

            return Result<IEnumerable<Device>>.Success(devices);
        }

        public async Task<Result<Device>> GetDeviceByIdAsync(Guid deviceId)
        {
            Device? device = await _deviceRepository.GetByIdAsync(deviceId);

            if(device == null) return Result<Device>.Failure(Errors.DeviceErrors.DeviceNotFound);

            return Result<Device>.Success(device);
        }

        public async Task<Result<Device>> GetDeviceByImeiAsync(string deviceImei)
        {
            var device = await _deviceRepository.GetDeviceByImeiAsync(deviceImei);

            if (device == null) return Result<Device>.Failure(Errors.DeviceErrors.DeviceNotFound);

            return Result<Device>.Success(device);
        }

        public async Task<DevicesMetrics> GetDeviceMetricsAsync()
        {
            var devices = await _deviceRepository.GetDeviceMetricsAsync();

            return devices;
        }

        public async Task<Result> UpdateDeviceAsync(DeviceConfigurationDto deviceConfigurationDto, Guid deviceId)
        {
            var deviceModel = await _deviceModelService.GetDeviceModelByNameAsync(deviceConfigurationDto.DeviceModelName);

            if (deviceModel.Data == null) return Result.Failure(Errors.DeviceModelErrors.DeviceModelNotFound); 

            Device? device = await _deviceRepository.GetByIdAsync(deviceId);

            if (device == null) return Result.Failure(Errors.DeviceErrors.DeviceNotFound);

            device = _mapper.Map(deviceConfigurationDto, device);
            device.DeviceModelId = deviceModel.Data.Id;

            _deviceRepository.Update(device);
            await _deviceRepository.SaveChangesAsync();

            return Result.Success();
        }
    }
}
