# Local data seed

Mock data for **local development only**, written as **repeatable Flyway
migrations** (`R__<description>.sql`).

Locally, Flyway scans the whole `database/` folder, so these run **after** the
schema migrations in [`../migrations`](../migrations). In CI and production Flyway
is pointed at `../migrations` only, so nothing here is ever applied.

Repeatable migrations re-run whenever their checksum changes, so keep them
idempotent (e.g. `INSERT … ON CONFLICT DO NOTHING`).
