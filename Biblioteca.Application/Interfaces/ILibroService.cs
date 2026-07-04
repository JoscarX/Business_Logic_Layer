using Biblioteca.Application.Common.Results;
using Biblioteca.Application.DTOs.Libro;

namespace Biblioteca.Application.Interfaces;

public interface ILibroService
{
    Task<Result<IEnumerable<LibroResponseDto>>> ObtenerTodosAsync(CancellationToken ct = default);
    Task<Result<LibroResponseDto>> ObtenerPorIdAsync(int id, CancellationToken ct = default);
    Task<Result<IEnumerable<LibroResponseDto>>> ObtenerDisponiblesAsync(CancellationToken ct = default);
    Task<Result<LibroResponseDto>> ObtenerPorIsbnAsync(string isbn, CancellationToken ct = default);
    Task<Result<LibroResponseDto>> CrearAsync(CrearLibroDto dto, CancellationToken ct = default);
    Task<Result> ActualizarAsync(int id, ActualizarLibroDto dto, CancellationToken ct = default);
    Task<Result> AgregarExistenciasAsync(int id, AgregarExistenciasDto dto, CancellationToken ct = default);
    Task<Result> ActualizarPrecioAsync(int id, ActualizarPrecioDto dto, CancellationToken ct = default);
    Task<Result> EliminarAsync(int id, CancellationToken ct = default);
}
