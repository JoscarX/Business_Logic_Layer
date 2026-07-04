namespace Biblioteca.Application.Common.Responses;

/// <summary>
/// Envoltorio estándar para todas las respuestas de la API.
/// Garantiza una estructura uniforme sin importar el endpoint.
/// </summary>
/// <typeparam name="T">Tipo del dato retornado.</typeparam>
public sealed class ApiResponse<T>
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public T? Data { get; init; }
    public IEnumerable<string>? Errors { get; init; }
    public int StatusCode { get; init; }

    private ApiResponse() { }

    /// <summary>Crea una respuesta exitosa con datos.</summary>
    public static ApiResponse<T> Ok(T data, string? message = null) => new()
    {
        Success = true,
        Data = data,
        Message = message,
        StatusCode = 200
    };

    /// <summary>Crea una respuesta de creación exitosa (201).</summary>
    public static ApiResponse<T> Created(T data, string? message = null) => new()
    {
        Success = true,
        Data = data,
        Message = message ?? "Recurso creado exitosamente.",
        StatusCode = 201
    };

    /// <summary>Crea una respuesta de error con mensaje y código de estado.</summary>
    public static ApiResponse<T> Failure(string error, int statusCode = 400) => new()
    {
        Success = false,
        Message = error,
        Errors = [error],
        StatusCode = statusCode
    };

    /// <summary>Crea una respuesta de error con múltiples mensajes de validación.</summary>
    public static ApiResponse<T> ValidationError(IEnumerable<string> errors) => new()
    {
        Success = false,
        Message = "La solicitud contiene errores de validación.",
        Errors = errors,
        StatusCode = 422
    };

    /// <summary>Crea una respuesta 404 Not Found.</summary>
    public static ApiResponse<T> NotFound(string message) => new()
    {
        Success = false,
        Message = message,
        Errors = [message],
        StatusCode = 404
    };
}

/// <summary>Respuesta estándar sin datos (operaciones void).</summary>
public sealed class ApiResponse
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public IEnumerable<string>? Errors { get; init; }
    public int StatusCode { get; init; }

    public static ApiResponse Ok(string? message = null) => new()
    {
        Success = true,
        Message = message ?? "Operación realizada exitosamente.",
        StatusCode = 200
    };

    public static ApiResponse Failure(string error, int statusCode = 400) => new()
    {
        Success = false,
        Message = error,
        Errors = [error],
        StatusCode = statusCode
    };

    public static ApiResponse ValidationError(IEnumerable<string> errors) => new()
    {
        Success = false,
        Message = "La solicitud contiene errores de validación.",
        Errors = errors,
        StatusCode = 422
    };

    public static ApiResponse NotFound(string message) => new()
    {
        Success = false,
        Message = message,
        Errors = [message],
        StatusCode = 404
    };
}
