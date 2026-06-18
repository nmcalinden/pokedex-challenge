using Microsoft.EntityFrameworkCore;
using Pokedex.Api.Models;

namespace Pokedex.Api.Data;

public class PokedexDbContext : DbContext
{
    public PokedexDbContext(DbContextOptions<PokedexDbContext> options) : base(options) { }

    public DbSet<Favourite> Favourites => Set<Favourite>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Favourite>(entity =>
        {
            entity.ToTable("favourites");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Token).HasColumnName("token").IsRequired();
            entity.Property(e => e.PokemonId).HasColumnName("pokemon_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at")
                  .HasDefaultValueSql("NOW()");

            entity.HasIndex(e => new { e.Token, e.PokemonId }).IsUnique();
        });
    }
}
