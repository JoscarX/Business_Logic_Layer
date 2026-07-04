using Biblioteca.Application.DTOs.Autor;
using Biblioteca.Application.DTOs.Categoria;
using Biblioteca.Application.DTOs.Editorial;

namespace Biblioteca.Application.DTOs.Libro;

public sealed record CrearLibroDto(
    string Nombre,
    int Paginas,
    int CantidadInicial,
    decimal Precio,
    int AutorId,
    int EditorialId,
    string? Sinopsis,
    string? Isbn,
    IEnumerable<int> CategoriaIds);

public sealed record ActualizarLibroDto(
    string Nombre,
    int Paginas,
    decimal Precio,
    int AutorId,
    int EditorialId,
    string? Sinopsis,
    string? Isbn,
    IEnumerable<int> CategoriaIds);

public sealed record AgregarExistenciasDto(int Cantidad);

public sealed record ActualizarPrecioDto(decimal NuevoPrecio);

public sealed record LibroResponseDto(
    int Id,
    string Nombre,
    string? Sinopsis,
    int Paginas,
    int CantidadDisponible,
    decimal Precio,
    string? Isbn,
    AutorResponseDto Autor,
    EditorialResponseDto Editorial,
    IEnumerable<CategoriaResponseDto> Categorias);
