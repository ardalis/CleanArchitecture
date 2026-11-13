---
title: "Customizing"
weight: 60
---

# Customizing Your Solution

A generated solution is yours to change. This page covers the most common first changes.

## Replace the example code

The example code exists to show where things go. Once you understand it, remove it and add your own. In the full template, the `Contributor` example touches:

| Project | Example code |
|---|---|
| Core | `ContributorAggregate/`, `Interfaces/IDeleteContributorService.cs`, `Services/DeleteContributorService.cs` |
| UseCases | `Contributors/` |
| Infrastructure | `Data/Config/ContributorConfiguration.cs`, the `Contributors` `DbSet` in `AppDbContext`, `Data/Queries/`, `Data/SeedData.cs`, `Data/Migrations/` |
| Web | `Contributors/`, the sample requests in `api.http` |
| Tests | Tests named for contributors, plus the seed-data assertions in FunctionalTests |

In the minimal template, the example domain is products, carts, guest users, and orders under `Domain/`, `CartFeatures/`, `ProductFeatures/`, and `Infrastructure/Data/`.

Consider deleting the existing migrations and creating a fresh initial migration once your model takes shape (see [Data & Migrations](data-and-migrations.md#starting-fresh)).

## Adding a feature (full template)

A typical end-to-end feature touches each layer once:

1. **Core**: add an aggregate folder with the entity, strongly typed ID, value objects, and any domain events and specifications.
2. **Infrastructure**: add an `IEntityTypeConfiguration<T>` in `Data/Config/`, a `DbSet<T>` on `AppDbContext`, and a migration. Add a query service if a read needs a custom query.
3. **UseCases**: add a folder per command or query, each with the message type and its handler.
4. **Web**: add FastEndpoints endpoints (request, response, validator) that send the commands and queries through `IMediator`.
5. **Tests**: unit-test the domain and handlers, and add functional tests for the endpoints.

Mediator discovers handlers in the assemblies listed in `Web/Configurations/MediatorConfig.cs`, which already covers Core, UseCases, Infrastructure, and Web.

## Replace Ardalis.SharedKernel

The template references the [Ardalis.SharedKernel](https://github.com/ardalis/Ardalis.SharedKernel) package for base types such as `EntityBase`, `ValueObject`, `IAggregateRoot`, the repository interfaces, domain event dispatching, and `LoggingBehavior`. It's meant as an example: create your own shared kernel package for your organization and replace it. See [The SharedKernel package](design-decisions.md#the-sharedkernel-package).

## Controllers, Razor Pages, and ApiEndpoints

Since template v8, the template only includes API endpoints built with FastEndpoints. If you want to use the Ardalis.ApiEndpoints library, Razor Pages, or Controllers, you can use the last template that included them, [version 7.1](https://www.nuget.org/packages/Ardalis.CleanArchitecture.Template/7.1.0). Alternatively, they're easy to add to a newer solution.

### Add Ardalis.ApiEndpoints

To use [Ardalis.ApiEndpoints](https://www.nuget.org/packages/Ardalis.ApiEndpoints) instead of (or in addition to) [FastEndpoints](https://fast-endpoints.com/), add the reference and use the base classes from its documentation:

```bash
dotnet add src/Your.ProjectName.Web package Ardalis.ApiEndpoints
```

### Add Controllers

Add support for controllers to `Program.cs`:

```csharp
builder.Services.AddControllers(); // AddControllersWithViews if you need views

// and

app.MapControllers();
```

Once these are in place, you can create a `Controllers` folder and (optionally) a `Views` folder, and everything should work as expected. Personally, I find Razor Pages to be much better than Controllers and Views, so if you haven't fully investigated Razor Pages, you might want to do so before you choose Views.

### Add Razor Pages

Add support for Razor Pages to `Program.cs`:

```csharp
builder.Services.AddRazorPages();

// and

app.MapRazorPages();
```

Then add a `Pages` folder in the root of the Web project and go from there.

## Using a different database

The full template supports SQL Server and SQLite out of the box (see [how the database is chosen](running.md#how-the-database-is-chosen)). To use another EF Core provider, such as PostgreSQL:

1. Add the provider package to Infrastructure (for example `Npgsql.EntityFrameworkCore.PostgreSQL`) and remove the ones you no longer need.
2. Update `AddInfrastructureServices()` in `InfrastructureServiceExtensions.cs` to call the new provider (`options.UseNpgsql(connectionString)`) and to read the connection string you want.
3. In the app host, replace `AddSqlServer(...)` with the matching Aspire hosting integration (for example `Aspire.Hosting.PostgreSQL` and `AddPostgres(...)`), keeping the database name the Web project expects.
4. Delete the existing migrations (they're SQL Server-specific) and create new ones.
5. Update `CustomWebApplicationFactory` in FunctionalTests to start the matching Testcontainers module.

The raw SQL inserts in `SeedData.cs` use SQL Server bracket quoting, so check them too.

## Removing .NET Aspire

Aspire is optional. To remove it from the full template:

1. Delete the `AspireHost` and `ServiceDefaults` projects (and `tests/*.AspireTests`) and remove them from the `.slnx`.
2. Remove the ServiceDefaults project reference from the Web project.
3. In the Web project's `Program.cs`, remove `builder.AddServiceDefaults()` and `app.MapDefaultEndpoints()`. Add back anything you still want, such as health checks (`builder.Services.AddHealthChecks()` and `app.MapHealthChecks("/health")`) or OpenTelemetry.
4. Delete `aspire.config.json` and the `.aspire` folder, and remove the Aspire and OpenTelemetry package versions from `Directory.Packages.props`.

The Web project then runs [standalone](running.md#option-2-run-the-web-project-directly). The minimal template works the same way, but it requires a SQL Server you provide.

## Email

Both templates send email through `IEmailSender`. `MimeKitEmailSender` sends via SMTP using the `Mailserver` settings, and `FakeEmailSender` only logs. Choose which one is registered in `Web/Configurations/ServiceConfigs.cs`, or add your own implementation in Infrastructure (for example, a transactional email API client).
