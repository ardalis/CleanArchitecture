var builder = DistributedApplication.CreateBuilder(args);

// secret SQL password parameter
var sqlPassword = builder.AddParameter("sql-password", "Your$tr0ngP@ss!", secret: true);

var sql = builder.AddSqlServer("sql", password: sqlPassword)
                 .WithDataVolume()
                 .WithLifetime(ContainerLifetime.Persistent);

// define the app database
var appDb = sql.AddDatabase("AppDb");

// register the API project and link the DB
builder.AddProject<Projects.MinimalClean_Architecture_Web>("web")
       .WithReference(appDb)
       .WaitFor(appDb)
       .WithUrl("https://localhost:57679"); // Match launchSettings

builder.Build().Run();
