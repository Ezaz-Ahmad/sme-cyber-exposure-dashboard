using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SmeCyberExposure.Api.Configuration;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Consistent API errors (ProblemDetails)
builder.Services.AddProblemDetails();

// Options binding + validation
builder.Services.AddOptions<JwtOptions>()
    .Bind(builder.Configuration.GetSection(JwtOptions.SectionName))
    .Validate(o => !string.IsNullOrWhiteSpace(o.SigningKey), "Jwt:SigningKey is required")
    .ValidateOnStart();

builder.Services.AddOptions<ShodanOptions>()
    .Bind(builder.Configuration.GetSection(ShodanOptions.SectionName));

// JWT Authentication (Bearer)
var jwtSection = builder.Configuration.GetSection(JwtOptions.SectionName);
var jwt = jwtSection.Get<JwtOptions>() ?? new JwtOptions();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt.Issuer,

            ValidateAudience = true,
            ValidAudience = jwt.Audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SigningKey)),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(2)
        };
    });

// Swagger + JWT Authorize button
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SmeCyberExposure.Api", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});

var app = builder.Build();

// Global exception handling
app.UseExceptionHandler();

// Swagger only in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// IMPORTANT: Auth middleware order
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Debug: raw config view (safe: doesn’t leak the key values)
app.MapGet("/debug/config", (IConfiguration config, IHostEnvironment env) =>
{
    return Results.Ok(new
    {
        Environment = env.EnvironmentName,
        ScannerBaseUrl = config["Scanner:BaseUrl"],
        HasJwtSigningKey = !string.IsNullOrWhiteSpace(config["Jwt:SigningKey"]),
        HasShodanKey = !string.IsNullOrWhiteSpace(config["Shodan:ApiKey"])
    });
}).ExcludeFromDescription();

// Debug: options binding proof (safe: shows length only)
app.MapGet("/debug/options", (IOptions<JwtOptions> jwt, IOptions<ShodanOptions> shodan) =>
{
    return Results.Ok(new
    {
        JwtIssuer = jwt.Value.Issuer,
        JwtAudience = jwt.Value.Audience,
        JwtSigningKeyLength = jwt.Value.SigningKey?.Length ?? 0,
        HasShodanKey = !string.IsNullOrWhiteSpace(shodan.Value.ApiKey)
    });
}).ExcludeFromDescription();

app.Run();
