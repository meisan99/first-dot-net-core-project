using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExtensions
{
    public static void MigrateDb(this WebApplication app)
    {
        // get access to instance of GameStoreContext 
        // and call Migrate() method to apply any pending migrations to the database
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<GameStoreContext>();

        dbContext.Database.Migrate();
    }

    public static void AddGameStoreDb(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("GameStore");

        builder.Services.AddSqlite<GameStoreContext>(
            connectionString,
            optionsAction: options =>
            {
                options.UseSeeding((context, _) =>
                {
                    if (!context.Set<Genre>().Any())
                    {
                        context.Set<Genre>().AddRange(
                            new Genre { Name = "Fighting" },
                            new Genre { Name = "Platformer" },
                            new Genre { Name = "Sport" },
                            new Genre { Name = "RPG" },
                            new Genre { Name = "Strategy" },
                            new Genre { Name = "Rogue-like" }
                        );
                    }

                    context.SaveChanges();
                });
            }
        );
    }
}