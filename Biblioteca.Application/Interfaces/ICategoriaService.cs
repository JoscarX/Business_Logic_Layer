using Biblioteca.Application.Common.Results;
using Biblioteca.Application.DTOs.Categoria;

namespace Biblioteca.Application.Interfaces;

public interface ICategoriaService
{
    Task<Result<IEnumerable<CategoriaResponseDto>>> ObtenerTodosAsync(CancellationToken ct = default);
    Task<Result<CategoriaResponseDto>> ObtenerPorIdAsync(int id, CancellationToken ct = default);
    Task<Result<CategoriaResponseDto>> CrearAsync(CrearCategoriaDto dto, CancellationToken ct = default);
    Task<Result> ActualizarAsync(int id, ActualizarCategoriaDto dto, CancellationToken ct = default);
    Task<Result> EliminarAsync(int id, CancellationToken ct = default);
}
