using Autofac.Extensions.DependencyInjection;
using Clean.Architecture.Infrastructure.Data;
namespace Clean.Architecture.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<AppDbContext>();
                //                    context.Database.Migrate();
                context.Database.EnsureCreated();
                SeedData.Initialize(services);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred seeding the DB.");
            }
        }

        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
Host.CreateDefaultBuilder(args)
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder
            .UseStartup<Startup>()
            .ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
            // logging.AddAzureWebAppDiagnostics(); add this if deploying to Azure
        });
    });

}
