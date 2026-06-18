using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Pokedex.Api.Models;

namespace Pokedex.Api.Services;

public class PokeApiService : IPokeApiService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(5);

    public PokeApiService(HttpClient httpClient, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _cache = cache;
    }

    public async Task<PokemonListResponse?> GetPokemonListAsync(
        int offset, int limit, CancellationToken ct = default)
    {
        var cacheKey = $"pokemon-list-{offset}-{limit}";

        if (_cache.TryGetValue(cacheKey, out PokemonListResponse? cached))
            return cached;

        var response = await _httpClient.GetAsync(
            $"pokemon?offset={offset}&limit={limit}", ct);

        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadFromJsonAsync<JsonElement>(ct);

        var result = new PokemonListResponse(
            Count: json.GetProperty("count").GetInt32(),
            Offset: offset,
            Limit: limit,
            Results: json.GetProperty("results")
                .EnumerateArray()
                .Select(r => new PokemonListItem(
                    r.GetProperty("name").GetString()!,
                    r.GetProperty("url").GetString()!))
                .ToList());

        _cache.Set(cacheKey, result, CacheTtl);
        return result;
    }
}
