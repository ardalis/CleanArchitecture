var builder = DistributedApplication.CreateBuilder(args);

// Use a random port for the web project
builder.AddProject<Projects.Clean_Architecture_Web>("web");

builder
  .Build()
  .Run();
