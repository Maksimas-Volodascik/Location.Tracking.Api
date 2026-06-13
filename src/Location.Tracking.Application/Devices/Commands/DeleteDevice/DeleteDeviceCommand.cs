using Location.Tracking.Application.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Devices.Commands.DeleteDevice
{
    public record DeleteDeviceCommand : IRequest<Result>
    {
        public Guid DeviceId { get; set; }
    }
}
