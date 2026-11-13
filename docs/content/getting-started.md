---
title: "Getting Started"
weight: 10
---

# Getting Started

This page takes you from nothing to a running API. The recommended way to use either template is the `dotnet new` CLI.

## Prerequisites

- **.NET SDK** matching the template version. Template v12.x targets **.NET 11** (`global.json` pins SDK `11.0.100` and rolls forward to newer SDKs). Older template versions target older SDKs; see [Versions](versions.md).
- **Docker** (or another container runtime supported by .NET Aspire) to run through the Aspire host. The full template's functional tests also use it to test against SQL Server, and fall back to SQLite without it. You can build, run the Web project directly, and run all the tests without Docker.
- An editor: Visual Studio, JetBrains Rider, or VS Code with the C# Dev Kit.

## 1. Install the templates

Install one or both templates from NuGet:

```bash
# Full Clean Architecture template (short name: clean-arch)
dotnet new install Ardalis.CleanArchitecture.Template

# Minimal Clean Architecture template (short name: min-clean)
dotnet new install Ardalis.MinimalClean.Template
```

Confirm they're installed:

```bash
dotnet new list clean
```

You should see **ASP.NET Clean Architecture Solution** (`clean-arch`) and **ASP.NET Minimal Clean Architecture Solution** (`min-clean`).

Not sure which one you want? See [Choosing a Template](templates/_index.md).

### Install a specific version

Append `::<version>` to the package ID:

```bash
dotnet new install Ardalis.CleanArchitecture.Template::11.1.1
```

Template package versions don't match .NET versions. Check [Versions](versions.md) to pick the one that targets the .NET release you want.

### Update or uninstall

```bash
# Check for and apply updates to all installed template packages
dotnet new update --check-only
dotnet new update

# List installed template packages (and their versions), or remove one
dotnet new uninstall
dotnet new uninstall Ardalis.CleanArchitecture.Template
```

## 2. Create a solution

Navigate to the parent folder where you want the solution folder created, then run:

```bash
dotnet new clean-arch -o Your.ProjectName
```

or, for the minimal template:

```bash
dotnet new min-clean -o Your.ProjectName
```

This creates a `Your.ProjectName` folder containing `Your.ProjectName.slnx` and all projects, properly namespaced and ready to build and test.

Neither template has template-specific options. `dotnet new clean-arch -?` shows only the standard `dotnet new` options:

```text
ASP.NET Clean Architecture Solution (C#)
Author: ardalis (Steve Smith)

Usage:
  dotnet new clean-arch [options] [template options]

Options:
  -n, --name <name>       The name for the output being created. If no name is specified, the name of the output directory is used.
  -o, --output <output>   Location to place the generated output.
  --dry-run               Displays a summary of what would happen if the given command line were run if it would result in a template creation. [default: False]
  --force                 Forces content to be generated even if it would change existing files. [default: False]
  --no-update-check       Disables checking for the template package updates when instantiating a template. [default: False]
  --project <project>     The project that should be used for context evaluation.
  -lang, --language <C#>  Specifies the template language to instantiate.
  --type <project>        Specifies the template type to instantiate.

Template options:
   (No options)
```

The .NET Aspire host projects are always included. You can delete them if you don't want them; see [Customizing](customizing.md#removing-net-aspire).

### Naming your solution

- Dotted names such as `Acme.Ordering` work well and become your root namespace and project prefixes (`Acme.Ordering.Core`, `Acme.Ordering.Web`, ...).
- Characters that aren't valid in a C# namespace are replaced. For example, `My-App` produces the namespace `My_App` and projects such as `My_App.Core`.
- **Don't use `Ardalis` as your root namespace.** It conflicts with the `Ardalis.*` NuGet packages the template depends on.

### From Visual Studio or Rider

After the templates are installed with `dotnet new install`, Visual Studio and JetBrains Rider list them in their **New Project** dialogs. Search for "Clean Architecture".

## 3. Build and run

```bash
cd Your.ProjectName
dotnet build
```

The quickest way to see everything working is to run the .NET Aspire host (both templates include one), which starts SQL Server and a test email server in containers. Docker must be running:

```bash
dotnet run --project src/Your.ProjectName.AspireHost
```

Open the Aspire dashboard URL printed in the console, then open the **web** resource. API documentation is served at `/scalar` (and `/swagger`) in Development.

You can also run the Web project by itself, without Aspire or Docker:

```bash
dotnet run --project src/Your.ProjectName.Web
```

The full template then uses SQL Server LocalDB on Windows or SQLite elsewhere. The minimal template needs a SQL Server instance. [Running the Application](running.md) covers all of this in detail, including URLs, ports, and the HTTPS development certificate.

Run the tests (full template):

```bash
dotnet test
```

See [Building & Testing](testing.md) for what each test project covers.

## 4. Make it yours

The template isn't a sample application. It contains just enough real code (a `Contributor` aggregate in the full template; carts, orders, and products in the minimal one) to show where things go. Once you understand why each piece is where it is, **delete the example code and add your own**. [Customizing](customizing.md) has a checklist.

That includes the [seed data](data-and-migrations.md#seed-data) that's inserted at startup. It's for local development and demos only, so replace it with your own or remove it before you point the app at a real database.

Looking for a larger, more complete example? The repository's [`sample`](https://github.com/ardalis/CleanArchitecture/tree/main/sample) folder contains **NimblePros.SampleToDo**, a fuller application built with the same architecture. It isn't part of either template.

## Alternative: start from the GitHub repository

If you'd rather not use `dotnet new`, you can get a copy of the repository and start from it directly. You have three options: download, fork, or clone. Most of the time, you probably just want to download.

- **Download** the repository (unblock the zip file on Windows) and extract it to a new folder if you just want to play with the project or use it as the starting point for an application. You'll need to rename the `Clean.Architecture.*` projects and namespaces yourself, which `dotnet new` does for you.
- **Fork** the repository only if you plan to submit a pull request, or if you'd like to keep a snapshot of it in your own GitHub account.
- **Clone** the repository if you're a contributor. See [Contributing](contributing.md).
