using AutoMapper;
using Location.Tracking.Application.DeviceModels.Dtos;
using Location.Tracking.Domain.Entities;
namespace Location.Tracking.Application.AutoMapper

{
    public class DeviceModelProfile : Profile
    {
        public DeviceModelProfile()
        {
            CreateMap<CreateDeviceModelRequest, DeviceModel>()
                .ForAllMembers(o => o.Condition((src, dest, srcMember) => srcMember != null)); //map parameters that are not null

            CreateMap<UpdateDeviceModelRequest, DeviceModel>()
                .ForAllMembers(o => o.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
