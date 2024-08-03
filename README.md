[![.NET Core](https://github.com/ardalis/CleanArchitecture/workflows/.NET%20Core/badge.svg)](https://github.com/ardalis/CleanArchitecture/actions)
[![publish Ardalis.CleanArchitecture Template to nuget](https://github.com/ardalis/CleanArchitecture/actions/workflows/publish.yml/badge.svg)](https://github.com/ardalis/CleanArchitecture/actions/workflows/publish.yml)
[![Ardalis.CleanArchitecture.Template on NuGet](https://img.shields.io/nuget/v/Ardalis.CleanArchitecture.Template?label=Ardalis.CleanArchitecture.Template)](https://www.nuget.org/packages/Ardalis.CleanArchitecture.Template/)

<a href="https://twitter.com/intent/follow?screen_name=ardalis">
    <img src="https://img.shields.io/twitter/follow/ardalis.svg?label=Follow%20@ardalis" alt="Follow @ardalis" />
</a> &nbsp; <a href="https://twitter.com/intent/follow?screen_name=nimblepros">
    <img src="https://img.shields.io/twitter/follow/nimblepros.svg?label=Follow%20@nimblepros" alt="Follow @nimblepros" />
</a>

# Clean Architecture

A starting point for Clean Architecture with ASP.NET Core. [Clean Architecture](https://8thlight.com/blog/uncle-bob/2012/08/13/the-clean-architecture.html) is just the latest in a series of names for the same loosely-coupled, dependency-inverted architecture. You will also find it named [hexagonal](http://alistair.cockburn.us/Hexagonal+architecture), [ports-and-adapters](http://www.dossier-andreas.net/software_architecture/ports_and_adapters.html), or [onion architecture](http://jeffreypalermo.com/blog/the-onion-architecture-part-1/).

This architecture is used in the [DDD Fundamentals course](https://www.pluralsight.com/courses/fundamentals-domain-driven-design) by [Steve Smith](https://ardalis.com) and [Julie Lerman](https://thedatafarm.com/).

:school: Contact Steve's company, [NimblePros](https://nimblepros.com/), for Clean Architecture or DDD training and/or implementation assistance for your team.

## Troubleshooting Chrome Errors

By default the site uses HTTPS and expects you to have a self-signed developer certificate for localhost use. If you get an error with Chrome [see this answer](https://stackoverflow.com/a/31900210/13729) for mitigation instructions.

## Table Of Contents

- [Clean Architecture](#clean-architecture)
  - [Troubleshooting Chrome Errors](#troubleshooting-chrome-errors)
  - [Table Of Contents](#table-of-contents)
  - [Give a Star! :star:](#give-a-star-star)
  - [Versions](#versions)
  - [Learn More](#learn-more)
- [Getting Started](#getting-started)
  - [Using the dotnet CLI template](#using-the-dotnet-cli-template)
  - [What about Controllers and Razor Pages?](#what-about-controllers-and-razor-pages)
    - [Add Ardalis.ApiEndpoints](#add-ardalisapiendpoints)
    - [Add Controllers](#add-controllers)
    - [Add Razor Pages](#add-razor-pages)
  - [Using the GitHub Repository](#using-the-github-repository)
  - [Running Migrations](#running-migrations)
- [Goals](#goals)
  - [History and Shameless Plug Section](#history-and-shameless-plug-section)
- [Design Decisions and Dependencies](#design-decisions-and-dependencies)
  - [Where To Validate](#where-to-validate)
  - [The Core Project](#the-core-project)
  - [The Use Cases Project](#the-use-cases-project)
  - [The Infrastructure Project](#the-infrastructure-project)
  - [The Web Project](#the-web-project)
  - [The SharedKernel Project](#the-sharedkernel-project)
  - [The Test Projects](#the-test-projects)
- [Patterns Used](#patterns-used)
  - [Domain Events](#domain-events)
  - [Related Projects](#related-projects)

## Give a Star! :star:
If you like or are using this project to learn or start your solution, please give it a star. Thanks!

Or if you're feeling really generous, we now support GitHub sponsorships - see the button above.

## Versions

The main branch is now using .NET 8. If you need a previous version use one of these tagged commits:

- [7.0](https://github.com/ardalis/CleanArchitecture/releases/tag/v8)
- [6.0](https://github.com/ardalis/CleanArchitecture/releases/tag/dotnet-6)
- [5.0](https://github.com/ardalis/CleanArchitecture/releases/tag/dotnet-core-5)
- [3.1](https://github.com/ardalis/CleanArchitecture/tree/dotnet-core-3.1)
- [2.2](https://github.com/ardalis/CleanArchitecture/tree/dotnet-core-2.2)
- [2.0](https://github.com/ardalis/CleanArchitecture/tree/dotnet-core-2.0)

## Learn More

- [Live Stream Recordings Working on Clean Architecture](https://www.youtube.com/c/Ardalis/search?query=clean%20architecture)
- [DotNetRocks Podcast Discussion with Steve "ardalis" Smith](https://player.fm/series/net-rocks/clean-architecture-with-steve-smith)
- [Fritz and Friends Streaming Discussion with Steve "ardalis" Smith](https://www.youtube.com/watch?v=k8cZUW4MS3I)

# Getting Started

To use this template, there are a few options:

- Install using `dotnet new` (recommended)
- Download this Repository (and modify as needed)

## Using the dotnet CLI template

First, install the template from [NuGet (https://www.nuget.org/packages/Ardalis.CleanArchitecture.Template/)](https://www.nuget.org/packages/Ardalis.CleanArchitecture.Template/):

```powershell
dotnet new install Ardalis.CleanArchitecture.Template
```

You should see the template in the list of templates from `dotnet new list` after this installs successfully. Look for "ASP.NET Clean Architecture Solution" with Short Name of "clean-arch".

Navigate to the parent directory in which you'd like the solution's folder to be created.

Run this command to create the solution structure in a subfolder name `Your.ProjectName`:

```
dotnet new clean-arch -o Your.ProjectName
```

The `Your.ProjectName` directory and solution file will be created, and inside that will be all of your new solution contents, properly namespaced and ready to run/test!

Example:
![powershell screenshot showing steps](https://user-images.githubusercontent.com/782127/101661723-9fd28e80-3a16-11eb-8be4-f9195d825ad6.png)

Thanks [@dahlsailrunner](https://github.com/dahlsailrunner) for your help getting this working!

**Known Issues**: 

- Don't include hyphens in the name. See [#201](https://github.com/ardalis/CleanArchitecture/issues/201).
- Don't use 'Ardalis' as your namespace (conflicts with dependencies).

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

# Goals

The goal of this repository is to provide a basic solution structure that can be used to build Domain-Driven Design (DDD)-based or simply well-factored, SOLID applications using .NET Core. Learn more about these topics here:

- [SOLID Principles for C# Developers](https://www.pluralsight.com/courses/csharp-solid-principles)
- [SOLID Principles of Object Oriented Design](https://www.pluralsight.com/courses/principles-oo-design) (the original, longer course)
- [Domain-Driven Design Fundamentals](https://www.pluralsight.com/courses/domain-driven-design-fundamentals)

If you're used to building applications as single-project or as a set of projects that follow the traditional UI -> Business Layer -> Data Access Layer "N-Tier" architecture, I recommend you check out these two courses (ideally before DDD Fundamentals):

- [Creating N-Tier Applications in C#, Part 1](https://www.pluralsight.com/courses/n-tier-apps-part1)
- [Creating N-Tier Applications in C#, Part 2](https://www.pluralsight.com/courses/n-tier-csharp-part2)

Steve Smith also maintains Microsoft's reference application, eShopOnWeb, and its associated free eBook. Check them out here:

- [eShopOnWeb on GitHub](https://github.com/dotnet-architecture/eShopOnWeb)
- [Architecting Modern Web Applications with ASP.NET Core and Microsoft Azure](https://aka.ms/webappebook) (eBook)

Note that the goal of this project and repository is **not** to provide a sample or reference application. It's meant to just be a template, but with enough pieces in place to show you where things belong as you set up your actual solution. Instead of useless "Class1.cs" there are a few real classes in place. Delete them as soon as you understand why they're there and where you should put your own, similar files.

## History and Shameless Plug Section

I've used this starter kit to teach the basics of ASP.NET Core using Domain-Driven Design concepts and patterns for some time now (starting when ASP.NET Core was still in pre-release). Typically I teach a one- or two-day hands-on workshop ahead of events like DevIntersection, or private on-site workshops for companies looking to bring their teams up to speed with the latest development technologies and techniques. Feel free to [contact me](https://ardalis.com/contact-us) if you'd like information about upcoming workshops.

# Design Decisions and Dependencies

The goal of this sample is to provide a fairly bare-bones starter kit for new projects. It does not include every possible framework, tool, or feature that a particular enterprise application might benefit from. Its choices of technology for things like data access are rooted in what is the most common, accessible technology for most business software developers using Microsoft's technology stack. It doesn't (currently) include extensive support for things like logging, monitoring, or analytics, though these can all be added easily. Below is a list of the technology dependencies it includes, and why they were chosen. Most of these can easily be swapped out for your technology of choice, since the nature of this architecture is to support modularity and encapsulation.

## Where To Validate

Validation of user input is a requirement of all software applications. The question is, where does it make sense to implement it in a concise and elegant manner? This solution template includes 4 separate projects, each of which might be responsible for performing validation as well as enforcing business invariants (which, given validation should already have occurred, are usually modeled as exceptions).

The domain model itself should generally rely on object-oriented design to ensure it is always in a consistent state. It leverages encapsulation and limits public state mutation access to achieve this, and it assumes that any arguments passed to it have already been validated, so null or other improper values yield exceptions, not validation results, in most cases.

The use cases / application project includes the set of all commands and queries the system supports. It's frequently responsible for validating its own command and query objects.

The Web project includes all API endpoints, which include their own request and response types, following the [REPR pattern](https://deviq.com/design-patterns/repr-design-pattern). The FastEndpoints library includes built-in support for validation using FluentValidation on the request types. This is a natural place to perform input validation as well.

Having validation occur both within the API endpoints and then again at the use case level is redundant, so in this template the choice has been made to validate at the edge of the application, in the API endpoints. This means some future consumer of the Use Cases project will also need to be responsible for its own validation as well, but in the vast majority of cases there won't be any other consumers of the use cases outside of the API endpoints.

## The Core Project

The Core project is the center of the Clean Architecture design, and all other project dependencies should point toward it. As such, it has very few external dependencies. The Core project should include the Domain Model including things like:

- Entities
- Aggregates
- Value Objects
- Domain Events
- Domain Event Handlers
- Domain Services
- Specifications
- Interfaces
- DTOs (sometimes)

## The Use Cases Project

An optional project, I've included it because many folks were demanding it and it's easier to remove than to add later. This is also often referred to as the *Application* or *Application Services* layer. The Use Cases project is organized following CQRS into Commands and Queries. Commands mutate the domain model and thus should always use Repository abstractions for their data access. Queries are readonly, and thus do not need to use the repository pattern, but instead can use whatever query service or approach is most convenient. However, since the Use Cases project is set up to depend on Core and not directly on Infrastructure, there will still need to be abstractions defined for its data access. And it *can* use things like specifications, which can sometimes help encapsulate query logic as well as result type mapping. But it doesn't *have* to use repository/specification - it can just issue a SQL query or call a stored procedure if that's the most efficient way to get the data.

Although this is an optional project to include (without it, your API endpoints would just work directly with the domain model or query services), it does provide a nice UI-ignorant place to add automated tests.

## The Infrastructure Project

Most of your application's dependencies on external resources should be implemented in classes defined in the Infrastructure project. These classes should implement interfaces defined in Core. If you have a very large project with many dependencies, it may make sense to have multiple Infrastructure projects (e.g. Infrastructure.Data), but for most projects one Infrastructure project with folders works fine. The sample includes data access and domain event implementations, but you would also add things like email providers, file access, web api clients, etc. to this project so they're not adding coupling to your Core or UI projects.

The Infrastructure project depends on `Microsoft.EntityFrameworkCore.SqlServer` and `Autofac`. The former is used because it's built into the default ASP.NET Core templates and is the least common denominator of data access. If desired, it can easily be replaced with a lighter-weight ORM like Dapper. Autofac (formerly StructureMap) is used to allow wireup of dependencies to take place closest to where the implementations reside. In this case, an InfrastructureRegistry class can be used in the Infrastructure class to allow wireup of dependencies there, without the entry point of the application even having to have a reference to the project or its types. [Learn more about this technique](https://ardalis.com/avoid-referencing-infrastructure-in-visual-studio-solutions). The current implementation doesn't include this behavior - it's something I typically cover and have students add themselves in my workshops.

## The Web Project

The entry point of the application is the ASP.NET Core web project. This is actually a console application, with a `public static void Main` method in `Program.cs`. It currently uses the default MVC organization (Controllers and Views folders) as well as most of the default ASP.NET Core project template code. This includes its configuration system, which uses the default `appsettings.json` file plus environment variables, and is configured in `Startup.cs`. The project delegates to the `Infrastructure` project to wire up its services using Autofac.

## The SharedKernel Project

Many solutions will also reference a separate **Shared Kernel** project/package. I recommend creating a separate SharedKernel project and solution if you will require sharing code between multiple [bounded contexts](https://ardalis.com/encapsulation-boundaries-large-and-small/) (see [DDD Fundamentals](https://www.pluralsight.com/courses/domain-driven-design-fundamentals)). I further recommend this be published as a NuGet package (most likely privately within your organization) and referenced as a NuGet dependency by those projects that require it.

Previously a project for SharedKernel was included in this project. However, for the above reasons I've made it a separate package, [Ardalis.SharedKernel](https://github.com/ardalis/Ardalis.SharedKernel), which you should replace with your own when you use this template.

If you want to see another [example of a SharedKernel package, the one I use in my updated Pluralsight DDD course is on NuGet here](https://www.nuget.org/packages/PluralsightDdd.SharedKernel/).


## The Test Projects

Test projects could be organized based on the kind of test (unit, functional, integration, performance, etc.) or by the project they are testing (Core, Infrastructure, Web), or both. For this simple starter kit, the test projects are organized based on the kind of test, with unit, functional and integration test projects existing in this solution. In terms of dependencies, there are three worth noting:

- [xunit](https://www.nuget.org/packages/xunit) I'm using xunit because that's what ASP.NET Core uses internally to test the product. It works great and as new versions of ASP.NET Core ship, I'm confident it will continue to work well with it.

- [NSubstitute](https://www.nuget.org/packages/NSubstitute) I'm using NSubstitute as a mocking framework for white box behavior-based tests. If I have a method that, under certain circumstances, should perform an action that isn't evident from the object's observable state, mocks provide a way to test that. I could also use my own Fake implementation, but that requires a lot more typing and files. NSubstitute is great once you get the hang of it, and assuming you don't have to mock the world (which we don't in this case because of good, modular design).

- [Microsoft.AspNetCore.TestHost](https://www.nuget.org/packages/Microsoft.AspNetCore.TestHost) I'm using TestHost to test my web project using its full stack, not just unit testing action methods. Using TestHost, you make actual HttpClient requests without going over the wire (so no firewall or port configuration issues). Tests run in memory and are very fast, and requests exercise the full MVC stack, including routing, model binding, model validation, filters, etc.

# Patterns Used

This solution template has code built in to support a few common patterns, especially Domain-Driven Design patterns. Here is a brief overview of how a few of them work.

## Domain Events

Domain events are a great pattern for decoupling a trigger for an operation from its implementation. This is especially useful from within domain entities since the handlers of the events can have dependencies while the entities themselves typically do not. In the sample, you can see this in action with the `ToDoItem.MarkComplete()` method. The following sequence diagram demonstrates how the event and its handler are used when an item is marked complete through a web API endpoint.

![Domain Event Sequence Diagram](https://user-images.githubusercontent.com/782127/75702680-216ce300-5c73-11ea-9187-ec656192ad3b.png)

## Related Projects

- [ApiEndpoints](https://github.com/ardalis/apiendpoints)
- [GuardClauses](https://github.com/ardalis/guardclauses)
- [HttpClientTestExtensions](https://github.com/ardalis/HttpClientTestExtensions)
- [Result](https://github.com/ardalis/result)
- [SharedKernel](https://github.com/ardalis/Ardalis.SharedKernel)
- [SmartEnum](https://github.com/ardalis/SmartEnum)
- [Specification](https://github.com/ardalis/specification)
