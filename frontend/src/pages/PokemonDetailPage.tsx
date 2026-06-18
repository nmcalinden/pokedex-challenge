import { useParams } from 'react-router';
import { usePokemon } from '../api/hooks/usePokemon';

export default function PokemonDetailPage() {
  const { id } = useParams<{ id: string }>();
  const { data, isLoading, error } = usePokemon(id!);

  if (isLoading) return <p>Loading…</p>;
  if (error) return <p>Error loading Pokémon details.</p>;
  if (!data) return null;

  // TODO: Frontend Task 2 — Style this detail page.
  // Apply a polished layout with proper spacing, typography, and colours.
  // Show the Pokémon sprite, name, types, and stats in a visually appealing way.
  // Creative freedom is encouraged — no Figma reference is provided.
  // Ensure the layout works on different screen sizes.
  // Consider using CSS modules, a utility framework, or styled-components.

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
