---
title: "Introduction"
type: docs
bookToC: true
---

# Ardalis Clean Architecture

[![Ardalis.CleanArchitecture.Template on NuGet](https://img.shields.io/nuget/v/Ardalis.CleanArchitecture.Template?label=Ardalis.CleanArchitecture.Template)](https://www.nuget.org/packages/Ardalis.CleanArchitecture.Template/)
[![Ardalis.MinimalClean.Template on NuGet](https://img.shields.io/nuget/v/Ardalis.MinimalClean.Template?label=Ardalis.MinimalClean.Template)](https://www.nuget.org/packages/Ardalis.MinimalClean.Template/)
[![GitHub stars](https://img.shields.io/github/stars/ardalis/CleanArchitecture?style=social)](https://github.com/ardalis/CleanArchitecture)

A starting point for Clean Architecture with ASP.NET Core. [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) is just the latest in a series of names for the same loosely-coupled, dependency-inverted architecture. You will also find it named [hexagonal](https://alistair.cockburn.us/hexagonal-architecture), [ports-and-adapters](http://www.dossier-andreas.net/software_architecture/ports_and_adapters.html), or [onion architecture](https://jeffreypalermo.com/2008/07/the-onion-architecture-part-1/).

This repository ships **two `dotnet new` templates** that give you a working, tested, well-factored ASP.NET Core API to build on, with a little real code in place (instead of an empty `Class1.cs`) so you can see where things belong.

## Two templates

| | **Clean Architecture** | **Minimal Clean Architecture** |
|---|---|---|
| Short name | `clean-arch` | `min-clean` |
| NuGet package | [`Ardalis.CleanArchitecture.Template`](https://www.nuget.org/packages/Ardalis.CleanArchitecture.Template/) | [`Ardalis.MinimalClean.Template`](https://www.nuget.org/packages/Ardalis.MinimalClean.Template/) |
| Shape | Multi-project solution: Core, UseCases, Infrastructure, Web | One Web project organized into vertical slices |
| Organized by | Layer (horizontal) | Feature (vertical) |
| Boundaries enforced by | Project references | Namespace rules ([NsDepCop](dependency-rules.md)) |
| Tests included | Unit, integration, functional | None (add your own) |
| Best for | Larger, long-lived apps and teams that want strict boundaries | MVPs, smaller apps, and teams that want guidance without ceremony |

Both templates include FastEndpoints (REPR), EF Core, the Mediator source generator, Ardalis.Specification, Serilog, and an optional .NET Aspire host. See [Choosing a Template](templates/_index.md) for a detailed comparison.

## Quick start

```bash
# Install (once)
dotnet new install Ardalis.CleanArchitecture.Template
dotnet new install Ardalis.MinimalClean.Template

# Create a solution
dotnet new clean-arch -o Your.ProjectName
# ...or the minimal version
dotnet new min-clean -o Your.ProjectName
```

Then follow [Getting Started](getting-started.md) to build and run it.

> [!IMPORTANT]
> **Template versions are not .NET versions.** For example, template **v11.x targets .NET 10** and template **v12.x targets .NET 11**. See [Versions](versions.md) for the full mapping before you upgrade or follow a [migration guide](migration-guides/_index.md).

## Where to go next

- [Getting Started](getting-started.md): install the templates, create a solution, and run it.
- [Choosing a Template](templates/_index.md): full vs. minimal, and what each one contains.
- [Running the Application](running.md): Aspire, databases, API docs, and email.
- [Data & Migrations](data-and-migrations.md): EF Core providers and migration commands.
- [Building & Testing](testing.md): build settings, test projects, Docker, and coverage.
- [Goals & Design Decisions](design-decisions.md): why the solution is shaped the way it is.
- [Enforcing Dependency Rules](dependency-rules.md): how NsDepCop keeps layers and feature slices apart.
- [Customizing](customizing.md): Controllers, Razor Pages, other databases, removing Aspire.
- [Migration Guides](migration-guides/_index.md): moving an existing app to a newer template version.
- [Contributing](contributing.md): how to help, and how to work on the templates themselves.
- [Resources](resources.md): courses, the book, videos, and podcasts.

## Learn more

Learn about Clean Architecture and this template in the NimblePros [Learn Clean Architecture](https://academy.nimblepros.com/p/learn-clean-architecture) course, taught by [Sarah "sadukie" Dutkiewicz](https://blog.nimblepros.com/authors/sadukie/) and [Steve "ardalis" Smith](https://blog.nimblepros.com/authors/ardalis/). Use code **ARDALIS** to save 20%.

This architecture is used in the [DDD Fundamentals course](https://www.pluralsight.com/courses/fundamentals-domain-driven-design) by [Steve Smith](https://ardalis.com) and [Julie Lerman](https://thedatafarm.com/).

Contact Steve's company, [NimblePros](https://nimblepros.com/), for Clean Architecture or DDD training and/or implementation assistance for your team.

## Give a star

If you like or are using this project to learn or start your solution, please [give it a star on GitHub](https://github.com/ardalis/CleanArchitecture). Thanks!

Or if you're feeling really generous, the project accepts [GitHub sponsorships](https://github.com/sponsors/ardalis).

## Sponsors

[Amazon AWS's FOSS fund](https://github.com/aws/dotnet-foss) has awarded a 12-month sponsorship to this project. Thank you, and thanks to all past and current sponsors!
