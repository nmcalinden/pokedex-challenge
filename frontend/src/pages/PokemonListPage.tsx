import { Link } from 'react-router';
import { usePokemonList } from '../api/hooks/usePokemonList';
import PokemonCard from '../components/PokemonCard';

export default function PokemonListPage() {
  const { data, isLoading, error } = usePokemonList();

  if (isLoading) return <p>Loading…</p>;
  if (error) return <p>Error loading Pokémon.</p>;

  return (
    <div>
      <h1>Pokémon List</h1>
      <nav><Link to="/favourites">View Favourites</Link></nav>

      {/* TODO: Frontend Task 4 — Add type filtering.
          Allow users to filter the Pokémon list by type.
          Fetch available types from GET https://pokeapi.co/api/v2/type
          When a type is selected, fetch Pokémon of that type from
          GET https://pokeapi.co/api/v2/type/{id}
          Display the filtered list (pagination not required when filtering).
          Users should be able to clear/change the filter.
          See: https://pokeapi.co/docs/v2#types */}

      <ul>
        {data?.results.map((pokemon) => {
          const id = pokemon.url.split('/').filter(Boolean).pop()!;
          return (
            <li key={id}>
              <Link to={`/pokemon/${id}`}>
                <PokemonCard name={pokemon.name} />
              </Link>
            </li>
          );
        })}
      </ul>
    </div>
  );
}
