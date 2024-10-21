var builder = DistributedApplication.CreateBuilder(args);

builder
.AddProject<Projects.Clean_Architecture_Web>("web")
.WithExternalHttpEndpoints();

builder.Build().Run();
