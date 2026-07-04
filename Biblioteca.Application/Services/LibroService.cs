using AutoMapper;
using Biblioteca.Application.Common.Interfaces;
using Biblioteca.Application.Common.Results;
using Biblioteca.Application.DTOs.Libro;
using Biblioteca.Application.Interfaces;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Biblioteca.Application.Services;

/// <summary>
/// LibroService orquesta los casos de uso del libro.
/// Las operaciones de stock delegan en los métodos del dominio:
/// Libro.AgregarExistencias(), Libro.ActualizarPrecio(), etc.
/// </summary>
public sealed class LibroService : ILibroService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<LibroService> _logger;

    public LibroService(IUnitOfWork uow, IMapper mapper, IDateTimeProvider dateTimeProvider, ILogger<LibroService> logger)
    {
        _uow = uow;
        _mapper = mapper;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<LibroResponseDto>>> ObtenerTodosAsync(CancellationToken ct = default)
    {
        var libros = await _uow.Libros.ObtenerTodosConCategoriasAsync(ct);
        return Result<IEnumerable<LibroResponseDto>>.Success(_mapper.Map<IEnumerable<LibroResponseDto>>(libros));
    }

    public async Task<Result<LibroResponseDto>> ObtenerPorIdAsync(int id, CancellationToken ct = default)
    {
        var libro = await _uow.Libros.ObtenerPorIdConCategoriasAsync(id, ct);
        if (libro is null)
            return Result<LibroResponseDto>.NotFound($"No se encontró el libro con Id {id}.");

        return Result<LibroResponseDto>.Success(_mapper.Map<LibroResponseDto>(libro));
    }

    public async Task<Result<IEnumerable<LibroResponseDto>>> ObtenerDisponiblesAsync(CancellationToken ct = default)
    {
        var libros = await _uow.Libros.ObtenerDisponiblesAsync(ct);
        return Result<IEnumerable<LibroResponseDto>>.Success(_mapper.Map<IEnumerable<LibroResponseDto>>(libros));
    }

    public async Task<Result<LibroResponseDto>> ObtenerPorIsbnAsync(string isbn, CancellationToken ct = default)
    {
        var libro = await _uow.Libros.ObtenerPorIsbnAsync(isbn, ct);
        if (libro is null)
            return Result<LibroResponseDto>.NotFound($"No se encontró el libro con ISBN '{isbn}'.");

        return Result<LibroResponseDto>.Success(_mapper.Map<LibroResponseDto>(libro));
    }

    public async Task<Result<LibroResponseDto>> CrearAsync(CrearLibroDto dto, CancellationToken ct = default)
    {
        if (!string.IsNullOrWhiteSpace(dto.Isbn) && await _uow.Libros.ExistePorIsbnAsync(dto.Isbn, ct))
            return Result<LibroResponseDto>.Conflict($"Ya existe un libro con el ISBN '{dto.Isbn}'.");

        if (await _uow.Autores.ObtenerPorIdAsync(dto.AutorId, ct) is null)
            return Result<LibroResponseDto>.ValidationError($"El autor con Id {dto.AutorId} no existe.");

        if (await _uow.Editoriales.ObtenerPorIdAsync(dto.EditorialId, ct) is null)
            return Result<LibroResponseDto>.ValidationError($"La editorial con Id {dto.EditorialId} no existe.");

        var libro = Libro.Crear(dto.Nombre, dto.Paginas, dto.CantidadInicial, dto.Precio,
            dto.AutorId, dto.EditorialId, dto.Sinopsis, dto.Isbn);
        libro.EstablecerCreacion(_dateTimeProvider.Ahora);

        // Asociar categorías
        foreach (var categoriaId in dto.CategoriaIds.Distinct())
        {
            var categoria = await _uow.Categorias.ObtenerPorIdAsync(categoriaId, ct);
            if (categoria is null)
                return Result<LibroResponseDto>.ValidationError($"La categoría con Id {categoriaId} no existe.");
        }

        await _uow.Libros.AgregarAsync(libro, ct);
        await _uow.GuardarCambiosAsync(ct);

        _logger.LogInformation("Libro '{TituloLibro}' creado con Id {LibroId}.", libro.Nombre, libro.Id);
        return Result<LibroResponseDto>.Success(_mapper.Map<LibroResponseDto>(
            await _uow.Libros.ObtenerPorIdConCategoriasAsync(libro.Id, ct) ?? libro));
    }

    public async Task<Result> ActualizarAsync(int id, ActualizarLibroDto dto, CancellationToken ct = default)
    {
        var libro = await _uow.Libros.ObtenerPorIdConCategoriasAsync(id, ct);
        if (libro is null)
            return Result.NotFound($"No se encontró el libro con Id {id}.");

        if (await _uow.Autores.ObtenerPorIdAsync(dto.AutorId, ct) is null)
            return Result.ValidationError($"El autor con Id {dto.AutorId} no existe.");

        if (await _uow.Editoriales.ObtenerPorIdAsync(dto.EditorialId, ct) is null)
            return Result.ValidationError($"La editorial con Id {dto.EditorialId} no existe.");

        libro.Actualizar(dto.Nombre, dto.Paginas, dto.Precio, dto.AutorId, dto.EditorialId, dto.Sinopsis, dto.Isbn);
        libro.EstablecerModificacion(_dateTimeProvider.Ahora);
        _uow.Libros.Actualizar(libro);
        await _uow.GuardarCambiosAsync(ct);

        return Result.Success();
    }

    public async Task<Result> AgregarExistenciasAsync(int id, AgregarExistenciasDto dto, CancellationToken ct = default)
    {
        var libro = await _uow.Libros.ObtenerPorIdAsync(id, ct);
        if (libro is null)
            return Result.NotFound($"No se encontró el libro con Id {id}.");

        libro.AgregarExistencias(dto.Cantidad); // ← lógica en la entidad
        libro.EstablecerModificacion(_dateTimeProvider.Ahora);
        _uow.Libros.Actualizar(libro);
        await _uow.GuardarCambiosAsync(ct);

        _logger.LogInformation("Se agregaron {Cantidad} existencias al libro Id {LibroId}.", dto.Cantidad, id);
        return Result.Success();
    }

    public async Task<Result> ActualizarPrecioAsync(int id, ActualizarPrecioDto dto, CancellationToken ct = default)
    {
        var libro = await _uow.Libros.ObtenerPorIdAsync(id, ct);
        if (libro is null)
            return Result.NotFound($"No se encontró el libro con Id {id}.");

        libro.ActualizarPrecio(dto.NuevoPrecio); // ← lógica en la entidad
        libro.EstablecerModificacion(_dateTimeProvider.Ahora);
        _uow.Libros.Actualizar(libro);
        await _uow.GuardarCambiosAsync(ct);

        return Result.Success();
    }

    public async Task<Result> EliminarAsync(int id, CancellationToken ct = default)
    {
        var libro = await _uow.Libros.ObtenerPorIdAsync(id, ct);
        if (libro is null)
            return Result.NotFound($"No se encontró el libro con Id {id}.");

        _uow.Libros.Eliminar(libro);
        await _uow.GuardarCambiosAsync(ct);

        return Result.Success();
    }
}
