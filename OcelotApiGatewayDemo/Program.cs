using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using OcelotApiGatewayDemo;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.Configure<TokenPreferences>(
    builder.Configuration.GetSection("TokenPreferences"));

// Register the API key authentication scheme
builder.Services.AddAuthentication("ApiKey")
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("ApiKey", null)
    .AddJwtBearer("Bearer", options =>
    {
        var tokenPreferences = builder.Configuration
            .GetSection("TokenPreferences").Get<TokenPreferences>();

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = tokenPreferences.Issuer,
            ValidAudience = tokenPreferences.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(tokenPreferences.SecurityKey)),
            ClockSkew = TimeSpan.Zero  // Token süresinin tam olarak uygulanmasý için
        };
    });

// Add Ocelot
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
      builder.Services.AddOcelot();

      var app = builder.Build();

      // Use authentication and authorization
      app.UseAuthentication();
      app.UseAuthorization();

      // Use Ocelot
      await app.UseOcelot();

      app.Run();
