namespace OcelotApiGatewayDemo;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private const string ApiKeyHeaderName = "X-API-KEY";

    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKeyHeaderValues))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var providedApiKey = apiKeyHeaderValues.FirstOrDefault();

        if (string.IsNullOrEmpty(providedApiKey))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        // Validate the API key (replace with your actual validation logic)
        if (providedApiKey == "test-api-key-123")
        {
            var claims = new[] { new Claim(ClaimTypes.Name, "APIKeyUser") };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        return Task.FromResult(AuthenticateResult.Fail("Invalid API Key"));
    }
}
