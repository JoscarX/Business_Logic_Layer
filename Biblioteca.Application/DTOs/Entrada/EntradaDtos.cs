using Biblioteca.Application.DTOs.Visitante;
using Biblioteca.Domain.Enums;

namespace Biblioteca.Application.DTOs.Entrada;

public sealed record CrearEntradaDto(int VisitanteId);

public sealed record RegistrarSalidaDto(DateTime FechaSalida);

public sealed record EntradaResponseDto(
    int Id,
    int VisitanteId,
    string NombreVisitante,
    DateTime FechaEntrada,
    DateTime? FechaSalida,
    bool EstaActiva,
    TimeSpan? DuracionVisita);
