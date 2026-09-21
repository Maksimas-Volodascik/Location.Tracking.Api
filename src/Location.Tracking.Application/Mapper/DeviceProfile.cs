using AutoMapper;
using Location.Tracking.Application.Devices.Dtos;
using Location.Tracking.Domain.Entities;

namespace Location.Tracking.Application.AutoMapper
{
    public class DeviceProfile : Profile
    {
        public DeviceProfile()
        {
            CreateMap<CreateDeviceRequest, Device>()
                .ForMember(dest => dest.DeviceModelId, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); //ignore null values (keep old)

            CreateMap<UpdateDeviceRequest, Device>()
                .ForMember(dest => dest.DeviceModelId, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
