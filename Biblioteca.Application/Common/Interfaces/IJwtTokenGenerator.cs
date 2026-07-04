using Biblioteca.Domain.Entities;

namespace Biblioteca.Application.Common.Interfaces;

/// <summary>
/// Abstracción del generador de tokens JWT.
/// Infrastructure implementa esta interfaz; Application solo la consume.
/// </summary>
public interface IJwtTokenGenerator
{
    /// <summary>Genera un token JWT firmado para el usuario autenticado.</summary>
    string GenerarToken(Usuario usuario);
}
