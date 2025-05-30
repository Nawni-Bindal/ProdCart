using Application.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class TokenService : ITokenService
    {
        public string GenerateToken(User user, JwtSettings settings)
        {
            var claims = new[]
            {
                   new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString() ?? string.Empty),
                   new Claim(ClaimTypes.Name, user.Username ?? string.Empty),
                   new Claim(ClaimTypes.Role, user.Role ?? string.Empty)
               };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: settings.Issuer,
                audience: settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(settings.AccessTokenExpirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, JwtSettings settings)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Secret)),
                ValidateIssuer = true,
                ValidIssuer = settings.Issuer,
                ValidateAudience = true,
                ValidAudience = settings.Audience,
                ValidateLifetime = false // Allow expired tokens  
            };

            return new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out _);
        }
    }
}