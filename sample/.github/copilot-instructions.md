# Solution Development Practices & Conventions

This document provides guidance for contributing to this solution, which is based on the Clean Architecture template for .NET 9. Follow these practices to ensure consistency, maintainability, and adherence to architectural principles.

---

## Table of Contents

- [Architecture Overview](#architecture-overview)
- [Project Structure](#project-structure)
- [Development Patterns](#development-patterns)
- [Use Case Guidelines](#use-case-guidelines)
- [API Endpoint Conventions](#api-endpoint-conventions)
- [Validation Strategy](#validation-strategy)
- [Testing](#testing)
- [File & Naming Conventions](#file--naming-conventions)
- [Common Gotchas](#common-gotchas)
- [Essential Commands](#essential-commands)

---

## Architecture Overview

- Follows Clean Architecture and Domain-Driven Design (DDD) principles.
- **Core** project contains domain models, value objects, and repository interfaces.
- **UseCases** project contains application logic, commands, queries, and handlers.
- **Infrastructure** project implements data access and external services.
- **Web** project exposes API endpoints using FastEndpoints.

**Dependency Flow:**  
`Core` ← `UseCases` ← `Infrastructure`  
`Core` ← `UseCases` ← `Web`  
_Core must never depend on outer layers._

---

## Project Structure

- **Core**: Domain entities, aggregates, value objects, domain events, repository interfaces.
- **UseCases**: CQRS handlers (commands/queries), Mediator, application logic.
- **Infrastructure**: EF Core, external services, repository implementations.
- **Web**: FastEndpoints API, request/response/validator classes, REPR pattern.

---

## Development Patterns

- **CQRS**: Use commands for mutations and queries for reads.
- **Mediator**: All use case handlers implement Mediator interfaces.
- **Repository Pattern**: Use repository interfaces from Core, implemented in Infrastructure.
- **Value Objects**: Use for domain concepts (e.g., `ProjectName`, `ContributorId`).
- **Domain Events**: Use for cross-aggregate communication.

---

## Use Case Guidelines

- **Inputs/Outputs**: Use Value Objects (`ProjectName`, `ContributorId`) for use case command/query parameters and return types. Use value objects internally within the domain. Translate from primitives in the endpoints.
- **Validation**: Validate at API and use case boundaries; enforce business invariants in the domain.
- **Error Handling**: Use `Ardalis.Result<T>` for use case results and error propagation.
- **Defensive Coding**: Validate commands/queries in handlers; assume pre-validated input in domain. Use `Ardalis.GuardClauses` to defend against invalid states in the domain.

---

## API Endpoint Conventions

- One endpoint per file (e.g., `Create.cs`, `Update.cs`).
- Separate files for request, response, and validator (e.g., `Create.CreateRequest.cs`, `Create.CreateResponse.cs`, `Create.CreateValidator.cs`).
- Endpoints inherit from `Endpoint<TRequest, TResponse>`.
- Use REPR (Request/Endpoint/Processor/Response) pattern.

---

## Validation Strategy

- **API Level**: Use FluentValidation for request DTOs.
- **Use Case Level**: Validate commands/queries in handlers or behaviors.
- **Domain Level**: Throw exceptions for business rule violations using guard clauses (e.g., `Ardalis.GuardClauses`).)

---

## Testing

- **UnitTests**: Test domain logic and use cases.
- **IntegrationTests**: Test infrastructure and database interactions.
- **FunctionalTests**: Test API endpoints using subcutaneous testing.

---

## File & Naming Conventions

- Use PascalCase for files, classes, and methods.
- Organize files by feature (e.g., `Contributors/Create.cs`, `Projects/Create.cs`).
- Place request, response, and validator files alongside their endpoint.
- Do not use hyphens in project or file names.
- Test classes are named for the class and method being tested. Examples: `ProjectAddItem` or `CreateContributorHandlerHandle`.
- Test methods are named such that the class name and method name form a sentence describing the test. (e.g. `ReturnsItemGivenValidInputs()` or `ThrowsExceptionGivenNegativeInput()`)

---

## Common Gotchas

- Do not allow Core to reference UseCases, Infrastructure, or Web.
- Use absolute paths for EF Core migration commands.
