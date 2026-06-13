using AutoMapper;
using Location.Tracking.Application.DeviceModels.Commands.CreateDeviceModel;
using Location.Tracking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.AutoMapper
{
    public class DeviceProfile : Profile
    {
        public DeviceProfile()
        {
            CreateMap<CreateDeviceModelCommand, Device>()
                .ForMember(dest => dest.DeviceModelId, opt => opt.Ignore());
        }
    }
}
