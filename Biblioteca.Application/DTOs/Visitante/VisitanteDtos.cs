namespace Biblioteca.Application.DTOs.Visitante;

public sealed record CrearVisitanteDto(
    string NombreCompleto,
    string Cedula,
    string Telefono,
    string Direccion,
    string? Correo);

public sealed record ActualizarVisitanteDto(
    string NombreCompleto,
    string Telefono,
    string Direccion,
    string? Correo);

public sealed record VisitanteResponseDto(
    int Id,
    string NombreCompleto,
    string Cedula,
    string? Correo,
    string Telefono,
    string Direccion);
