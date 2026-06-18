import { Link } from 'react-router';

export default function FavouritesPage() {
  // TODO: Frontend Task 3 — Implement the favourites feature.
  //
  // This page should display the current user's favourited Pokémon.
  // The API endpoints for favourites are:
  //   GET    /api/favourites              — List favourites
  //   POST   /api/favourites              — Add { pokemonId: number }
  //   DELETE /api/favourites/{pokemonId}  — Remove a favourite
  //
  // Authentication:
  //   Every request must include the header:
  //     Authorization: Bearer <VITE_SESSION_TOKEN>
  //   The apiFetch() helper in src/api/client.ts already attaches this header.
  //
  // Requirements:
  //   - Fetch and display the user's favourites on this page
  //   - Allow unfavouriting from this page
  //   - Add a favourite/unfavourite toggle on the detail page and list page
  //   - Use useMutation from @tanstack/react-query for add/remove operations
  //   - Invalidate the favourites query after mutations
  //
  // Note: These endpoints require Backend Task 2 to be completed first.

  return (
    <div>
      <h1>Favourites</h1>
      <nav><Link to="/">← Back to list</Link></nav>
      <p>No favourites yet. Complete Backend Task 2 and Frontend Task 3 to enable this feature.</p>
    </div>
  );
}
