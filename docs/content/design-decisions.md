---
title: "Goals & Design Decisions"
weight: 20
---

# Goals

The goal of this repository is to provide a basic solution structure that can be used to build Domain-Driven Design (DDD)-based or simply well-factored, SOLID applications using .NET Core. Learn more about these topics here:

- [SOLID Principles for C# Developers](https://www.pluralsight.com/courses/csharp-solid-principles)
- [Domain-Driven Design Fundamentals](https://www.pluralsight.com/courses/fundamentals-domain-driven-design)
- [Refactoring to SOLID C# Code](https://www.pluralsight.com/courses/refactoring-solid-c-sharp-code)

If you're used to building applications as single-project or as a set of projects that follow the traditional UI -> Business Layer -> Data Access Layer "N-Tier" architecture, I recommend you check out these two courses (ideally before DDD Fundamentals):

- [Creating N-Tier Applications in C#, Part 1](https://www.pluralsight.com/courses/n-tier-apps-part1)
- [Creating N-Tier Applications in C#, Part 2](https://www.pluralsight.com/courses/n-tier-csharp-part2)

Steve Smith also maintains Microsoft's reference application, eShopOnWeb, and its associated free eBook. Check them out here:

- [eShopOnWeb on GitHub](https://github.com/nimblepros/eShopOnWeb) (now supported by [NimblePros](https://nimblepros.com))
- [Architecting Modern Web Applications with ASP.NET Core and Microsoft Azure](https://aka.ms/webappebook) (eBook)

Note that the goal of this project and repository is **not** to provide a sample or reference application. It's meant to just be a template, but with enough pieces in place to show you where things belong as you set up your actual solution. Instead of useless "Class1.cs" there are a few real classes in place. Delete them as soon as you understand why they're there and where you should put your own, similar files. There *is* a sample application in the `/sample` folder, if you're looking for that.

## History and Shameless Plug Section

I've used this starter kit to teach the basics of ASP.NET Core using Domain-Driven Design concepts and patterns for some time now (starting when ASP.NET Core was still in pre-release). Typically I teach a one- or two-day hands-on workshop ahead of events like DevIntersection, or private on-site workshops for companies looking to bring their teams up to speed with the latest development technologies and techniques. Feel free to [contact me](https://nimblepros.com/) if you'd like information about upcoming workshops.

# Design Decisions and Dependencies

The goal of this solution template is to provide a fairly bare-bones starter kit for new projects. It does not include every possible framework, tool, or feature that a particular enterprise application might benefit from. Its choices of technology for things like data access are rooted in what is the most common, accessible technology for most business software developers using Microsoft's technology stack. It doesn't (currently) include extensive support for things like logging, monitoring, or analytics, though these can all be added easily. Below is a list of the technology dependencies it includes, and why they were chosen. Most of these can easily be swapped out for your technology of choice, since the nature of this architecture is to support modularity and encapsulation.

## Where To Validate

Validation of user input is a requirement of all software applications. The question is, where does it make sense to implement it in a concise and elegant manner? This solution template includes 4 separate projects, each of which might be responsible for performing validation as well as enforcing business invariants (which, given validation should already have occurred, are usually modeled as exceptions).

- [When to Validate and When to Throw Exceptions](https://www.youtube.com/watch?v=dpPcnAT7n7M)

The domain model itself should generally rely on object-oriented design to ensure it is always in a consistent state. It leverages encapsulation and limits public state mutation access to achieve this, and it assumes that any arguments passed to it have already been validated, so null or other improper values yield exceptions, not validation results, in most cases.

The use cases / application project includes the set of all commands and queries the system supports. It's frequently responsible for validating its own command and query objects. This is most easily done using a [chain of responsibility pattern](https://deviq.com/design-patterns/chain-of-responsibility-pattern) via Mediator behaviors or some other pipeline.

The Web project includes all API endpoints, which include their own request and response types, following the [REPR pattern](https://deviq.com/design-patterns/repr-design-pattern). The FastEndpoints library includes built-in support for validation using FluentValidation on the request types. This is a natural place to perform input validation as well.

Having validation occur both within the API endpoints and then again at the use case level may be considered redundant. There are tradeoffs to adding essentially the same validation in two places, one for API requests and another for messages sent to Use Case handlers. Following defensive coding, it often makes sense to add validation in both places, as the overhead is minimal and the peace of mind of mind and greater application robustness is often worth it.

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

You can learn more about these patterns and how to apply them here:

- [DDD Fundamentals](https://www.pluralsight.com/courses/fundamentals-domain-driven-design)
- [DDD Concepts](https://deviq.com/domain-driven-design/ddd-overview)

## The Use Cases Project

An optional project, I've included it because many folks were demanding it and it's easier to remove than to add later. This is also often referred to as the *Application* or *Application Services* layer. The Use Cases project is organized following CQRS into Commands and Queries (I considered having folders for `Commands` and `Queries` but felt it added little - the folders per actual *command* or *query* is sufficient without extra nesting). Commands mutate the domain model and thus should always use Repository abstractions for their data access (Repositories are how one fetches and persists domain model types). Queries are readonly, and thus **do not need to use the repository pattern**, but instead can use whatever query service or approach is most convenient.

Since the Use Cases project is set up to depend on Core and does not depend on Infrastructure, there will still need to be abstractions defined for its data access. And it *can* use things like specifications, which can sometimes help encapsulate query logic as well as result type mapping. But it doesn't *have* to use repository/specification - it can just issue a SQL query or call a stored procedure if that's the most efficient way to get the data.

Although this is an optional project to include (without it, your API endpoints would just work directly with the domain model or query services), it does provide a nice UI-ignorant place to add automated tests, and lends itself toward applying policies for cross-cutting concerns using a Chain of Responsibility pattern around the message handlers (for things like validation, caching, auth, logging, timing, etc.). The template includes an example of this for logging, which is located in the [SharedKernel NuGet package](https://github.com/ardalis/Ardalis.SharedKernel/blob/main/src/Ardalis.SharedKernel/LoggingBehavior.cs).

## The Infrastructure Project

Most of your application's dependencies on external resources should be implemented in classes defined in the Infrastructure project. These classes should implement interfaces defined in Core. If you have a very large project with many dependencies, it may make sense to have multiple Infrastructure projects (e.g. Infrastructure.Data), but for most projects one Infrastructure project with folders works fine. The template includes data access and domain event implementations, but you would also add things like email providers, file access, web api clients, etc. to this project so they're not adding coupling to your Core or UI projects.

## The Web Project

The entry point of the application is the ASP.NET Core web project (or possibly the AspireHost project, which in turn loads the Web project). This is actually a console application, with a `public static void Main` method in `Program.cs`. It leverages FastEndpoints and the REPR pattern to organize its API endpoints.

## The SharedKernel Package

A [Shared Kernel](https://deviq.com/domain-driven-design/shared-kernel) is used to share common elements between bounded contexts. It's a DDD term but many organizations leverage "common" projects or packages for things that are useful to share between several applications.

I recommend creating a separate SharedKernel project and solution if you will require sharing code between multiple [bounded contexts](https://ardalis.com/encapsulation-boundaries-large-and-small/) (see [DDD Fundamentals](https://www.pluralsight.com/courses/domain-driven-design-fundamentals)). I further recommend this be published as a NuGet package (most likely privately within your organization) and referenced as a NuGet dependency by those projects that require it.

Previously a project for SharedKernel was included in this project. However, for the above reasons I've made it a separate package, [Ardalis.SharedKernel](https://github.com/ardalis/Ardalis.SharedKernel), which **you should replace with your own when you use this template**.

If you want to see another [example of a SharedKernel package, the one I use in my updated Pluralsight DDD course is on NuGet here](https://www.nuget.org/packages/PluralsightDdd.SharedKernel/).

## The Test Projects

Test projects could be organized based on the kind of test (unit, functional, integration, performance, etc.) or by the project they are testing (Core, Infrastructure, Web), or both. For this simple starter kit, the test projects are organized based on the kind of test, with unit, functional and integration test projects existing in this solution. Functional tests are a special kind of [integration test](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-8.0) that perform [subcutaneous testing](https://martinfowler.com/bliki/SubcutaneousTest.html) of the APIs of the Web project, without actually hosting a real website or going over the network. I've created a bunch of [test helpers](https://github.com/ardalis/HttpClientTestExtensions) to make these kinds of tests shorter and easier to maintain.

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
- [FastEndpoints](https://fast-endpoints.com/)

## Presentations and Videos on Clean Architecture

- [What's New in Clean Architecture Template 9.1](https://www.youtube.com/watch?v=EJIgjL41em4)
- [The REPR Pattern and Clean Architecture](https://www.youtube.com/watch?v=-AJcEJPwagQ)
- [Clean Architecture with ASP.NET Core 8](https://www.youtube.com/watch?v=yF9SwL0p0Y0)
- [Getting Started with Clean Architecture and .NET 8 (webinar)](https://www.youtube.com/watch?v=IsmyqNrfQQw)
