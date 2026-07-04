using Biblioteca.Application.Common.Results;
using Biblioteca.Application.DTOs.Visitante;

namespace Biblioteca.Application.Interfaces;

public interface IVisitanteService
{
    Task<Result<IEnumerable<VisitanteResponseDto>>> ObtenerTodosAsync(CancellationToken ct = default);
    Task<Result<VisitanteResponseDto>> ObtenerPorIdAsync(int id, CancellationToken ct = default);
    Task<Result<VisitanteResponseDto>> CrearAsync(CrearVisitanteDto dto, CancellationToken ct = default);
    Task<Result> ActualizarAsync(int id, ActualizarVisitanteDto dto, CancellationToken ct = default);
    Task<Result> EliminarAsync(int id, CancellationToken ct = default);
}
