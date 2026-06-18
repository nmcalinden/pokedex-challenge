import { useQuery } from '@tanstack/react-query';

export function usePokemon(id: string) {
  return useQuery({
    queryKey: ['pokemon', id],
    queryFn: async () => {
      // Currently fetches directly from PokéAPI.
      // Once Backend Task 4 is implemented, switch to the backend proxy:
      //   const res = await apiFetch(`/api/pokemon/${id}`);
      // The backend proxy will return enriched data including species
      // description and evolution chain alongside core Pokémon data.
      const res = await fetch(`https://pokeapi.co/api/v2/pokemon/${id}`);
      if (!res.ok) throw new Error('Failed to fetch Pokémon details');
      return res.json();
    },
    enabled: !!id,
  });
}
