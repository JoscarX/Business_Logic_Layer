using AutoMapper;
using Biblioteca.Application.Common.Interfaces;
using Biblioteca.Application.Common.Results;
using Biblioteca.Application.DTOs.Autor;
using Biblioteca.Application.Interfaces;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Biblioteca.Application.Services;

public sealed class AutorService : IAutorService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<AutorService> _logger;

    public AutorService(IUnitOfWork uow, IMapper mapper, IDateTimeProvider dateTimeProvider, ILogger<AutorService> logger)
    {
        _uow = uow;
        _mapper = mapper;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<AutorResponseDto>>> ObtenerTodosAsync(CancellationToken ct = default)
    {
        var autores = await _uow.Autores.ObtenerTodosAsync(ct);
        return Result<IEnumerable<AutorResponseDto>>.Success(_mapper.Map<IEnumerable<AutorResponseDto>>(autores));
    }

    public async Task<Result<AutorResponseDto>> ObtenerPorIdAsync(int id, CancellationToken ct = default)
    {
        var autor = await _uow.Autores.ObtenerPorIdAsync(id, ct);
        if (autor is null)
            return Result<AutorResponseDto>.NotFound($"No se encontró el autor con Id {id}.");

        return Result<AutorResponseDto>.Success(_mapper.Map<AutorResponseDto>(autor));
    }

    public async Task<Result<AutorResponseDto>> CrearAsync(CrearAutorDto dto, CancellationToken ct = default)
    {
        var autor = Autor.Crear(dto.Nombre, dto.Biografia, dto.Pais, dto.FechaNacimiento);
        autor.EstablecerCreacion(_dateTimeProvider.Ahora);

        await _uow.Autores.AgregarAsync(autor, ct);
        await _uow.GuardarCambiosAsync(ct);

        _logger.LogInformation("Autor '{NombreAutor}' creado con Id {AutorId}.", autor.Nombre, autor.Id);
        return Result<AutorResponseDto>.Success(_mapper.Map<AutorResponseDto>(autor));
    }

    public async Task<Result> ActualizarAsync(int id, ActualizarAutorDto dto, CancellationToken ct = default)
    {
        var autor = await _uow.Autores.ObtenerPorIdAsync(id, ct);
        if (autor is null)
            return Result.NotFound($"No se encontró el autor con Id {id}.");

        autor.ActualizarInformacion(dto.Nombre, dto.Biografia, dto.Pais, dto.FechaNacimiento);
        autor.EstablecerModificacion(_dateTimeProvider.Ahora);
        _uow.Autores.Actualizar(autor);
        await _uow.GuardarCambiosAsync(ct);

        return Result.Success();
    }

    public async Task<Result> EliminarAsync(int id, CancellationToken ct = default)
    {
        var autor = await _uow.Autores.ObtenerPorIdAsync(id, ct);
        if (autor is null)
            return Result.NotFound($"No se encontró el autor con Id {id}.");

        _uow.Autores.Eliminar(autor);
        await _uow.GuardarCambiosAsync(ct);

        return Result.Success();
    }
}
