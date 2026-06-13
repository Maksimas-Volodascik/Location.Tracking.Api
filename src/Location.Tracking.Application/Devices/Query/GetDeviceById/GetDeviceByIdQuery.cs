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
    public record GetDeviceByIdQuery : IRequest<Result<Device>>
    {
        public Guid DeviceId { get; set; } = Guid.Empty;
    }
}
