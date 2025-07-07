var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Clean_Architecture_Web>("web");

builder.Build().Run();
