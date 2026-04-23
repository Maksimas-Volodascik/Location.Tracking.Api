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
    public class DeviceModelService : IDeviceModelService
    {
        private readonly IDeviceModelRepository _deviceModelRepository;
        private readonly IMapper _mapper;
        public DeviceModelService(IDeviceModelRepository deviceModelRepository, IMapper mapper)
        {
            _deviceModelRepository = deviceModelRepository;
            _mapper = mapper;
        }

        public async Task<Result> CreateDeviceModelAsync(DeviceModelDto deviceModelDto)
        {
            DeviceModel deviceModel = new DeviceModel();

            _mapper.Map(deviceModelDto, deviceModel);

            await _deviceModelRepository.AddAsync(deviceModel);
            await _deviceModelRepository.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> DeleteDeviceModelAsync(Guid deviceModelId)
        {
            var deviceModel = await GetDeviceModelByIdAsync(deviceModelId);

            if (!deviceModel.IsSuccess) return Result.Failure(deviceModel.Error!);

            _deviceModelRepository.Delete(deviceModel.Data!);
            await _deviceModelRepository.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<IEnumerable<DeviceModel>>> GetAllDeviceModelsAsync()
        {
            var deviceModelList = await _deviceModelRepository.GetAllDeviceModelsAsync();

            return Result<IEnumerable<DeviceModel>>.Success(deviceModelList);
        }

        public async Task<Result<DeviceModel>> GetDeviceModelByIdAsync(Guid deviceModelId)
        {
            DeviceModel? deviceModel = await _deviceModelRepository.GetByIdAsync(deviceModelId);

            if (deviceModel == null) return Result<DeviceModel>.Failure(Errors.DeviceModelErrors.DeviceModelNotFound);

            return Result<DeviceModel>.Success(deviceModel);
        }

        public async Task<Result<DeviceModel>> GetDeviceModelByNameAsync(string model)
        {
            DeviceModel? deviceModel = await _deviceModelRepository.GetDeviceModelByName(model);

            if (deviceModel == null) return Result<DeviceModel>.Failure(Errors.DeviceModelErrors.DeviceModelNotFound);

            return Result<DeviceModel>.Success(deviceModel);
        }

        public async Task<Result> UpdateDeviceModelAsync(DeviceModelDto deviceModelDto, Guid deviceModelId)
        {
            var deviceModel = await GetDeviceModelByIdAsync(deviceModelId);

            if (!deviceModel.IsSuccess) return Result.Failure(deviceModel.Error!);

            _mapper.Map(deviceModelDto, deviceModel.Data);

            _deviceModelRepository.Update(deviceModel.Data!);
            await _deviceModelRepository.SaveChangesAsync();

            return Result.Success();
        }
    }
}
