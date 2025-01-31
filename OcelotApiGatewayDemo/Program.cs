using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using OcelotApiGatewayDemo;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Ýki farklý Bearer authentication için
builder.Services.AddAuthentication()
    // Google OAuth için
    .AddJwtBearer("GoogleBearer", options =>
    {
        options.Authority = "https://accounts.google.com";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Authentication:Google:ClientId"],
            ValidIssuer = "https://accounts.google.com",
            ValidateIssuerSigningKey = true
        };
    })
    // Custom JWT için
    .AddJwtBearer("CustomBearer", options =>
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
                Encoding.UTF8.GetBytes(tokenPreferences.SecurityKey))
        };
    })
        .AddJwtBearer("IdentityServer", options =>
        {
            options.Authority = "https://localhost:5005";
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = "demoapi"
            };
        })
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("ApiKey", null);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
await app.UseOcelot();
app.Run();