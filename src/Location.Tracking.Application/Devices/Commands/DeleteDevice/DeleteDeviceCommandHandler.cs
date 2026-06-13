using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.Shared;
using Location.Tracking.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Devices.Commands.DeleteDevice
{
    public class DeleteDeviceCommandHandler : IRequestHandler<DeleteDeviceCommand, Result>
    {
        private readonly IDeviceRepository _deviceRepository;
        public DeleteDeviceCommandHandler(IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

        public async Task<Result> Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
        {
            var device = await _deviceRepository.GetByIdAsync(request.DeviceId);

            if (device == null) return Result<Device>.Failure(Errors.DeviceErrors.DeviceNotFound);

            _deviceRepository.Delete(device);
            await _deviceRepository.SaveChangesAsync();

            return Result.Success();
        }
    }
}
