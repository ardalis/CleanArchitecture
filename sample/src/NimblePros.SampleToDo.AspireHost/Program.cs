using System.Net.Sockets;

var builder = DistributedApplication.CreateBuilder(args);

// Papercut container
var papercut = builder.AddContainer("papercut", "jijiechen/papercut", "latest")
  .WithEndpoint("smtp", e =>
  {
    e.TargetPort = 25;   // container port
    e.Port = 25;         // host port (optional—omit to auto-assign)
    e.Protocol = ProtocolType.Tcp;
    e.UriScheme = "smtp"; // makes the resolved value look like smtp://host:port
  })
  .WithEndpoint("ui", e =>
  {
    e.TargetPort = 37408;
    e.Port = 37408;      // optional – Aspire can allocate
    e.UriScheme = "http";
  });

// Your web project
var web = builder.AddProject<Projects.NimblePros_SampleToDo_Web>("web")
    .WithHttpHealthCheck("/health")
    // Pass the endpoints to the app via env vars (EndpointReference resolves to a URL at run time)
    .WithEnvironment("Papercut__Smtp__Url", papercut.GetEndpoint("smtp"))
    .WithEnvironment("Papercut__Ui__Url", papercut.GetEndpoint("ui"));

// (optionally) if your app wants separate host/port values, you can parse the URL at startup,
// or expose two env vars and parse them from the URL inside the app.

builder.Build().Run();
