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
    public class DeviceProfile : Profile
    {
        public DeviceProfile()
        {
            CreateMap<DeviceConfigurationDto, Device>()
                .ForMember(d => d.DeviceModelId, opt => opt.Ignore());
        }
    }
}
