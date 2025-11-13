## Infrastructure Project

In Clean Architecture, Infrastructure concerns are kept separate from the core business rules (or domain model in DDD).

The only project that should have code concerned with EF, Files, Email, Web Services, Azure/AWS/GCP, etc is Infrastructure.

Infrastructure should depend on Core (and, optionally, Use Cases) where abstractions (interfaces) exist.

Infrastructure classes implement interfaces found in the Core (Use Cases) project(s).

These implementations are wired up at startup using DI.

In this case using `Microsoft.Extensions.DependencyInjection` and extension methods defined in the project.

## Database Support

This project supports both **SQL Server** and **SQLite**:

- **SQL Server**: When running through .NET Aspire (AspireHost project), a SQL Server container is automatically provisioned and the connection string is provided via Aspire service discovery (using the "DefaultConnection" key).
- **SQLite**: When running the Web project standalone, it uses the SQLite connection from appsettings.json as a fallback.

The `InfrastructureServiceExtensions.AddInfrastructureServices()` method automatically detects which connection string is available and configures the appropriate database provider.

Need help? Check out the larger sample here:
https://github.com/ardalis/CleanArchitecture/tree/main/sample

Still need help?
Contact us at https://nimblepros.com
