var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.PetStore_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.PetStore_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.AddProject<Projects.Identity_API>("identity-api");

builder.AddProject<Projects.Catalog_API>("catalog-api");

builder.Build().Run();
