using GameStore.Api.Dtos;
using GameStore.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GenresEndpoints
{
    public static void MapGenresEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/genres");

        // GET /genres
        group.MapGet("/", async (GameStoreContext dbContext) =>
        {
            var genres = await dbContext.Genres
                .Select(g => new GenreDto(g.Id, g.Name))
                .AsNoTracking()
                .ToListAsync();

            return Results.Ok(genres);
        });

        // GET /genres/{id}
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            var genre = await dbContext.Genres
                .Where(g => g.Id == id)
                .Select(g => new GenreDto(g.Id, g.Name))
                .FirstOrDefaultAsync();

            if (genre == null)
            {
                return Results.NotFound($"Genre with id '{id}' not found.");
            }

            return Results.Ok(genre);
        });
    }
}