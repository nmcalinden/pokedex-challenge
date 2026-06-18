# Full-Stack Pokédex Code Challenge

## Overview

This challenge evaluates your ability to build and extend a full-stack application using **React with TypeScript** on the front end and **.NET with C#** on the back end. The project is a Pokédex that fetches data from the [PokéAPI](https://pokeapi.co/), displays it through a React UI, and persists user data via a .NET API backed by PostgreSQL.

The initial scaffold includes a working reference implementation — a paginated Pokémon list endpoint that returns the first page of results. You are expected to extend this foundation and implement additional features across both the front end and back end.

**Time box:** You should spend at most **4 hours** on this challenge. There is no requirement to complete every task — focus on solid, well-structured implementations rather than rushing through all of them.

---

## Prerequisites

### Docker Path (Recommended)

You only need **[Docker Desktop](https://www.docker.com/products/docker-desktop/)** installed. All services (database, API, frontend) run in containers.

### Local Development Path

If you prefer running services outside Docker:

- **Node.js 22 LTS** — [Download](https://nodejs.org/)
- **.NET 10 SDK** — [Download](https://dotnet.microsoft.com/download/dotnet/10.0)
- **PostgreSQL 17** — [Download](https://www.postgresql.org/download/)

---

## Quick Start

```bash
# Clone and start everything
git clone <repository-url>
cd <repository-name>
docker compose up --build
```

Once running:
- **Frontend:** [http://localhost:3000](http://localhost:3000)
- **API:** [http://localhost:5001/api/pokemon](http://localhost:5001/api/pokemon)

The database schema is applied automatically when the API starts — no manual migration commands are needed.

### Local Development (Without Docker)

```bash
# Terminal 1: Start PostgreSQL (if not already running)
# Ensure a database named 'pokedex' exists with user/password 'pokedex'

# Terminal 2: Start the API
cd backend
dotnet run --project Pokedex.Api

# Terminal 3: Start the frontend
cd frontend
npm install
npm run dev
```

---

## Architecture

```
┌─────────────┐     ┌──────────────┐     ┌──────────────┐
│   React UI  │────▶│  .NET API    │────▶│   PokéAPI    │
│  :3000      │     │  :5001       │     │  (external)  │
└─────────────┘     └──────┬───────┘     └──────────────┘
                           │
                    ┌──────▼───────┐
                    │  PostgreSQL  │
                    │  :5433       │
                    └──────────────┘
```

- **React frontend** (`frontend/`) — Vite + TypeScript + TanStack React Query
- **.NET API** (`backend/`) — ASP.NET Core Web API + Entity Framework Core
- **PostgreSQL** — Stores user favourites; managed via EF Core Migrations
- **PokéAPI** — External data source for Pokémon information; responses are cached in-memory (5-minute TTL)

---

## Authentication

The favourites feature uses a **static Bearer token** for user identification. There is no login flow or JWT validation — the token is a simple string that scopes database operations to a specific user session.

### How It Works

1. The token value is defined in `frontend/.env`:
   ```
   VITE_SESSION_TOKEN=pokedex-challenge-2027-a1b2c3d4-e5f6-7890-abcd-ef1234567890
   ```

2. The frontend's `apiFetch()` helper (in `src/api/client.ts`) attaches it to every request:
   ```
   Authorization: Bearer pokedex-challenge-2027-a1b2c3d4-e5f6-7890-abcd-ef1234567890
   ```

3. The backend reads the token from the `Authorization` header and uses it to scope all favourites database queries to that token value. The token is stored in the `token` column of the `favourites` table.

**Key details:**
- **Header:** `Authorization`
- **Format:** `Bearer <token>`
- **Token value:** Read from the `VITE_SESSION_TOKEN` environment variable
- **Backend behaviour:** Extract the raw token string from the header — no cryptographic validation required

---

## Your Tasks

### Backend Tasks

#### Backend Task 1: Extend Paginated Pokémon List

**File:** `backend/Pokedex.Api/Controllers/PokemonController.cs`

The existing `GET /api/pokemon` endpoint returns the first page of results (offset=0, limit=20). Extend it to accept `offset` and `limit` query parameters for arbitrary pagination.

- Validate: `offset >= 0`, `1 <= limit <= 100`
- Return appropriate error responses for invalid parameters
- [PokéAPI Pagination Docs](https://pokeapi.co/docs/v2#resource-listspagination-section)

#### Backend Task 2: Implement Favourites API

**File:** `backend/Pokedex.Api/Controllers/FavouritesController.cs`

Implement CRUD endpoints for user favourites, scoped to the authenticated user's token:

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/favourites` | List all favourites for the current user |
| `POST` | `/api/favourites` | Add a favourite (`{ "pokemonId": <int> }`) |
| `DELETE` | `/api/favourites/{pokemonId}` | Remove a favourite |

- Return `401` if the `Authorization` header is missing or malformed
- Return `409` if the Pokémon is already favourited
- Return `404` on DELETE if the favourite doesn't exist
- The `Favourite` entity and database migration are already provided

#### Backend Task 3: Type Statistics Aggregation

**File:** `backend/Pokedex.Api/Controllers/PokemonController.cs`

Add a `GET /api/pokemon/types/stats` endpoint that returns aggregated statistics for each Pokémon type. For each type, return:
- The type name (e.g. "fire", "water")
- The count of Pokémon with that type
- The average value of each base stat (hp, attack, defense, special-attack, special-defense, speed)

- [PokéAPI Pokémon Docs](https://pokeapi.co/docs/v2#pokemon)

#### Backend Task 4: Pokémon Detail Proxy with Species Enrichment

**File:** `backend/Pokedex.Api/Controllers/PokemonController.cs`

Add a `GET /api/pokemon/{id}` endpoint that returns an enriched detail response composed from multiple PokéAPI sources:

1. Core data (name, height, weight, sprites, types, stats, abilities) from `/pokemon/{id}`
2. Species description (English flavour text) from `/pokemon-species/{id}`
3. Evolution chain from the URL in the species response

Consider how to structure your service layer, caching strategy, and graceful degradation for partial upstream failures.

- [PokéAPI Pokémon](https://pokeapi.co/docs/v2#pokemon) · [Species](https://pokeapi.co/docs/v2#pokemon-species) · [Evolution Chains](https://pokeapi.co/docs/v2#evolution-chains)

### Frontend Tasks

#### Frontend Task 1: Add Pagination

**File:** `frontend/src/api/hooks/usePokemonList.ts`

The current hook fetches only the first page. Implement infinite scroll or a "Load More" button using `useInfiniteQuery` from TanStack React Query.

- [TanStack Infinite Queries](https://tanstack.com/query/latest/docs/framework/react/guides/infinite-queries)

#### Frontend Task 2: Style the Detail Page

**File:** `frontend/src/pages/PokemonDetailPage.tsx`

Apply a polished layout with proper spacing, typography, and colours. Show the Pokémon sprite, name, types, and stats in a visually appealing way. Creative freedom is encouraged — no Figma reference is provided. Ensure the layout works on different screen sizes.

#### Frontend Task 3: Add Favouriting

**File:** `frontend/src/pages/FavouritesPage.tsx`

Implement the favourites feature using the API endpoints from Backend Task 2:
- Display favourited Pokémon on the Favourites page
- Add favourite/unfavourite toggles on the list and detail pages
- Use `useMutation` from TanStack React Query with cache invalidation

#### Frontend Task 4: Add Type Filtering

**File:** `frontend/src/pages/PokemonListPage.tsx`

Allow users to filter the Pokémon list by type:
- Fetch types from `GET https://pokeapi.co/api/v2/type`
- When selected, fetch Pokémon of that type
- Allow clearing/changing the filter

- [PokéAPI Types](https://pokeapi.co/docs/v2#types)

---

## API Reference

### Working Endpoint (Reference Implementation)

```
GET /api/pokemon
```

**Response (200 OK):**
```json
{
  "count": 1302,
  "offset": 0,
  "limit": 20,
  "results": [
    { "name": "bulbasaur", "url": "https://pokeapi.co/api/v2/pokemon/1/" },
    { "name": "ivysaur", "url": "https://pokeapi.co/api/v2/pokemon/2/" }
  ]
}
```

### Error Format (RFC 7807 Problem Details)

All API errors return a consistent JSON format:

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.6.3",
  "title": "Bad Gateway",
  "status": 502,
  "detail": "Unable to retrieve Pokémon data from the upstream PokéAPI service."
}
```

This pattern is demonstrated in the reference endpoint — use it consistently for all error responses.

---

## Running Tests

```bash
# Backend (xUnit)
cd backend
dotnet test

# Frontend (Vitest)
cd frontend
npm test
```

Both projects have a configured test runner with at least one passing example test. Use these as a starting point for your own tests.

---

## Project Structure

```
├── backend/
│   ├── Pokedex.Api/              # .NET 10 Web API
│   │   ├── Controllers/          # API endpoints
│   │   ├── Services/             # PokéAPI proxy with caching
│   │   ├── Models/               # DTOs and EF Core entities
│   │   ├── Data/                 # DbContext configuration
│   │   ├── Migrations/           # EF Core database migrations
│   │   └── Program.cs            # Application entry point
│   ├── Pokedex.Api.Tests/        # Integration tests (xUnit)
│   └── Dockerfile
├── frontend/
│   ├── src/
│   │   ├── api/                  # API client and React Query hooks
│   │   ├── components/           # Reusable UI components
│   │   ├── pages/                # Route pages
│   │   └── types/                # TypeScript interfaces
│   ├── .env                      # API URL and session token
│   └── Dockerfile
├── docker-compose.yml            # Full-stack orchestration
└── README.md                     # This file
```

---

## Technical Expectations

- Use modern patterns for both frameworks (hooks, functional components, dependency injection, async/await)
- Keep classes, components, and services focused and well-structured
- Write clear, self-explanatory code
- Implement reasonable error handling throughout
- Add tests where appropriate — both projects have a test runner configured

You do **not** need to:
- Complete every task — quality over quantity
- Over-engineer or introduce unnecessary abstractions
- Achieve pixel-perfect styling — a clean, functional layout is sufficient

---

## Submission

Please provide:
- A link to your completed repository
- Any brief notes or trade-offs you feel are worth calling out
