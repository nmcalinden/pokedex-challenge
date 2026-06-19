import { Link } from 'react-router';

export default function FavouritesPage() {
  return (
    <div>
      <h1>Favourites</h1>
      <nav><Link to="/">← Back to list</Link></nav>
    </div>
  );
}
