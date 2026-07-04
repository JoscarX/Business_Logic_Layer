using Biblioteca.Application.Common.Results;
using Biblioteca.Application.DTOs.Entrada;

namespace Biblioteca.Application.Interfaces;

public interface IEntradaService
{
    Task<Result<IEnumerable<EntradaResponseDto>>> ObtenerTodosAsync(CancellationToken ct = default);
    Task<Result<IEnumerable<EntradaResponseDto>>> ObtenerActivasAsync(CancellationToken ct = default);
    Task<Result<EntradaResponseDto>> ObtenerPorIdAsync(int id, CancellationToken ct = default);
    Task<Result<IEnumerable<EntradaResponseDto>>> ObtenerPorVisitanteAsync(int visitanteId, CancellationToken ct = default);
    Task<Result<EntradaResponseDto>> RegistrarEntradaAsync(CrearEntradaDto dto, CancellationToken ct = default);
    Task<Result> RegistrarSalidaAsync(int id, RegistrarSalidaDto dto, CancellationToken ct = default);
}
