namespace Biblioteca.Application.DTOs.Autor;

public sealed record CrearAutorDto(
    string Nombre,
    string? Biografia,
    string? Pais,
    DateOnly? FechaNacimiento);

public sealed record ActualizarAutorDto(
    string Nombre,
    string? Biografia,
    string? Pais,
    DateOnly? FechaNacimiento);

public sealed record AutorResponseDto(
    int Id,
    string Nombre,
    string? Biografia,
    string? Pais,
    DateOnly? FechaNacimiento);
