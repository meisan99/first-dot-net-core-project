using GameStore.Api.Dtos;
using GameStore.Api.Data;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GameEndpoints
{
    const string GetGameEndpointName = "GetGames";

    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");

        // GET /games
        group.MapGet("/", async (GameStoreContext dbContext) =>
        {
            var games = await dbContext.Games
                .Include(g => g.Genre)
                .Select(g => new GameSummaryDto(
                    g.Id,
                    g.Name,
                    g.Genre!.Name,
                    g.Price,
                    g.ReleaseDate
                 ))
                .AsNoTracking()
                .ToListAsync();

            return Results.Ok(games);
        });

        // GET /games/{id}
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            var game = await dbContext.Games
                .Include(g => g.Genre)
                .FirstOrDefaultAsync(g => g.Id == id);

            return game != null ? Results.Ok(new GameDetailsDto(
                game.Id,
                game.Name,
                game.Genre!.Id,
                game.Price,
                game.ReleaseDate
            )) : Results.NotFound();
        })
        .WithName(GetGameEndpointName);

        // POST /games
        group.MapPost("/", async (CreateGameDto createGameDto, GameStoreContext dbContext) =>
        {
            Game game = new Game
            {
                Name = createGameDto.Name,
                GenreId = createGameDto.GenreId,
                Price = createGameDto.Price,
                ReleaseDate = createGameDto.ReleaseDate
            };

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            GameDetailsDto gameDto = new GameDetailsDto(
                game.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.ReleaseDate
            );

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = gameDto.Id }, gameDto);
        });

        // PUT /games/{id}
        group.MapPut("/{id}", async (int id, UpdateGameDto updateGameDto, GameStoreContext dbContext) =>
        {
            var existingGame = await dbContext.Games.FindAsync(id);
            if (existingGame == null)
            {
                return Results.NotFound($"Game with id '{id}' not found.");
            }

            existingGame.Name = updateGameDto.Name;
            existingGame.GenreId = updateGameDto.GenreId;
            existingGame.Price = updateGameDto.Price;
            existingGame.ReleaseDate = updateGameDto.ReleaseDate;

            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        });

        // DELETE /games/{id}
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Games
                .Where(g => g.Id == id)
                .ExecuteDeleteAsync();

            return Results.NoContent();
        });
    }
}