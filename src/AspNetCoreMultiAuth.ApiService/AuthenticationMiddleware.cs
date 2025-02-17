using AspNetCoreMultiAuth.ApiService.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace AspNetCoreMultiAuth.ApiService;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    public AuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext context, IConfiguration config, ITokenService tokenService)
    {
        var token = context.Request.Headers[HeaderNames.Authorization].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            var jwtHandler = new JwtSecurityTokenHandler();

            try
            {
                jwtHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidIssuer = config["jwt-issuer-azure-b2c"],
                    ValidAudience = config["audience-azure-b2c"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["jwt-key-azure-b2c"]!))
                }, out var validatedToken);

                // prepare user claims
                await _next(context);
                return;
            }
            catch
            {
                // ignore and try another one
            }

            try { /* same as above but for Okta*/ }
            catch {/* ignore and try another one*/ }


            try { /* try to validate the token with Custom token*/ }
            catch {/* ignore*/}
        }

        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("Unauthorized");
    }
}
