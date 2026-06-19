# Migrations

Versioned **Flyway** migrations that define the database schema. Applied in
**every** environment — local, CI, and production.

Name them `V<n>__<description>.sql` — applied once, in order. (Repeatable
migrations for mock data live in [`../local-data-seed`](../local-data-seed).)

This folder ships **empty** — designing the schema is part of the challenge. Apply
the migrations with `make migrate` (or automatically via `make dev`).
