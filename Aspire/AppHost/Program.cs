var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.API>("api");

builder.AddProject<Projects.UI>("ui")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
