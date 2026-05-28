using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record UpdateGameDto(
    [Required][StringLength(50, MinimumLength = 1)] string Name,
    [Required][Range(1, int.MaxValue)] int GenreId,
    [Range(1, double.MaxValue)] decimal Price,
    DateOnly ReleaseDate
);