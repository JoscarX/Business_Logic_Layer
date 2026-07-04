using Biblioteca.Application.Common.Results;
using Biblioteca.Application.DTOs.Auth;
using Biblioteca.Application.DTOs.Usuario;

namespace Biblioteca.Application.Interfaces;

public interface IUsuarioService
{
    Task<Result<IEnumerable<UsuarioResponseDto>>> ObtenerTodosAsync(CancellationToken ct = default);
    Task<Result<UsuarioResponseDto>> ObtenerPorIdAsync(int id, CancellationToken ct = default);
    Task<Result<UsuarioResponseDto>> CrearAsync(CrearUsuarioDto dto, CancellationToken ct = default);
    Task<Result> ActualizarAsync(int id, ActualizarUsuarioDto dto, CancellationToken ct = default);
    Task<Result> CambiarPasswordAsync(int id, CambiarPasswordDto dto, CancellationToken ct = default);
    Task<Result> ActivarAsync(int id, CancellationToken ct = default);
    Task<Result> DesactivarAsync(int id, CancellationToken ct = default);
    Task<Result<TokenResponseDto>> LoginAsync(LoginDto dto, CancellationToken ct = default);
}
