var builder = DistributedApplication.CreateBuilder(args);

var jwtKeyAzureB2C = "very-secret-key-A_79c0d948-c4c1-4779-9df8-861c2a90c043";
var jwtKeyOkta = "very-secret-key-B_259d2cd0-f2d5-408f-a4f4-7675dd97cd6a";
var IdentityAzureB2CIssuer = "https://identity-server-azure-b2c.com";
var IdentityOktaIssuer = "https://identity-server-okta.com";
var audienceAzureB2C = "79c0d948-c4c1-4779-9df8-861c2a90c043";
var audienceOkta = "259d2cd0-f2d5-408f-a4f4-7675dd97cd6a";

var apiService = builder.AddProject<Projects.AspNetCoreMultiAuth_ApiService>("apiservice")
    .WithEnvironment("jwt-key-azure-b2c", jwtKeyAzureB2C)
    .WithEnvironment("jwt-issuer-azure-b2c", IdentityAzureB2CIssuer)
    .WithEnvironment("jwt-key-okta", jwtKeyOkta)
    .WithEnvironment("jwt-issuer-okta", IdentityOktaIssuer)
    .WithEnvironment("audience-azure-b2c", audienceAzureB2C)
    .WithEnvironment("audience-okta", audienceOkta);

builder.AddProject<Projects.AspNetCoreMultiAuth_AzureB2C>("identityAzureB2C")
    .WithEnvironment("jwt-key-azure-b2c", jwtKeyAzureB2C)
    .WithEnvironment("audience-azure-b2c", audienceAzureB2C)
    .WithEnvironment("jwt-issuer-azure-b2c", IdentityAzureB2CIssuer);


builder.AddProject<Projects.AspNetCoreMultiAuth_Okta>("identityOkta")
    .WithEnvironment("jwt-key-okta", jwtKeyOkta)
    .WithEnvironment("audience-okta", audienceOkta)
    .WithEnvironment("jwt-issuer-okta", IdentityOktaIssuer);

builder.AddProject<Projects.AspNetCoreMultiAuth_CustomToken>("customtoken");

builder.Build().Run();
