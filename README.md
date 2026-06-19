# Full-Stack Pokédex Code Challenge

## Overview

A take-home that assesses senior-level, full-stack engineering across the whole
delivery lifecycle: a **React + TypeScript** front end, a **.NET + C#** API, a
**PostgreSQL** database, and the **infrastructure and pipeline** to test and ship
it on **AWS**.

We're evaluating **judgment and ownership**, not the ability to follow a spec.
This brief deliberately states *what* we're looking for and leaves the *how* —
architecture, tooling, trade-offs — to you. A few well-reasoned, well-tested,
production-minded changes tell us far more than ticking off every box.

## What's provided

A small, runnable baseline — the rest is yours to build:

- A working `GET /api/pokemon` endpoint that proxies and caches the
  [PokéAPI](https://pokeapi.co/) list.
- A minimal React app (list + detail views) wired to that API.
- PostgreSQL and **Flyway** wired into the stack — but with **no schema**.
- `make dev` runs it locally — PostgreSQL (with migrations) in Docker, the API and UI natively.

It is intentionally minimal and a little rough around the edges. Applying good
structure, RESTful conventions, and consistent UI patterns as you extend it — and
tidying the rough edges you come across — is part of the exercise.

## Getting started

```bash
make dev
```

`make dev` runs **PostgreSQL and the migrations in Docker**, then runs the **API
(`dotnet`) and UI (`npm`) natively**, streaming both. Stop everything with Ctrl+C.

- **UI:** http://localhost:3000
- **API:** http://localhost:5001/api/pokemon

Prefer to run the pieces yourself (e.g. in separate terminals)?

```bash
make db        # PostgreSQL (Docker)
make migrate   # apply Flyway migrations + local seed (Docker; a no-op until you add some)
make server    # API on :5001 (dotnet)
make ui        # UI on :3000 (npm)
```

Requirements: Node.js 22, .NET 10 SDK, and Docker (for the database). Versions are
pinned in `.nvmrc`, `.tool-versions`, and `server/global.json`.

## Functional requirements

Where the app needs to get to, described as outcomes — the *how* (data model,
endpoints, components, state) is yours to decide.

- **Browse the Pokédex.** View Pokémon as a list / grid.
- **View a Pokémon.** Open one and see its details — sprite, types, base stats,
  abilities, and so on.
- **Favourites.** Mark and unmark Pokémon as favourites and view the current user's
  favourites; they persist and are scoped to the user (see
  [Authentication](#authentication)).
- **Compare Pokémon.** Pick two or more and compare them side by side (e.g. base
  stats and types).
- **Find Pokémon.** Search by name and / or filter by type.

Data comes from the [PokéAPI](https://pokeapi.co/) and the database you design.
The starter already does a basic browse + detail view — extend from there.

## Engineering goals

How well you build the above is what we're assessing. Treat these as outcomes, not
instructions; scope to the time agreed with your reviewer — **depth beats breadth**.

- **Designed database (Flyway).** Model a sensible schema and evolve it with
  versioned Flyway migrations in [`database/migrations`](database/migrations)
  (applied in every environment). Put mock data for local development in
  [`database/local-data-seed`](database/local-data-seed) — the local stack loads
  it; CI and production never do.
- **RESTful API.** Expose the functionality above through an API that follows REST
  best practices — sensible resource naming, correct status codes, consistent error
  shapes (RFC 7807), input validation, and a clean separation of concerns.
- **UI with a clear structure.** Deliver the features in the React app with a
  **clear folder structure and consistent conventions**, a typed data flow, and
  reasonable UX.
- **Tests.** Meaningful automated tests at both the **unit** and **integration**
  levels, on **server and UI** — set up the tooling and write the tests.
- **CI/CD.** Wire build, tests, and deployment into **GitHub Actions**.
- **Infrastructure (IaC).** Describe the cloud infrastructure as code with
  **Terraform** and deploy/host the application on **AWS**.

You are not expected to complete everything. Tell us what you prioritised, what
you'd do next, and the trade-offs you made.

## Stretch goals

We don't spell these out as requirements — recognising they're needed and adding
them is part of the signal:

- **Pagination / infinite scroll** — the Pokédex is large; don't fetch it all at once.
- **Caching** — a considered strategy (the starter only caches the list endpoint);
  avoid hammering PokéAPI and keep the UI responsive.
- **Sorting and richer search / filtering.**

And, as time allows:

- End-to-end (Playwright) and load (k6) tests.
- Observability — structured logging, health / readiness endpoints, metrics.
- Resilience — timeouts / retries, rate limiting.
- Accessibility and responsive polish.
- Multi-environment IaC, zero-downtime deploys / rollback, deeper secrets handling.

## Authentication

The favourites capability identifies a user with a **static Bearer token** — there
is no login or JWT validation, just a string that scopes a user's data.

- The token is defined in `ui/.env` as `VITE_SESSION_TOKEN` and sent as
  `Authorization: Bearer <token>` (the `apiFetch` helper in
  `ui/src/api/client.ts` already attaches it).
- The API should read the raw token from the header and scope favourites to it —
  no cryptographic validation required.

## API reference

The one provided endpoint, as a reference for the response/error style:

```
GET /api/pokemon  →  200 OK
{
  "count": 1302, "offset": 0, "limit": 20,
  "results": [ { "name": "bulbasaur", "url": "https://pokeapi.co/api/v2/pokemon/1/" } ]
}
```

Errors use RFC 7807 Problem Details (`application/problem+json`):

```json
{ "type": "...", "title": "Bad Gateway", "status": 502, "detail": "..." }
```

## Project structure

```
├── server/                 # .NET 10 Web API
│   └── Pokedex.Api/
│       ├── Controllers/    # API endpoints (provided: GET /api/pokemon)
│       ├── Services/       # PokéAPI proxy with caching
│       ├── Models/         # DTOs
│       ├── Data/           # DbContext (database wired; no entities yet)
│       └── Program.cs
├── ui/                     # React + TypeScript + Vite + TanStack Query
│   └── src/                # api client, hooks, components, pages, types
├── database/               # Flyway migrations + local mock-data seed
├── docker-compose.yml      # PostgreSQL + Flyway (local dev only)
├── Makefile
└── README.md
```

## Technical expectations

- Use modern, idiomatic patterns for both frameworks.
- Keep components, services, and modules focused and well-structured.
- Handle errors and edge cases reasonably.
- Make deliberate choices and be ready to explain them.

## Submission

Please share:

- A link to your repository.
- Brief notes on your approach: what you prioritised, key trade-offs, and what
  you'd do next with more time.
