using Location.Tracking.Application.Shared;
using Location.Tracking.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.DeviceModels.Query.GetDeviceModelById
{
    public record GetDeviceModelByIdQuery: IRequest<Result<DeviceModel>>
    {
        public Guid DeviceModelId { get; set; }
    }
}
