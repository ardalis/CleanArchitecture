var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.NimblePros_SampleToDo_Web>("web");

builder.Build().Run();
