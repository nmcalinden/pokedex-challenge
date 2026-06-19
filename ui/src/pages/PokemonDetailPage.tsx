import { useParams } from 'react-router';
import { usePokemon } from '../api/hooks/usePokemon';

export default function PokemonDetailPage() {
  const { id } = useParams<{ id: string }>();
  const { data, isLoading, error } = usePokemon(id!);

  if (isLoading) return <p>Loading…</p>;
  if (error) return <p>Error loading Pokémon details.</p>;
  if (!data) return null;

  return (
    <div>
      <h1>{data.name}</h1>
      <img src={data.sprites.front_default} alt={data.name} />

      <h2>Types</h2>
      <ul>
        {data.types.map((t: any) => (
          <li key={t.type.name}>{t.type.name}</li>
        ))}
      </ul>

      <h2>Stats</h2>
      <ul>
        {data.stats.map((s: any) => (
          <li key={s.stat.name}>
            {s.stat.name}: {s.base_stat}
          </li>
        ))}
      </ul>
    </div>
  );
}
