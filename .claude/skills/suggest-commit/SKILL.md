---
name: suggest-commit
description: Suggest a Conventional Commits message for the currently staged changes. Use when the user asks for a commit name, commit message, or "what should I commit this as". Reads staged files and diff, proposes one message following the project convention. Does NOT create the commit — only proposes.
---

# Suggest commit message

Propose a Conventional Commits message for the currently staged changes. Do not run `git commit`; only suggest.

## Steps

1. **Check what is staged.** Run these in parallel:
   - `git diff --cached --stat` — see which files changed and rough size
   - `git diff --cached` — see the actual changes (may be large; if over ~500 lines, also read the top of `git diff --cached --name-status` and sample a few hunks)
   - `git log --oneline -20` — check recent commit style in this repo for consistency

2. **If nothing is staged**, tell the user "no staged changes — run `git add` first" and stop.

3. **Analyze the diff and classify.** Pick exactly one `type`:
   - `feat` — user-observable new capability (endpoint, config option, feature)
   - `fix` — bug fix
   - `refactor` — internal reorg, no behavior change
   - `perf` — performance improvement
   - `docs` — documentation only
   - `test` — tests only
   - `build` — `.csproj`, NuGet packages, Dockerfile
   - `ci` — GitHub Actions, pipelines
   - `chore` — routine maintenance that fits none of the above
   - `style` — whitespace/formatting only, no behavior change
   - `revert` — reverts a prior commit

   If the diff spans multiple types (e.g. a feature + unrelated refactor), point that out and suggest splitting into separate commits before committing.

4. **Pick a scope.** Use one of the project's stable scopes:
   - Top-level projects: `api`, `application`, `domain`, `infrastructure`
   - Vertical-slice features (once they exist): `users`, `surveys`, `reports`
   - Cross-cutting: `db`, `migrations`, `config`, `deps`

   Omit the scope only if the change is truly global (e.g. `.gitignore`, `README`, tooling at repo root).

5. **Write the description.**
   - Imperative mood: "add", "fix", "move" — NOT "added", "adds", "adding"
   - Lowercase, no trailing period
   - Under 72 chars total including `type(scope): `
   - Say *what* changed, not *why* (why goes in the body)

6. **Detect breaking changes.**
   - Public API signature change, renamed exported type, removed public method, changed HTTP response contract, migration that drops columns → BREAKING.
   - If breaking, use `type(scope)!: description` and add a `BREAKING CHANGE:` footer explaining the migration.

7. **Optional body.** Include a body only if the *why* is non-obvious from the diff. Wrap at 100 chars. Skip it for small self-explanatory changes.

8. **Present the suggestion.** Output format:

   ```
   Suggested commit:

   <the full commit message in a code block>

   Reasoning: <one sentence on why this type/scope was chosen>
   ```

   Then, if useful, offer 1–2 alternatives (e.g. different type or split-commit plan).

9. **Do not run `git commit`.** The user reviews the suggestion and commits themselves. If they explicitly say "commit it" or "yes commit", then do it — otherwise stop after suggesting.

## Rules

- Use ONLY these types: `feat`, `fix`, `docs`, `refactor`, `perf`, `test`, `build`, `ci`, `chore`, `style`, `revert`. No custom types.
- Never invent a scope not seen elsewhere in the repo. Prefer the closest existing one over a new one.
- If in doubt between `feat` and `refactor`: does a user (of the app or of the library) observe anything different? Yes → `feat`. No → `refactor`.
- If in doubt between `chore` and `build`: does it touch NuGet/`.csproj`/Dockerfile? → `build`. Otherwise → `chore`.
- If the diff is trivial (< 5 lines, single file), a scope may be overkill — use judgment.
- If the diff bundles clearly unrelated changes, flag it and suggest `git reset` + selective `git add` to split into separate commits.

## Examples

Staged: added `DatabaseOptions.cs`, wired it into `Extensions.cs`, replaced hardcoded connection string.
→ `refactor(infrastructure): replace hardcoded connection string with Options pattern`

Staged: `API.csproj` gains `Microsoft.Extensions.Options.DataAnnotations` package reference only.
→ `build: add Microsoft.Extensions.Options.DataAnnotations package`

Staged: new `Infrastructure/Migrations/20260902_Init.cs` and `AppDbContextModelSnapshot.cs`.
→ `feat(db): add initial migration for TestEntity`

Staged: fixed `.env` load order in `Program.cs` — was called after `CreateBuilder`, so env vars weren't reaching config.
→ `fix(api): load .env before CreateBuilder so env vars reach config`

Staged: renamed a public class and updated all callers.
→ `refactor(domain)!: rename User to Account` (with BREAKING CHANGE footer if this is a published API)