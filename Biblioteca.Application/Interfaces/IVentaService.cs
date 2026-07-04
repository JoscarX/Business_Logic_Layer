using Biblioteca.Application.Common.Results;
using Biblioteca.Application.DTOs.Venta;

namespace Biblioteca.Application.Interfaces;

public interface IVentaService
{
    Task<Result<IEnumerable<VentaResponseDto>>> ObtenerTodosAsync(CancellationToken ct = default);
    Task<Result<VentaResponseDto>> ObtenerPorIdAsync(int id, CancellationToken ct = default);
    Task<Result<IEnumerable<VentaResponseDto>>> ObtenerPorVisitanteAsync(int visitanteId, CancellationToken ct = default);
    Task<Result<VentaResponseDto>> CrearAsync(CrearVentaDto dto, CancellationToken ct = default);
    Task<Result> AnularAsync(int id, CancellationToken ct = default);
}
