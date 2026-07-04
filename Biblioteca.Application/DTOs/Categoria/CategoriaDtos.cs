namespace Biblioteca.Application.DTOs.Categoria;

public sealed record CrearCategoriaDto(string Nombre);

public sealed record ActualizarCategoriaDto(string Nombre);

public sealed record CategoriaResponseDto(int Id, string Nombre);
