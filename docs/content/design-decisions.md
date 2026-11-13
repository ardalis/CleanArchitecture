---
title: "Goals & Design Decisions"
weight: 50
---

# Goals & Design Decisions

## Goals

The goal of this repository is to provide a basic solution structure that can be used to build Domain-Driven Design (DDD)-based or simply well-factored, SOLID applications using .NET. Learn more about these topics here:

- [SOLID Principles for C# Developers](https://www.pluralsight.com/courses/csharp-solid-principles)
- [Domain-Driven Design Fundamentals](https://www.pluralsight.com/courses/fundamentals-domain-driven-design)
- [Refactoring to SOLID C# Code](https://www.pluralsight.com/courses/refactoring-solid-c-sharp-code)

If you're used to building applications as a single project, or as a set of projects that follow the traditional UI → Business Layer → Data Access Layer "N-Tier" architecture, I recommend you check out these two courses (ideally before DDD Fundamentals):

- [Creating N-Tier Applications in C#, Part 1](https://www.pluralsight.com/courses/n-tier-apps-part1)
- [Creating N-Tier Applications in C#, Part 2](https://www.pluralsight.com/courses/n-tier-csharp-part2)

Steve Smith also maintains Microsoft's reference application, eShopOnWeb, and its associated free eBook. Check them out here:

- [eShopOnWeb on GitHub](https://github.com/nimblepros/eShopOnWeb) (now supported by [NimblePros](https://nimblepros.com))
- [Architecting Modern Web Applications with ASP.NET Core and Microsoft Azure](https://aka.ms/webappebook) (eBook)

Note that the goal of this project and repository is **not** to provide a sample or reference application. It's meant to be a template, but with enough pieces in place to show you where things belong as you set up your actual solution. Instead of a useless `Class1.cs`, there are a few real classes in place. Delete them as soon as you understand why they're there and where you should put your own, similar files. There *is* a sample application in the [`/sample`](https://github.com/ardalis/CleanArchitecture/tree/main/sample) folder, if you're looking for that.

## History and shameless plug section

I've used this starter kit to teach the basics of ASP.NET Core using Domain-Driven Design concepts and patterns for some time now (starting when ASP.NET Core was still in pre-release). Typically I teach a one- or two-day hands-on workshop ahead of events like DevIntersection, or private on-site workshops for companies looking to bring their teams up to speed with the latest development technologies and techniques. Feel free to [contact me](https://nimblepros.com/) if you'd like information about upcoming workshops.

## Design decisions and dependencies

The goal of this solution template is to provide a fairly bare-bones starter kit for new projects. It doesn't include every possible framework, tool, or feature that a particular enterprise application might benefit from. Its choices of technology for things like data access are rooted in what is most common and accessible for business software developers using Microsoft's technology stack. Below is the reasoning behind its structure and dependencies. Most of them can easily be swapped for your technology of choice, since the point of this architecture is to support modularity and encapsulation. The [Clean Architecture template page](templates/clean-architecture.md#key-dependencies) lists the packages themselves.

Significant decisions are recorded as [Architecture Decision Records](architecture-decisions/_index.md).

### Where to validate

Validation of user input is a requirement of all software applications. The question is, where does it make sense to implement it in a concise and elegant manner? This solution template includes four separate projects, each of which might be responsible for performing validation as well as enforcing business invariants (which, given that validation should already have occurred, are usually modeled as exceptions).

- [When to Validate and When to Throw Exceptions](https://www.youtube.com/watch?v=dpPcnAT7n7M)

The domain model itself should generally rely on object-oriented design to ensure it is always in a consistent state. It leverages encapsulation and limits public state mutation to achieve this, and it assumes that any arguments passed to it have already been validated, so null or other improper values yield exceptions, not validation results, in most cases. Value objects take this further by following *parse, don't validate*: a `ContributorName` can only be created through `ContributorName.From(...)`, which rejects invalid input, so code that holds one knows it's valid.

- [Parse, Don't Validate](https://www.youtube.com/watch?v=KQVy0CaB7ds)

The use cases (application) project includes the set of all commands and queries the system supports. It's frequently responsible for validating its own command and query objects. This is most easily done using a [chain of responsibility pattern](https://deviq.com/design-patterns/chain-of-responsibility-pattern) via Mediator pipeline behaviors or some other pipeline.

The Web project includes all API endpoints, which have their own request and response types, following the [REPR pattern](https://deviq.com/design-patterns/repr-design-pattern). The FastEndpoints library includes built-in support for validation using FluentValidation on the request types. This is a natural place to perform input validation as well.

Having validation occur both within the API endpoints and then again at the use case level may be considered redundant. There are trade-offs to adding essentially the same validation in two places, one for API requests and another for messages sent to use case handlers. Following defensive coding, it often makes sense to add validation in both places, as the overhead is minimal and the peace of mind and greater application robustness are often worth it.

### The Core project

The Core project is the center of the Clean Architecture design, and all other project dependencies should point toward it. As such, it has very few external dependencies. The Core project should include the domain model, including things like:

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

### The Use Cases project

An optional project, I've included it because many folks were demanding it and it's easier to remove than to add later. This is also often referred to as the *Application* or *Application Services* layer. The Use Cases project is organized following CQRS into commands and queries. (I considered having folders for `Commands` and `Queries` but felt it added little; the folders per actual *command* or *query* are sufficient without extra nesting.) Commands mutate the domain model and thus should always use repository abstractions for their data access (repositories are how one fetches and persists domain model types). Queries are read-only, and thus **do not need to use the repository pattern**, but instead can use whatever query service or approach is most convenient.

Since the Use Cases project is set up to depend on Core and does not depend on Infrastructure, there still need to be abstractions defined for its data access. And it *can* use things like specifications, which can sometimes help encapsulate query logic as well as result type mapping. But it doesn't *have* to use repository/specification. It can just issue a SQL query or call a stored procedure if that's the most efficient way to get the data.

Although this is an optional project (without it, your API endpoints would just work directly with the domain model or query services), it does provide a nice UI-ignorant place to add automated tests, and it lends itself to applying policies for cross-cutting concerns using a chain of responsibility pattern around the message handlers (for things like validation, caching, auth, logging, timing, etc.). The template includes an example of this for logging, which is located in the [SharedKernel NuGet package](https://github.com/ardalis/Ardalis.SharedKernel/blob/main/src/Ardalis.SharedKernel/LoggingBehavior.cs).

Commands and queries are dispatched with [Mediator](https://github.com/martinothamar/Mediator), a source-generated mediator. It replaced MediatR in template v11 (see the [v10 to v11 migration guide](migration-guides/v10-to-v11.md)).

- [Tame Cross-Cutting Concerns (CodeMash 2025)](https://www.youtube.com/watch?v=vagyJdrWLr0)

### The Infrastructure project

Most of your application's dependencies on external resources should be implemented in classes defined in the Infrastructure project. These classes should implement interfaces defined in Core. If you have a very large project with many dependencies, it may make sense to have multiple Infrastructure projects (e.g. Infrastructure.Data), but for most projects one Infrastructure project with folders works fine. The template includes data access and domain event implementations, but you would also add things like email providers, file access, web API clients, etc. to this project so they're not adding coupling to your Core or UI projects.

### The Web project

The entry point of the application is the ASP.NET Core web project (or possibly the AspireHost project, which in turn launches the Web project). This is actually a console application, with top-level statements in `Program.cs`. It leverages FastEndpoints and the REPR pattern to organize its API endpoints.

Since template v8, the Web project includes only API endpoints built with FastEndpoints. If you want Controllers, Razor Pages, or Ardalis.ApiEndpoints, see [Customizing](customizing.md#controllers-razor-pages-and-apiendpoints).

### The SharedKernel package

A [Shared Kernel](https://deviq.com/domain-driven-design/shared-kernel) is used to share common elements between bounded contexts. It's a DDD term, but many organizations leverage "common" projects or packages for things that are useful to share between several applications.

I recommend creating a separate SharedKernel project and solution if you will require sharing code between multiple [bounded contexts](https://ardalis.com/encapsulation-boundaries-large-and-small/) (see [DDD Fundamentals](https://www.pluralsight.com/courses/fundamentals-domain-driven-design)). I further recommend this be published as a NuGet package (most likely privately within your organization) and referenced as a NuGet dependency by those projects that require it.

Previously a project for SharedKernel was included in this project. However, for the above reasons I've made it a separate package, [Ardalis.SharedKernel](https://github.com/ardalis/Ardalis.SharedKernel), which **you should replace with your own when you use this template**.

If you want to see another [example of a SharedKernel package, the one I use in my updated Pluralsight DDD course is on NuGet here](https://www.nuget.org/packages/PluralsightDdd.SharedKernel/).

### The test projects

Test projects could be organized based on the kind of test (unit, functional, integration, performance, etc.) or by the project they are testing (Core, Infrastructure, Web), or both. For this simple starter kit, the test projects are organized based on the kind of test, with unit, functional, and integration test projects existing in this solution. Functional tests are a special kind of [integration test](https://learn.microsoft.com/aspnet/core/test/integration-tests) that perform [subcutaneous testing](https://martinfowler.com/bliki/SubcutaneousTest.html) of the APIs of the Web project, without actually hosting a real website or going over the network. I've created a bunch of [test helpers](https://github.com/ardalis/HttpClientTestExtensions) to make these kinds of tests shorter and easier to maintain. See [Building & Testing](testing.md).

### Enforcing dependencies

In this template, project references enforce the dependency direction. Core doesn't reference UseCases, Infrastructure, or Web, so it can't use their types, and the compiler catches any attempt.

Project references can't control dependencies *within* a project, though. The sample application (`/sample`) adds [NsDepCop](https://github.com/realvizu/NsDepCop) to its Core and UseCases projects for that. For example, it fails the build if an aggregate depends on a domain service, or if the `Contributors` use cases depend on the `Projects` use cases. The Minimal Clean template uses NsDepCop to enforce all of its layer boundaries, since it has only one project. See [Enforcing Dependency Rules](dependency-rules.md) for the rules and how to add your own.

## Patterns used

This solution template has code built in to support a few common patterns, especially Domain-Driven Design patterns. Here is a brief overview of how a few of them work.

### Domain events

Domain events are a great pattern for decoupling a trigger for an operation from its implementation. This is especially useful from within domain entities, since the handlers of the events can have dependencies while the entities themselves typically do not.

In the template, an entity registers an event (for example, `Contributor.UpdateName()` registers a `ContributorNameUpdatedEvent`). After `SaveChanges` succeeds, the `EventDispatchInterceptor` in Infrastructure dispatches the registered events through Mediator to their handlers, such as `ContributorNameUpdatedEmailNotificationHandler`, which sends an email. The following sequence diagram shows the same flow in an earlier version of the template, in which a to-do item was marked complete through a web API endpoint:

![Domain Event Sequence Diagram](https://user-images.githubusercontent.com/782127/75702680-216ce300-5c73-11ea-9187-ec656192ad3b.png)

### Repository, unit of work, and transactions

The sample repository (`EfRepository<T>`) saves changes on every `AddAsync`, `UpdateAsync`, and `DeleteAsync` call. It does not support a unit of work or a transaction that spans several repository calls. Because `AppDbContext` is scoped and shared by all repositories in a request, it's easy to add your own unit of work abstraction that calls `SaveChangesAsync` once, at the end of a use case. See [Unit of Work & Transactions](unit-of-work-and-transactions.md) for details.

### More

The [Resources](resources.md) page lists related libraries, presentations, and videos about these patterns.
