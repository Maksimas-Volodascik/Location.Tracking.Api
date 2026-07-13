using AutoMapper;
using Location.Tracking.Application.Devices.Commands.UpdateDevice;
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
            CreateMap<DeviceConfiguration, Device>()
                .ForMember(dest => dest.DeviceModelId, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); //ignore null values (keep old)
        }
    }
}
