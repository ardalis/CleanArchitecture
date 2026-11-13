---
title: "Enforcing Dependency Rules"
weight: 52
---

# Enforcing Dependency Rules with NsDepCop

Clean Architecture depends on dependencies pointing in the right direction: inward, toward the domain model. How that rule is enforced differs between the solutions in this repository.

- **Full template (`clean-arch`)**: Layers are separate projects. Core has no project reference to Infrastructure, so code in Core *can't* use Infrastructure types. The compiler enforces the layering with no extra tooling.
- **Minimal template (`min-clean`)**: Everything is in one Web project, so project references can't keep `Domain` away from `Infrastructure`. [NsDepCop](https://github.com/realvizu/NsDepCop) does that job instead.
- **Sample (`NimblePros.SampleToDo`)**: Project references enforce the layers, the same as the full template. NsDepCop adds rules *inside* the Core and UseCases projects, which project references can't express.

## How NsDepCop Works

NsDepCop is a Roslyn analyzer. It reads a `config.nsdepcop` file in the project folder and reports error `NSDEPCOP01` whenever code in one namespace references a type in a namespace the config forbids. Because it's an analyzer, violations show up in the IDE as you type and fail `dotnet build`, both locally and in CI.

Each project that uses it has three pieces:

1. A central package version, in `Directory.Packages.props`:

   ```xml
   <PackageVersion Include="NsDepCop" Version="3.3.0" />
   ```

2. A package reference, plus a setting that treats violations as errors, in the `.csproj`:

   ```xml
   <PropertyGroup>
     <WarningsAsErrors>NSDEPCOP01</WarningsAsErrors>
   </PropertyGroup>

   <ItemGroup>
     <PackageReference Include="NsDepCop">
       <PrivateAssets>all</PrivateAssets>
       <IncludeAssets>runtime; build; native; contentfiles; analyzers</IncludeAssets>
     </PackageReference>
   </ItemGroup>
   ```

3. A `config.nsdepcop` file next to the `.csproj`, which holds the rules.

### Rule Syntax

All of the configs in this repository use the same approach: allow everything, then list the specific dependencies that are forbidden.

```xml
<NsDepCopConfig>
    <Allowed From="*" To="*" />

    <Disallowed From="MyApp.Domain.*" To="MyApp.Infrastructure.*" />

    <AnalyzerConfig>
        <IssueKind Name="IllegalDependency" Severity="Error" />
    </AnalyzerConfig>
</NsDepCopConfig>
```

- `MyApp.Domain.*` matches `MyApp.Domain` **and all of its child namespaces**.
- `MyApp.Domain` (without `.*`) matches **only that exact namespace**, not its children. The sample's Core rules rely on this to restrict an aggregate's own namespace while leaving its `Handlers` child namespace unrestricted.
- **`Disallowed` always wins over `Allowed`.** You can't carve out an exception to a `Disallowed` rule with a narrower `Allowed` rule. To exempt a child namespace, make the `Disallowed` rule more specific instead.

Rules only see namespaces. They enforce anything only if your folder structure and namespaces line up, so keep namespaces matching folders. A type declared in the wrong namespace (for example, one copied from another project) quietly sits outside every rule.

### What a Violation Looks Like

```text
error NSDEPCOP01: Illegal namespace reference: MinimalClean.Architecture.Web.Domain.CartAggregate->MinimalClean.Architecture.Web.ProductFeatures (Type: Cart->ProductDto)
```

The message gives the two namespaces and the two types involved. Fix it by moving the dependency in the right direction. Usually that means defining an interface or type in the inner namespace, or moving shared code into `Domain`/Core. Don't loosen the rule just to make the build pass.

## Minimal Template Rules

File: `src/MinimalClean.Architecture.Web/config.nsdepcop`

| From | May not depend on | Why |
|------|-------------------|-----|
| `Domain.*` | `Infrastructure.*` | The domain is the innermost layer; it defines interfaces that Infrastructure implements. |
| `Domain.*` | `CartFeatures.*`, `ProductFeatures.*` | Features use the domain, never the reverse. |
| `Domain.*` | `Configurations.*` | App wiring (DI, middleware, Mediator setup) is an outer concern. |
| `CartFeatures.*`, `ProductFeatures.*` | `Infrastructure.*` | Feature slices work with domain abstractions, not concrete data access or email implementations. |
| `CartFeatures.*` | `ProductFeatures.*` (and vice versa) | Vertical slices are independent; share code through `Domain` instead of reaching into another slice. |
| `Domain.OrderAggregate.*` | `Domain.CartAggregate.*` | Checkout creates an order *from* a cart, but the Order aggregate must not depend on Cart. |

All namespaces above are prefixed with `MinimalClean.Architecture.Web.`.

When you add a new feature slice (for example `OrderFeatures`), add matching rules: `Domain.*` must not depend on it, it must not depend on `Infrastructure.*`, and it and the other slices must not depend on each other.

## Sample Rules

### Core

File: `sample/src/NimblePros.SampleToDo.Core/config.nsdepcop`

Here, the *aggregate namespaces* mean the namespace of an aggregate (exact match) plus its `Events` and `Specifications` child namespaces. Its `Handlers` namespace is intentionally left out.

| From | May not depend on | Why |
|------|-------------------|-----|
| Aggregate namespaces of `ContributorAggregate` and `ProjectAggregate` | `Core.Services.*` | Domain services coordinate aggregates; aggregates don't call domain services. |
| Aggregate namespaces of `ContributorAggregate` and `ProjectAggregate` | `Core.Interfaces.*` | Abstractions for external services (such as `IEmailSender`) belong to handlers and services, not entities. |
| Aggregate namespaces of `ContributorAggregate` | `ProjectAggregate.*` | Contributor is upstream of Project. A `ToDoItem` refers to a contributor by `ContributorId`, never the reverse. |
| `ProjectAggregate.*` | `ContributorAggregate.Events`, `.Handlers`, `.Specifications` | Project may use `ContributorId` (in the `ContributorAggregate` namespace), but not Contributor's internals. |

Domain event handlers are exempt from these rules on purpose. `ContributorAggregate.Handlers.ContributorDeletedHandler` reacts to a contributor being deleted by loading and updating projects, and `ProjectAggregate.Handlers` uses `IEmailSender` to send notifications. Handlers are where the work that crosses aggregates belongs.

All namespaces above are prefixed with `NimblePros.SampleToDo.Core.`.

### UseCases

File: `sample/src/NimblePros.SampleToDo.UseCases/config.nsdepcop`

| From | May not depend on | Why |
|------|-------------------|-----|
| `UseCases.Contributors.*` | `UseCases.Projects.*` | Feature folders are independent; share behavior through Core. |
| `UseCases.Projects.*` | `UseCases.Contributors.*` | Same as above. |

All namespaces above are prefixed with `NimblePros.SampleToDo.`.

## Why the Full Template Doesn't Include NsDepCop

In `clean-arch`, project references already enforce every layer rule NsDepCop would express, so adding it would mostly repeat the compiler's checks and add another config file to learn. If your Core or UseCases project grows enough that you want rules *inside* it (for example, keeping aggregates or feature folders independent), follow the sample: add the package to that project and create a `config.nsdepcop` next to its `.csproj`.

## Adding or Changing Rules

1. Edit the `config.nsdepcop` for the project. Add a `Disallowed` rule for each dependency you want to forbid.
2. Build. If existing code breaks the new rule, the build fails with `NSDEPCOP01` and lists every violation.
3. To confirm a new rule actually fires, add a type that breaks it, build, check the error, and then delete the type.

## Resources

- [NsDepCop on GitHub](https://github.com/realvizu/NsDepCop)
- [NsDepCop configuration reference](https://github.com/realvizu/NsDepCop/blob/master/doc/Help.md)
