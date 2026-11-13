---
title: "Versions"
weight: 70
---

# Versions

> [!IMPORTANT]
> **Version numbers on this site are template package versions, not .NET versions.** Since template v8, the template's major version has been **one higher** than the .NET version it targets. Template **v11** targets **.NET 10**, and template **v12** targets **.NET 11**.

## Clean Architecture template (`Ardalis.CleanArchitecture.Template`)

| Template version | Targets | First released | Notes |
|---|---|---|---|
| **12.x** | **.NET 11** | November 2026 | Both templates now share one version number. See the [v11 to v12 migration guide](migration-guides/v11-to-v12.md). |
| **11.x** | **.NET 10** | November 2025 | Mediator source generator replaces MediatR; Aspire 13. See the [v10 to v11 migration guide](migration-guides/v10-to-v11.md). Latest: 11.1.1. |
| 10.x | .NET 9 | November 2024 | Latest: 10.1.1 |
| 9.x | .NET 8 | November 2023 | |
| 8.x | .NET 7 | August 2023 | API endpoints use FastEndpoints only; Controllers, Razor Pages, and ApiEndpoints were removed. |
| 7.x | .NET 7 | November 2022 | 7.1 is the last version with Controllers, Razor Pages, and ApiEndpoints. |
| 6.x | .NET 6 | October 2021 | |
| 5.x | .NET 5 | April 2021 | |

Exact release dates and notes are on the [GitHub Releases](https://github.com/ardalis/CleanArchitecture/releases) page and the [NuGet version history](https://www.nuget.org/packages/Ardalis.CleanArchitecture.Template#versions-body-tab). Each release is tagged in Git (for example `v11.1.1`), so you can browse the code for any version on GitHub.

## Minimal Clean template (`Ardalis.MinimalClean.Template`)

The minimal template was introduced alongside full template v11, with its own 1.x version numbers. **Starting with 12.0.0, both templates share one version number and ship together**, so MinimalClean jumped from 1.1.1 to 12.0.0. The jump only aligns the numbers.

| Template version | Targets | First released |
|---|---|---|
| **12.x** | **.NET 11** | November 2026 |
| 1.x | .NET 10 | November 2025 (latest: 1.1.1) |

See the [NuGet version history](https://www.nuget.org/packages/Ardalis.MinimalClean.Template#versions-body-tab) for every release.

## Installing a specific version

```bash
dotnet new install Ardalis.CleanArchitecture.Template::11.1.1
dotnet new install Ardalis.MinimalClean.Template::1.1.1
```

Installing a different version of an already-installed package replaces it. To see which versions you have installed:

```bash
dotnet new uninstall
```

With no arguments, this lists installed template packages and their versions without uninstalling anything.

## Which version did my solution come from?

There's no reliable marker of the template version in a generated solution. The target framework in `Directory.Build.props` (and the SDK in `global.json`) tells you the .NET version, which narrows it down using the table above. **Tip:** note the template name and version in your first commit message or README, so you know which [migration guide](migration-guides/_index.md) to start from later.
