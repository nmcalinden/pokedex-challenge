# UI (React + TypeScript)

A Vite + React + TypeScript single-page app using TanStack React Query and React
Router. It talks to the API in [`../server`](../server).

## Run

```bash
# from the repo root
make ui              # Vite dev server
# or
cd ui && npm install && npm run dev   # → http://localhost:3000
```

The full stack (database + API + UI) runs via `make dev` from the repo root.

## Configuration

`.env` holds:

- `VITE_API_BASE_URL` — the API base URL (default `http://localhost:5001`).
- `VITE_SESSION_TOKEN` — the static bearer token attached to API requests (see the
  repo README's Authentication section).

## Layout

- `src/api` — the `apiFetch` client and React Query hooks.
- `src/components` — reusable components.
- `src/pages` — routed pages (list, detail, favourites).
- `src/types` — shared TypeScript types.

## Notes

- Automated tests are expected but not pre-configured; choose your tooling (e.g.
  Vitest + Testing Library) and add them.
