using Biblioteca.Application.Common.Results;
using Biblioteca.Application.DTOs.Autor;

namespace Biblioteca.Application.Interfaces;

public interface IAutorService
{
    Task<Result<IEnumerable<AutorResponseDto>>> ObtenerTodosAsync(CancellationToken ct = default);
    Task<Result<AutorResponseDto>> ObtenerPorIdAsync(int id, CancellationToken ct = default);
    Task<Result<AutorResponseDto>> CrearAsync(CrearAutorDto dto, CancellationToken ct = default);
    Task<Result> ActualizarAsync(int id, ActualizarAutorDto dto, CancellationToken ct = default);
    Task<Result> EliminarAsync(int id, CancellationToken ct = default);
}
