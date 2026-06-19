# Server (.NET API)

ASP.NET Core (.NET 10) Web API for the Pokédex. It proxies the
[PokéAPI](https://pokeapi.co/) and is wired to PostgreSQL via EF Core — but the
schema itself is owned by **Flyway** in [`../database`](../database).

## Run

```bash
# from the repo root
make server          # API on :5001 (needs PostgreSQL — see `make db`)
# or, equivalently:
cd server && ASPNETCORE_URLS=http://localhost:5001 dotnet run --project Pokedex.Api
```

The full stack (database + API + UI) runs via `make dev` from the repo root.

## Layout

- `Pokedex.Api/Controllers` — HTTP endpoints (provided: `GET /api/pokemon`).
- `Pokedex.Api/Services` — the PokéAPI client + in-memory caching.
- `Pokedex.Api/Models` — DTOs.
- `Pokedex.Api/Data` — `PokedexDbContext` (the database connection is wired; add
  entities as you design the schema).

## Notes

- Errors follow RFC 7807 Problem Details (`application/problem+json`).
- The database ships with no schema — add Flyway migrations under
  [`../database/migrations`](../database/migrations).
- Automated tests are expected but not pre-configured; choose your tooling and add
  them.
