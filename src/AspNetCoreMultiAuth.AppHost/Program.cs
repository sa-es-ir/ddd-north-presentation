var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.AspNetCoreMultiAuth_ApiService>("apiservice");

builder.AddProject<Projects.AspNetCoreMultiAuth_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
