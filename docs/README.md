# Clean Architecture documentation site

Source for **[cleanarchitecture.ardalis.com](https://cleanarchitecture.ardalis.com)**: a [Hugo](https://gohugo.io/) site using the [hugo-book](https://github.com/alex-shpak/hugo-book) theme, deployed by [Cloudflare Pages](https://developers.cloudflare.com/pages/).

```text
docs/
├── content/          Markdown pages (this is what you edit)
├── layouts/          Site-level template overrides (robots.txt)
├── static/           Copied to the site root as-is: _headers, logo.png, favicon.png
├── hugo.toml         Site configuration
├── build.sh          Build script used by Cloudflare Pages, CI, and locally
├── themes/           hugo-book theme, fetched by build.sh (gitignored)
└── public/           Build output (gitignored)
```

## Prerequisites

- **Hugo 0.158.0 or newer** ([install](https://gohugo.io/installation/)). CI and Cloudflare use **0.169.0**. The standard edition is enough; the extended edition isn't required.
- **Git**, which `build.sh` uses to download the theme
- **Bash** to run `build.sh`. On Windows, use Git Bash or WSL, or run the PowerShell commands below.

## Run the docs locally

From the `docs` folder:

```bash
./build.sh        # fetches the pinned theme into themes/hugo-book, then does a full build
hugo server       # live-reloading preview
```

Open <http://localhost:1313/>. The server rebuilds and refreshes the browser when you save a file.

You only need to run `build.sh` once, and again after the pinned theme version changes. On Windows, run it from Git Bash (installed with Git for Windows) or WSL. Without Bash, fetch the theme commit that `HUGO_BOOK_REF` in `build.sh` pins, then start the server:

```powershell
$ref = '7875ae6727c38350ff330d66cb5ee2f70e6cfc54'   # HUGO_BOOK_REF from build.sh
git init themes/hugo-book
git -C themes/hugo-book fetch --depth 1 https://github.com/alex-shpak/hugo-book $ref
git -C themes/hugo-book checkout --detach FETCH_HEAD
hugo server
```

`hugo server` shows drafts only if you pass `-D`. Pages don't use drafts by default.

## Production build

```bash
./build.sh
```

This writes the site to `docs/public/`, minified, using `baseURL = 'https://cleanarchitecture.ardalis.com/'` from `hugo.toml`. To build for another URL, set `BASE_URL` (for example `BASE_URL=http://localhost:8080/ ./build.sh`). Any extra arguments are passed to Hugo.

`build.sh` reads these environment variables, all optional:

| Variable | Default | Purpose |
|---|---|---|
| `HUGO_BOOK_REF` | The commit pinned in `build.sh` | hugo-book commit or tag to fetch |
| `BASE_URL` | Unset | Overrides the site URL for every build |
| `PRODUCTION_BRANCH` | `main` | Cloudflare builds of any other branch use the preview URL as the base URL |

The build **fails** if a page links to another page that doesn't exist. Internal links are plain relative Markdown links (`[Versions](versions.md)`), checked by hugo-book's `BookPortableLinks = 'error'` setting.

## Writing pages

- Each page is a Markdown file in `content/` with front matter:

  ```markdown
  ---
  title: "My New Page"
  weight: 45
  ---
  ```

  `title` is the label in the left menu. `weight` sets the order (lower numbers first). Top-level pages use weights 10 to 110; check the neighbors when you add one.
- A folder with an `_index.md` is a menu section (for example `content/migration-guides/`).
- Link to other pages by relative path to the `.md` file. Those links also work when browsing the docs on GitHub.
- Callouts use GitHub alert syntax: `> [!NOTE]`, `> [!TIP]`, `> [!IMPORTANT]`, `> [!WARNING]`.
- Diagrams use ```` ```mermaid ```` code blocks.
- The theme's shortcodes (tabs, columns, details, buttons, ...) are documented in the [hugo-book README](https://github.com/alex-shpak/hugo-book#shortcodes).

The site's [Contributing](https://cleanarchitecture.ardalis.com/contributing/#contributing-to-the-docs) page has more guidance.

## Deploying to Cloudflare Pages

The site deploys through Cloudflare Pages' **Git integration**. Every push to `main` builds and publishes production, and every other branch gets a preview deployment.

### One-time project setup

1. In the Cloudflare dashboard, go to **Workers & Pages → Create → Pages → Connect to Git**, and select the `ardalis/CleanArchitecture` repository.
2. Configure the build:

   | Setting | Value |
   |---|---|
   | Production branch | `main` |
   | Framework preset | `None` (the build command below does everything) |
   | Build command | `bash build.sh` |
   | Build output directory | `public` |
   | Root directory (advanced) | `docs` |

3. Add an **environment variable** for both the **Production** and **Preview** environments:

   | Variable | Value |
   |---|---|
   | `HUGO_VERSION` | `0.169.0` |

   This is required. Cloudflare's default Hugo (0.147.x in build image v3) is older than the 0.158.0 the theme needs, and the build fails with a Hugo version error without it. Keep this value in sync with `HUGO_VERSION` in `.github/workflows/docs-build.yml`.
   (If the Cloudflare build log shows an older `hugo version`, this variable is missing.)
4. Save and deploy. The first deployment is served at `https://<project>.pages.dev`.
5. Optional: under **Settings → Builds → Build watch paths**, set the include path to `docs/*` so commits that only touch the templates' code don't trigger a docs build.

### Custom domain: cleanarchitecture.ardalis.com

In the Pages project, go to **Custom domains → Set up a custom domain** and enter `cleanarchitecture.ardalis.com`.

- If the `ardalis.com` DNS zone is on Cloudflare, Cloudflare creates the DNS record for you.
- If it isn't, add the domain in the Pages dashboard **first**, then create a `CNAME` record at your DNS provider: `cleanarchitecture` → `<project>.pages.dev`. Creating the CNAME before adding the domain in the dashboard results in 522 errors.

Cloudflare issues the TLS certificate automatically once the record resolves.

### Preview deployments

Cloudflare builds every non-production branch pushed to this repository at `https://<branch>.<project>.pages.dev` (and each deployment at a unique `https://<hash>.<project>.pages.dev` URL), and links the preview from the Cloudflare check on the pull request. `build.sh` detects these builds through the `CF_PAGES_URL` and `CF_PAGES_BRANCH` variables Cloudflare provides, and sets Hugo's `baseURL` to the preview URL so links work there. If you rename the production branch, set a `PRODUCTION_BRANCH` environment variable to match. Pull requests from forks don't get preview deployments.

`static/_headers` adds `X-Robots-Tag: noindex` to all `*.pages.dev` URLs, so only the custom domain is indexed by search engines. It also sets a few basic security headers for every page.

The **Docs** GitHub Actions workflow (`.github/workflows/docs-build.yml`) also builds the site for every pull request that touches `docs/`, including pull requests from forks, and for pushes to `main`. Broken builds and broken links fail that check.

### The old GitHub Pages URL

The docs used to be published to `https://ardalis.github.io/CleanArchitecture/`. The **Deploy docs redirect to GitHub Pages** workflow (`.github/workflows/hugo-docs.yml`) now publishes a small redirect page there instead, so every old link, including deep links such as `/CleanArchitecture/migration-guides/`, forwards to the same path on `cleanarchitecture.ardalis.com`. It runs when that workflow file changes on `main`, or manually from the Actions tab. Leave the repository's **Settings → Pages** source set to **GitHub Actions** so the redirect stays published.

### Alternative: deploy from GitHub Actions

If you'd rather build in GitHub Actions and upload the result, create the Pages project as a **Direct Upload** project, add `CLOUDFLARE_API_TOKEN` (with the *Cloudflare Pages: Edit* permission) and `CLOUDFLARE_ACCOUNT_ID` as repository secrets, and add a deploy step after the build in `docs-build.yml`:

```yaml
      - name: Deploy to Cloudflare Pages
        if: github.event_name == 'push' && github.ref == 'refs/heads/main'
        uses: cloudflare/wrangler-action@v4
        with:
          apiToken: ${{ secrets.CLOUDFLARE_API_TOKEN }}
          accountId: ${{ secrets.CLOUDFLARE_ACCOUNT_ID }}
          command: pages deploy docs/public --project-name=<project>
```

The `CF_PAGES_*` variables don't exist in GitHub Actions, so these builds always use the production `baseURL`.

> **Pages or Workers?** Cloudflare now suggests Workers static assets for *new* projects. Pages is still fully supported, and it can attach a custom domain whose DNS zone isn't on Cloudflare, which Workers can't. The build output in `public/` would work unchanged if the site ever moves to Workers.

## Upgrading Hugo or the theme

- **Hugo**: change `HUGO_VERSION` in the Cloudflare project's environment variables *and* in `.github/workflows/docs-build.yml`.
- **Theme**: change `HUGO_BOOK_REF` in `build.sh` (and in the PowerShell commands above), then run `./build.sh`, which fetches the new commit. Check the [theme's commits and releases](https://github.com/alex-shpak/hugo-book/releases) first: it's versioned 0.x, and changes can break the site or raise the minimum Hugo version (`min_version` in the theme's `theme.toml`).

## Troubleshooting

| Problem | Fix |
|---|---|
| `module "hugo-book" is not compatible with this Hugo version` | Hugo is older than the theme requires. Install a newer Hugo, or set `HUGO_VERSION` on Cloudflare. |
| `found no layout file for "html" for kind "home"` or similar | The theme is missing. Run `./build.sh` (or fetch the theme as shown above). |
| `can't evaluate field Locale in type *langs.Language` | Hugo is older than 0.158.0. Upgrade Hugo. |
| `Image reference '...' not found` or a link error naming a page | A relative link points to a page that doesn't exist. Fix the path (relative to the current page, ending in `.md`). |
| Links on a preview deployment point to the production site | The build didn't see `CF_PAGES_URL`. Make sure the build command is `bash build.sh`. |
