using DemoApi;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OcelotApiGatewayDemo;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

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

// TokenPreferences'ý kaydet
builder.Services.Configure<TokenPreferences>(
    builder.Configuration.GetSection("TokenPreferences"));

// ITokenService'i kaydet
builder.Services.AddScoped<ITokenService, TokenService>();  

// Add Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo API v1");
    });
}

// Use authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();