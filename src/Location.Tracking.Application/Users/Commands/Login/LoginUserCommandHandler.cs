using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.Shared;
using Location.Tracking.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Users.Commands.Login
{
    public class LoginUserCommandHandler : IRequestHandler<LoginCommand, Result<TokenResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtSettings _jwtSettings;
        public LoginUserCommandHandler(IUserRepository userRepository, IOptions<JwtSettings> options)
        {
            _userRepository = userRepository;
            _jwtSettings = options.Value;
        }

        public async Task<Result<TokenResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);

            if (user == null) return Result<TokenResponse>.Failure(Errors.UserErrors.InvalidCredentials); // user does not exist

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                return Result<TokenResponse>.Failure(Errors.UserErrors.InvalidCredentials); //invalid password
            }

            TokenResponse token = new TokenResponse { accessToken = CreateAccessToken(user) };

            return Result<TokenResponse>.Success(token);
        }

        private string CreateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Token)); //key for signature

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256); //header contents

            var accessToken = new JwtSecurityToken( //token object (header + payload)
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_jwtSettings.TokenExpiryInHours),
                signingCredentials: signingCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(accessToken); // build JWT and sign it //xx.yy.zz
        }
    }
}
