# Clean Architecture

[![Build & Test (Clean Architecture)](https://github.com/ardalis/CleanArchitecture/actions/workflows/cross-platform-build-test.yml/badge.svg)](https://github.com/ardalis/CleanArchitecture/actions/workflows/cross-platform-build-test.yml)
[![Build & Test (MinimalClean)](https://github.com/ardalis/CleanArchitecture/actions/workflows/cross-platform-build-test-minimal.yml/badge.svg)](https://github.com/ardalis/CleanArchitecture/actions/workflows/cross-platform-build-test-minimal.yml)
[![CodeQL](https://github.com/ardalis/CleanArchitecture/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/ardalis/CleanArchitecture/actions/workflows/codeql-analysis.yml)
[![Docs](https://github.com/ardalis/CleanArchitecture/actions/workflows/docs-build.yml/badge.svg)](https://cleanarchitecture.ardalis.com/)

[![Ardalis.CleanArchitecture.Template on NuGet](https://img.shields.io/nuget/v/Ardalis.CleanArchitecture.Template?label=Ardalis.CleanArchitecture.Template)](https://www.nuget.org/packages/Ardalis.CleanArchitecture.Template/)
[![Ardalis.CleanArchitecture.Template downloads](https://img.shields.io/nuget/dt/Ardalis.CleanArchitecture.Template?label=downloads)](https://www.nuget.org/packages/Ardalis.CleanArchitecture.Template/)
[![Ardalis.MinimalClean.Template on NuGet](https://img.shields.io/nuget/v/Ardalis.MinimalClean.Template?label=Ardalis.MinimalClean.Template)](https://www.nuget.org/packages/Ardalis.MinimalClean.Template/)
[![Ardalis.MinimalClean.Template downloads](https://img.shields.io/nuget/dt/Ardalis.MinimalClean.Template?label=downloads)](https://www.nuget.org/packages/Ardalis.MinimalClean.Template/)

[![.NET 11](https://img.shields.io/badge/.NET-11-512BD4)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/github/license/ardalis/CleanArchitecture)](https://github.com/ardalis/CleanArchitecture/blob/main/LICENSE)
[![GitHub stars](https://img.shields.io/github/stars/ardalis/CleanArchitecture?style=social)](https://github.com/ardalis/CleanArchitecture/stargazers)
[![Follow @ardalis](https://img.shields.io/twitter/follow/ardalis?label=Follow%20%40ardalis)](https://twitter.com/intent/follow?screen_name=ardalis)
[![Follow @nimblepros](https://img.shields.io/twitter/follow/nimblepros?label=Follow%20%40nimblepros)](https://twitter.com/intent/follow?screen_name=nimblepros)

`dotnet new` templates for building ASP.NET Core applications with [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html), also known as hexagonal, ports-and-adapters, or onion architecture. The domain model sits at the center, and dependencies point inward toward it.

There are two templates:

- **Full Clean Architecture (`clean-arch`)**: a multi-project solution (Core, UseCases, Infrastructure, Web) with DDD patterns, CQRS via Mediator, and FastEndpoints. Best for larger, long-lived applications.
- **Minimal Clean Architecture (`min-clean`)**: a single project organized by vertical slices, with layer boundaries enforced by [NsDepCop](https://github.com/realvizu/NsDepCop). Best for MVPs and smaller applications.

A complete sample application lives in [`/sample`](https://github.com/ardalis/CleanArchitecture/tree/main/sample).

## Getting Started

Install the templates from NuGet:

```powershell
dotnet new install Ardalis.CleanArchitecture.Template
dotnet new install Ardalis.MinimalClean.Template
```

Create a solution from either template:

```powershell
# Full Clean Architecture
dotnet new clean-arch -o Your.ProjectName

# Minimal Clean Architecture
dotnet new min-clean -o Your.ProjectName
```

## Documentation

**Everything else is in the docs: [cleanarchitecture.ardalis.com](https://cleanarchitecture.ardalis.com/).** That includes getting started, choosing between the templates, design decisions, dependency rules, and [migration guides](https://cleanarchitecture.ardalis.com/migration-guides/) for upgrading from earlier versions.

## Learn More

- [Introducing Clean Architecture](https://academy.nimblepros.com/p/learn-clean-architecture) course from NimblePros (use code ARDALIS to save 20%)
- [NimblePros](https://nimblepros.com/) offers Clean Architecture and DDD training and implementation help for teams

If this project helps you, please give it a star :star:. You can also support it through [GitHub Sponsors](https://github.com/sponsors/ardalis).
