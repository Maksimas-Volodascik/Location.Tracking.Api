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
    public class DeviceModelProfile : Profile
    {
        public DeviceModelProfile()
        {
            CreateMap<CreateDeviceModelCommand, DeviceModel>()
                .ForAllMembers(o => o.Condition((src, dest, srcMember) => srcMember != null)); //map parameters that are not null
        }
    }
}
