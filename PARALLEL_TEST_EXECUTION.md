# Parallel Test Execution Configuration

This workspace is configured to run all three test projects in parallel in Visual Studio Test Explorer.

## Configuration Files

### `.runsettings` (Solution Root)
- Enables parallel test execution at the assembly level
- `MaxCpuCount=0` uses all available processors to run test assemblies in parallel
- `DisableParallelization=false` ensures parallelization is enabled

### `xunit.runner.json` (In Each Test Project)
Each test project contains an `xunit.runner.json` file with:
- `parallelizeAssembly: true` - Allows tests from this assembly to run in parallel with other assemblies
- `parallelizeTestCollections: true` - Runs test collections within the assembly in parallel
- `maxParallelThreads: 0` - Uses xUnit's default algorithm (processors × 2)

## How to Use in Visual Studio

### Option 1: Configure via Test Explorer Settings
1. Open **Test Explorer** (Test ? Test Explorer)
2. Click the settings icon (??) in the toolbar
3. Select **Configure Run Settings** ? **Select Solution Wide runsettings File**
4. Browse to and select the `.runsettings` file at the solution root

### Option 2: Configure via Visual Studio Settings
1. Go to **Tools** ? **Options**
2. Navigate to **Test** ? **General**
3. Under **Run Settings File**, browse and select the `.runsettings` file

### Option 3: Automatic Detection
Visual Studio will automatically detect and use `.runsettings` in the solution root in most cases.

## Verification

After configuration, when you run all tests:
- The three test projects (UnitTests, IntegrationTests, FunctionalTests) will run in parallel
- Within each project, test collections will also run in parallel
- You should see multiple tests running simultaneously in Test Explorer

## Performance Considerations

**Recommended for:**
- Fast unit tests (UnitTests project)
- Independent integration tests that don't share state

**Use with caution for:**
- Tests using shared resources (databases, files, ports)
- Tests with Testcontainers - these may need collection fixtures to control parallelization

If you encounter issues with FunctionalTests (which use Testcontainers), you may need to:
1. Disable parallelization for that specific project by setting `parallelizeAssembly: false` in its `xunit.runner.json`
2. Use xUnit Collection Fixtures to control which tests can run in parallel
3. Configure Testcontainers to use unique ports/databases per test class

## Disabling Parallel Execution

To disable parallel execution for a specific test project, update its `xunit.runner.json`:
```json
{
  "shadowCopy": false,
  "parallelizeAssembly": false,
  "parallelizeTestCollections": false
}
```

## Command Line Usage

To use the .runsettings file when running tests from the command line:
```bash
dotnet test --settings .runsettings
```

## Related Documentation
- [xUnit Parallel Test Execution](https://xunit.net/docs/running-tests-in-parallel)
- [Visual Studio Run Settings](https://learn.microsoft.com/en-us/visualstudio/test/configure-unit-tests-by-using-a-dot-runsettings-file)
