using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static DemoApi.TokenService;

namespace DemoApi;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public AuthController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        // Kullanıcı doğrulama
        if (model.Username == "demo" && model.Password == "demo123")
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim(ClaimTypes.Role, "User")
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            return Ok(new
            {
                accessToken,
                refreshToken,
                tokenType = "Bearer"
            });
        }

        return Unauthorized();
    }
}