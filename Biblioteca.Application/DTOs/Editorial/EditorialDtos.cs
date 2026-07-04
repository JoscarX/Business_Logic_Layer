namespace Biblioteca.Application.DTOs.Editorial;

public sealed record CrearEditorialDto(
    string Nombre,
    string? Pais);

public sealed record ActualizarEditorialDto(
    string Nombre,
    string? Pais);

public sealed record EditorialResponseDto(
    int Id,
    string Nombre,
    string? Pais);
