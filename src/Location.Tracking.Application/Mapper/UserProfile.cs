using AutoMapper;
using Location.Tracking.Application.Users.Commands.UpdateUser;
using Location.Tracking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Mapper
{
    public class UserProfile : Profile
    {
            public UserProfile()
            {
                CreateMap<UserConfiguration, User>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); //ignore null values (keep old)
            }
    }
}
