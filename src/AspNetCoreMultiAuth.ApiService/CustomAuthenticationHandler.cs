using AspNetCoreMultiAuth.ApiService.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace AspNetCoreMultiAuth.ApiService;

public class CustomAuthenticationHandler : AuthenticationHandler<CustomAuthSchemeOptions>
{
    private readonly ITokenService _tokenService;
    public CustomAuthenticationHandler(IOptionsMonitor<CustomAuthSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ITokenService tokenService) : base(options, logger, encoder)
    {
        _tokenService = tokenService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var customToken))
            return AuthenticateResult.Fail("No AccessToken");

        var dbToken = await _tokenService.GetAsync(customToken.ToString().GetAccessToken()!);

        if (dbToken is null)
            return AuthenticateResult.Fail("invalid token");

        var claims = new ClaimsIdentity(nameof(CustomAuthenticationHandler));
        // you can more claim based on authorization for example add Roles or Policies

        var ticket = new AuthenticationTicket(
                   new ClaimsPrincipal(claims), Scheme.Name);
        return AuthenticateResult.Success(ticket);
    }
}

public class CustomAuthSchemeOptions
   : AuthenticationSchemeOptions
{

}
