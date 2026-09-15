---
name: suggest-migration-name
description: Suggest a name for a new EF Core migration based on the pending model changes. Use when the user asks "what should I name this migration", "suggest a migration name", or is about to run `dotnet ef migrations add`. Does NOT run `dotnet ef migrations add` — only proposes a name.
---

# Suggest migration name

Propose a name for the next EF Core migration in this repo. Do not run `dotnet ef migrations add`; only suggest the name string.

## Steps

1. **Locate the migrations folder and existing names.** Run in parallel:
   - `ls Infrastructure/Database/Migrations/` — see existing migration files and confirm the folder path
   - `git log --oneline -20` — check recent commit style for the same change, if any

2. **Detect the pending model change.** Run in parallel:
   - `git status` — see modified/added entity, configuration, and `DbContext` files under `Domain/`, `Application/`, `Infrastructure/Database/`
   - `git diff` on the changed entity/configuration files — understand *what* schema move this migration will encode (new table, added column, renamed column, new index, FK, seed, drop, etc.)

   If the working tree has no schema-relevant changes, tell the user "no pending model changes I can see — stage or make the entity/configuration change first" and stop.

3. **Classify the change.** Pick exactly one `<Action>`:
   - `Init` — first migration introducing an aggregate/table cluster
   - `Add<Thing>` — new column, table, index, FK, constraint, seed row (`AddDepartmentColumn`, `AddStatusIndex`, `AddOrdersTable`)
   - `Remove<Thing>` — drop column/table/index/constraint (`RemoveLegacyEmailColumn`)
   - `Rename<Old>To<New>` — rename of column/table (`RenameUserNameToDisplayName`)
   - `Alter<Thing>` — type/nullability/length change on an existing column (`AlterEmailMaxLength`)
   - `Seed<Thing>` — data-only migration inserting reference/lookup rows (`SeedDepartments`)
   - `Backfill<Thing>` — data migration filling values into an existing column
   - `Drop<Thing>` — full-table drop (reserve for destructive moves; call it out explicitly)

   The very first migration in the repo is named `Initial` and has no aggregate prefix — do not reuse that name.

4. **Pick the aggregate prefix.** Use the aggregate/module the change targets, in PascalCase singular-or-plural matching existing usage:
   - Existing seen so far: `Users`
   - Cross-cutting infra (rare — schema, extensions, functions): no prefix, use a bare `<Action>` like `AddSchemaAuditFunctions`
   - If the migration touches multiple aggregates, prefer the dominant one and mention the others in the reasoning; if it's genuinely cross-cutting, suggest splitting into separate migrations before adding.

5. **Assemble the name.** Format: `<Aggregate>_<Action>` in PascalCase, one underscore separator only.
   - Good: `Users_Init`, `Users_AddDepartment`, `Users_RenameUserNameToDisplayName`, `Orders_AddStatusIndex`
   - Bad: `users_add_department` (snake_case), `AddUserDepartment` (missing aggregate), `Users_Add_Department` (extra underscore), `20260915_UsersAddDepartment` (EF adds the timestamp)
   - Under ~60 chars total. If longer, shorten the `<Action>` — the diff carries the detail, the name is a label.

6. **Detect destructive/data-loss migrations.**
   - Dropping a column/table, tightening nullability, shortening a `MaxLength`, changing a PK, or a `Backfill`/`Drop` action → flag as destructive.
   - Suggest the user double-check the generated `Up`/`Down` methods and, for prod-facing schemas, split into a two-phase migration (add-nullable → backfill → tighten) before applying.

7. **Present the suggestion.** Output format:

   ```
   Suggested migration name:

   <Aggregate>_<Action>

   Full command: dotnet ef migrations add <Aggregate>_<Action> --project Infrastructure --startup-project API

   Reasoning: <one sentence on what schema change this encodes and why this action verb>
   ```

   Then, if useful, offer 1–2 alternatives (e.g. a narrower/broader action, or splitting into two migrations).

8. **Do not run `dotnet ef migrations add`.** The user reviews the suggestion and runs it themselves. If they explicitly say "add it" or "run it", then do it — otherwise stop after suggesting.

## Rules

- Use ONLY the action verbs listed above: `Init`, `Add`, `Remove`, `Rename`, `Alter`, `Seed`, `Backfill`, `Drop`. No custom verbs like `Update`, `Fix`, `Change` — they're too vague to read six months later.
- Never invent an aggregate prefix that doesn't match an existing folder under `Domain/` or an existing migration prefix. Prefer the closest existing one.
- One migration = one logical change. If the pending diff mixes (e.g.) an `Add` and a `Rename`, flag it and suggest splitting into two migrations.
- Do not include the timestamp in the suggested name — EF prepends it automatically.
- If the entity being touched is brand new and it's the first migration for that aggregate, prefer `<Aggregate>_Init` over `<Aggregate>_Add<Entity>Table`.

## Examples

Pending: new `DepartmentEntity` + configuration + `DbSet<DepartmentEntity>` added to context.
→ `Departments_Init`

Pending: `Department` property added to `RegularUserEntity`; configuration updated with `HasConversion`.
→ `Users_AddDepartment`

Pending: `UserEntity.UserName` renamed to `DisplayName` across entity + configuration.
→ `Users_RenameUserNameToDisplayName`

Pending: new `IX_Users_Email` unique index added in `UserEntityConfiguration`.
→ `Users_AddEmailUniqueIndex`

Pending: `UserEntity.Bio` column removed.
→ `Users_RemoveBio` (flag as destructive; check `Down` restores it)

Pending: seed rows for `UserDepartment` reference table.
→ `Departments_SeedDefaults`
