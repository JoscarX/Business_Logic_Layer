using AutoMapper;
using Biblioteca.Application.Common.Interfaces;
using Biblioteca.Application.Common.Results;
using Biblioteca.Application.DTOs.Visitante;
using Biblioteca.Application.Interfaces;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Biblioteca.Application.Services;

public sealed class VisitanteService : IVisitanteService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<VisitanteService> _logger;

    public VisitanteService(
        IUnitOfWork uow,
        IMapper mapper,
        IDateTimeProvider dateTimeProvider,
        ILogger<VisitanteService> logger)
    {
        _uow = uow;
        _mapper = mapper;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<VisitanteResponseDto>>> ObtenerTodosAsync(CancellationToken ct = default)
    {
        var visitantes = await _uow.Visitantes.ObtenerTodosAsync(ct);
        return Result<IEnumerable<VisitanteResponseDto>>.Success(_mapper.Map<IEnumerable<VisitanteResponseDto>>(visitantes));
    }

    public async Task<Result<VisitanteResponseDto>> ObtenerPorIdAsync(int id, CancellationToken ct = default)
    {
        var visitante = await _uow.Visitantes.ObtenerPorIdAsync(id, ct);
        if (visitante is null)
            return Result<VisitanteResponseDto>.NotFound($"No se encontró el visitante con Id {id}.");

        return Result<VisitanteResponseDto>.Success(_mapper.Map<VisitanteResponseDto>(visitante));
    }

    public async Task<Result<VisitanteResponseDto>> CrearAsync(CrearVisitanteDto dto, CancellationToken ct = default)
    {
        if (await _uow.Visitantes.ExistePorCedulaAsync(dto.Cedula, ct))
            return Result<VisitanteResponseDto>.Conflict($"Ya existe un visitante con la cédula '{dto.Cedula}'.");

        if (!string.IsNullOrWhiteSpace(dto.Correo) && await _uow.Visitantes.ExistePorCorreoAsync(dto.Correo, ct))
            return Result<VisitanteResponseDto>.Conflict($"El correo '{dto.Correo}' ya está registrado.");

        var visitante = Visitante.Crear(dto.NombreCompleto, dto.Cedula, dto.Telefono, dto.Direccion, dto.Correo);
        visitante.EstablecerCreacion(_dateTimeProvider.Ahora);

        await _uow.Visitantes.AgregarAsync(visitante, ct);
        await _uow.GuardarCambiosAsync(ct);

        _logger.LogInformation("Visitante '{Nombre}' registrado con Id {VisitanteId}.", visitante.NombreCompleto, visitante.Id);
        return Result<VisitanteResponseDto>.Success(_mapper.Map<VisitanteResponseDto>(visitante));
    }

    public async Task<Result> ActualizarAsync(int id, ActualizarVisitanteDto dto, CancellationToken ct = default)
    {
        var visitante = await _uow.Visitantes.ObtenerPorIdAsync(id, ct);
        if (visitante is null)
            return Result.NotFound($"No se encontró el visitante con Id {id}.");

        visitante.ActualizarDatos(dto.NombreCompleto, dto.Telefono, dto.Direccion, dto.Correo);
        visitante.EstablecerModificacion(_dateTimeProvider.Ahora);
        _uow.Visitantes.Actualizar(visitante);
        await _uow.GuardarCambiosAsync(ct);

        return Result.Success();
    }

    public async Task<Result> EliminarAsync(int id, CancellationToken ct = default)
    {
        var visitante = await _uow.Visitantes.ObtenerPorIdAsync(id, ct);
        if (visitante is null)
            return Result.NotFound($"No se encontró el visitante con Id {id}.");

        _uow.Visitantes.Eliminar(visitante);
        await _uow.GuardarCambiosAsync(ct);

        return Result.Success();
    }
}
