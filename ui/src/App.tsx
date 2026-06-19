import { Routes, Route } from 'react-router';
import PokemonListPage from './pages/PokemonListPage';
import PokemonDetailPage from './pages/PokemonDetailPage';
import FavouritesPage from './pages/FavouritesPage';

export default function App() {
  return (
    <Routes>
      <Route path="/" element={<PokemonListPage />} />
      <Route path="/pokemon/:id" element={<PokemonDetailPage />} />
      <Route path="/favourites" element={<FavouritesPage />} />
    </Routes>
  );
}
