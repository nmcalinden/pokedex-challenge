namespace Pokedex.Api.Models;

public record PokemonListResponse(int Count, int Offset, int Limit, List<PokemonListItem> Results);
