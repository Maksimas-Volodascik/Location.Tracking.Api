using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.Shared;
using Location.Tracking.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Devices.Query.GetAllDevices
{
    public class GetAllDevicesQueryHandler : IRequestHandler<GetAllDevicesQuery, Result<IEnumerable<Device>>>
    {
        private readonly IDeviceRepository _deviceRepository;
        public GetAllDevicesQueryHandler(IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

        public async Task<Result<IEnumerable<Device>>> Handle(GetAllDevicesQuery request, CancellationToken cancellationToken)
        {
            var devices = await _deviceRepository.GetAllDevicesAsync();

            return Result<IEnumerable<Device>>.Success(devices);
        }
    }
}
