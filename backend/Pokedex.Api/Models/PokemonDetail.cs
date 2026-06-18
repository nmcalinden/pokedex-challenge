using System.Text.Json.Serialization;

namespace Pokedex.Api.Models;

public record PokemonDetail(
    int Id,
    string Name,
    List<PokemonTypeSlot> Types,
    List<PokemonStat> Stats);

public record PokemonTypeSlot(PokemonTypeInfo Type);

public record PokemonTypeInfo(string Name);

public record PokemonStat(
    [property: JsonPropertyName("base_stat")] int BaseStat,
    StatInfo Stat);

public record StatInfo(string Name);
