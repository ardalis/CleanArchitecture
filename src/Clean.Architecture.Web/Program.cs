using System.Reflection;
using Ardalis.GuardClauses;
using Ardalis.ListStartupServices;
using Ardalis.SharedKernel;
using Clean.Architecture.Core.ContributorAggregate;
using Clean.Architecture.Core.Interfaces;
using Clean.Architecture.Core.Services;
using Clean.Architecture.Infrastructure;
using Clean.Architecture.Infrastructure.Data;
using Clean.Architecture.Infrastructure.Data.Queries;
using Clean.Architecture.Infrastructure.Email;
using Clean.Architecture.UseCases.Contributors.Create;
using Clean.Architecture.UseCases.Contributors.List;
using FastEndpoints;
using FastEndpoints.Swagger;
using MediatR;
using MediatR.Pipeline;
using Serilog;

var logger = Serilog.Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateLogger();

logger.Information("Starting web host");

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));

builder.Services.Configure<CookiePolicyOptions>(options =>
{
  options.CheckConsentNeeded = context => true;
  options.MinimumSameSitePolicy = SameSiteMode.None;
});

string? connectionString = builder.Configuration.GetConnectionString("SqliteConnection");
Guard.Against.Null(connectionString);
builder.Services.AddApplicationDbContext(connectionString);

builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument(o =>
{
  o.ShortSchemaNames = true;
});

// add list services for diagnostic purposes - see https://github.com/ardalis/AspNetCoreStartupServices
builder.Services.Configure<ServiceConfig>(config =>
{
  config.Services = new List<ServiceDescriptor>(builder.Services);

  // optional - default path to view services is /listallservices - recommended to choose your own path
  config.Path = "/listservices";
});

var assemblies = new[]
{
  Assembly.GetAssembly(typeof(Contributor)),
  Assembly.GetAssembly(typeof(CreateContributorCommand))
};

var mediatrOpenTypes = new[]
{
  typeof(IRequestHandler<,>),
  typeof(IRequestExceptionHandler<,,>),
  typeof(IRequestExceptionAction<,>),
  typeof(INotificationHandler<>),
};

foreach (var assembly in assemblies)
{
  foreach (var openInterfaceType in mediatrOpenTypes)
  {
    if (assembly != null)
    {
      assembly.GetTypes()
        .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == openInterfaceType))
        .ToList()
        .ForEach(implementationType =>
        {
          var serviceType = implementationType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == openInterfaceType);
          builder.Services.AddScoped(serviceType, implementationType);
        });
    }
  }
}

builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<IListContributorsQueryService, ListContributorsQueryService>();
builder.Services.AddScoped<IMediator, Mediator>();
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();
builder.Services.AddScoped<IListContributorsQueryService, ListContributorsQueryService>();
builder.Services.AddScoped<IDeleteContributorService, DeleteContributorService>();

if (builder.Environment.IsDevelopment())
{
  builder.Services.AddScoped<IEmailSender, FakeEmailSender>();  
}
else
{
  builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
  app.UseShowAllServicesMiddleware(); // see https://github.com/ardalis/AspNetCoreStartupServices
}
else
{
  app.UseDefaultExceptionHandler(); // from FastEndpoints
  app.UseHsts();
}
app.UseFastEndpoints();
app.UseSwaggerGen(); // FastEndpoints middleware

app.UseHttpsRedirection();

SeedDatabase(app);

app.Run();

static void SeedDatabase(WebApplication app)
{
  using var scope = app.Services.CreateScope();
  var services = scope.ServiceProvider;

  try
  {
    var context = services.GetRequiredService<AppDbContext>();
    //          context.Database.Migrate();
    context.Database.EnsureCreated();
    SeedData.Initialize(services);
  }
  catch (Exception ex)
  {
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred seeding the DB. {exceptionMessage}", ex.Message);
  }
}

// Make the implicit Program.cs class public, so integration tests can reference the correct assembly for host building
public partial class Program
{
}
