using AutoMapper;
using Biblioteca.Application.Common.Interfaces;
using Biblioteca.Application.Common.Results;
using Biblioteca.Application.DTOs.Editorial;
using Biblioteca.Application.Interfaces;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Biblioteca.Application.Services;

public sealed class EditorialService : IEditorialService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<EditorialService> _logger;

    public EditorialService(IUnitOfWork uow, IMapper mapper, IDateTimeProvider dateTimeProvider, ILogger<EditorialService> logger)
    {
        _uow = uow;
        _mapper = mapper;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<EditorialResponseDto>>> ObtenerTodosAsync(CancellationToken ct = default)
    {
        var editoriales = await _uow.Editoriales.ObtenerTodosAsync(ct);
        return Result<IEnumerable<EditorialResponseDto>>.Success(_mapper.Map<IEnumerable<EditorialResponseDto>>(editoriales));
    }

    public async Task<Result<EditorialResponseDto>> ObtenerPorIdAsync(int id, CancellationToken ct = default)
    {
        var editorial = await _uow.Editoriales.ObtenerPorIdAsync(id, ct);
        if (editorial is null)
            return Result<EditorialResponseDto>.NotFound($"No se encontró la editorial con Id {id}.");

        return Result<EditorialResponseDto>.Success(_mapper.Map<EditorialResponseDto>(editorial));
    }

    public async Task<Result<EditorialResponseDto>> CrearAsync(CrearEditorialDto dto, CancellationToken ct = default)
    {
        if (await _uow.Editoriales.ExistePorNombreAsync(dto.Nombre, ct))
            return Result<EditorialResponseDto>.Conflict($"Ya existe una editorial con el nombre '{dto.Nombre}'.");

        var editorial = Editorial.Crear(dto.Nombre, dto.Pais);
        editorial.EstablecerCreacion(_dateTimeProvider.Ahora);

        await _uow.Editoriales.AgregarAsync(editorial, ct);
        await _uow.GuardarCambiosAsync(ct);

        return Result<EditorialResponseDto>.Success(_mapper.Map<EditorialResponseDto>(editorial));
    }

    public async Task<Result> ActualizarAsync(int id, ActualizarEditorialDto dto, CancellationToken ct = default)
    {
        var editorial = await _uow.Editoriales.ObtenerPorIdAsync(id, ct);
        if (editorial is null)
            return Result.NotFound($"No se encontró la editorial con Id {id}.");

        editorial.Actualizar(dto.Nombre, dto.Pais);
        editorial.EstablecerModificacion(_dateTimeProvider.Ahora);
        _uow.Editoriales.Actualizar(editorial);
        await _uow.GuardarCambiosAsync(ct);

        return Result.Success();
    }

    public async Task<Result> EliminarAsync(int id, CancellationToken ct = default)
    {
        var editorial = await _uow.Editoriales.ObtenerPorIdAsync(id, ct);
        if (editorial is null)
            return Result.NotFound($"No se encontró la editorial con Id {id}.");

        _uow.Editoriales.Eliminar(editorial);
        await _uow.GuardarCambiosAsync(ct);

        return Result.Success();
    }
}
