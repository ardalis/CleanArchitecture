# Testcontainers Implementation for Functional Tests

## Summary

Successfully migrated functional tests from in-memory database to **Testcontainers** with SQL Server 2022. This provides a more realistic testing environment that matches production database behavior.

## Changes Made

### 1. Package References

**Added to `Directory.Packages.props`:**
```xml
<PackageVersion Include="Testcontainers" Version="4.3.0" />
<PackageVersion Include="Testcontainers.MsSql" Version="4.3.0" />
```

**Updated `tests\Clean.Architecture.FunctionalTests\Clean.Architecture.FunctionalTests.csproj`:**
- Removed: `Microsoft.EntityFrameworkCore.InMemory`
- Added: `Testcontainers` and `Testcontainers.MsSql`

### 2. CustomWebApplicationFactory

**File:** `tests\Clean.Architecture.FunctionalTests\CustomWebApplicationFactory.cs`

Key changes:
- Implements `IAsyncLifetime` for proper async initialization/cleanup
- Creates a SQL Server container using `MsSqlBuilder`
- Uses SQL Server 2022 image: `mcr.microsoft.com/mssql/server:2022-latest`
- Applies EF Core migrations instead of `EnsureCreated()`
- Each test run gets a fresh containerized SQL Server instance

```csharp
private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
  .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
  .WithPassword("Your_password123!")
  .Build();
```

### 3. Test Configuration

Tests now:
- Use real SQL Server running in Docker containers
- Apply actual EF Core migrations (matches production)
- Run against isolated database instances (parallel test safety)
- Clean up containers automatically after tests complete

## Benefits

? **Realistic Testing**: Tests run against actual SQL Server, not in-memory provider  
? **Migration Testing**: Validates that migrations work correctly  
? **Production Parity**: Database behavior matches production environment  
? **Isolation**: Each test class gets its own containerized database  
? **Automatic Cleanup**: Containers are disposed after tests complete

## Requirements

- **Docker Desktop** must be running on the development machine
- Tests take slightly longer (~10-13 seconds vs instant with in-memory)
- First run downloads SQL Server 2022 Docker image (~1.5 GB)

## Test Results

All 18 tests passing:
- ? Unit Tests: 15 tests
- ? Functional Tests: 3 tests
- ? Total Duration: ~12 seconds

## Usage

Run tests normally:
```bash
dotnet test
```

Or specifically for functional tests:
```bash
dotnet test tests\Clean.Architecture.FunctionalTests\Clean.Architecture.FunctionalTests.csproj
```

## Notes

- Tests use the "Testing" environment configuration
- SQL Server password: `Your_password123!` (only for test containers)
- Container lifecycle managed by xUnit's `IAsyncLifetime`
- Containers are automatically cleaned up even if tests fail
