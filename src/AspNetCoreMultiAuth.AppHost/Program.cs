var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.AspNetCoreMultiAuth_ApiService>("apiservice");

var jwtKeyA = $"very-secret-key-A-{Guid.NewGuid().ToString()}";
var jwtKeyB = $"very-secret-key-B-{Guid.NewGuid().ToString()}";

builder.AddProject<Projects.AspNetCoreMultiAuth_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.AddProject<Projects.AspNetCoreMultiAuth_IdentityServerA>("IdentityServerA")
    .WithEnvironment("jwt-key", jwtKeyA);


builder.AddProject<Projects.AspNetCoreMultiAuth_IdentityServerB>("IdentityServerB")
    .WithEnvironment("jwt-key", jwtKeyB);

builder.Build().Run();
