using AutoMapper;
using Location.Tracking.Application.DeviceModels.Dtos;
using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.Shared.Interface;
using Location.Tracking.Application.Shared.Results;
using Location.Tracking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Location.Tracking.Application.DeviceModels
{
    public class DeviceModelService : IDeviceModelService
    {
        private readonly ITrackingDbContext _context;
        private readonly IMapper _mapper;
        public DeviceModelService(ITrackingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> CreateNewDeviceModelAsync(CreateDeviceModelRequest createDeviceModel)
        {
            DeviceModel deviceModel = new DeviceModel();

            _mapper.Map(createDeviceModel, deviceModel);

            await _context.DeviceModel.AddAsync(deviceModel);
            await _context.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> DeleteDeviceModelAsync(Guid modelId)
        {
            var deviceModel = await _context.DeviceModel.FindAsync(modelId);

            if (deviceModel == null) return Result.Failure(Errors.DeviceModelErrors.DeviceModelNotFound);

            _context.DeviceModel.Remove(deviceModel);
            await _context.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<IEnumerable<DeviceModelDetails>>> GetAllDeviceModelsAsync()
        {
            var deviceModelList = await _context.DeviceModel
                                        .Select(dm => new DeviceModelDetails
                                        {
                                            Name = dm.Name,
                                            ProtocolName = dm.ProtocolName,
                                            Description = dm.Description,
                                            Id = dm.Id
                                        }).ToListAsync();

            return Result<IEnumerable<DeviceModelDetails>>.Success(deviceModelList);
        }

        public async Task<Result<DeviceModelDetails>> GetDeviceModelByIdAsync(Guid modelId)
        {
            var deviceModel = await _context.DeviceModel
                                            .Where(device => device.Id == modelId)
                                            .Select(dm => new DeviceModelDetails
                                            {
                                                Name = dm.Name,
                                                ProtocolName = dm.ProtocolName,
                                                Description = dm.Description,
                                                Id = dm.Id
                                            }).SingleOrDefaultAsync();

            if (deviceModel == null)
            {
                return Result<DeviceModelDetails>.Failure(Errors.DeviceModelErrors.DeviceModelNotFound);
            }

            return Result<DeviceModelDetails>.Success(deviceModel);
        }

        public async Task<Result<DeviceModelDetails>> GetDeviceModelByNameAsync(string modelName)
        {
            var deviceModel = await _context.DeviceModel
                                .Where(device => device.ProtocolName == modelName)
                                .Select(dm => new DeviceModelDetails
                                {
                                    Name = dm.Name,
                                    ProtocolName = dm.ProtocolName,
                                    Description = dm.Description,
                                    Id = dm.Id
                                }).SingleOrDefaultAsync();

            if (deviceModel == null)
            {
                return Result<DeviceModelDetails>.Failure(Errors.DeviceModelErrors.DeviceModelNotFound);
            }

            return Result<DeviceModelDetails>.Success(deviceModel);
        }

        public async Task<Result> UpdateDeviceModelAsync(Guid modelId, UpdateDeviceModelRequest updateDeviceModel)
        {
            var deviceModel = await _context.DeviceModel.FindAsync(modelId);

            if (deviceModel == null) return Result.Failure(Errors.DeviceModelErrors.DeviceModelNotFound);

            _mapper.Map(updateDeviceModel, deviceModel);

            _context.DeviceModel.Update(deviceModel);
            await _context.SaveChangesAsync();

            return Result.Success();
        }
    }
}
