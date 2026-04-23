using Location.Tracking.Application.DTOs;
using Location.Tracking.Application.Shared;
using Location.Tracking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Interfaces.Services
{
    public interface IDeviceModelService
    {
        Task<Result<DeviceModel>> GetDeviceModelByNameAsync(string model);
        Task<Result<DeviceModel>> GetDeviceModelByIdAsync(Guid deviceModelId);
        Task<Result<IEnumerable<DeviceModel>>> GetAllDeviceModelsAsync();
        Task<Result> CreateDeviceModelAsync(DeviceModelDto deviceModelDto);
        Task<Result> UpdateDeviceModelAsync(DeviceModelDto deviceModelDto, Guid deviceModelId);
        Task<Result> DeleteDeviceModelAsync(Guid deviceModelId);
    }
}
