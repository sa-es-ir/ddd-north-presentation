var builder = DistributedApplication.CreateBuilder(args);

var jwtKeyA = "very-secret-key-A_79c0d948-c4c1-4779-9df8-861c2a90c043";
var jwtKeyB = "very-secret-key-B_259d2cd0-f2d5-408f-a4f4-7675dd97cd6a";
var IdentityServerAIssuer = "https://identity-server-a.com";
var IdentityServerBIssuer = "https://identity-server-b.com";
var audienceA = "79c0d948-c4c1-4779-9df8-861c2a90c043";
var audienceB = "259d2cd0-f2d5-408f-a4f4-7675dd97cd6a";

var apiService = builder.AddProject<Projects.AspNetCoreMultiAuth_ApiService>("apiservice")
    .WithEnvironment("jwt-key-a", jwtKeyA)
    .WithEnvironment("jwt-issuer-a", IdentityServerAIssuer)
    .WithEnvironment("jwt-key-b", jwtKeyB)
    .WithEnvironment("jwt-issuer-b", IdentityServerBIssuer)
    .WithEnvironment("audience-a", audienceA)
    .WithEnvironment("audience-b", audienceB);

builder.AddProject<Projects.AspNetCoreMultiAuth_IdentityServerA>("identityServerA")
    .WithEnvironment("jwt-key-a", jwtKeyA)
    .WithEnvironment("audience-a", audienceA)
    .WithEnvironment("jwt-issuer-a", IdentityServerAIssuer);


builder.AddProject<Projects.AspNetCoreMultiAuth_IdentityServerB>("identityServerB")
    .WithEnvironment("jwt-key-b", jwtKeyB)
    .WithEnvironment("audience-b", audienceB)
    .WithEnvironment("jwt-issuer-b", IdentityServerBIssuer);

builder.AddProject<Projects.AspNetCoreMultiAuth_CustomToken>("customtoken");

builder.Build().Run();
