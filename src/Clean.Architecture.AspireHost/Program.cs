var builder = DistributedApplication.CreateBuilder(args);

// Add SQL Server container
var sqlServer = builder.AddSqlServer("sqlserver")
  .WithLifetime(ContainerLifetime.Persistent);

// Add the database
var cleanArchDb = sqlServer.AddDatabase("cleanarchitecture");

// Add the web project with the database connection
builder.AddProject<Projects.Clean_Architecture_Web>("web")
  .WithReference(cleanArchDb)
  .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName);

builder
  .Build()
  .Run();
