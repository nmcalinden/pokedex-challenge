interface PokemonCardProps {
  name: string;
}

export default function PokemonCard({ name }: PokemonCardProps) {
  const displayName = name.charAt(0).toUpperCase() + name.slice(1);
  return <span>{displayName}</span>;
}
