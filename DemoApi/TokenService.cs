using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OcelotApiGatewayDemo;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DemoApi;



public class TokenService : ITokenService
{
    private readonly TokenPreferences _tokenPreferences;

    public TokenService(IOptions<TokenPreferences> tokenPreferences)
    {
        _tokenPreferences = tokenPreferences.Value;
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_tokenPreferences.SecurityKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _tokenPreferences.Issuer,
            audience: _tokenPreferences.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_tokenPreferences.AccessTokenExpiration),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public string GenerateForgotPasswordToken(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_tokenPreferences.SecurityKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _tokenPreferences.Issuer,
            audience: _tokenPreferences.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_tokenPreferences.ForgotPasswordTokenExpiration),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}