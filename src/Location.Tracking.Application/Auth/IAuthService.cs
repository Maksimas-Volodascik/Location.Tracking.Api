using Location.Tracking.Application.Auth.Dtos;
using Location.Tracking.Application.Shared.Results;
using Location.Tracking.Application.Users.Commands.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Auth
{
    internal interface IAuthService
    {
        public Task<Result<TokenResponse>> Login(LoginRequest loginRequest);
        public Task<TokenResponse> Register(string email, string password);
    }
}
