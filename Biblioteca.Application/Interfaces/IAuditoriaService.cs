using Biblioteca.Application.Common.Results;
using Biblioteca.Application.DTOs.Auditoria;
using Biblioteca.Domain.Enums;

namespace Biblioteca.Application.Interfaces;

public interface IAuditoriaService
{
    Task<Result<IEnumerable<AuditoriaResponseDto>>> ObtenerTodosAsync(CancellationToken ct = default);
    Task<Result<IEnumerable<AuditoriaResponseDto>>> ObtenerPorTablaAsync(string tabla, CancellationToken ct = default);
    Task<Result<IEnumerable<AuditoriaResponseDto>>> ObtenerPorUsuarioAsync(int usuarioId, CancellationToken ct = default);
    Task<Result<IEnumerable<AuditoriaResponseDto>>> ObtenerPorRangoFechaAsync(DateTime desde, DateTime hasta, CancellationToken ct = default);
}
