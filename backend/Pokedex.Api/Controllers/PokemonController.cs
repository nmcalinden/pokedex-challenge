using Microsoft.AspNetCore.Mvc;
using Pokedex.Api.Services;

namespace Pokedex.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PokemonController : ControllerBase
{
    private readonly IPokeApiService _pokeApiService;

    public PokemonController(IPokeApiService pokeApiService)
    {
        _pokeApiService = pokeApiService;
    }

    /// <summary>
    /// Returns a paginated list of Pokémon proxied from PokéAPI.
    /// Currently returns the first page only (offset=0, limit=20).
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetPokemon(CancellationToken ct)
    {
        // TODO: Task 1 — Accept `offset` and `limit` query parameters to support
        // arbitrary pagination. Currently hardcoded to offset=0, limit=20 (first page only).
        // The client should be able to request any page, e.g. GET /api/pokemon?offset=20&limit=20
        // Validate that offset >= 0 and 1 <= limit <= 100.
        // See: https://pokeapi.co/docs/v2#resource-listspagination-section

        var result = await _pokeApiService.GetPokemonListAsync(offset: 0, limit: 20, ct);

        if (result is null)
        {
            return Problem(
                type: "https://tools.ietf.org/html/rfc7231#section-6.6.3",
                title: "Bad Gateway",
                statusCode: StatusCodes.Status502BadGateway,
                detail: "Unable to retrieve Pokémon data from the upstream PokéAPI service.");
        }

        return Ok(result);
    }

    // TODO: Task 3 — Add a GET endpoint at /api/pokemon/types/stats that returns
    // aggregated type statistics from the cached Pokémon data.
    // For each Pokémon type present, return:
    //   - The type name (e.g. "fire", "water")
    //   - The count of Pokémon with that type
    //   - The average value of each of the six base stats:
    //     hp, attack, defense, special-attack, special-defense, speed
    // You will need to fetch individual Pokémon details from PokéAPI and cache them.
    // Return an appropriate error response if the upstream data is unavailable.
    // See: https://pokeapi.co/docs/v2#pokemon

    // TODO: Task 4 — Add a GET endpoint at /api/pokemon/{id} that returns
    // an enriched Pokémon detail response composed from multiple PokéAPI sources.
    //
    // The response should include:
    //   1. Core Pokémon data (name, height, weight, sprites, types, stats, abilities)
    //      from https://pokeapi.co/api/v2/pokemon/{id}
    //   2. Species description (English flavor_text from the latest game version)
    //      from https://pokeapi.co/api/v2/pokemon-species/{id}
    //   3. Evolution chain (list of species in the evolution line with level/trigger info)
    //      from the URL in the species response's evolution_chain.url field
    //
    // This requires chaining multiple upstream API calls. Consider:
    //   - How to structure your service layer for this composition
    //   - Caching each upstream response independently with IMemoryCache (5+ min TTL)
    //   - Graceful degradation: if species or evolution chain fails, return the core
    //     Pokémon data with null for the missing sections rather than returning 502.
    //     Only return 502 Problem Details if the core /pokemon/{id} call fails.
    //   - Whether to fetch species and evolution data sequentially or in parallel
    //
    // See: https://pokeapi.co/docs/v2#pokemon
    // See: https://pokeapi.co/docs/v2#pokemon-species
    // See: https://pokeapi.co/docs/v2#evolution-chains
}
