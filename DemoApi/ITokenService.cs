using System.Security.Claims;

namespace DemoApi;

public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    string GenerateForgotPasswordToken(IEnumerable<Claim> claims);
}