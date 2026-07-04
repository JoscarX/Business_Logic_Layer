namespace Biblioteca.Application.DTOs.Auth;

public sealed record LoginDto(
    string Cedula,
    string Password);

public sealed record TokenResponseDto(
    string Token,
    DateTime Expiracion,
    string NombreUsuario,
    string Rol);
