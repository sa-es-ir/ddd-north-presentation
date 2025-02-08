var builder = DistributedApplication.CreateBuilder(args);

var jwtKeyA = $"very-secret-key-A-{Guid.NewGuid().ToString()}";
var jwtKeyB = $"very-secret-key-B-{Guid.NewGuid().ToString()}";
var IdentityServerAIssuer = "https://identity-server-a.com";
var IdentityServerBIssuer = "https://identity-server-b.com";

var apiService = builder.AddProject<Projects.AspNetCoreMultiAuth_ApiService>("apiservice")
    .WithEnvironment("jwt-key-a", jwtKeyA)
    .WithEnvironment("jwt-issuer-a", IdentityServerAIssuer)
    .WithEnvironment("jwt-key-b", jwtKeyB)
    .WithEnvironment("jwt-issuer-b", IdentityServerBIssuer);

builder.AddProject<Projects.AspNetCoreMultiAuth_IdentityServerA>("IdentityServerA")
    .WithEnvironment("jwt-key-a", jwtKeyA)
    .WithEnvironment("jwt-issuer-a", IdentityServerAIssuer);


builder.AddProject<Projects.AspNetCoreMultiAuth_IdentityServerB>("IdentityServerB")
    .WithEnvironment("jwt-key-b", jwtKeyB)
    .WithEnvironment("jwt-issuer-b", IdentityServerBIssuer);

builder.Build().Run();
