namespace Pokedex.Api.Models;

public class Favourite
{
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public int PokemonId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
