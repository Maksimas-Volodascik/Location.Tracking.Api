using Location.Tracking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Shared.Interface
{
    public interface ITokenIssuer
    {
        string CreateAccessToken(User user);
    }
}
