using Location.Tracking.Application.Auth.Dtos;
using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.Shared.Interface;
using Location.Tracking.Application.Shared.Results;
using Location.Tracking.Application.Users.Commands.Login;
using Location.Tracking.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Auth
{
    internal class AuthService : IAuthService
    {
        private readonly ITrackingDbContext _context;
        private readonly ITokenIssuer _tokenIssuer;
        public AuthService(ITrackingDbContext context, ITokenIssuer tokenIssuer)
        {
            _context = context;
            _tokenIssuer = tokenIssuer;
        }

        public async Task<Result<TokenResponse>> Login(LoginRequest loginRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(b => b.Email == loginRequest.Email);

            if (user == null) return Result<TokenResponse>.Failure(Errors.UserErrors.InvalidCredentials); // user does not exist

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, loginRequest.Password) == PasswordVerificationResult.Failed)
            {
                return Result<TokenResponse>.Failure(Errors.UserErrors.InvalidCredentials); //invalid password
            }

            TokenResponse token = new TokenResponse { accessToken = _tokenIssuer.CreateAccessToken(user) };

            return Result<TokenResponse>.Success(token);
        }

        public Task<TokenResponse> Register(string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}
