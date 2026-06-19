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

    /// <summary>Returns a paginated list of Pokémon proxied from PokéAPI (first page).</summary>
    [HttpGet]
    public async Task<IActionResult> GetPokemon(CancellationToken ct)
    {
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
}
