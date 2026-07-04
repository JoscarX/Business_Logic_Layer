using Biblioteca.Domain.Enums;

namespace Biblioteca.Application.DTOs.Prestamo;

public sealed record CrearPrestamoDto(
    int VisitanteId,
    int LibroId,
    DateTime FechaEntrega);

public sealed record PrestamoResponseDto(
    int Id,
    int VisitanteId,
    string NombreVisitante,
    int LibroId,
    string TituloLibro,
    DateTime FechaInicio,
    DateTime FechaEntrega,
    DateTime? FechaDevolucion,
    string Estado);
