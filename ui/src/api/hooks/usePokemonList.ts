import { useQuery } from '@tanstack/react-query';
import { apiFetch } from '../client';
import type { PokemonListResponse } from '../../types/pokemon';

export function usePokemonList() {
  return useQuery<PokemonListResponse>({
    queryKey: ['pokemon', 'list'],
    queryFn: async () => {
      const res = await apiFetch('/api/pokemon');
      if (!res.ok) throw new Error('Failed to fetch Pokémon list');
      return res.json();
    },
  });
}
