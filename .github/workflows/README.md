# GitHub Actions Workflows

This directory contains CI/CD workflows for the Clean Architecture template.

## Workflows

### 🪟 Windows Build & Test (`windows-build-test.yml`)

**Purpose**: Windows-specific build and test pipeline for .NET 10

**Triggers**:
- Push to `main`, `develop`, or `issue-*/**` branches
- Pull requests to `main` or `develop`
- Manual workflow dispatch

**Matrix**:
- **OS**: `windows-latest`
- **Configurations**: `Debug`, `Release`

**Steps**:
1. Checkout code
2. Setup .NET 10 SDK (preview)
3. Use CI-specific NuGet.Config (nuget.org only, no Nexus)
4. Restore dependencies
5. Build solution (Debug & Release)
6. Run all tests with code coverage
7. Upload test results and coverage artifacts

**Artifacts**:
- Test results (TRX format, 30 days retention)
- Code coverage (Cobertura format, 30 days retention)

---

### 🌍 Cross-Platform Build & Test (`cross-platform-build-test.yml`)

**Purpose**: Ensure compatibility across Windows, Linux, and macOS

**Triggers**:
- Push to `main` or `develop`
- Pull requests to `main` or `develop`
- Manual workflow dispatch

**Matrix**:
- **OS**: `windows-latest`, `ubuntu-latest`, `macos-latest`
- **Configurations**: `Debug`, `Release`
- **Total jobs**: 6 (3 OS × 2 configurations)

**Steps**:
1. Checkout code
2. Setup .NET 10 SDK (preview)
3. Restore dependencies
4. Build solution
5. Run tests with coverage
6. Upload artifacts

---

## Local Development vs CI/CD

### NuGet Configuration

**Local Development** (`NuGet.Config` in root):
```xml
<packageSources>
  <add key="nexus" value="http://localhost:8081/repository/nuget-group/index.json" allowInsecureConnections="true" />
  <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
</packageSources>
```

**CI/CD** (`.github/NuGet.Config`):
```xml
<packageSources>
  <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
</packageSources>
```

The workflows automatically copy `.github/NuGet.Config` to the root during the build process.

---

## .NET 10 SDK

All workflows use:
```yaml
- name: Setup .NET 10 SDK
  uses: actions/setup-dotnet@v4
  with:
    dotnet-version: '10.0.x'
    dotnet-quality: 'preview'
```

**Note**: .NET 10 is in preview. Update `dotnet-quality` to `ga` when it reaches General Availability.

---

## Running Workflows Manually

### Via GitHub UI
1. Go to **Actions** tab
2. Select workflow (e.g., "Windows Build & Test")
3. Click **Run workflow**
4. Select branch
5. Click **Run workflow** button

### Via GitHub CLI
```bash
# Windows Build & Test
gh workflow run windows-build-test.yml --ref issue-915/windows-build-test

# Cross-Platform Build & Test
gh workflow run cross-platform-build-test.yml --ref main
```

---

## Viewing Results

### Build Status Badges

Add to your README.md:

```markdown
![Windows Build](https://github.com/MartinHock/CleanArchitecture/actions/workflows/windows-build-test.yml/badge.svg)
![Cross-Platform Build](https://github.com/MartinHock/CleanArchitecture/actions/workflows/cross-platform-build-test.yml/badge.svg)
```

### Artifacts

Test results and code coverage are available as downloadable artifacts for 30 days:
- **Artifacts tab** in each workflow run
- Named: `test-results-{os}-{configuration}` and `coverage-{os}-{configuration}`

---

## Troubleshooting

### Restore Fails with HTTP Error

The workflows use only `nuget.org`. If restore fails:
1. Check if .NET 10 SDK packages are available
2. Verify no local `NuGet.Config` overrides CI config
3. Check GitHub Actions logs for detailed errors

### Test Failures

1. Check **Test Results** artifact (TRX file)
2. Review **Code Coverage** artifact
3. Run locally: `dotnet test --configuration Debug --logger "trx"`

### Build Fails on Specific OS

The cross-platform workflow uses `fail-fast: false`, so builds continue even if one OS fails.
Check individual job logs to identify OS-specific issues.

---

## Best Practices

✅ **Do**:
- Run workflows on feature branches before merging
- Review test results and coverage artifacts
- Fix failing tests promptly
- Keep workflows updated with latest GitHub Actions versions

❌ **Don't**:
- Commit local `NuGet.Config` with Nexus configuration
- Skip tests to make builds "pass"
- Ignore warnings in build output
- Use outdated SDK versions

---

## Related Documentation

- [GitHub Actions Docs](https://docs.github.com/en/actions)
- [.NET 10 Preview](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Clean Architecture Template](../../README.md)
- [Copilot Instructions](../copilot-instructions.md)
