using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Users.Commands.Login
{
    public record TokenResponse
    {
        public string accessToken { get; set; } = string.Empty;
    }
}
