using Location.Tracking.Application.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Devices.Commands.UpdateDevice
{
    public record UpdateDeviceCommand : IRequest<Result>
    {
        public DeviceConfiguration DeviceConfiguration { get; set; } = new DeviceConfiguration();
        public Guid DeviceId { get; set; } = Guid.Empty;
    }
}
