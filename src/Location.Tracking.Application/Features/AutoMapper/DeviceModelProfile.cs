using AutoMapper;
using Location.Tracking.Application.DTOs;
using Location.Tracking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Features.AutoMapper
{
    public class DeviceModelProfile : Profile
    {
        public DeviceModelProfile()
        {
            CreateMap<DeviceModelDto, DeviceModel>()
                .ForAllMembers(o => o.Condition((src, dest, srcMember) => srcMember != null)); //map parameters that are not null
        }
    }
}
