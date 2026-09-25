using AutoMapper;
using Location.Tracking.Application.DeviceModels.Dtos;
using Location.Tracking.Application.Devices.Dtos;
using Location.Tracking.Application.Shared.Interface;
using Location.Tracking.Application.Shared.Results;
using Location.Tracking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Location.Tracking.Application.Devices
{
    public class DeviceService : IDeviceService
    {
        private readonly ITrackingDbContext _context;
        private readonly IMapper _mapper;
        public DeviceService(ITrackingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result> CreateNewDeviceAsync(CreateDeviceRequest createDevice, Guid userId)
        {
            //todo: check if user exists

            var deviceModel = await _context.DeviceModel.FindAsync(createDevice.DeviceModelId);

            if (deviceModel == null) return Result<Device>.Failure(Errors.DeviceModelErrors.DeviceModelNotFound);

            var existingDevice = await _context.Devices.Select(d => d.Imei == createDevice.Imei).FirstOrDefaultAsync();

            if (existingDevice) return Result<Device>.Failure(Errors.DeviceErrors.DeviceExists);

            Device device = new Device()
            {
                Imei = createDevice.Imei,
                IsEnabled = createDevice.IsEnabled,
                Name = createDevice.Name,
                DeviceModelId = deviceModel.Id,
                UserId = userId
            };

            await _context.Devices.AddAsync(device);
            await _context.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> DeleteDeviceAsync(Guid deviceId)
        {
            var device = await _context.Devices.FindAsync(deviceId);

            if (device == null) return Result<Device>.Failure(Errors.DeviceErrors.DeviceNotFound);

            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<IEnumerable<DeviceDetails>>> GetAllDevicesAsync()
        {
            var deviceList = await _context.Devices
                                        .Select(d => new DeviceDetails
                                        {
                                            Id = d.Id,
                                            Name = d.Name,
                                            Imei = d.Imei,
                                            DateAdded = d.DateAdded,
                                            LastSeen = d.LastSeen,
                                            DeviceModelId = d.DeviceModelId,
                                            IsEnabled = d.IsEnabled
                                        }).ToListAsync();

            return Result<IEnumerable<DeviceDetails>>.Success(deviceList);
        }

        public async Task<Result<DeviceDetails>> GetDeviceByIdAsync(Guid deviceId)
        {
            var device = await _context.Devices
                            .Where(device => device.Id == deviceId)
                            .Select(d => new DeviceDetails
                            {
                                Id = d.Id,
                                Name = d.Name,
                                Imei = d.Imei,
                                DateAdded = d.DateAdded,
                                LastSeen = d.LastSeen,
                                DeviceModelId = d.DeviceModelId,
                                IsEnabled = d.IsEnabled
                            }).SingleOrDefaultAsync();

            if (device == null)
            {
                return Result<DeviceDetails>.Failure(Errors.DeviceModelErrors.DeviceModelNotFound);
            }

            return Result<DeviceDetails>.Success(device);
        }

        public async Task<Result<DeviceDetails>> GetDeviceByImeiAsync(string imei)
        {
            var device = await _context.Devices
                            .Where(device => device.Imei == imei)
                            .Select(d => new DeviceDetails
                            {
                                Id = d.Id,
                                Name = d.Name,
                                Imei = d.Imei,
                                DateAdded = d.DateAdded,
                                LastSeen = d.LastSeen,
                                DeviceModelId = d.DeviceModelId,
                                IsEnabled = d.IsEnabled
                            }).SingleOrDefaultAsync();

            if (device == null)
            {
                return Result<DeviceDetails>.Failure(Errors.DeviceModelErrors.DeviceModelNotFound);
            }

            return Result<DeviceDetails>.Success(device);
        }

        public async Task<Result> UpdateDeviceAsync(Guid deviceId, UpdateDeviceRequest updateDevice)
        {
            var deviceModel = await _context.DeviceModel.FindAsync(updateDevice.DeviceModelId);

            if (deviceModel == null) return Result.Failure(Errors.DeviceModelErrors.DeviceModelNotFound);

            var device = await _context.Devices.FindAsync(updateDevice.Id);

            if (device == null) return Result.Failure(Errors.DeviceErrors.DeviceNotFound);

            device = _mapper.Map(updateDevice, device);

            _context.Devices.Update(device);
            await _context.SaveChangesAsync();

            return Result.Success();
        }
    }
}
