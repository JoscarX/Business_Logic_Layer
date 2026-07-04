using Biblioteca.Application.Common.Results;
using Biblioteca.Application.DTOs.Rol;

namespace Biblioteca.Application.Interfaces;

public interface IRolService
{
    Task<Result<IEnumerable<RolResponseDto>>> ObtenerTodosAsync(CancellationToken ct = default);
    Task<Result<RolResponseDto>> ObtenerPorIdAsync(int id, CancellationToken ct = default);
    Task<Result<RolResponseDto>> CrearAsync(CrearRolDto dto, CancellationToken ct = default);
    Task<Result> ActualizarAsync(int id, ActualizarRolDto dto, CancellationToken ct = default);
    Task<Result> EliminarAsync(int id, CancellationToken ct = default);
}
