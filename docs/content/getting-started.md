---
title: "Getting Started"
weight: 10
---

# Getting Started

To use this template, there are **two template options**:

1. **Full Clean Architecture** (`clean-arch`) - Complete multi-project solution with Core, UseCases, Infrastructure, and Web
2. **Minimal Clean Architecture** (`min-clean`) - Simplified single-project vertical slice architecture

Choose based on your project's complexity and team preferences. See [Template Comparison](#template-comparison) below.

## Template Installation

Install the templates from NuGet:

```powershell
# Full Clean Architecture template
dotnet new install Ardalis.CleanArchitecture.Template

# Minimal Clean Architecture template  
dotnet new install Ardalis.MinimalClean.Template
```

## Using the dotnet CLI template

### Full Clean Architecture (`clean-arch`)

First, install the template from [NuGet (https://www.nuget.org/packages/Ardalis.CleanArchitecture.Template/)](https://www.nuget.org/packages/Ardalis.CleanArchitecture.Template/):

```powershell
dotnet new install Ardalis.CleanArchitecture.Template
```

You can see available options by running the command with the `-?` option:

```powershell
dotnet new clean-arch -?
ASP.NET Clean Architecture Solution (C#)
Author: Steve Smith @ardalis, Erik Dahl

Usage:
  dotnet new clean-arch [options] [template options]

Options:
  -n, --name <name>       The name for the output being created. If no name is specified, the name of the output
                          directory is used.
  -o, --output <output>   Location to place the generated output.
  --dry-run               Displays a summary of what would happen if the given command line were run if it would result
                          in a template creation.
  --force                 Forces content to be generated even if it would change existing files.
  --no-update-check       Disables checking for the template package updates when instantiating a template.
  --project <project>     The project that should be used for context evaluation.
  -lang, --language <C#>  Specifies the template language to instantiate.
  --type <project>        Specifies the template type to instantiate.

Template options:
  -as, --aspire  Include .NET Aspire.
                 Type: bool
                 Default: false
```

You should see the template in the list of templates from `dotnet new list` after this installs successfully. Look for "ASP.NET Clean Architecture Solution" with Short Name of "clean-arch".

Navigate to the parent directory in which you'd like the solution's folder to be created.

Run this command to create the solution structure in a subfolder name `Your.ProjectName`:

```powershell
dotnet new clean-arch -o Your.ProjectName
```

The `Your.ProjectName` directory and solution file will be created, and inside that will be all of your new solution contents, properly namespaced and ready to run/test!

Thanks [@dahlsailrunner](https://github.com/dahlsailrunner) for your help getting this working!

**Known Issues**:

- Don't include hyphens in the name. See [#201](https://github.com/ardalis/CleanArchitecture/issues/201).
- Don't use 'Ardalis' as your namespace (conflicts with dependencies).

### Minimal Clean Architecture (`min-clean`)

For a simpler, single-project vertical slice architecture, use the Minimal Clean Architecture template:

```powershell
dotnet new install Ardalis.MinimalClean.Template
dotnet new min-clean -o Your.ProjectName
```

This template provides:

- **Single Web project** with all code organized by vertical slices (features)
- **Simplified architecture** - no separate Core, UseCases, or Infrastructure projects
- **Domain-Driven Design patterns** - entities, aggregates, but pragmatic
- **FastEndpoints** for clean API endpoints
- **Entity Framework Core** with migrations
- **Aspire** support for cloud-ready development (optional)

Perfect for MVPs, smaller applications, or teams that want architectural guidance without multi-project complexity.

## Template Comparison

| Feature | Full Clean Architecture | Minimal Clean Architecture |
|---------|------------------------|----------------------------|
| **Command** | `clean-arch` | `min-clean` |
| **Projects** | 4+ (Core, UseCases, Infrastructure, Web) | 1 (Web only) |
| **Organization** | By layer (horizontal) | By feature (vertical slices) |
| **DDD Patterns** | Extensive (Aggregates, Value Objects, Domain Events, Specifications) | Pragmatic (simplified domain model) |
| **Repository Pattern** | Yes, with Specifications | Optional (direct DbContext or simple repos) |
| **Mediator/CQRS** | Yes (separate UseCases project) | Optional (can be in endpoints) |
| **Complexity** | Higher - more abstractions | Lower - simpler structure |
| **Best For** | Large enterprise apps, long-term maintenance | MVPs, smaller apps, rapid iteration |
| **Team Size** | Multiple teams, strict boundaries | Small teams, collaborative |
| **Learning Curve** | Steeper - more patterns to learn | Gentler - focused essentials |
| **Migration Path** | Can simplify to minimal | Can grow into full template |

### Which Template Should I Use?

**Choose Full Clean Architecture if:**

- Building large, complex enterprise applications
- Multiple teams working on different layers
- Long-term maintenance and evolution expected
- Need strict separation of concerns and testability
- Domain complexity requires extensive DDD patterns

**Choose Minimal Clean Architecture if:**

- Building MVPs or smaller applications
- Want architectural guidance without project overhead
- Prefer vertical slice architecture
- Team values simplicity and fast iteration
- May grow into full Clean Architecture later

**Not sure?** Start with Minimal Clean and migrate to Full Clean Architecture if your application grows in complexity.

## What about Controllers and Razor Pages?

As of version 9, this solution template only includes support for API Endpoints using the FastEndpoints library. If you want to use my ApiEndpoints library, Razor Pages, and/or Controllers you can use the last template that included them, [version 7.1](https://www.nuget.org/packages/Ardalis.CleanArchitecture.Template/7.1.0). Alternately, they're easily added to this template after installation.

### Add Ardalis.ApiEndpoints

To use [Ardalis.ApiEndpoints](https://www.nuget.org/packages/Ardalis.ApiEndpoints) instead of (or in addition to) [FastEndpoints](https://fast-endpoints.com/), just add the reference and use the base classes from the documentation.

```powershell
dotnet add package Ardalis.ApiEndpoints
```

### Add Controllers

You'll need to add support for controllers to the Program.cs file. You need:

```csharp
builder.Services.AddControllers(); // ControllersWithView if you need Views

// and

app.MapControllers();
```

Once these are in place, you should be able to create a Controllers folder and (optionally) a Views folder and everything should work as expected. Personally I find Razor Pages to be much better than Controllers and Views so if you haven't fully investigated Razor Pages you might want to do so right about now before you choose Views.

### Add Razor Pages

You'll need to add support for Razor Pages to the Program.cs file. You need:

```csharp
builder.Services.AddRazorPages();

// and

app.MapRazorPages();
```

Then you just add a Pages folder in the root of the project and go from there.

## Using the GitHub Repository

To get started based on this repository, you need to get a copy locally. You have three options: fork, clone, or download. Most of the time, you probably just want to download.

You should **download the repository**, unblock the zip file, and extract it to a new folder if you just want to play with the project or you wish to use it as the starting point for an application.

You should **fork this repository** only if you plan on submitting a pull request. Or if you'd like to keep a copy of a snapshot of the repository in your own GitHub account.

You should **clone this repository** if you're one of the contributors and you have commit access to it. Otherwise you probably want one of the other options.

## Running Migrations

You shouldn't need to do this to use this template, but if you want migrations set up properly in the Infrastructure project, you need to specify that project name when you run the migrations command.

In Visual Studio, open the Package Manager Console, and run `Add-Migration InitialMigrationName -StartupProject Your.ProjectName.Web -Context AppDbContext -Project Your.ProjectName.Infrastructure`.

In a terminal with the CLI, the command is similar. Run this from the Web project directory:

```powershell
dotnet ef migrations add MIGRATIONNAME -c AppDbContext -p ../Your.ProjectName.Infrastructure/Your.ProjectName.Infrastructure.csproj -s Your.ProjectName.Web.csproj -o Data/Migrations
```

To use SqlServer, change `options.UseSqlite(connectionString));` to `options.UseSqlServer(connectionString));` in the `Your.ProjectName.Infrastructure.StartupSetup` file. Also remember to replace the `SqliteConnection` with `DefaultConnection` in the `Your.ProjectName.Web.Program` file, which points to your Database Server.

To update the database use this command from the Web project folder (replace `Clean.Architecture` with your project's name):

```powershell
dotnet ef database update -c AppDbContext -p ../Clean.Architecture.Infrastructure/Clean.Architecture.Infrastructure.csproj -s Clean.Architecture.Web.csproj
```
