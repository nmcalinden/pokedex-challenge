using Microsoft.EntityFrameworkCore;

namespace Pokedex.Api.Data;

public class PokedexDbContext : DbContext
{
    public PokedexDbContext(DbContextOptions<PokedexDbContext> options) : base(options) { }
}
