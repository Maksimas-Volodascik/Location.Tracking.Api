using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.Shared;
using Location.Tracking.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Devices.Commands.CreateNewDevice
{
    public class CreateNewDeviceCommandHandler : IRequestHandler<CreateNewDeviceCommand, Result>
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IDeviceModelRepository _deviceModelRepository;
        public CreateNewDeviceCommandHandler(IDeviceRepository deviceRepository, IDeviceModelRepository deviceModelRepository)
        {
            _deviceRepository = deviceRepository;
            _deviceModelRepository = deviceModelRepository;
        }

        public async Task<Result> Handle(CreateNewDeviceCommand request, CancellationToken cancellationToken)
        {
            //todo: check if user exists

            var deviceModel = await _deviceModelRepository.GetDeviceModelByName(request.DeviceData.DeviceModelName);

            if (deviceModel == null) return Result<Device>.Failure(Errors.DeviceModelErrors.DeviceModelNotFound);

            var existingDevice = await _deviceRepository.GetDeviceByImeiAsync(request.DeviceData.Imei);

            if (existingDevice != null) return Result<Device>.Failure(Errors.DeviceErrors.DeviceNotFound);

            Device device = new Device()
            {
                Imei = request.DeviceData.Imei,
                IsEnabled = request.DeviceData.IsEnabled,
                Name = request.DeviceData.Name,
                DeviceModelId = deviceModel.Id,
                UserId = request.UserId
            };

            await _deviceRepository.AddAsync(device);
            await _deviceRepository.SaveChangesAsync();

            return Result.Success();
        }
    }
}
