using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace AspNetCoreMultiAuth.ApiService;

public static class ServiceCollectionExtensions
{
    public static void SetupAuthenticationFirst(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
         //AzureB2C, the Scheme name is JwtBearerDefaults.AuthenticationScheme (Bearer)
         .AddJwtBearer(options =>
         {
             options.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidIssuer = config["jwt-issuer-azure-b2c"],
                 ValidAudience = config["audience-azure-b2c"],
                 ClockSkew = TimeSpan.Zero,
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["jwt-key-azure-b2c"]!))
             };
         })
         //Okta, the Scheme name is Scheme_Okta
         .AddJwtBearer("Scheme_Okta", options =>
         {
             options.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidIssuer = config["jwt-issuer-okta"],
                 ValidAudience = config["audience-okta"],
                 ClockSkew = TimeSpan.Zero,
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["jwt-key-okta"]!))
             };
         })
         //The scheme name is CustomToken
         .AddScheme<CustomAuthSchemeOptions, CustomAuthenticationHandler>("CustomToken", options =>
         {
             // no need to set any options because you will handle them in the handler implementation
         });
    }

    public static void SetupAuthenticationSecond(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            //the scheme name is JwtBearerDefaults.AuthenticationScheme and this is the default and all requests go to this scheme
            // and then redirect to appropriate authentication scheme
            .AddPolicyScheme(JwtBearerDefaults.AuthenticationScheme, JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.ForwardDefaultSelector = context =>
                {
                    var jwtHandler = new JwtSecurityTokenHandler();
                    var token = context.Request.Headers[HeaderNames.Authorization].ToString().GetAccessToken();
                    if (!string.IsNullOrEmpty(token) && jwtHandler.CanReadToken(token))
                    {
                        var tokenIssuer = jwtHandler.ReadJwtToken(token).Issuer;

                        if (tokenIssuer == config["jwt-issuer-azure-b2c"])
                            return "Scheme_AzureB2C";
                        else
                            return "Scheme_Okta";
                    }

                    return "CustomToken";
                };
            })
          //AzureB2C, the Scheme name is Scheme_AzureB2C
          .AddJwtBearer("Scheme_AzureB2C", options =>
          {
              options.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidIssuer = config["jwt-issuer-azure-b2c"],
                  ValidAudience = config["audience-azure-b2c"],
                  ClockSkew = TimeSpan.Zero,
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["jwt-key-azure-b2c"]!))
              };
          })

          //Okta, the Scheme name is Scheme_Okta
          .AddJwtBearer("Scheme_Okta", options =>
          {
              options.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidIssuer = config["jwt-issuer-okta"],
                  ValidAudience = config["audience-okta"],
                  ClockSkew = TimeSpan.Zero,
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["jwt-key-okta"]!))
              };
          })
          //The scheme name is CustomToken
          .AddScheme<CustomAuthSchemeOptions, CustomAuthenticationHandler>("CustomToken", options =>
          {
              // no need to set any options because you will handle them in the handler implementation
          });
    }
    public static string? GetAccessToken(this string? authToken)
    {
        //var authToken = context.Request.Headers[HeaderNames.Authorization].ToString();

        if (string.IsNullOrEmpty(authToken))
            return authToken;

        var splitToken = authToken.Split(' ');

        if (splitToken.Length > 1)
            return splitToken[1];

        return authToken;
    }
}
