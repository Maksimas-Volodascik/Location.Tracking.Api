using Location.Tracking.Application.Auth.Dtos;
using Location.Tracking.Application.Shared.Results;
using Location.Tracking.Application.Users.Commands.Login;
using Location.Tracking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Auth
{
    public interface IAuthService
    {
        public Task<Result<TokenResponse>> Login(LoginRequest loginRequest);
        public Task<Result> Register(RegisterRequest registerRequest);
        public Task<Result<string>> CreateInvite(Guid inviterId, InviteUserRequest request);
        public Task<Result> AcceptInvite(AcceptInviteRequest request);
    }
}
