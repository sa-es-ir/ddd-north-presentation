using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Servers = Array.Empty<ScalarServer>();
    });
}

app.UseHttpsRedirection();


app.MapGet("/token", (IConfiguration configuration) =>
{
    var jwtSecurity = new JwtSecurityTokenHandler();
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt-key"]!));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
    };

    var tokenDescriptor = new JwtSecurityToken(
        issuer: configuration["jwt-issuer-a"]!,
        audience: Guid.NewGuid().ToString(),
        claims: claims,
        expires: DateTime.UtcNow.AddHours(3),
        signingCredentials: credentials
    );

    return jwtSecurity.WriteToken(tokenDescriptor);
});

app.Run();
