namespace DemoApi;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")]
public class DemoController : ControllerBase
{
    [HttpGet("test")]
    [Authorize(AuthenticationSchemes = "ApiKey")]
    public IActionResult GetTest()
    {
        return Ok(new { Message = "Hello from Demo API (API Key Authentication)!" });
    }

    [HttpGet("secure/test")]
    [Authorize(AuthenticationSchemes = "GoogleBearer")]  // Google OAuth için güncellendi
    public IActionResult GetSecureTest()
    {
        var email = User.FindFirst("email")?.Value;
        return Ok(new
        {
            Message = "Authenticated with Google!",
            Email = email
        });
    }

    [HttpGet("secure/test/jwt")]
    [Authorize(AuthenticationSchemes = "CustomBearer")]
    public IActionResult GetSecureTestJwt()
    {
        var userName = User.Identity.Name;
        var userClaims = User.Claims;
        return Ok(new
        {
            Message = "Hello from Custom JWT Endpoint!",
            User = userName,
            Claims = userClaims.Select(c => new { c.Type, c.Value })
        });
    }

    [HttpGet("secure/ids")]
    [Authorize(AuthenticationSchemes = "IdentityServer")]
    public IActionResult GetSecureIdsTest()
    {
        var claims = User.Claims.Select(c => new { c.Type, c.Value });
        return Ok(new
        {
            Message = "Authenticated with Identity Server!",
            Claims = claims
        });
    }
}







