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
    [Authorize(AuthenticationSchemes = "Bearer")]
    public IActionResult GetSecureTest()
    {
        var email = User.FindFirst("email")?.Value;
        return Ok(new
        {
            Message = "Authenticated with Google!",
            Email = email
        });
    }


}








