using Biblioteca.Application.Common.Results;
using Biblioteca.Application.DTOs.Editorial;

namespace Biblioteca.Application.Interfaces;

public interface IEditorialService
{
    Task<Result<IEnumerable<EditorialResponseDto>>> ObtenerTodosAsync(CancellationToken ct = default);
    Task<Result<EditorialResponseDto>> ObtenerPorIdAsync(int id, CancellationToken ct = default);
    Task<Result<EditorialResponseDto>> CrearAsync(CrearEditorialDto dto, CancellationToken ct = default);
    Task<Result> ActualizarAsync(int id, ActualizarEditorialDto dto, CancellationToken ct = default);
    Task<Result> EliminarAsync(int id, CancellationToken ct = default);
}
