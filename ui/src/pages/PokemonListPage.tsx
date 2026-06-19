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
