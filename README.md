[![.NET Core](https://github.com/ardalis/CleanArchitecture/workflows/.NET%20Core/badge.svg)](https://github.com/ardalis/CleanArchitecture/actions)

# Clean Architecture

A starting point for Clean Architecture with ASP.NET Core. [Clean Architecture](https://8thlight.com/blog/uncle-bob/2012/08/13/the-clean-architecture.html) is just the latest in a series of names for the same loosely-coupled, dependency-inverted architecture. You will also find it named [hexagonal](http://alistair.cockburn.us/Hexagonal+architecture), [ports-and-adapters](http://www.dossier-andreas.net/software_architecture/ports_and_adapters.html), or [onion architecture](http://jeffreypalermo.com/blog/the-onion-architecture-part-1/).

## Give a Star! :star:
If you like or are using this project to learn or start your solution, please give it a star. Thanks!

Or if you're feeling really generous, we now support GitHub sponsorships - see the button above.

## *Now available as a [project template](https://marketplace.visualstudio.com/items?itemName=GregTrevellick.CleanArchitecture) within Visual Studio.*

## Versions

The master branch is now using .NET 5. If you need a previous version use one of these tagged commits:

- [3.1](https://github.com/ardalis/CleanArchitecture/tree/dotnet-core-3.1)
- [2.2](https://github.com/ardalis/CleanArchitecture/tree/dotnet-core-2.2)
- [2.0](https://github.com/ardalis/CleanArchitecture/tree/dotnet-core-2.0)

## Learn More

- [DotNetRocks Podcast Discussion with Steve "ardalis" Smith](https://player.fm/series/net-rocks/clean-architecture-with-steve-smith)
- [Fritz and Friends Streaming Discussion with Steve "ardalis" Smith](https://www.youtube.com/watch?v=k8cZUW4MS3I)

# Getting Started

To use this template, there are a few options:

- [Install the Visual Studio Template](https://marketplace.visualstudio.com/items?itemName=GregTrevellick.CleanArchitecture) and use it within Visual Studio
- Download this Repository

I'll cover both options here.

## Using the Visual Studio Item Template

After installing the template, you should be able to create a new project in Visual Studio and search for Clean Architecture. You should see the template appear in your list of project templates:

![Clean Architecture Project Template](https://user-images.githubusercontent.com/782127/80412393-cd116880-889b-11ea-886f-9b91fffbc767.png)

After choosing this template, provide a project name and finish the project creation wizard. You should be all set.

![Clean Architecture Project Template step 2](https://user-images.githubusercontent.com/782127/80412455-e5818300-889b-11ea-8219-379581583a92.png)

Note that the template is generally only updated with major updates to the project. The GitHub repository will always have the latest bug fixes and enhancements.

## Using the dotnet CLI template

First, install the template from NuGet:

```
dotnet new -i Ardalis.CleanArchitecture.Template
```

You should see the template in the list of templates from `dotnet new` after this install successfully. Look for "Steve Smith Clean Architecture" with Short Name of "clean-arch".

Navigate to the directory where you will put the new solution.
Run `dotnet new clean-arch -o Your.ProjectName` where `Your.ProjectName` is whatever you want to name the project.
The `Your.ProjectName` directory and solution file will be created, and inside that will be all of your new solution contents, properly namespaced and ready to run/test!

Example:
![powershell screenshot showing steps](https://user-images.githubusercontent.com/782127/101661723-9fd28e80-3a16-11eb-8be4-f9195d825ad6.png)

## Using the GitHub Repository

To get started based on this repository, you need to get a copy locally. You have three options: fork, clone, or download. Most of the time, you probably just want to download.

You should **download the repository**, unblock the zip file, and extract it to a new folder if you just want to play with the project or you wish to use it as the starting point for an application.

You should **fork this repository** only if you plan on submitting a pull request. Or if you'd like to keep a copy of a snapshot of the repository in your own GitHub account.

You should **clone this repository** if you're one of the contributors and you have commit access to it. Otherwise you probably want one of the other options.

# Goals

The goal of this repository is to provide a basic solution structure that can be used to build Domain-Driven Design (DDD)-based or simply well-factored, SOLID applications using .NET Core. Learn more about these topics here:

- [SOLID Principles for C# Developers](https://www.pluralsight.com/courses/csharp-solid-principles)
- [SOLID Principles of Object Oriented Design](https://www.pluralsight.com/courses/principles-oo-design) (the original, longer course)
- [Domain-Driven Design Fundamentals](https://www.pluralsight.com/courses/domain-driven-design-fundamentals)

If you're used to building applications as single-project or as a set of projects that follow the traditional UI -> Business Layer -> Data Access Layer "N-Tier" architecture, I recommend you check out these two courses (ideally before DDD Fundamentals):

- [Creating N-Tier Applications in C#, Part 1](https://www.pluralsight.com/courses/n-tier-apps-part1)
- [Creating N-Tier Applications in C#, Part 2](https://www.pluralsight.com/courses/n-tier-csharp-part2)

I also maintain Microsoft's reference application, eShopOnWeb, and its associated free eBook. Check them out here:

- [eShopOnWeb on GitHub](https://github.com/dotnet-architecture/eShopOnWeb)
- [Architecting Modern Web Applications with ASP.NET Core and Microsoft Azure](https://aka.ms/webappebook) (eBook)

## History and Shameless Plug Section

I've used this starter kit to teach the basics of ASP.NET Core using Domain-Driven Design concepts and patterns for some time now (starting when ASP.NET Core was still in pre-release). Typically I teach a one- or two-day hands-on workshop ahead of events like DevIntersection, or private on-site workshops for companies looking to bring their teams up to speed with the latest development technologies and techniques. Feel free to [contact me](https://ardalis.com/contact-us) if you'd like information about upcoming workshops.

# Design Decisions and Dependencies

The goal of this sample is to provide a fairly bare-bones starter kit for new projects. It does not include every possible framework, tool, or feature that a particular enterprise application might benefit from. Its choices of technology for things like data access are rooted in what is the most common, accessible technology for most business software developers using Microsoft's technology stack. It doesn't (currently) include extensive support for things like logging, monitoring, or analytics, though these can all be added easily. Below is a list of the technology dependencies it includes, and why they were chosen. Most of these can easily be swapped out for your technology of choice, since the nature of this architecture is to support modularity and encapsulation.

## The Core Project

The Core project is the center of the Clean Architecture design, and all other project dependencies should point toward it. As such, it has very few external dependencies. The one exception in this case is the `System.Reflection.TypeExtensions` package, which is used by `ValueObject` to help implement its `IEquatable<>` interface. The Core project should include things like:

- Entities
- Aggregates
- Domain Events
- DTOs
- Interfaces
- Event Handlers
- Domain Services
- Specifications

## The SharedKernel Project

Many solutions will also reference a separate **Shared Kernel** project/package. I recommend creating a separate SharedKernel project and solution if you will require sharing code between multiple projects. I further recommend this be published as a nuget package (more likely privately within your organization) and referenced as a nuget dependency by those projects that require it. For this sample, in the interest of simplicity, I've added a SharedKernel project to the solution. It contains types that would likely be shared between multiple projects, in my experience.

## The Infrastructure Project

Most of your application's dependencies on external resources should be implemented in classes defined in the Infrastructure project. These classes should implement interfaces defined in Core. If you have a very large project with many dependencies, it may make sense to have multiple Infrastructure projects (e.g. Infrastructure.Data), but for most projects one Infrastructure project with folders works fine. The sample includes data access and domain event implementations, but you would also add things like email providers, file access, web api clients, etc. to this project so they're not adding coupling to your Core or UI projects.

The Infrastructure project depends on `Microsoft.EntityFrameworkCore.SqlServer` and `Autofac`. The former is used because it's built into the default ASP.NET Core templates and is the least common denominator of data access. If desired, it can easily be replaced with a lighter-weight ORM like Dapper. Autofac (formerly StructureMap) is used to allow wireup of dependencies to take place closest to where the implementations reside. In this case, an InfrastructureRegistry class can be used in the Infrastructure class to allow wireup of dependencies there, without the entry point of the application even having to have a reference to the project or its types. [Learn more about this technique](https://ardalis.com/avoid-referencing-infrastructure-in-visual-studio-solutions). The current implementation doesn't include this behavior - it's something I typically cover and have students add themselves in my workshops.

## The Web Project

The entry point of the application is the ASP.NET Core web project. This is actually a console application, with a `public static void Main` method in `Program.cs`. It currently uses the default MVC organization (Controllers and Views folders) as well as most of the default ASP.NET Core project template code. This includes its configuration system, which uses the default `appsettings.json` file plus environment variables, and is configured in `Startup.cs`. The project delegates to the `Infrastructure` project to wire up its services using Autofac.

## The Test Projects

Test projects could be organized based on the kind of test (unit, functional, integration, performance, etc.) or by the project they are testing (Core, Infrastructure, Web), or both. For this simple starter kit, the test projects are organized based on the kind of test, with unit, functional and integration test projects existing in this solution. In terms of dependencies, there are three worth noting:

- [xunit](https://www.nuget.org/packages/xunit) I'm using xunit because that's what ASP.NET Core uses internally to test the product. It works great and as new versions of ASP.NET Core ship, I'm confident it will continue to work well with it.

- [Moq](https://www.nuget.org/packages/Moq/) I'm using Moq as a mocking framework for white box behavior-based tests. If I have a method that, under certain circumstances, should perform an action that isn't evident from the object's observable state, mocks provide a way to test that. I could also use my own Fake implementation, but that requires a lot more typing and files. Moq is great once you get the hang of it, and assuming you don't have to mock the world (which we don't in this case because of good, modular design).

- [Microsoft.AspNetCore.TestHost](https://www.nuget.org/packages/Microsoft.AspNetCore.TestHost) I'm using TestHost to test my web project using its full stack, not just unit testing action methods. Using TestHost, you make actual HttpClient requests without going over the wire (so no firewall or port configuration issues). Tests run in memory and are very fast, and requests exercise the full MVC stack, including routing, model binding, model validation, filters, etc.

# Patterns Used

This solution template has code built in to support a few common patterns, especially Domain-Driven Design patterns. Here is a brief overview of how a few of them work.

## Domain Events

Domain events are a great pattern for decoupling a trigger for an operation from its implementation. This is especially useful from within domain entities since the handlers of the events can have dependencies while the entities themselves typically do not. In the sample, you can see this in action with the `ToDoItem.MarkComplete()` method. The following sequence diagram demonstrates how the event and its handler are used when an item is marked complete through a web API endpoint.

![Domain Event Sequence Diagram](https://user-images.githubusercontent.com/782127/75702680-216ce300-5c73-11ea-9187-ec656192ad3b.png)

## Related Projects

- [ApiEndpoints](https://github.com/ardalis/apiendpoints)
- [GuardClauses](https://github.com/ardalis/guardclauses)
- [Result](https://github.com/ardalis/result)
- [Specification](https://github.com/ardalis/specification)

