using Biblioteca.Domain.Enums;

namespace Biblioteca.Application.DTOs.Auditoria;

public sealed record AuditoriaResponseDto(
    int Id,
    string Tabla,
    string Accion,
    int IdRegistro,
    string? DatosAntes,
    string? DatosDespues,
    int? UsuarioId,
    string? NombreUsuario,
    DateTime Fecha);
