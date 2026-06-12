using Location.Tracking.Application.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.DeviceModels.Commands.DeleteDeviceModel
{
    public record DeleteDeviceModelCommand : IRequest<Result>
    {
        public Guid deviceModelId { get; set; }
    }
}
