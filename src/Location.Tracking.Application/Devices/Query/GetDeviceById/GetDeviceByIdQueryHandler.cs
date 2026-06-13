using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.Shared;
using Location.Tracking.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Devices.Query.GetDeviceById
{
    public class GetDeviceByIdQueryHandler : IRequestHandler<GetDeviceByIdQuery, Result<Device>>
    {
        private readonly IDeviceRepository _deviceRepository;
        public GetDeviceByIdQueryHandler(IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

        public async Task<Result<Device>> Handle(GetDeviceByIdQuery request, CancellationToken cancellationToken)
        {
            var device = await _deviceRepository.GetByIdAsync(request.DeviceId);

            if (device == null) return Result<Device>.Failure(Errors.DeviceErrors.DeviceNotFound);

            return Result<Device>.Success(device);
        }
    }
}
