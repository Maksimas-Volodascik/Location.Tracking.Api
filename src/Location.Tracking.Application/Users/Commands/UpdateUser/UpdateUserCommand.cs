using Location.Tracking.Application.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Users.Commands.UpdateUser
{
    public record UpdateUserCommand : IRequest<Result>
    {
        public UserConfiguration UserConfiguration { get; set; } = new UserConfiguration();
        public Guid UserId { get; set; } = Guid.Empty;
    }
}
