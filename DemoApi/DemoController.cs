namespace DemoApi;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")]
public class DemoController : ControllerBase
{
    [HttpGet("test")]
    [Authorize(AuthenticationSchemes = "ApiKey")] // Require API key authentication
    public IActionResult GetTest()
    {
        return Ok(new { Message = "Hello from Demo API (API Key Authentication)!" });
    }

    [HttpGet("secure/test")]
    [Authorize(AuthenticationSchemes = "Bearer")] // JWT Bearer authentication
    public IActionResult GetSecureTest()
    {
        // User bilgisine erişebilirsiniz
        var userName = User.Identity.Name;
        var userClaims = User.Claims;

        return Ok(new
        {
            Message = "Hello from Secure Endpoint!",
            User = userName,
            Claims = userClaims.Select(c => new { c.Type, c.Value })
        });
    }
}








