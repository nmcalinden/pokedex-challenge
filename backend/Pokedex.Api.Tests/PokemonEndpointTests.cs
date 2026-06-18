using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pokedex.Api.Data;
using Pokedex.Api.Models;
using Pokedex.Api.Services;

namespace Pokedex.Api.Tests;

public class PokemonEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public PokemonEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    private HttpClient CreateClientWithFakeUpstream(bool upstreamReturnsNull)
    {
        return _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove ALL DbContext registrations (both the options and the context itself)
                var descriptorsToRemove = services
                    .Where(d =>
                        d.ServiceType == typeof(DbContextOptions<PokedexDbContext>) ||
                        d.ServiceType == typeof(PokedexDbContext) ||
                        d.ServiceType.FullName?.Contains("DbContextOptions") == true)
                    .ToList();
                foreach (var d in descriptorsToRemove) services.Remove(d);

                // Also remove the Npgsql-registered internal services
                var npgsqlDescriptors = services
                    .Where(d => d.ServiceType.FullName?.Contains("Npgsql") == true ||
                                d.ImplementationType?.FullName?.Contains("Npgsql") == true)
                    .ToList();
                foreach (var d in npgsqlDescriptors) services.Remove(d);

                var dbName = $"TestDb-{Guid.NewGuid()}";
                services.AddDbContext<PokedexDbContext>(options =>
                    options.UseInMemoryDatabase(dbName));

                // Remove real IPokeApiService registrations
                var pokeApiDescriptors = services
                    .Where(d => d.ServiceType == typeof(IPokeApiService))
                    .ToList();
                foreach (var d in pokeApiDescriptors) services.Remove(d);

                // Register fake
                services.AddSingleton<IPokeApiService>(
                    new FakePokeApiService(returnsNull: upstreamReturnsNull));
            });
        }).CreateClient();
    }

    [Fact]
    public async Task GetPokemon_ReturnsOk_WithPaginationFields()
    {
        var client = CreateClientWithFakeUpstream(upstreamReturnsNull: false);

        var response = await client.GetAsync("/api/pokemon");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(json.TryGetProperty("count", out _));
        Assert.True(json.TryGetProperty("offset", out _));
        Assert.True(json.TryGetProperty("limit", out _));
        Assert.True(json.TryGetProperty("results", out _));
    }

    [Fact]
    public async Task GetPokemon_Returns502ProblemDetails_WhenUpstreamUnavailable()
    {
        var client = CreateClientWithFakeUpstream(upstreamReturnsNull: true);

        var response = await client.GetAsync("/api/pokemon");

        Assert.Equal(HttpStatusCode.BadGateway, response.StatusCode);

        var contentType = response.Content.Headers.ContentType?.MediaType;
        Assert.Equal("application/problem+json", contentType);

        var problem = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(502, problem.GetProperty("status").GetInt32());
        Assert.True(problem.TryGetProperty("title", out _));
        Assert.True(problem.TryGetProperty("detail", out _));
    }
}

internal class FakePokeApiService : IPokeApiService
{
    private readonly bool _returnsNull;

    public FakePokeApiService(bool returnsNull)
    {
        _returnsNull = returnsNull;
    }

    public Task<PokemonListResponse?> GetPokemonListAsync(
        int offset, int limit, CancellationToken ct = default)
    {
        if (_returnsNull)
            return Task.FromResult<PokemonListResponse?>(null);

        var result = new PokemonListResponse(
            Count: 1302,
            Offset: offset,
            Limit: limit,
            Results: new List<PokemonListItem>
            {
                new("bulbasaur", "https://pokeapi.co/api/v2/pokemon/1/"),
                new("ivysaur", "https://pokeapi.co/api/v2/pokemon/2/")
            });

        return Task.FromResult<PokemonListResponse?>(result);
    }
}
