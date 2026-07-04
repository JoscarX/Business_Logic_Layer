namespace Biblioteca.Application.Common.Interfaces;

/// <summary>
/// Abstracción para el hashing de contraseñas.
/// Permite intercambiar la implementación (BCrypt, Argon2, etc.) sin afectar Application.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>Genera un hash seguro a partir de una contraseña en texto plano.</summary>
    string Hash(string password);

    /// <summary>Verifica si una contraseña en texto plano coincide con su hash.</summary>
    bool Verificar(string password, string hash);
}
