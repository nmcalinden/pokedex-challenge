using Microsoft.AspNetCore.Mvc;
using Pokedex.Api.Data;

namespace Pokedex.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FavouritesController : ControllerBase
{
    private readonly PokedexDbContext _db;

    public FavouritesController(PokedexDbContext db)
    {
        _db = db;
    }

    // TODO: Task 2 — Implement favourites endpoints scoped to the authenticated user.
    //
    // Authentication:
    //   The client sends an "Authorization: Bearer <token>" header with every request.
    //   Extract the token from this header and use it to scope all database operations.
    //   The token value is a static string configured in the frontend environment
    //   variable VITE_SESSION_TOKEN (see frontend/.env).
    //   No JWT validation or login flow is required — simply read the raw token string.
    //
    // Required endpoints:
    //   GET    /api/favourites              — List all favourites for the current token
    //   POST   /api/favourites              — Add a Pokémon (body: { "pokemonId": <int> })
    //   DELETE /api/favourites/{pokemonId}  — Remove a Pokémon from favourites
    //
    // Requirements:
    //   - Return 401 if the Authorization header is missing or malformed
    //   - Return 409 if the Pokémon is already favourited
    //   - Return 404 on DELETE if the favourite doesn't exist
    //   - All error responses must use RFC 7807 Problem Details format
    //   - The Favourite entity and initial migration are already provided in:
    //     Models/Favourite.cs and Data/PokedexDbContext.cs
    //
    // Helper to extract token from Authorization header:
    //   var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
}
