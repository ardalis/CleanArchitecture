---
title: "Data & Migrations"
weight: 35
---

# Data & Migrations

Both templates use Entity Framework Core. You shouldn't need to do anything here to *run* a new solution, because startup creates or migrates the database for you in Development (see [Running the Application](running.md#migrations-and-seed-data-at-startup)). Read on when you change the model.

## Install the EF Core tools

```bash
dotnet tool install --global dotnet-ef
# or update an existing install
dotnet tool update --global dotnet-ef
```

## Full template

The `AppDbContext` and migrations live in the **Infrastructure** project (`Data/Migrations`), while the **Web** project is the startup project that supplies configuration. Specify both when you run EF Core commands.

### Add a migration

From the Web project directory:

```bash
dotnet ef migrations add MigrationName -c AppDbContext -p ../Your.ProjectName.Infrastructure/Your.ProjectName.Infrastructure.csproj -s Your.ProjectName.Web.csproj -o Data/Migrations
```

Or from the solution folder:

```bash
dotnet ef migrations add MigrationName -c AppDbContext -p src/Your.ProjectName.Infrastructure -s src/Your.ProjectName.Web -o Data/Migrations
```

In Visual Studio's Package Manager Console:

```powershell
Add-Migration MigrationName -StartupProject Your.ProjectName.Web -Context AppDbContext -Project Your.ProjectName.Infrastructure -OutputDir Data/Migrations
```

> [!IMPORTANT]
> The design-time tools run the Web project's startup code, so they use the [same provider selection](running.md#how-the-database-is-chosen) as the app. On Windows that's SQL Server; on macOS and Linux it's **SQLite** unless you set `USE_SQL_SERVER=true`. The template's migrations target SQL Server, so on non-Windows machines set the variable first:
>
> ```bash
> export USE_SQL_SERVER=true    # PowerShell: $env:USE_SQL_SERVER = "true"
> ```
>
> Adding a migration doesn't need a reachable database, only the right provider.

### Update the database

Startup applies migrations automatically in Development (SQL Server only). To apply them yourself, from the Web project directory:

```bash
dotnet ef database update -c AppDbContext -p ../Your.ProjectName.Infrastructure/Your.ProjectName.Infrastructure.csproj -s Your.ProjectName.Web.csproj
```

This uses the `DefaultConnection` connection string, so point it at the server you want to update.

### SQL Server and SQLite

- **SQL Server** (Aspire, or `DefaultConnection`) uses **migrations**: `Database.MigrateAsync()` at startup.
- **SQLite** (the fallback) uses **`EnsureCreated`**, which builds the schema straight from the model and skips migrations, because the migrations are SQL Server-flavored. Delete `database.sqlite` to recreate it after model changes.

To make the app always use SQL Server, set `USE_SQL_SERVER=true` or run through Aspire. To use only SQLite, remove `DefaultConnection` from `appsettings.json`.

To switch to a different database entirely (PostgreSQL, for example), see [Customizing](customizing.md#using-a-different-database).

### Starting fresh

The template ships with migrations for its example `Contributor` model. When you replace the example domain with your own, it's often simplest to delete `Data/Migrations` and create a new initial migration for your model:

```bash
dotnet ef migrations add InitialCreate -c AppDbContext -p src/Your.ProjectName.Infrastructure -s src/Your.ProjectName.Web -o Data/Migrations
```

## Minimal Clean template

Everything is in the Web project, and migrations live in `Infrastructure/Data/Migrations`:

```bash
dotnet ef migrations add MigrationName -c AppDbContext -p src/Your.ProjectName.Web -s src/Your.ProjectName.Web -o Infrastructure/Data/Migrations
dotnet ef database update -c AppDbContext -p src/Your.ProjectName.Web -s src/Your.ProjectName.Web
```

The minimal template includes a design-time `AppDbContextFactory`, which the EF Core tools use, and which always targets SQL Server. It reads the `ConnectionStrings__AppDb` environment variable and otherwise uses a placeholder connection to `localhost,1433`. Set the variable when you need the tools to reach a real database (for `database update`):

```bash
export ConnectionStrings__AppDb="Server=localhost,1433;Database=AppDb;User ID=sa;Password=<password>;TrustServerCertificate=true"
```

At runtime, the app applies pending migrations at startup and seeds sample products.

## Seed data

Both templates and the sample seed the database at startup so you have data to work with right away. This seeding is meant for local development and demos only.

- **Full template** (`clean-arch`): `Infrastructure/Data/SeedData.cs` inserts 27 contributors (enough to demonstrate paging), but only when the `Contributors` table is empty. `SeedData.InitializeAsync` is called from `MiddlewareConfig`, and only in the Development environment or when `Database:ApplyMigrationsOnStartup` is `true`. The functional tests use the same seed data.
- **Minimal template** (`min-clean`): `Infrastructure/Data/SeedData.cs` inserts sample products when there are none.
- **Sample** (`NimblePros.SampleToDo`): seeds in **every** environment, after `EnsureCreated`. If the `ToDoItems` table is empty, `PopulateTestDataAsync` **deletes all Projects, ToDoItems, and Contributors** before inserting sample data. If you point a development build at a shared or production database, or someone empties that table, you can lose real data.

> [!WARNING]
> Before using either template or the sample as the basis for a real application, replace the sample seeding with seeding based on your own data, or remove it. If you build on the sample, at minimum restrict seeding to the Development environment (for example, wrap the call in `if (app.Environment.IsDevelopment())`). See [#953](https://github.com/ardalis/CleanArchitecture/issues/953).

## Strongly typed IDs

Entity IDs are [Vogen](https://github.com/SteveDunn/Vogen) value objects (for example `ContributorId`). EF Core stores them as their underlying type through value converters (`Data/Config/VogenEfCoreConverters.cs`). In the full template, integer IDs are generated by the database (`ValueGeneratedOnAdd()`, an `IDENTITY` column on SQL Server). See the [v11 to v12 migration guide](migration-guides/v11-to-v12.md#4-strongly-typed-ids-are-now-generated-by-the-database) if you're upgrading from an older template that generated IDs in application code.
