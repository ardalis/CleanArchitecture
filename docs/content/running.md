---
title: "Running the Application"
weight: 30
---

# Running the Application

Both templates can run two ways: through the **.NET Aspire host** (recommended, needs Docker), or by running the **Web project directly**.

## Option 1: run with .NET Aspire (recommended)

```bash
dotnet run --project src/Your.ProjectName.AspireHost
```

Or set the `AspireHost` project as the startup project in Visual Studio or Rider and press F5. If you have the [Aspire CLI](https://learn.microsoft.com/dotnet/aspire/cli/overview) installed, `aspire run` from the solution folder works too (in the full template, `aspire.config.json` points it at the app host).

The Aspire host starts:

| Resource | What it is |
|---|---|
| `sqlserver` | A SQL Server container with a **persistent** lifetime, so data survives restarts |
| `cleanarchitecture` / `AppDb` | The application database (full / minimal template), created automatically |
| `papercut` | A [Papercut SMTP](https://github.com/ChangemakerStudios/Papercut-SMTP) container listening on port 25, with a web UI at `http://localhost:37408` to view sent email |
| `web` | Your Web project, with the database connection string injected and `WaitFor` dependencies on the containers |

The console prints the Aspire dashboard URL, including a login token. The dashboard shows each resource's logs, traces, and metrics (the Web project exports telemetry through the ServiceDefaults project).

> [!NOTE]
> The app host projects suppress the `ASPIRE010` warning on purpose. They use the Aspire DCP and dashboard NuGet packages rather than the Aspire CLI bundle, so F5 and `dotnet run` work without installing the Aspire CLI.

To reset the database, stop the app, delete the `sqlserver` container (from the dashboard or with `docker rm`), and start again.

## Option 2: run the Web project directly

```bash
dotnet run --project src/Your.ProjectName.Web
```

No Docker required, but you need a database the app can reach.

### How the database is chosen

**Full template.** `AddInfrastructureServices()` in Infrastructure picks the provider at startup, in this order:

| Order | Condition | Provider | Connection string |
|---|---|---|---|
| 1 | `ConnectionStrings:cleanarchitecture` is set (Aspire provides this) | SQL Server | Provided by Aspire |
| 2 | Running on **Windows**, or the `USE_SQL_SERVER` environment variable is `true`, and `ConnectionStrings:DefaultConnection` is set | SQL Server | `DefaultConnection` (LocalDB by default) |
| 3 | Otherwise | SQLite | `SqliteConnection` (`Data Source=database.sqlite`) |

So on Windows the Web project uses SQL Server LocalDB, which Visual Studio installs. On macOS and Linux it uses a local SQLite file. To use a different SQL Server, change `DefaultConnection` in `appsettings.json` (or override it with the `ConnectionStrings__DefaultConnection` environment variable) and set `USE_SQL_SERVER=true` on non-Windows machines.

**Minimal template.** It always uses SQL Server, with the `AppDb` connection string. It defaults to LocalDB in `appsettings.json`, and Aspire overrides it.

### Migrations and seed data at startup

- **Full template**: in the Development environment, or when `Database:ApplyMigrationsOnStartup` is `true`, startup applies EF Core migrations (SQL Server) or calls `EnsureCreated` (SQLite), then seeds sample contributors if the table is empty. Seeding errors are logged but don't stop the app.
- **Minimal template**: startup always applies pending migrations, then seeds sample products if there are none.

See [Data & Migrations](data-and-migrations.md) to create migrations or switch providers.

## URLs

The `https` launch profile is used by default:

| | Full template | Minimal template |
|---|---|---|
| API base URL | `https://localhost:57679` | `https://localhost:57379` |
| Scalar API reference (Development) | `/scalar` | `/scalar` |
| Swagger UI (Development) | `/swagger` | `/swagger` |
| OpenAPI document (Development) | `/openapi/v1.json` | `/openapi/v1.json` |
| Health checks (Development) | `/health` and `/alive` | `/health` and `/alive` |
| Registered services list (Development) | `/listservices` | `/listservices` |

`/listservices` comes from [Ardalis.ListStartupServices](https://github.com/ardalis/AspNetCoreStartupServices) and lists every service registered in the DI container, which is handy for debugging registrations. Choose your own path (or remove it) before you deploy anything sensitive.

Each Web project includes an `api.http` file with sample requests you can send from Visual Studio, Rider, or VS Code (with the REST Client extension).

## Email

Development uses `MimeKitEmailSender`, which sends through the SMTP server configured in the `Mailserver` section of `appsettings.json` (`localhost:25`). Under Aspire, that's the Papercut container, and you can view the messages at `http://localhost:37408`. In the full template, renaming a contributor sends an email.

If you run the Web project without Aspire and have no SMTP server on port 25, register `FakeEmailSender` instead (it just logs), in `Configurations/ServiceConfigs.cs`:

```csharp
// services.AddScoped<IEmailSender, MimeKitEmailSender>();
services.AddScoped<IEmailSender, FakeEmailSender>();
```

More background: [Configuring a Local Test Email Server](https://ardalis.com/configuring-a-local-test-email-server/).

## Logging and telemetry

The Web project uses Serilog for console output (configured in the `Serilog` section of `appsettings.json`; file and Application Insights sinks are sketched in comments) and OpenTelemetry for logs, metrics, and traces through `builder.AddServiceDefaults()`. When running under Aspire, all of it appears in the dashboard.

## Troubleshooting

### HTTPS certificate errors

The site uses HTTPS and expects a trusted ASP.NET Core development certificate for `localhost`:

```bash
dotnet dev-certs https --trust
```

If Chrome still complains, [see this answer](https://stackoverflow.com/a/31900210/13729) for mitigation steps.

### Docker isn't running

The Aspire host can't start the SQL Server or Papercut containers without Docker (or another supported container runtime). Start Docker Desktop, or use [option 2](#option-2-run-the-web-project-directly).

### Port 25 or 37408 is already in use

Papercut binds host ports 25 (SMTP) and 37408 (web UI). Stop whatever else is using them, or change the ports in the app host's `AppHost.cs` (and `Mailserver:Port` to match).

### LocalDB isn't installed (Windows)

Install SQL Server Express LocalDB (included with Visual Studio's ASP.NET workload), point `DefaultConnection` at another SQL Server, or (full template) remove `DefaultConnection` so the app falls back to SQLite.
