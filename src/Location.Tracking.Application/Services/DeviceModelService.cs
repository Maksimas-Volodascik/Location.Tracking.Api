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
        public DeviceModelService(IDeviceModelRepository deviceModelRepository)
        {
            _deviceModelRepository = deviceModelRepository;
        }

        public Task<Result> CreateDeviceModelAsync(DeviceModelDto deviceModelDto)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteDeviceModelAsync(Guid deviceModelId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<DeviceModel>>> GetAllDeviceModelsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Result<DeviceModel>> GetDeviceModelByNameAsync(string model)
        {
            DeviceModel? deviceModel = await _deviceModelRepository.GetDeviceModelByName(model);

            if (deviceModel == null) return Result<DeviceModel>.Failure(Errors.DeviceModelErrors.DeviceModelNotFound);

            return Result<DeviceModel>.Success(deviceModel);
        }

        public Task<Result> UpdateDeviceModelAsync(DeviceModelDto deviceModelDto, Guid deviceModelId)
        {
            throw new NotImplementedException();
        }
    }
}
