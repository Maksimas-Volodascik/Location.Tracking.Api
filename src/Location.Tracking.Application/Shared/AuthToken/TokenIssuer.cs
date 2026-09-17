using Location.Tracking.Application.Shared.Interface;
using Location.Tracking.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Shared.AuthToken
{
    public class TokenIssuer : ITokenIssuer
    {
        private readonly JwtSettings _jwtSettings;
        public TokenIssuer(IOptions<JwtSettings> options)
        {
            _jwtSettings = options.Value;
        }

        public string CreateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new (JwtRegisteredClaimNames.Name,  $"{user.FirstName} {user.LastName}"),
                new (JwtRegisteredClaimNames.Email, user.Email),
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
