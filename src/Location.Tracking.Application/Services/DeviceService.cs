using AutoMapper;
using Location.Tracking.Application.DTOs;
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

        public async Task<Result> CreateNewDeviceAsync(DeviceConfigurationDto deviceConfigurationDto)
        {
            var deviceModel = await _deviceModelService.GetDeviceModelByName(deviceConfigurationDto.DeviceModelName);

            if (deviceModel == null) return Result<Device>.Failure(Errors.DeviceModelErrors.DeviceModelNotFound);

            Device device = new Device()
            {
                Imei = deviceConfigurationDto.Imei,
                IsEnabled = deviceConfigurationDto.IsEnabled,
                DeviceModelId = deviceModel.Id,
                UserId = new Guid("019d971c-510c-7d7f-ac02-c7d5456dfa2c")//temporary
            };

            await _deviceRepository.AddAsync(device);
            await _deviceRepository.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> DeleteDeviceAsync(Guid deviceId)
        {
            var device = await _deviceRepository.GetByIdAsync(deviceId);

            if (device == null) return Result<Device>.Failure(Errors.DeviceErrors.DeviceNotFound);

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

        public async Task<Result> UpdateDeviceAsync(DeviceConfigurationDto deviceConfigurationDto, Guid deviceId)
        {
            var deviceModel = await _deviceModelService.GetDeviceModelByName(deviceConfigurationDto.DeviceModelName);

            if (deviceModel == null) return Result<Device>.Failure(Errors.DeviceModelErrors.DeviceModelNotFound); 

            Device? device = await _deviceRepository.GetByIdAsync(deviceId);

            if (device == null) return Result<Device>.Failure(Errors.DeviceErrors.DeviceNotFound);

            device = _mapper.Map(deviceConfigurationDto, device);
            device.DeviceModelId = deviceModel.Id;

            _deviceRepository.Update(device);
            await _deviceRepository.SaveChangesAsync();

            return Result.Success();
        }
    }
}
