using Pokedex.Api.Models;

namespace Pokedex.Api.Services;

public interface IPokeApiService
{
    Task<PokemonListResponse?> GetPokemonListAsync(int offset, int limit, CancellationToken ct = default);
}
