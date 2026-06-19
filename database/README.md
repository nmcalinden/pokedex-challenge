# Database

The schema is managed with **[Flyway](https://flyway.org/)**, in two folders:

- **[`migrations/`](migrations)** — versioned schema migrations (`V1__…`, `V2__…`),
  applied in **every** environment.
- **[`local-data-seed/`](local-data-seed)** — repeatable Flyway migrations
  (`R__…`) holding mock data for **local development only**.

Locally, Flyway scans the **whole `database/` folder**, so one run applies the
schema migrations and then the local seed — `make migrate` (and `make dev`) do
exactly this. In **CI and production**, point Flyway at **`database/migrations`
only**, so the local seed is never applied there.

```bash
make db        # start PostgreSQL (Docker)
make migrate   # apply migrations + local seed (a no-op until you add some)
```

## Conventions

- **Versioned** (`V<n>__<description>.sql`) — applied once, in order; live in
  `migrations/`.
- **Repeatable** (`R__<description>.sql`) — re-applied whenever their checksum
  changes; used for the seed in `local-data-seed/`.
