---
title: "Contributing"
weight: 100
---

# Contributing

We love your input! We want to make contributing to this project as easy and transparent as possible, whether it's:

- Reporting a bug
- Discussing the current state of the code
- Submitting a fix
- Proposing new features
- Improving these docs

**If you're unsure or afraid of *anything*, just ask, or submit the issue or pull request anyway.** You won't be yelled at for giving your best effort. The worst that can happen is that you'll be politely asked to change something.

## Issues and discussions

- **Questions and ideas**: use [GitHub Discussions](https://github.com/ardalis/CleanArchitecture/discussions).
- **Bugs**: [open an issue](https://github.com/ardalis/CleanArchitecture/issues/new/choose). Before you do:
  - Test against the latest released version. It's possible the bug has already been fixed.
  - Check for an existing open issue. If there is one, upvote it or comment with your specific scenario.
  - Provide a reproducible test case. If a contributor can't reproduce an issue, it's much less likely to get fixed.
  - Respond promptly to questions from the maintainers. Stale issues are eventually closed.

## Pull requests

Pull requests are welcome, and are most likely to be accepted when they address an existing issue. **Ask before you start**: open an issue (or comment on an existing one) saying you'd like to submit a pull request. It's better etiquette, and you'll find out early whether the change fits.

Look for [issues labeled `help wanted`](https://github.com/ardalis/CleanArchitecture/issues?q=is%3Aissue+is%3Aopen+label%3A%22help+wanted%22) for good places to start.

1. Fork the repository and create your branch from `main`.
2. If you've added code that should be tested, add tests.
3. If you've changed behavior, update the documentation (see [Contributing to the docs](#contributing-to-the-docs)).
4. Make sure the build and the test suites pass (see [Building and testing the repository](#building-and-testing-the-repository)).
5. Follow the existing coding conventions (`.editorconfig` is the source of truth).
6. Open the pull request. If it addresses an issue, include "Fixes #NNN" in the description so the issue is linked and closed when it merges.

Helpful guides:

- [Pull Request Checklist](https://ardalis.com/github-pull-request-checklist/)
- [Resync your fork with this upstream repo](https://ardalis.com/syncing-a-fork-of-a-github-repository-with-upstream/)

Any contributions you make are under the same [MIT License](https://github.com/ardalis/CleanArchitecture/blob/main/LICENSE) that covers the project.

This project has adopted the code of conduct defined by the Contributor Covenant. For more information, see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).

## Repository layout

| Path | What it is |
|---|---|
| `src/`, `tests/`, `Clean.Architecture.slnx` | Source of the **Clean Architecture** template (`clean-arch`) |
| `.template.config/template.json` | `dotnet new` configuration for the full template |
| `CleanArchitecture.nuspec` | NuGet package definition for `Ardalis.CleanArchitecture.Template` |
| `MinimalClean/` | Source of the **Minimal Clean** template (`min-clean`), with its own `.template.config/` and `MinimalClean.Architecture.slnx` |
| `MinimalClean.nuspec` | NuGet package definition for `Ardalis.MinimalClean.Template` |
| `README.template.md`, `MinimalClean/README.template.md` | Become the `README.md` of generated solutions |
| `sample/` | **NimblePros.SampleToDo**, a fuller sample app (not part of either template) |
| `docs/` | This documentation site (Hugo) |
| `.github/workflows/` | CI and publishing workflows |

## Building and testing the repository

Prerequisites: the .NET SDK from `global.json` (currently .NET 11) and Docker (for the Aspire hosts, and so the functional tests run against SQL Server rather than the SQLite fallback).

```bash
# Full template
dotnet build Clean.Architecture.slnx
dotnet test Clean.Architecture.slnx

# Minimal template
dotnet build MinimalClean/MinimalClean.Architecture.slnx

# Sample application
dotnet build sample/NimblePros.SampleToDo.slnx
dotnet test sample/NimblePros.SampleToDo.slnx
```

Warnings are treated as errors in every solution, so a clean build means zero warnings. See [Building & Testing](testing.md) for details on the test projects, Docker, and parallel execution.

## Working on the templates

### How generation works

- `sourceName` in `template.json` (`Clean.Architecture` or `MinimalClean.Architecture`) is replaced with the name the user gives to `dotnet new`, in file contents and in file and folder names. A derived `safeSourceName` symbol applies the `safe_namespace` transform, so names like `My-App` become valid namespaces (`My_App`).
- All text files are processed for replacements, except the `copyOnly` patterns (`*.dll`, `*.png`, `*.ico`, `*.jpg`, `*.jpeg`, `*.ps1`), which are copied as-is.
- `README.template.md` is renamed to `README.md` in the generated output.
- The `exclude` list in each `template.json` keeps tool and IDE folders, the other parts of the repository (`docs`, `sample`, `MinimalClean`), and repository-only files (such as `CONTRIBUTING.md`, `CODE_OF_CONDUCT.md`, and `icon.png`) out of generated solutions. **Anything not excluded is copied**, so when you add a file to the repository root, decide whether new solutions should get it, and add it to the `exclude` list in `.template.config/template.json` if not.
- `CleanArchitecture.nuspec` packs the repository root, minus `MinimalClean`, `docs`, `sample`, `.claude`, and build output. It must not include `MinimalClean`: that folder has its own `.template.config`, so including it would make `Ardalis.CleanArchitecture.Template` install the `min-clean` template too. `MinimalClean.nuspec` packs only the `MinimalClean` folder (plus `icon.png` for the package icon).

### Testing template changes locally

You can install a template straight from your working folder:

```bash
dotnet new install .                 # from the repository root: installs both templates
dotnet new install ./MinimalClean    # minimal template only

dotnet new clean-arch -o ../TryIt/My.App
dotnet build ../TryIt/My.App

dotnet new uninstall .               # uninstall by the same path
```

A folder install uses whatever is in the folder, including untracked files and build output the template doesn't exclude. So test from a clean clone, or test the actual package the way CI builds it:

```bash
nuget pack CleanArchitecture.nuspec -NoDefaultExcludes -OutputDirectory artifacts
dotnet new install artifacts/Ardalis.CleanArchitecture.Template.<version>.nupkg
```

To keep experiments out of your real template list, add `--debug:custom-hive <folder>` to each `dotnet new` command. That uses a separate, throwaway template cache.

Generate into a folder **outside** the repository (as in the example above), so test output never ends up in a commit or in the next folder install.

## Contributing to the docs

The docs are a [Hugo](https://gohugo.io/) site in the [`docs`](https://github.com/ardalis/CleanArchitecture/tree/main/docs) folder, published to [cleanarchitecture.ardalis.com](https://cleanarchitecture.ardalis.com) by Cloudflare Pages when changes merge to `main`. Every page has an **Edit this page** link at the bottom.

- Pages are Markdown files under `docs/content/`, each with front matter (`title`, and `weight` to set the order in the menu). A folder with an `_index.md` becomes a menu section.
- Link to other pages with relative Markdown links to the `.md` file, for example `[Versions](versions.md)` or `[Choosing a Template](templates/_index.md)`. These work on GitHub too, and **the build fails if a link points to a page that doesn't exist**.
- Use GitHub-style callouts (`> [!NOTE]`, `> [!IMPORTANT]`, `> [!WARNING]`) and ```` ```mermaid ```` code blocks for diagrams.
- When you add a new major template version, add a migration guide under `docs/content/migration-guides/` and update [Versions](versions.md).

[`docs/README.md`](https://github.com/ardalis/CleanArchitecture/blob/main/docs/README.md) explains how to preview the site locally and how the Cloudflare Pages deployment is configured.

## Continuous integration

Workflows in `.github/workflows/`:

| Workflow | Runs when | What it does |
|---|---|---|
| `cross-platform-build-test.yml` | Every pull request; pushes that change the templates, sample, or build files | Builds `Clean.Architecture.slnx` and runs the unit, integration, and functional tests on Windows, Linux, and macOS, uploading test results and coverage. On Ubuntu it also runs the SQL Server migration test with `REQUIRE_SQL_SERVER_TESTS=true`, so it can't be skipped. A separate Ubuntu job builds and tests the sample. These are required checks on `main`. |
| `cross-platform-build-test-minimal.yml` | Changes under `MinimalClean/` | Builds the minimal template on Windows, Linux, and macOS |
| `docs-build.yml` | Changes under `docs/` | Builds the docs site, failing on errors or broken internal links |
| `hugo-docs.yml` | Changes to the workflow file on `main`, or manually | Publishes the redirect from the old `ardalis.github.io/CleanArchitecture` docs to `cleanarchitecture.ardalis.com` |
| `codeql-analysis.yml` | Monthly, or manually | CodeQL security analysis of the full template |
| `publish.yml` | GitHub release published, or a `v*` tag pushed | Builds, tests, packs, and publishes `Ardalis.CleanArchitecture.Template` |
| `publish-templates.yml` | GitHub release published, or manually | Builds and tests both templates, then packs and publishes whichever are selected |
| `repro-bug.yml` | A pull request from this repository is labeled `bug`, or manually | Runs the app and records a Playwright video for bug reproduction |

## Releasing (maintainers)

1. Update `<version>` and `<releaseNotes>` in **both** `CleanArchitecture.nuspec` and `MinimalClean.nuspec`. Since 12.0.0 the two templates share one version number. Remember that template versions are independent of .NET versions (see [Versions](versions.md)).
2. For a new major version, add a migration guide and update the Versions page.
3. Publish a **GitHub Release** tagged `vX.Y.Z`. Its `release: published` event makes **Publish Templates to NuGet** pack both `.nuspec` files with `nuget pack` and push them to NuGet.org using NuGet trusted publishing (OIDC; the `NUGET_USER` secret holds the NuGet account name). Pushing only a tag publishes only the full template, through **Publish to Nuget**. To publish just one template, run **Publish Templates to NuGet** manually and tick the template to publish.

## Sponsor the project

If you don't have the time or expertise to contribute code, you can still support the project by [sponsoring](https://github.com/sponsors/ardalis).
