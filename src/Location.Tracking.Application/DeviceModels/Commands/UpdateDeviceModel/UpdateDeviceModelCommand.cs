using Location.Tracking.Application.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.DeviceModels.Commands.UpdateDeviceModel
{
    public record UpdateDeviceModelCommand: IRequest<Result>
    {
        public DeviceModelParams? DeviceModelParams { get; set; }
        public Guid DeviceModelId { get; set; }
    }

    public record DeviceModelParams
    {
        public string Name { get; set; } = string.Empty;
        public string ProtocolName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
