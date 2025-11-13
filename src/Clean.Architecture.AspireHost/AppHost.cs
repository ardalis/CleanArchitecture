using System.Net.Sockets;

var builder = DistributedApplication.CreateBuilder(args);

// Add SQL Server container
var sqlServer = builder.AddSqlServer("sqlserver")
  .WithLifetime(ContainerLifetime.Persistent);

// Add the database
var cleanArchDb = sqlServer.AddDatabase("cleanarchitecture");

// Papercut SMTP container for email testing
var papercut = builder.AddContainer("papercut", "jijiechen/papercut", "latest")
  .WithEndpoint("smtp", e =>
  {
    e.TargetPort = 25;   // container port
    e.Port = 25;         // host port
    e.Protocol = ProtocolType.Tcp;
    e.UriScheme = "smtp";
  })
  .WithEndpoint("ui", e =>
  {
    e.TargetPort = 37408;
    e.Port = 37408;
    e.UriScheme = "http";
  });

// Add the web project with the database connection
builder.AddProject<Projects.Clean_Architecture_Web>("web")
  .WithReference(cleanArchDb)
  .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
  .WithEnvironment("Papercut__Smtp__Url", papercut.GetEndpoint("smtp"))
  .WaitFor(cleanArchDb)
  .WaitFor(papercut);

builder
  .Build()
  .Run();
