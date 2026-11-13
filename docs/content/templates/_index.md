---
title: "Choosing a Template"
weight: 20
bookCollapseSection: false
---

# Choosing a Template

This repository publishes two templates. They share the same principles (dependencies point inward toward the domain, infrastructure sits behind abstractions, and business logic is testable without the web or the database) but package them differently.

1. **[Clean Architecture](clean-architecture.md)** (`clean-arch`): a multi-project solution with separate Core, UseCases, Infrastructure, and Web projects, plus unit, integration, and functional test projects.
2. **[Minimal Clean Architecture](minimal-clean.md)** (`min-clean`): a single Web project organized into vertical slices (features), with namespace boundaries checked at build time.

```bash
dotnet new install Ardalis.CleanArchitecture.Template   # clean-arch
dotnet new install Ardalis.MinimalClean.Template        # min-clean
```

## Comparison

| | Clean Architecture | Minimal Clean Architecture |
|---|---|---|
| **Command** | `dotnet new clean-arch` | `dotnet new min-clean` |
| **NuGet package** | `Ardalis.CleanArchitecture.Template` | `Ardalis.MinimalClean.Template` |
| **Package versioning** | 12.x targets .NET 11 (see [Versions](../versions.md)) | Same version as the full template since 12.0.0 (1.x targeted .NET 10) |
| **Application projects** | Core, UseCases, Infrastructure, Web | Web |
| **Aspire projects** | AspireHost, ServiceDefaults | AspireHost, ServiceDefaults |
| **Test projects** | UnitTests, IntegrationTests, FunctionalTests, AspireTests (placeholder) | None |
| **Organization** | By layer (horizontal) | By feature (vertical slices) |
| **Boundary enforcement** | Project references (the compiler) | Namespace rules via the NsDepCop analyzer (build errors); see [Enforcing Dependency Rules](../dependency-rules.md) |
| **DDD patterns** | Aggregates, strongly typed IDs, value objects, smart enums, domain events, domain services, specifications | Aggregates, strongly typed IDs, value objects, specifications; domain events optional |
| **Use cases / CQRS** | Commands and queries with Mediator handlers in a separate UseCases project | Mediator commands and queries colocated with endpoints in each feature folder; optional |
| **Data access** | Repository + specifications; query services for reads | Repository + specifications where helpful; query services for reads |
| **Database** | SQL Server (Aspire container or LocalDB on Windows) with SQLite fallback | SQL Server (Aspire container or LocalDB) |
| **Example domain** | Contributors | Products, carts, guest users, orders |
| **Complexity** | Higher: more projects and abstractions | Lower: one project to navigate |
| **Best for** | Large or long-lived apps, multiple teams, complex domains | MVPs, smaller apps, rapid iteration |
| **Growth path** | Can be simplified to minimal | Can grow into the full template |

## Which template should I use?

**Choose Clean Architecture (`clean-arch`) if:**

- You're building a large or complex application that will be maintained for years
- Multiple teams or developers need strict, compiler-enforced boundaries
- The domain is complex enough to benefit from the full set of DDD patterns
- You want a ready-made test suite structure (unit, integration, functional)

**Choose Minimal Clean Architecture (`min-clean`) if:**

- You're building an MVP or a smaller application
- You want architectural guidance without multi-project overhead
- You prefer vertical slice (feature-folder) organization
- Your team values simplicity and fast iteration
- You might grow into the full template later

**Not sure?** Start with Minimal Clean and move to the full Clean Architecture template if your application grows in complexity. [Minimal Clean Architecture](minimal-clean.md#migration-paths) describes how to make that move, and the reverse.

## What both templates include

- **ASP.NET Core** API using [FastEndpoints](https://fast-endpoints.com/) and the [REPR pattern](https://deviq.com/design-patterns/repr-design-pattern), with [FluentValidation](https://docs.fluentvalidation.net/) request validators
- **OpenAPI** documents with [Scalar](https://scalar.com/) and Swagger UI in Development
- **EF Core** with migrations, a generic `EfRepository<T>`, and [Ardalis.Specification](https://github.com/ardalis/Specification)
- **[Mediator](https://github.com/martinothamar/Mediator)** (a source-generated mediator) for commands, queries, domain events, and pipeline behaviors such as logging
- **[Vogen](https://github.com/SteveDunn/Vogen)** strongly typed IDs and value objects
- **[Ardalis.Result](https://github.com/ardalis/Result)** for returning success, not-found, and validation outcomes without exceptions, and **[Ardalis.GuardClauses](https://github.com/ardalis/GuardClauses)**
- **Serilog** logging and **OpenTelemetry** via the Aspire service defaults
- **.NET Aspire** host that runs SQL Server and a [Papercut](https://github.com/ChangemakerStudios/Papercut-SMTP) SMTP server in containers
- **Central Package Management** (`Directory.Packages.props`), nullable reference types, and warnings treated as errors
