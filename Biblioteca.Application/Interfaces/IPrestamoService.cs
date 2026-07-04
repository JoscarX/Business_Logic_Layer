using Biblioteca.Application.Common.Results;
using Biblioteca.Application.DTOs.Prestamo;

namespace Biblioteca.Application.Interfaces;

public interface IPrestamoService
{
    Task<Result<IEnumerable<PrestamoResponseDto>>> ObtenerTodosAsync(CancellationToken ct = default);
    Task<Result<PrestamoResponseDto>> ObtenerPorIdAsync(int id, CancellationToken ct = default);
    Task<Result<IEnumerable<PrestamoResponseDto>>> ObtenerActivosAsync(CancellationToken ct = default);
    Task<Result<IEnumerable<PrestamoResponseDto>>> ObtenerVencidosAsync(CancellationToken ct = default);
    Task<Result<IEnumerable<PrestamoResponseDto>>> ObtenerPorVisitanteAsync(int visitanteId, CancellationToken ct = default);
    Task<Result<PrestamoResponseDto>> CrearAsync(CrearPrestamoDto dto, CancellationToken ct = default);
    Task<Result> MarcarComoDevueltoAsync(int id, CancellationToken ct = default);
    Task<Result> ProcesarVencimientosAsync(CancellationToken ct = default);
}
