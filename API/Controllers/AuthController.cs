using Microsoft.AspNetCore.Mvc;
using Infrastructure.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly JwtSettings _jwtSettings;

    public AuthController(ITokenService tokenService, IOptions<JwtSettings> jwtSettings)
    {
        _tokenService = tokenService;
        _jwtSettings = jwtSettings.Value;
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        // 1. Validate the refresh token (implement this in your service)
        var principal = _tokenService.GetPrincipalFromExpiredToken(request.RefreshToken, _jwtSettings);
        if (principal == null)
            return Unauthorized();

        // 2. Retrieve user info from claims
        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier) ?? principal.FindFirst(ClaimTypes.Name);
        var usernameClaim = principal.FindFirst(ClaimTypes.Name);
        var roleClaim = principal.FindFirst(ClaimTypes.Role);

        if (userIdClaim == null || usernameClaim == null || roleClaim == null)
            return Unauthorized();

        var user = new User
        {
            Id = Guid.TryParse(userIdClaim.Value, out var id) ? id : Guid.Empty,
            Username = usernameClaim.Value,
            Role = roleClaim.Value
        };

        // 3. (Optional) Validate refresh token in your store here

        // 4. Generate new tokens
        var newAccessToken = _tokenService.GenerateToken(user, _jwtSettings);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        // 5. (Optional) Save the new refresh token and invalidate the old one

        return Ok(new { AccessToken = newAccessToken, RefreshToken = newRefreshToken });
    }
}
