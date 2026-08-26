using Location.Tracking.Application.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Users.Commands.DeleteUser
{
    public record DeleteUserCommand : IRequest<Result>
    {
        public Guid UserId { get; set; }
    }
}
