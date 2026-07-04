namespace Biblioteca.Application.DTOs.Usuario;

public sealed record CrearUsuarioDto(
    string Nombre,
    string Cedula,
    string Password,
    int RolId);

public sealed record ActualizarUsuarioDto(
    string Nombre,
    int RolId);

public sealed record CambiarPasswordDto(
    string PasswordActual,
    string NuevoPassword,
    string ConfirmarPassword);

public sealed record UsuarioResponseDto(
    int Id,
    string Nombre,
    string Cedula,
    int RolId,
    string NombreRol,
    bool Activo,
    DateTime FechaCreacion);
