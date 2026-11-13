#!/usr/bin/env bash
# Builds the Hugo docs site into ./public.
#
# Used by Cloudflare Pages (root directory: docs, build command: bash build.sh,
# output directory: public), by the Docs GitHub Actions workflow, and locally
# from the docs folder. Extra arguments are passed to hugo, for example:
#   ./build.sh --baseURL http://localhost:8080/
#
# Environment variables:
#   HUGO_BOOK_REF      hugo-book theme commit/tag to use (default: pinned commit below)
#   BASE_URL           Optional override for the site URL. Normally leave unset:
#                      - production (main) builds use baseURL from hugo.toml
#                      - Cloudflare preview builds (other branches) use CF_PAGES_URL,
#                        so canonical links/RSS point at the preview deployment.
#   PRODUCTION_BRANCH  Branch Cloudflare Pages deploys to production (default: main)
set -euo pipefail

cd "$(dirname "$0")"

HUGO_BOOK_REF="${HUGO_BOOK_REF:-7875ae6727c38350ff330d66cb5ee2f70e6cfc54}"
THEME_DIR="themes/hugo-book"

if [ ! -d "$THEME_DIR/.git" ]; then
  echo "Fetching hugo-book theme @ $HUGO_BOOK_REF"
  rm -rf "$THEME_DIR"
  git init -q "$THEME_DIR"
  git -C "$THEME_DIR" remote add origin https://github.com/alex-shpak/hugo-book
fi
git -C "$THEME_DIR" fetch -q --depth 1 origin "$HUGO_BOOK_REF"
git -C "$THEME_DIR" checkout -q --detach FETCH_HEAD

hugo version

PRODUCTION_BRANCH="${PRODUCTION_BRANCH:-main}"
HUGO_ARGS=(--minify --gc)
if [ -n "${BASE_URL:-}" ]; then
  HUGO_ARGS+=(--baseURL "$BASE_URL")
elif [ -n "${CF_PAGES_URL:-}" ] && [ "${CF_PAGES_BRANCH:-}" != "$PRODUCTION_BRANCH" ]; then
  echo "Preview build for branch '${CF_PAGES_BRANCH:-}': using base URL ${CF_PAGES_URL%/}/"
  HUGO_ARGS+=(--baseURL "${CF_PAGES_URL%/}/")
fi

hugo "${HUGO_ARGS[@]}" "$@"
