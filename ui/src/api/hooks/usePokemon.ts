import { useQuery } from '@tanstack/react-query';

export function usePokemon(id: string) {
  return useQuery({
    queryKey: ['pokemon', id],
    queryFn: async () => {
      const res = await fetch(`https://pokeapi.co/api/v2/pokemon/${id}`);
      if (!res.ok) throw new Error('Failed to fetch Pokémon details');
      return res.json();
    },
    enabled: !!id,
  });
}
