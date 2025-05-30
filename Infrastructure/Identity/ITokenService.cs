using System.Security.Claims;

namespace Infrastructure.Identity
{
    public interface ITokenService
    {
        string GenerateToken(User user, JwtSettings settings);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token, JwtSettings settings);
    }
}
