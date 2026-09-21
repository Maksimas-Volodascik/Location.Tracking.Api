using Location.Tracking.Application.DeviceModels.Dtos;
using Location.Tracking.Application.Shared.Results;

namespace Location.Tracking.Application.DeviceModels
{
    public interface IDeviceModelService
    {
        public Task<Result<IEnumerable<DeviceModelDetails>>> GetAllDeviceModelsAsync();
        public Task<Result<DeviceModelDetails>> GetDeviceModelByNameAsync(string modelName);
        public Task<Result<DeviceModelDetails>> GetDeviceModelByIdAsync(Guid modelId);
        public Task<Result> CreateNewDeviceModelAsync(CreateDeviceModelRequest createDeviceModel);
        public Task<Result> DeleteDeviceModelAsync(Guid modelId);
        public Task<Result> UpdateDeviceModelAsync(Guid modelId, UpdateDeviceModelRequest updateDeviceModel);
    }
}
