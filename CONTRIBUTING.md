# Contributing to Ardalis.CleanArchitecture

We love your input! We want to make contributing to this project as easy and transparent as possible, whether it's:

- Reporting a bug
- Discussing the current state of the code
- Submitting a fix
- Proposing new features

## We Develop with GitHub

Obviously...

## We Use Pull Requests

Mostly. But pretty much exclusively for non-maintainers. You'll need to fork the repo in order to submit a pull request. Here are the basic steps:

1. Fork the repo and create your branch from `main`.
2. If you've added code that should be tested, add tests.
3. If you've changed APIs, update the documentation.
4. Ensure the test suite passes.
5. Make sure your code lints.
6. Issue that pull request!

- [Pull Request Check List](https://ardalis.com/github-pull-request-checklist/)
- [Resync your fork with this upstream repo](https://ardalis.com/syncing-a-fork-of-a-github-repository-with-upstream/)

## Ask before adding a pull request

You can just add a pull request out of the blue if you want, but it's much better etiquette (and more likely to be accepted) if you open a new issue or comment in an existing issue stating you'd like to make a pull request.

## Getting Started

Look for [issues marked with 'help wanted'](https://github.com/ardalis/CleanArchitecture/issues?q=is%3Aissue+is%3Aopen+label%3A%22help+wanted%22) to find good places to start contributing.

## Any contributions you make will be under the MIT Software License

In short, when you submit code changes, your submissions are understood to be under the same [MIT License](http://choosealicense.com/licenses/mit/) that covers this project.

## Report bugs using Github's [issues](https://github.com/ardalis/CleanArchitecture/issues)

We use GitHub issues to track public bugs. Report a bug by [opening a new issue](https://github.com/ardalis/CleanArchitecture/issues/new/choose); it's that easy!

## Contributing to Documentation

The project docs are a [Hugo](https://gohugo.io/) site located in the `/docs` folder and are published to [ardalis.github.io/CleanArchitecture](https://ardalis.github.io/CleanArchitecture/) automatically when changes to `docs/**` are merged to `main`.

### Adding or editing a page

All content lives under `docs/content/`. Each file is standard Markdown with a small YAML front matter block at the top:

```markdown
---
title: "My New Page"
weight: 25
---

Page content here.
```

- `title` — the label shown in the left navigation.
- `weight` — controls the sort order; lower numbers appear higher in the nav.

To add a **new section** (a collapsible group in the nav), create a subdirectory and add an `_index.md` inside it using the same front matter pattern.

Existing content sections for reference:

| Path | Purpose |
| --- | --- |
| `docs/content/getting-started.md` | Installation and first steps |
| `docs/content/design-decisions.md` | Goals and design rationale |
| `docs/content/minimal-clean-architecture.md` | Minimal template overview |
| `docs/content/architecture-decisions/` | ADR records |
| `docs/content/migration-guides/` | Version upgrade guides |

### Building and previewing docs locally

**Prerequisites:** [Hugo extended](https://gohugo.io/installation/) v0.158 or higher.

1. Clone the theme alongside the docs (mirrors what CI does):

   ```bash
   git clone https://github.com/alex-shpak/hugo-book docs/themes/hugo-book
   ```

2. Start the local dev server from the `docs/` directory:

   ```bash
   cd docs
   hugo server --minify
   ```

3. Open [http://localhost:1313/CleanArchitecture/](http://localhost:1313/CleanArchitecture/) in your browser. The server hot-reloads on file saves.

The generated `docs/public/` output directory is gitignored — do not commit it.

## Sponsor us

If you don't have the time or expertise to contribute code, you can still support us by [sponsoring](https://github.com/sponsors/ardalis).
