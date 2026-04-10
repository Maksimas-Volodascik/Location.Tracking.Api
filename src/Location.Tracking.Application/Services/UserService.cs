using Location.Tracking.Application.DTOs;
using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.Interfaces.Services;
using Location.Tracking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Location.Tracking.Application.Shared;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace Location.Tracking.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtSettings _jwtSettings;
        public UserService(IUserRepository userRepository, IOptions<JwtSettings> options)
        {
            _userRepository = userRepository;
            _jwtSettings = options.Value;
        }

        public async Task<TokenResponse> LoginAsync(LoginDto credentials)
        {
            var user = await _userRepository.GetUserByEmailAsync(credentials.Email);

            if  (user == null) return null; // user does not exist

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, credentials.Password) == PasswordVerificationResult.Failed)
            {
                return new TokenResponse { accessToken = "password does not match" }; //password does not match
            }

            TokenResponse token = new TokenResponse { accessToken = CreateAccessToken(user) };
            //TokenResponse token = new TokenResponse { accessToken = _jwtSettings.Token };
               
            return token;
        }

        public async Task<User> RegisterAsync(RegisterDto credentials)
        {
            if (await _userRepository.GetUserByEmailAsync(credentials.Email) != null) return null;

            User newUser = new User
            {
                FirstName = credentials.FirstName,
                LastName = credentials.LastName,
                Email = credentials.Email,
            };

            var hashedPassword = new PasswordHasher<User>().HashPassword(newUser, credentials.Password);

            newUser.PasswordHash = hashedPassword;

            await _userRepository.AddAsync(newUser);
            await _userRepository.SaveChangesAsync();

            return newUser;
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
