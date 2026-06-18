export interface PokemonListItem {
  name: string;
  url: string;
}

export interface PokemonListResponse {
  count: number;
  offset: number;
  limit: number;
  results: PokemonListItem[];
}
