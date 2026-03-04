# Plan: Migrate to Aspire Single-File AppHost

**Date:** 2026-03-04
**Aspire Version:** 13.0.0
**Requires:** .NET 10 SDK, Aspire CLI (`aspire` tool)
**References:**
- [What's New in Aspire 13](https://aspire.dev/whats-new/aspire-13/)
- [Aspire Service Defaults](https://aspire.dev/fundamentals/service-defaults/)
- [Single-File AppHost PR #11393](https://github.com/dotnet/aspire/pull/11393)

---

## Goal

Replace the two-project Aspire setup (traditional csproj-based AppHost + ServiceDefaults class library) with the `aspire-apphost-singlefile` template introduced in Aspire 13. The single-file format uses `#:sdk` and `#:package` directives instead of a `.csproj` file, eliminating project scaffolding overhead.

### Projects to Remove
- `src/Clean.Architecture.AspireHost/` — replaced by a single-file `apphost.cs`
- `src/Clean.Architecture.ServiceDefaults/` — its extensions inlined into the Web project (see Step 4)

---

## Pre-Flight Checks

Before executing, verify:
1. `dotnet --version` reports .NET 10 SDK
2. `aspire --version` reports 13.x
3. Single-file feature flag is enabled (Aspire 13 may require it):
   ```bash
   aspire config set features:singlefileAppHostEnabled true
   ```

---

## Step 1 — Enable the Experimental Feature and Scaffold the Template

Run from the `src/` directory:

```bash
cd src
aspire new aspire-apphost-singlefile -n Clean.Architecture.AppHost
```

This creates:
- `src/apphost.cs` — the single-file AppHost entry point
- `src/apphost.run.json` — launch profile (replaces `launchSettings.json`)

> **Note:** If the CLI names the output file differently (e.g., after the `-n` argument), rename to `apphost.cs` for clarity.

### Expected generated skeleton (`apphost.cs`)

```csharp
#:sdk Aspire.AppHost.Sdk@13.0.0
#:package Aspire.Hosting.AppHost@13.0.0

var builder = DistributedApplication.CreateBuilder(args);
builder.Build().Run();
```

---

## Step 2 — Migrate AppHost Behavior

Migrate all content from `src/Clean.Architecture.AspireHost/AppHost.cs` into the new `apphost.cs`, adding `#:package` directives for each NuGet dependency.

### Current behavior in `AppHost.cs`
- SQL Server container with persistent lifetime
- `cleanarchitecture` database on that server
- Papercut SMTP container (ports 25 and 37408)
- Web project reference, wired to the database and Papercut

### Target `apphost.cs`

```csharp
#:sdk Aspire.AppHost.Sdk@13.0.0
#:package Aspire.Hosting.AppHost@13.0.0
#:package Aspire.Hosting.SqlServer@13.0.0

using System.Net.Sockets;

var builder = DistributedApplication.CreateBuilder(args);

// Add SQL Server container
var sqlServer = builder.AddSqlServer("sqlserver")
  .WithLifetime(ContainerLifetime.Persistent);

// Add the database
var cleanArchDb = sqlServer.AddDatabase("cleanarchitecture");

// Papercut SMTP container for email testing
var papercut = builder.AddContainer("papercut", "jijiechen/papercut", "latest")
  .WithEndpoint("smtp", e =>
  {
    e.TargetPort = 25;
    e.Port = 25;
    e.Protocol = ProtocolType.Tcp;
    e.UriScheme = "smtp";
  })
  .WithEndpoint("ui", e =>
  {
    e.TargetPort = 37408;
    e.Port = 37408;
    e.UriScheme = "http";
  });

// Add the web project with the database connection
builder.AddProject<Projects.Clean_Architecture_Web>("web")
  .WithReference(cleanArchDb)
  .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
  .WithEnvironment("Papercut__Smtp__Url", papercut.GetEndpoint("smtp"))
  .WaitFor(cleanArchDb)
  .WaitFor(papercut);

builder
  .Build()
  .Run();
```

> **Important:** The `AddProject<Projects.Clean_Architecture_Web>` strongly-typed reference requires Aspire metadata generation. Confirm the Web project is still discoverable by the Aspire tooling (it does not change — the Web project retains its `.csproj`). If the single-file apphost cannot resolve `Projects.*` metadata without a companion `.csproj`, fall back to `builder.AddProject("web", "../Clean.Architecture.Web/Clean.Architecture.Web.csproj")` string-based overload instead.

---

## Step 3 — Migrate ServiceDefaults Behavior

The `Clean.Architecture.ServiceDefaults` project contains `Extensions.cs` with:
- `AddServiceDefaults<TBuilder>()` — OpenTelemetry + health checks + service discovery
- `ConfigureOpenTelemetry<TBuilder>()` — metrics, tracing, OTLP exporter
- `AddDefaultHealthChecks<TBuilder>()` — liveness check
- `MapDefaultEndpoints(WebApplication)` — `/health` and `/alive` endpoints (dev-only)

The Web project currently references this as a `ProjectReference`. Since Aspire documentation recommends keeping ServiceDefaults as a separate project even in single-file scenarios, **two options** are presented:

### Option A — Inline into the Web project (removes ServiceDefaults project)

Move `Extensions.cs` content directly into the `Clean.Architecture.Web` project:

1. Add the NuGet packages from `ServiceDefaults.csproj` directly to `Clean.Architecture.Web.csproj`:
   ```xml
   <PackageReference Include="Microsoft.Extensions.Http.Resilience" />
   <PackageReference Include="Microsoft.Extensions.ServiceDiscovery" />
   <PackageReference Include="OpenTelemetry.Exporter.OpenTelemetryProtocol" />
   <PackageReference Include="OpenTelemetry.Extensions.Hosting" />
   <PackageReference Include="OpenTelemetry.Instrumentation.AspNetCore" />
   <PackageReference Include="OpenTelemetry.Instrumentation.Http" />
   <PackageReference Include="OpenTelemetry.Instrumentation.Runtime" />
   ```
2. Copy `Extensions.cs` into `src/Clean.Architecture.Web/` (e.g., as `AspireExtensions.cs`), keeping the same `namespace Microsoft.Extensions.Hosting`.
3. Remove the `<ProjectReference Include="..\Clean.Architecture.ServiceDefaults\..." />` from `Web.csproj`.
4. Delete `src/Clean.Architecture.ServiceDefaults/`.

### Option B — Regenerate ServiceDefaults with latest template (keeps it as separate project)

```bash
cd src
dotnet new aspire-servicedefaults -n Clean.Architecture.ServiceDefaults --force
```

This refreshes the project with the latest Aspire 13 template while preserving the separate project structure. Only the project file and Extensions.cs would change if the template has been updated.

**Recommended: Option A** — since the user goal is to remove both projects and the Extensions.cs content is small (127 lines). The single-file apphost + inlined service defaults gives the cleanest result with no orphan helper projects.

---

## Step 4 — Update `Clean.Architecture.Web.csproj`

Apply the Option A changes to [src/Clean.Architecture.Web/Clean.Architecture.Web.csproj](src/Clean.Architecture.Web/Clean.Architecture.Web.csproj):

- **Remove:**
  ```xml
  <ProjectReference Include="..\Clean.Architecture.ServiceDefaults\Clean.Architecture.ServiceDefaults.csproj" />
  ```
- **Add:**
  ```xml
  <PackageReference Include="Microsoft.Extensions.Http.Resilience" />
  <PackageReference Include="Microsoft.Extensions.ServiceDiscovery" />
  <PackageReference Include="OpenTelemetry.Exporter.OpenTelemetryProtocol" />
  <PackageReference Include="OpenTelemetry.Extensions.Hosting" />
  <PackageReference Include="OpenTelemetry.Instrumentation.AspNetCore" />
  <PackageReference Include="OpenTelemetry.Instrumentation.Http" />
  <PackageReference Include="OpenTelemetry.Instrumentation.Runtime" />
  ```

Copy `src/Clean.Architecture.ServiceDefaults/Extensions.cs` → `src/Clean.Architecture.Web/AspireExtensions.cs` (unchanged content, same namespace).

---

## Step 5 — Update `Clean.Architecture.slnx`

Edit [Clean.Architecture.slnx](Clean.Architecture.slnx):

### Remove
```xml
<Folder Name="/src/_aspire/">
  <Project Path="src/Clean.Architecture.AspireHost/Clean.Architecture.AspireHost.csproj" />
  <Project Path="src/Clean.Architecture.ServiceDefaults/Clean.Architecture.ServiceDefaults.csproj" />
</Folder>
```

### Add (as a Solution Items file reference)
```xml
<Folder Name="/src/_aspire/">
  <File Path="src/apphost.cs" />
  <File Path="src/apphost.run.json" />
</Folder>
```

> The `.slnx` format supports `<File>` entries (as seen in the existing "Solution Items" folder) so the single-file apphost can still be visible in the IDE.

---

## Step 6 — Update `Directory.Packages.props`

Remove entries that were only needed by the deleted projects:

```xml
<!-- Remove — no longer needed by AspireHost csproj -->
<PackageVersion Include="Aspire.Hosting.AppHost" Version="13.0.0" />
<PackageVersion Include="Aspire.Hosting.SqlServer" Version="13.0.0" />
```

> Keep if the `apphost.cs` single-file directives resolve independently (they do — `#:package` directives are self-contained and do not use `Directory.Packages.props`).

Also check `Aspire.Hosting.Testing` (currently `9.5.1`) — it may need to be bumped to `13.0.0` to match the rest of the Aspire stack if functional tests use it.

The OpenTelemetry and service discovery package versions are still needed by `Clean.Architecture.Web` after inlining:
- `Microsoft.Extensions.Http.Resilience` — keep
- `Microsoft.Extensions.ServiceDiscovery` — keep
- `OpenTelemetry.*` entries — keep

---

## Step 7 — Delete Legacy Project Directories

After verifying the build succeeds:

```bash
rm -rf src/Clean.Architecture.AspireHost
rm -rf src/Clean.Architecture.ServiceDefaults
```

---

## Step 8 — Verify

1. `dotnet build Clean.Architecture.slnx` — should succeed with no project references to deleted projects
2. `aspire run src/apphost.cs` — should launch the dashboard with sqlserver, papercut, and web resources
3. Web app `/health` and `/alive` endpoints respond (confirms ServiceDefaults extensions are active)
4. Run tests: `dotnet test Clean.Architecture.slnx`

---

## File Changes Summary

| Action | Path |
|--------|------|
| **Create** | `src/apphost.cs` |
| **Create** | `src/apphost.run.json` |
| **Create** | `src/Clean.Architecture.Web/AspireExtensions.cs` |
| **Modify** | `src/Clean.Architecture.Web/Clean.Architecture.Web.csproj` |
| **Modify** | `Clean.Architecture.slnx` |
| **Modify** | `Directory.Packages.props` |
| **Delete** | `src/Clean.Architecture.AspireHost/` (entire folder) |
| **Delete** | `src/Clean.Architecture.ServiceDefaults/` (entire folder) |

---

## Risks and Mitigations

| Risk | Mitigation |
|------|-----------|
| `Projects.Clean_Architecture_Web` metadata not available without companion `.csproj` | Fall back to string-based `AddProject("web", "../Clean.Architecture.Web/Clean.Architecture.Web.csproj")` overload |
| Single-file apphost not discoverable by Visual Studio | Acceptable — VS doesn't support single-file apphost; use VS Code or `aspire run` CLI |
| `Aspire.Hosting.Testing` version mismatch in functional tests | Bump `Aspire.Hosting.Testing` from `9.5.1` to `13.0.0` in `Directory.Packages.props` |
| ServiceDefaults namespace collision after inlining | Extensions.cs uses `namespace Microsoft.Extensions.Hosting` — verify no duplicate type errors in Web project |
| `#:package` versions not pinned via Central Package Management | Single-file directives are self-managed; document that these versions must be updated manually when upgrading Aspire |
