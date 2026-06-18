import { render, screen } from '@testing-library/react';
import { describe, it, expect } from 'vitest';
import PokemonCard from '../components/PokemonCard';

describe('PokemonCard', () => {
  it('renders the Pokémon name with first letter capitalised', () => {
    render(<PokemonCard name="bulbasaur" />);
    expect(screen.getByText('Bulbasaur')).toBeInTheDocument();
  });
});
