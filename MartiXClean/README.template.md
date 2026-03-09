# MartiX.Clean.Architecture

Welcome to your new project generated with the **MartiX Clean Architecture** template!

This is a streamlined template centered on a single backend Web project with Vertical Slice Architecture (VSA). It follows Clean Architecture principles without the overhead of multiple backend layers, and it can now optionally add a dedicated Blazor frontend project when you want a UI in the generated solution.

## Getting Started

### Build and Run

```powershell
# Build the solution
dotnet build

# Run the application
dotnet run --project src/MartiX.Clean.Architecture.Web

# Or run with Aspire (if using)
dotnet run --project src/MartiX.Clean.Architecture.AspireHost
```

### Template Options

Generate the default backend-only solution:

```powershell
dotnet new min-clean -n YourApp
```

Generate the solution with the optional Blazor frontend:

```powershell
dotnet new min-clean -n YourApp --IncludeBlazor true
```

### Database Setup

This template uses **SQL Server in a container** managed by Aspire. When you run the Aspire AppHost, it automatically starts a SQL Server container and creates the database.

#### Option 1: Run with Aspire (Recommended)

```powershell
dotnet run --project src/MartiX.Clean.Architecture.AspireHost
```

The SQL Server container and database are automatically provisioned and migrations are applied on startup.

#### Option 2: Run Web project directly (SQL Server LocalDB)

If running the Web project without Aspire, update `appsettings.json` to use LocalDB:

```powershell
dotnet ef database update -c AppDbContext -p src/MartiX.Clean.Architecture.Web -s src/MartiX.Clean.Architecture.Web
dotnet run --project src/MartiX.Clean.Architecture.Web
```

## Project Structure

This template always uses a backend **Web** project organized by **vertical slices**. If you enable Blazor, it also adds a dedicated frontend project:

```text
src/
├── MartiX.Clean.Architecture.Web/       # Backend API, domain, infrastructure, features
├── MartiX.Clean.Architecture.AspireHost/ # Local orchestration (SQL Server, Papercut, app hosts)
├── MartiX.Clean.Architecture.ServiceDefaults/
└── MartiX.Clean.Architecture.Blazor/    # Optional Blazor frontend when IncludeBlazor=true
```

### Key Design Decisions

```text
Single project vertical slice architecture
```

- **Backend-first**: The `Web` project remains the API and application backend
- **Optional Blazor UI**: Add a dedicated frontend host only when you need it
- **Vertical Slices**: Backend features stay organized by slice (Cart, Product, etc.) instead of layer
- **Domain-Driven Design**: Entities use proper encapsulation and business logic
- **FastEndpoints**: REPR pattern for clean, testable API endpoints
- **Entity Framework Core**: SQL Server-ready data access with migrations
- **Mediator Pattern**: Optional - use for cross-cutting concerns or remove for simplicity

## What's Different from Full Clean Architecture?

This minimal template simplifies the full Clean Architecture template:

| Full Template | Minimal Template |
|--------------|------------------|
| 4+ projects (Core, UseCases, Infrastructure, Web) | 1 Web project |
| Repository pattern with Specifications | Repository pattern with Specifications if needed |
| Extensive use of interfaces and abstractions | Pragmatic abstractions where needed |
| Separate Use Cases project with Mediator | Optional Mediator; logic can be in endpoints |
| Complex domain patterns (Aggregates, Value Objects, Domain Events) | Pragmatic DDD patterns (Aggregates, Value Objects) |

## When to Use This Template

**Use Minimal Clean Architecture when:**

- ✓ Building MVPs or smaller applications
- ✓ You want architectural guidance without project ceremony
- ✓ Team prefers simplicity and fast iteration
- ✓ Vertical slice architecture appeals to you
- ✓ You may grow into full Clean Architecture later

**Use Full Clean Architecture when:**

- × Building large, complex enterprise applications
- × Multiple teams working on different layers
- × Need strict separation of concerns and dependencies
- × Domain complexity requires extensive DDD patterns
- × Long-term maintenance and evolution expected

## Technology Stack

- **.NET 10**: Latest LTS framework
- **FastEndpoints**: REPR pattern for API endpoints
- **Blazor**: Optional server-hosted frontend for the first unified-template iteration
- **Entity Framework Core**: Data access with migrations
- **SQL Server**: Containerized database via Aspire (easily switched to PostgreSQL, SQLite, etc.)
- **Aspire**: Cloud-ready orchestration and observability
- **Serilog**: Structured logging
- **Scalar**: API exploration for FastEndpoints/OpenAPI

## Common Tasks

### Adding a New Feature (Vertical Slice)

1. **Create Domain Entity**: Add to `Domain/YourFeatureAggregate/`
2. **Add EF Configuration**: Create config in `Infrastructure/Data/Config/`
3. **Create Migration**: `dotnet ef migrations add AddYourFeature`
4. **Create Endpoints**: Add FastEndpoints in `Endpoints/YourFeature/`

### Switching to SQL Server

Update `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=YourDb;Trusted_Connection=true;"
  }
}
```

### Running Tests

```powershell
dotnet test
```

## Migration Path to Full Clean Architecture

As your application grows, you can migrate to the full Clean Architecture template:

1. **Extract Core**: Move Domain entities to separate Core project
2. **Extract UseCases**: Move business logic to UseCases project with Mediator
3. **Extract Infrastructure**: Move data access to Infrastructure project
4. **Update Dependencies**: Set up proper dependency flow (Core ← UseCases ← Infrastructure)

Learn more: [Clean Architecture Template](https://github.com/ardalis/CleanArchitecture)

## Resources

- [Clean Architecture Template Repository](https://github.com/ardalis/CleanArchitecture)
- [Vertical Slice Architecture](https://jimmybogard.com/vertical-slice-architecture/)
- [FastEndpoints Documentation](https://fast-endpoints.com/)
- [Ardalis on YouTube](https://www.youtube.com/@Ardalis)

## Need Help?

- Report issues: [GitHub Issues](https://github.com/ardalis/CleanArchitecture/issues)
- Ask questions: [Discussions](https://github.com/ardalis/CleanArchitecture/discussions)
- Follow updates: [@ardalis](https://twitter.com/ardalis)

---

**Happy Coding!** 🚀
