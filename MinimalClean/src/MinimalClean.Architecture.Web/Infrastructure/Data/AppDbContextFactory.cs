using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MinimalClean.Architecture.Web.Infrastructure.Data;

/// <summary>
/// Design-time factory for creating DbContext instances for EF Core tools.
/// This is ONLY used when CREATING migrations (e.g., 'dotnet ef migrations add').
/// It is NOT used at runtime - Aspire handles the runtime connection.
/// 
/// Migrations are automatically APPLIED at runtime via MigrateAsync() in MiddlewareConfig.cs.
/// 
/// To create migrations with SQL Server, set an environment variable:
/// PowerShell: $env:ConnectionStrings__AppDb = "Server=localhost,1433;Database=AppDb;User ID=sa;Password=YourPassword;TrustServerCertificate=true"
/// Bash: export ConnectionStrings__AppDb="Server=localhost,1433;Database=AppDb;User ID=sa;Password=YourPassword;TrustServerCertificate=true"
/// 
/// Always uses SQL Server for migrations.
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
  public AppDbContext CreateDbContext(string[] args)
  {
    var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

    // Try to get connection string from environment variable (ASP.NET Core format)
    var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__AppDb")
                          ?? "Server=localhost,1433;Database=AppDb_Design;User ID=sa;Password=Your$tr0ngP@ss!;TrustServerCertificate=true"; // Default SQL Server connection for design-time

    optionsBuilder.UseSqlServer(connectionString);

    return new AppDbContext(optionsBuilder.Options);
  }
}
