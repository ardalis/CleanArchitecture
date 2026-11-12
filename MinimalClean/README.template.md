# MinimalClean.Architecture

Welcome to your new project generated with the **Minimal Clean Architecture** template!

This is a streamlined, single-project Vertical Slice Architecture (VSA) that follows Clean Architecture principles without the overhead of multiple projects. Perfect for smaller applications, MVPs, or teams that want architectural guidance without complex project boundaries.

## Getting Started

### Build and Run

```powershell
# Build the solution
dotnet build

# Run the application
dotnet run --project src/MinimalClean.Architecture.Web

# Or run with Aspire (if using)
dotnet run --project src/MinimalClean.Architecture.AspireHost
```

### Database Setup

This template uses **SQL Server in a container** managed by Aspire. When you run the Aspire AppHost, it automatically starts a SQL Server container and creates the database.

#### Option 1: Run with Aspire (Recommended)

```powershell
dotnet run --project src/MinimalClean.Architecture.AspireHost
```

The SQL Server container and database are automatically provisioned and migrations are applied on startup.

#### Option 2: Run Web project directly (SQL Server LocalDB)

If running the Web project without Aspire, update `appsettings.json` to use LocalDB:

```powershell
dotnet ef database update -c AppDbContext -p src/MinimalClean.Architecture.Web -s src/MinimalClean.Architecture.Web
dotnet run --project src/MinimalClean.Architecture.Web
```

## Project Structure

This template uses a **single Web project** organized by **vertical slices** (features):

```text
src/MinimalClean.Architecture.Web/
├── Domain/                    # Domain entities, aggregates, and interfaces
│   ├── CartAggregate/
│   ├── GuestUserAggregate/
│   ├── OrderAggregate/
│   ├── ProductAggregate/
│   └── Interfaces/
├── Infrastructure/            # Data access and external services
│   ├── Data/
│   │   ├── AppDbContext.cs
│   │   ├── Config/           # EF Core configurations
│   │   ├── Queries/          # Query service implementations
│   │   └── Migrations/
│   └── Email/                # Email services
├── CartFeatures/              # Vertical slice: endpoints + handlers
│   ├── AddToCart/
│   ├── Checkout/
│   └── GetById/
├── ProductFeatures/           # Vertical slice: endpoints + handlers
│   ├── Create/
│   ├── GetById/
│   └── List/
├── Configurations/            # DI, middleware, and Mediator setup
├── config.nsdepcop            # Namespace dependency rules (NsDepCop)
└── Program.cs                # Application startup
```

### Key Design Decisions

```text
Single project vertical slice architecture
```

- **Single Project**: All code in one Web project - simpler dependencies, faster builds
- **Vertical Slices**: Organized by feature (Cart, Order, Product) not layer
- **Domain-Driven Design**: Entities use proper encapsulation and business logic
- **FastEndpoints**: REPR pattern for clean, testable API endpoints
- **Entity Framework Core**: Simple data access with SQLite (easily switched to SQL Server)
- **Mediator Pattern**: Optional - use for cross-cutting concerns or remove for simplicity
- **Enforced Boundaries**: [NsDepCop](https://github.com/realvizu/NsDepCop) rules fail the build when code crosses a layer or slice boundary (see below)

### Dependency Rules (NsDepCop)

With everything in one project, project references can't keep layers apart. Instead, `config.nsdepcop` defines namespace rules, and any violation fails the build with error `NSDEPCOP01`:

- `Domain` must not depend on `Infrastructure`, `CartFeatures`, `ProductFeatures`, or `Configurations`
- Feature slices (`CartFeatures`, `ProductFeatures`) must not depend on `Infrastructure`. Use interfaces defined in `Domain`.
- Feature slices must not depend on each other. Share code through `Domain`.
- `Domain.OrderAggregate` must not depend on `Domain.CartAggregate`

Rules match namespaces, not folders, so keep each file's namespace matching its folder. When you add a new feature slice, add matching rules for it in `config.nsdepcop`. See [Enforcing Dependency Rules](https://cleanarchitecture.ardalis.com/dependency-rules/) for details.

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

- **.NET 11**: Latest framework
- **FastEndpoints**: REPR pattern for API endpoints
- **Entity Framework Core**: Data access with migrations
- **SQL Server**: Containerized database via Aspire (easily switched to PostgreSQL, SQLite, etc.)
- **Aspire**: Cloud-ready orchestration and observability
- **Serilog**: Structured logging
- **FluentValidation**: Request validation

## Common Tasks

### Adding a New Feature (Vertical Slice)

1. **Create Domain Entity**: Add to `Domain/YourFeatureAggregate/`
2. **Add EF Configuration**: Create config in `Infrastructure/Data/Config/`
3. **Create Migration**: `dotnet ef migrations add AddYourFeature`
4. **Create Endpoints**: Add FastEndpoints endpoints and handlers in `YourFeatureFeatures/` (one folder per use case)
5. **Add Dependency Rules**: In `config.nsdepcop`, add `Disallowed` rules so `Domain` can't depend on the new slice, the slice can't depend on `Infrastructure`, and the slice and the existing slices can't depend on each other

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
- [NsDepCop](https://github.com/realvizu/NsDepCop)
- [Ardalis on YouTube](https://www.youtube.com/@Ardalis)

## Need Help?

- Report issues: [GitHub Issues](https://github.com/ardalis/CleanArchitecture/issues)
- Ask questions: [Discussions](https://github.com/ardalis/CleanArchitecture/discussions)
- Follow updates: [@ardalis](https://twitter.com/ardalis)

---

**Happy Coding!** 🚀
