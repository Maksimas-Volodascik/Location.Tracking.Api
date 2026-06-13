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
    public record GetAllDevicesQuery : IRequest<Result<IEnumerable<Device>>> { }
}
