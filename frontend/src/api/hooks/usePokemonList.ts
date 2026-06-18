import { useQuery } from '@tanstack/react-query';
import { apiFetch } from '../client';
import type { PokemonListResponse } from '../../types/pokemon';

export function usePokemonList() {
  return useQuery<PokemonListResponse>({
    queryKey: ['pokemon', 'list'],
    queryFn: async () => {
      // TODO: Frontend Task 1 — Add pagination support.
      // Currently fetches the first page only with no offset/limit params.
      // Implement infinite scroll or a "Load More" button that requests
      // subsequent pages by passing offset and limit query parameters.
      // The API endpoint GET /api/pokemon supports offset and limit
      // (once Backend Task 1 is completed).
      // Consider using useInfiniteQuery from @tanstack/react-query.
      // See: https://tanstack.com/query/latest/docs/framework/react/guides/infinite-queries

      const res = await apiFetch('/api/pokemon');
      if (!res.ok) throw new Error('Failed to fetch Pokémon list');
      return res.json();
    },
  });
}
