using Location.Tracking.Application.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Users.Query.GetUsers
{
    public class GetUsersQuery : IRequest<Result<IEnumerable<UserData>>> {}
}
