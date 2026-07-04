namespace Biblioteca.Application.DTOs.Rol;

public sealed record CrearRolDto(
    string Nombre,
    string? Descripcion);

public sealed record ActualizarRolDto(
    string Nombre,
    string? Descripcion);

public sealed record RolResponseDto(
    int Id,
    string Nombre,
    string? Descripcion);
