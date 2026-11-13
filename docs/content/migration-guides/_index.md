---
title: "Migration Guides"
weight: 80
bookCollapseSection: false
---

# Migration Guides

> [!IMPORTANT]
> **Guide titles use template versions, not .NET versions.** "v11 to v12" means moving from template **11.x (.NET 10)** to template **12.x (.NET 11)**. See [Versions](../versions.md) for the full mapping.

| Guide | From | To |
|---|---|---|
| [v11 to v12](v11-to-v12.md) | Template 11.x on .NET 10 | Template 12.x on .NET 11 |
| [v10 to v11](v10-to-v11.md) | Template 10.x on .NET 9 | Template 11.x on .NET 10 |

## Why there's no automatic upgrade

This template is a **starting point**, not a library you keep referencing like a NuGet package. Once you generate a solution, the code is yours, and you've probably changed it a lot. So there's no automated upgrade path. These guides list what changed between versions and why, so you can decide what to adopt.

You don't *have* to upgrade. Moving to a newer .NET version is something you can do on your own schedule; the template's newer architectural changes are worth adopting when they solve a problem you have.

## The diff strategy

The most reliable way to see exactly what changed is to compare two freshly generated solutions that use **your project name**:

```bash
# 1. Generate the version you started from
dotnet new install Ardalis.CleanArchitecture.Template::11.1.1
dotnet new clean-arch -o old/Your.ProjectName

# 2. Generate the version you're moving to (installing replaces the old version)
dotnet new install Ardalis.CleanArchitecture.Template
dotnet new clean-arch -o new/Your.ProjectName

# 3. Compare them
git diff --no-index old/Your.ProjectName new/Your.ProjectName
```

Any diff tool works for step 3 (Beyond Compare, WinMerge, VS Code's folder compare, ...). Because both solutions use your names and namespaces, the differences are exactly the template's changes, and you can apply them to your codebase file by file.

You can also compare the release tags directly on GitHub, for example [v11.1.1...main](https://github.com/ardalis/CleanArchitecture/compare/v11.1.1...main).
