using AutoMapper;
using Biblioteca.Application.Common.Interfaces;
using Biblioteca.Application.Common.Results;
using Biblioteca.Application.DTOs.Categoria;
using Biblioteca.Application.Interfaces;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Biblioteca.Application.Services;

public sealed class CategoriaService : ICategoriaService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<CategoriaService> _logger;

    public CategoriaService(IUnitOfWork uow, IMapper mapper, IDateTimeProvider dateTimeProvider, ILogger<CategoriaService> logger)
    {
        _uow = uow;
        _mapper = mapper;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<CategoriaResponseDto>>> ObtenerTodosAsync(CancellationToken ct = default)
    {
        var categorias = await _uow.Categorias.ObtenerTodosAsync(ct);
        return Result<IEnumerable<CategoriaResponseDto>>.Success(_mapper.Map<IEnumerable<CategoriaResponseDto>>(categorias));
    }

    public async Task<Result<CategoriaResponseDto>> ObtenerPorIdAsync(int id, CancellationToken ct = default)
    {
        var categoria = await _uow.Categorias.ObtenerPorIdAsync(id, ct);
        if (categoria is null)
            return Result<CategoriaResponseDto>.NotFound($"No se encontró la categoría con Id {id}.");

        return Result<CategoriaResponseDto>.Success(_mapper.Map<CategoriaResponseDto>(categoria));
    }

    public async Task<Result<CategoriaResponseDto>> CrearAsync(CrearCategoriaDto dto, CancellationToken ct = default)
    {
        if (await _uow.Categorias.ExistePorNombreAsync(dto.Nombre, ct))
            return Result<CategoriaResponseDto>.Conflict($"Ya existe una categoría con el nombre '{dto.Nombre}'.");

        var categoria = Categoria.Crear(dto.Nombre);
        categoria.EstablecerCreacion(_dateTimeProvider.Ahora);

        await _uow.Categorias.AgregarAsync(categoria, ct);
        await _uow.GuardarCambiosAsync(ct);

        return Result<CategoriaResponseDto>.Success(_mapper.Map<CategoriaResponseDto>(categoria));
    }

    public async Task<Result> ActualizarAsync(int id, ActualizarCategoriaDto dto, CancellationToken ct = default)
    {
        var categoria = await _uow.Categorias.ObtenerPorIdAsync(id, ct);
        if (categoria is null)
            return Result.NotFound($"No se encontró la categoría con Id {id}.");

        categoria.Actualizar(dto.Nombre);
        categoria.EstablecerModificacion(_dateTimeProvider.Ahora);
        _uow.Categorias.Actualizar(categoria);
        await _uow.GuardarCambiosAsync(ct);

        return Result.Success();
    }

    public async Task<Result> EliminarAsync(int id, CancellationToken ct = default)
    {
        var categoria = await _uow.Categorias.ObtenerPorIdAsync(id, ct);
        if (categoria is null)
            return Result.NotFound($"No se encontró la categoría con Id {id}.");

        _uow.Categorias.Eliminar(categoria);
        await _uow.GuardarCambiosAsync(ct);

        return Result.Success();
    }
}
