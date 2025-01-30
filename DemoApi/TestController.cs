using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoApi;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;

    public TestController(ILogger<TestController> logger)
    {
        _logger = logger;
    }

    [HttpGet("google-verify")]
    [Authorize(AuthenticationSchemes = "GoogleBearer")] // Google OAuth için güncellendi
    public IActionResult TestGoogleToken()
    {
        var email = User.FindFirst("email")?.Value;
        return Ok(new
        {
            Message = "OAuth2 Works!",
            Email = email
        });
    }
}