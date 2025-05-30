using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Identity
{   
    // Add the missing JwtSettings class definition  
    public class JwtSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessTokenExpirationMinutes { get; set; }
    }

    // Add the missing User class definition  
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}
