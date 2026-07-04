namespace Biblioteca.Application.Common.Interfaces;

/// <summary>
/// Abstracción del proveedor de fecha y hora.
/// Facilita pruebas unitarias y evita dependencia de DateTime.Now directamente.
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>Fecha y hora actual en UTC.</summary>
    DateTime AhoraUtc { get; }

    /// <summary>Fecha y hora actual en la zona horaria local del servidor.</summary>
    DateTime Ahora { get; }

    /// <summary>Fecha actual (sin hora) en UTC.</summary>
    DateOnly HoyUtc { get; }
}
