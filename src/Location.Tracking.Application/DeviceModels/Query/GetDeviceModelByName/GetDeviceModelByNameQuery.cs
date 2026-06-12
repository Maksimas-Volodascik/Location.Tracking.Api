using Location.Tracking.Application.Shared;
using Location.Tracking.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.DeviceModels.Query.GetDeviceModelByName
{
    public record GetDeviceModelByNameQuery : IRequest<Result<DeviceModel>>
    {
        public string ModelName { get; set; } = string.Empty;
    }
}
