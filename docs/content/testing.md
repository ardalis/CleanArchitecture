---
title: "Building & Testing"
weight: 40
---

# Building & Testing

This page covers building and testing a solution generated from either template. The full template (`clean-arch`) includes three test projects. MinimalClean (`min-clean`) has no test projects; see [its testing strategy](templates/minimal-clean.md#testing-strategy).

## Prerequisites

- **.NET SDK**: template v12.x needs the .NET 11 SDK. `global.json` pins `11.0.100` with `rollForward: latestMajor` and `allowPrerelease: true`, so any newer SDK also works.
- **Docker** (optional): the functional tests run against SQL Server in a container when Docker can run Linux containers, and fall back to SQLite when it can't. The Aspire host also needs Docker.

## Build

```bash
dotnet build                                   # from the solution folder
dotnet build Your.ProjectName.slnx -c Release
```

Solution-wide build settings live in `Directory.Build.props`:

| Setting | Value | Effect |
|---|---|---|
| `TargetFramework` | `net11.0` | Every project targets the same framework |
| `Nullable` | `enable` | Nullable reference types everywhere |
| `ImplicitUsings` | `enable` | Common `using` directives are implicit; each project also has a `GlobalUsings.cs` |
| `TreatWarningsAsErrors` | `true` | **Any compiler or analyzer warning fails the build** |
| `NoWarn` | `1591` | Missing XML doc comments don't warn (remove to require them) |
| `ManagePackageVersionsCentrally` | `true` | Central Package Management (see below) |
| `CentralPackageTransitivePinningEnabled` | `true` | Central versions also pin transitive dependencies |

Because warnings are errors, analyzer rules matter. For example, the xUnit v3 analyzers require tests to pass `TestContext.Current.CancellationToken` to async calls that accept a cancellation token. In MinimalClean, a broken [dependency rule](dependency-rules.md) is also a build error.

### Central Package Management

Projects reference packages without versions (`<PackageReference Include="FastEndpoints" />`). Every version lives in `Directory.Packages.props`. To add or upgrade a package, use the CLI and it updates the right file:

```bash
dotnet add src/Your.ProjectName.Web package Some.Package
```

`CentralPackageTransitivePinningEnabled` lets you fix a vulnerable *transitive* dependency just by adding a `PackageVersion` for it, without referencing it from any project. The template pins `System.Security.Cryptography.Pkcs` this way.

### Package sources

`nuget.config` clears inherited package sources and maps every package to nuget.org, so restores behave the same on every machine and in CI. Add your private feeds there (with matching `packageSourceMapping` entries) if you need them.

## Test projects

Run every test from the solution root:

```bash
dotnet test                                    # everything in the solution
dotnet test tests/Your.ProjectName.UnitTests   # one project
```

| Project | What it tests | Database |
| --- | --- | --- |
| `UnitTests` | Domain entities, value objects, and use case handlers | None (dependencies are substituted with [NSubstitute](https://nsubstitute.github.io/)) |
| `IntegrationTests` | `EfRepository` and EF Core persistence | EF Core InMemory provider, plus in-memory SQLite for behavior that needs a relational provider |
| `FunctionalTests` | API endpoints, end to end, through `WebApplicationFactory`, using [Ardalis.HttpClientTestExtensions](https://github.com/ardalis/HttpClientTestExtensions) | SQL Server in Docker, or a SQLite file when Docker isn't available |

The `tests` folder also has an `AspireTests` project, an empty placeholder for [Aspire integration tests](https://learn.microsoft.com/dotnet/aspire/testing/write-your-first-test). It isn't in the `.slnx`, so `dotnet test` doesn't run it.

Conventions used throughout:

- **xUnit v3**, via the `xunit.v3.mtp-off` package (xUnit v3 running on VSTest, so `dotnet test` and IDE test explorers work as usual)
- **[Shouldly](https://docs.shouldly.org/)** assertions: `result.Id.ShouldBe(1)`
- One test class per method or behavior under test, named for it (`ContributorConstructor`, `ContributorUpdateName`, `EfRepositoryAdd`), with test methods describing the expected outcome
- Async tests use `TestContext.Current.CancellationToken`

Some EF Core behavior differs between the InMemory provider and relational providers. For example, relational providers give new entities a temporary negative key until `SaveChanges`. Test that kind of behavior against SQLite, as `SqliteContributorPersistence` does.

## Functional tests and the database

`CustomWebApplicationFactory` chooses a database when each test class starts:

1. **SQL Server, when Docker can run Linux containers.** The factory uses [Testcontainers](https://dotnet.testcontainers.org/) to start `mcr.microsoft.com/mssql/server:2025-latest`, then creates the schema with `EnsureCreated()` and seeds it with `SeedData`. The container is disposed when the test class finishes.
2. **SQLite otherwise.** If Docker isn't installed or running, or it's set to Windows containers, the factory uses a temporary SQLite file and deletes it afterward.

The factory logs which provider it chose: `Functional tests are using the {DatabaseProvider} database provider.`

Things to know:

- **Docker isn't required.** Tests pass either way, but only SQL Server catches SQL Server–specific problems. Without Docker, `DockerAvailabilityTests` is skipped with a message saying so.
- **First run is slow.** The SQL Server image is about 1.5 GB to download, and each test class starts its own container.
- **Endpoint tests don't run your migrations.** `EnsureCreated()` builds the schema straight from the current model. `SqlServerOnLinuxTests` is the test that applies the real EF Core migrations to SQL Server. It skips when Docker can't run Linux containers, unless `REQUIRE_SQL_SERVER_TESTS=true` is set, in which case it fails. CI sets that variable on Ubuntu, so a broken migration fails the build.
- **The fallback is tested too.** `SqliteFallbackTests` simulates Docker being unavailable, and checks that the factory falls back to SQLite (and doesn't when SQL Server is requested explicitly).
- **The container password is for tests only.** `Your_password123!` is set in the factory and used only by the throwaway container.

## Running tests in parallel

Parallelism is set in two places.

**`.runsettings`** at the solution root (in both templates) runs test assemblies in parallel, using all available processors:

```xml
<RunSettings>
  <RunConfiguration>
    <MaxCpuCount>0</MaxCpuCount>
    <DisableParallelization>false</DisableParallelization>
  </RunConfiguration>
  <xUnit>
    <ParallelizeAssembly>true</ParallelizeAssembly>
    <ParallelizeTestCollections>true</ParallelizeTestCollections>
    <MaxParallelThreads>0</MaxParallelThreads>
  </xUnit>
</RunSettings>
```

Visual Studio picks up a `.runsettings` file in the solution root automatically. To choose one explicitly, use **Test > Configure Run Settings > Select Solution Wide runsettings File**. From the command line, pass it to `dotnet test`:

```bash
dotnet test --settings .runsettings
```

**`xunit.runner.json`** in each test project controls parallelism inside that assembly. All three projects allow it:

```json
{
  "shadowCopy": false,
  "parallelizeAssembly": true,
  "parallelizeTestCollections": true,
  "maxParallelThreads": 0
}
```

`maxParallelThreads: 0` uses xUnit's default, which is one thread per processor.

### When tests share state

The endpoint test classes in `FunctionalTests` are marked `[Collection("Sequential")]`, so they run one after another instead of all starting containers at once. Give any new test class that shares a database, file, or port the same collection.

To turn off parallelism for a whole project, set both flags to `false` in its `xunit.runner.json`:

```json
{
  "shadowCopy": false,
  "parallelizeAssembly": false,
  "parallelizeTestCollections": false
}
```

## Code coverage

The test projects reference `coverlet.collector`:

```bash
dotnet test --collect:"XPlat Code Coverage"
```

This writes a `coverage.cobertura.xml` per test project under `TestResults/`. To turn those into an HTML report, use [ReportGenerator](https://reportgenerator.io/):

```bash
dotnet tool install --global dotnet-reportgenerator-globaltool
reportgenerator -reports:"**/TestResults/**/coverage.cobertura.xml" -targetdir:coveragereport -reporttypes:Html
```

## Continuous integration

Generated solutions don't include CI workflows. A minimal GitHub Actions job looks like this:

```yaml
- uses: actions/setup-dotnet@v5
  with:
    dotnet-version: '11.0.x'
- run: dotnet restore
- run: dotnet build --configuration Release --no-restore
- run: dotnet test --configuration Release --no-build
```

GitHub-hosted Ubuntu runners include Docker running Linux containers, so the functional tests use SQL Server there. On Windows and macOS runners, the functional tests fall back to SQLite. Set `REQUIRE_SQL_SERVER_TESTS: 'true'` on the Ubuntu job so the migration test can't be skipped. The template repository's own workflows are described in [Contributing](contributing.md#continuous-integration).

## Further reading

- [xUnit: Running tests in parallel](https://xunit.net/docs/running-tests-in-parallel)
- [Configure unit tests with a .runsettings file](https://learn.microsoft.com/visualstudio/test/configure-unit-tests-by-using-a-dot-runsettings-file)
- [Testcontainers for .NET](https://dotnet.testcontainers.org/)
