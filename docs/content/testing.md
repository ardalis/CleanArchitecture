---
title: "Testing"
weight: 37
---

# Testing

The full template (`clean-arch`) includes three test projects. MinimalClean (`min-clean`) has no test projects. Run every test from the solution root:

```bash
dotnet test
```

## Test projects

| Project | What it tests | Database |
| --- | --- | --- |
| `UnitTests` | Domain entities, value objects, and use case handlers | None (dependencies are substituted with NSubstitute) |
| `IntegrationTests` | `EfRepository` and EF Core persistence | EF Core InMemory provider, plus in-memory SQLite for behavior that needs a relational provider |
| `FunctionalTests` | API endpoints, end to end, through `WebApplicationFactory` | SQL Server in Docker, or a SQLite file when Docker isn't available |

All test projects use xUnit v3 and [Shouldly](https://docs.shouldly.org/) assertions.

Some EF Core behavior differs between the InMemory provider and relational providers. For example, relational providers give new entities a temporary negative key until `SaveChanges`. Test that kind of behavior against SQLite, as `SqliteContributorPersistence` does.

## Functional tests and the database

`CustomWebApplicationFactory` chooses a database when each test class starts:

1. **SQL Server, when Docker can run Linux containers.** The factory uses [Testcontainers](https://dotnet.testcontainers.org/) to start `mcr.microsoft.com/mssql/server:2025-latest`, then creates the schema with `EnsureCreated()` and seeds it with `SeedData`. The container is disposed when the test class finishes.
2. **SQLite otherwise.** If Docker isn't installed or running, or it's set to Windows containers, the factory uses a temporary SQLite file and deletes it afterward.

The factory logs which provider it chose: `Functional tests are using the {DatabaseProvider} database provider.`

Things to know:

- **Docker isn't required.** Tests pass either way, but only SQL Server catches SQL Server–specific problems.
- **First run is slow.** The SQL Server image is about 1.5 GB to download, and each test class starts its own container.
- **Endpoint tests don't run your migrations.** `EnsureCreated()` builds the schema straight from the current model. `SqlServerOnLinuxTests` is the test that applies the real EF Core migrations to SQL Server. It skips when Docker can't run Linux containers, unless `REQUIRE_SQL_SERVER_TESTS=true` is set, in which case it fails. CI sets that variable on Ubuntu, so a broken migration fails the build.
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

## Further reading

- [xUnit: Running tests in parallel](https://xunit.net/docs/running-tests-in-parallel)
- [Configure unit tests with a .runsettings file](https://learn.microsoft.com/visualstudio/test/configure-unit-tests-by-using-a-dot-runsettings-file)
- [Testcontainers for .NET](https://dotnet.testcontainers.org/)
