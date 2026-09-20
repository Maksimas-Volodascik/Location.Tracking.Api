using Location.Tracking.Application.DeviceModels.Dtos;
using Location.Tracking.Application.Shared.Results;

namespace Location.Tracking.Application.DeviceModels
{
    public interface IDeviceModelService
    {
        public Task<Result<IEnumerable<DeviceModelDetails>>> GetAllDeviceModelsAsync();
        public Task<Result<DeviceModelDetails>> GetDeviceModelByNameAsync(string modelName);
        public Task<Result<DeviceModelDetails>> GetDeviceModelByIdAsync(Guid deviceId);
        public Task<Result> CreateNewDeviceModelAsync(CreateDeviceModelRequest createDevice);
        public Task<Result> DeleteDeviceModelAsync(Guid deviceId);
        public Task<Result> UpdateDeviceModelAsync(Guid deviceId, UpdateDeviceModelRequest updateDevice);
    }
}
