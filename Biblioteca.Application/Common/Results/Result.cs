namespace Biblioteca.Application.Common.Results;

/// <summary>
/// Patrón Result para evitar el uso de excepciones para flujos de control.
/// Un servicio retorna Result&lt;T&gt; en lugar de lanzar excepciones manejables.
/// </summary>
/// <typeparam name="T">Tipo del valor en caso de éxito.</typeparam>
public sealed class Result<T>
{
    public T? Value { get; }
    public string? Error { get; }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public ResultType Type { get; }

    private Result(T value)
    {
        Value = value;
        IsSuccess = true;
        Type = ResultType.Success;
    }

    private Result(string error, ResultType type)
    {
        Error = error;
        IsSuccess = false;
        Type = type;
    }

    /// <summary>Crea un resultado exitoso con el valor especificado.</summary>
    public static Result<T> Success(T value) => new(value);

    /// <summary>Crea un resultado de error genérico.</summary>
    public static Result<T> Failure(string error) => new(error, ResultType.Failure);

    /// <summary>Crea un resultado de recurso no encontrado.</summary>
    public static Result<T> NotFound(string error) => new(error, ResultType.NotFound);

    /// <summary>Crea un resultado de error de validación.</summary>
    public static Result<T> ValidationError(string error) => new(error, ResultType.ValidationError);

    /// <summary>Crea un resultado de conflicto (ya existe).</summary>
    public static Result<T> Conflict(string error) => new(error, ResultType.Conflict);
}

/// <summary>Versión de Result sin valor de retorno (operaciones void).</summary>
public sealed class Result
{
    public string? Error { get; }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public ResultType Type { get; }

    private Result()
    {
        IsSuccess = true;
        Type = ResultType.Success;
    }

    private Result(string error, ResultType type)
    {
        Error = error;
        IsSuccess = false;
        Type = type;
    }

    public static Result Success() => new();
    public static Result Failure(string error) => new(error, ResultType.Failure);
    public static Result NotFound(string error) => new(error, ResultType.NotFound);
    public static Result ValidationError(string error) => new(error, ResultType.ValidationError);
    public static Result Conflict(string error) => new(error, ResultType.Conflict);
}

/// <summary>Tipo de resultado para mapear al código HTTP apropiado en el controller.</summary>
public enum ResultType
{
    Success = 0,
    Failure = 1,
    NotFound = 2,
    ValidationError = 3,
    Conflict = 4
}
