using AutoMapper;
using Biblioteca.Application.Common.Interfaces;
using Biblioteca.Application.Common.Results;
using Biblioteca.Application.DTOs.Entrada;
using Biblioteca.Application.Interfaces;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Biblioteca.Application.Services;

public sealed class EntradaService : IEntradaService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<EntradaService> _logger;

    public EntradaService(IUnitOfWork uow, IMapper mapper, IDateTimeProvider dateTimeProvider, ILogger<EntradaService> logger)
    {
        _uow = uow;
        _mapper = mapper;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<EntradaResponseDto>>> ObtenerTodosAsync(CancellationToken ct = default)
    {
        var entradas = await _uow.Entradas.ObtenerTodosAsync(ct);
        return Result<IEnumerable<EntradaResponseDto>>.Success(_mapper.Map<IEnumerable<EntradaResponseDto>>(entradas));
    }

    public async Task<Result<IEnumerable<EntradaResponseDto>>> ObtenerActivasAsync(CancellationToken ct = default)
    {
        var entradas = await _uow.Entradas.ObtenerActivasAsync(ct);
        return Result<IEnumerable<EntradaResponseDto>>.Success(_mapper.Map<IEnumerable<EntradaResponseDto>>(entradas));
    }

    public async Task<Result<EntradaResponseDto>> ObtenerPorIdAsync(int id, CancellationToken ct = default)
    {
        var entrada = await _uow.Entradas.ObtenerPorIdAsync(id, ct);
        if (entrada is null)
            return Result<EntradaResponseDto>.NotFound($"No se encontró la entrada con Id {id}.");

        return Result<EntradaResponseDto>.Success(_mapper.Map<EntradaResponseDto>(entrada));
    }

    public async Task<Result<IEnumerable<EntradaResponseDto>>> ObtenerPorVisitanteAsync(int visitanteId, CancellationToken ct = default)
    {
        var entradas = await _uow.Entradas.ObtenerPorVisitanteAsync(visitanteId, ct);
        return Result<IEnumerable<EntradaResponseDto>>.Success(_mapper.Map<IEnumerable<EntradaResponseDto>>(entradas));
    }

    public async Task<Result<EntradaResponseDto>> RegistrarEntradaAsync(CrearEntradaDto dto, CancellationToken ct = default)
    {
        var visitante = await _uow.Visitantes.ObtenerPorIdAsync(dto.VisitanteId, ct);
        if (visitante is null)
            return Result<EntradaResponseDto>.NotFound($"No se encontró el visitante con Id {dto.VisitanteId}.");

        // Verificar que no tenga una entrada activa
        var entradaActiva = await _uow.Entradas.ObtenerEntradaActivaPorVisitanteAsync(dto.VisitanteId, ct);
        if (entradaActiva is not null)
            return Result<EntradaResponseDto>.Conflict("El visitante ya tiene una entrada activa en la biblioteca.");

        var entrada = Entrada.Crear(dto.VisitanteId, _dateTimeProvider.Ahora);
        entrada.EstablecerCreacion(_dateTimeProvider.Ahora);

        await _uow.Entradas.AgregarAsync(entrada, ct);
        await _uow.GuardarCambiosAsync(ct);

        _logger.LogInformation("Entrada registrada para visitante Id {VisitanteId}.", dto.VisitanteId);
        return Result<EntradaResponseDto>.Success(_mapper.Map<EntradaResponseDto>(entrada));
    }

    public async Task<Result> RegistrarSalidaAsync(int id, RegistrarSalidaDto dto, CancellationToken ct = default)
    {
        var entrada = await _uow.Entradas.ObtenerPorIdAsync(id, ct);
        if (entrada is null)
            return Result.NotFound($"No se encontró la entrada con Id {id}.");

        entrada.RegistrarSalida(dto.FechaSalida); // ← lógica en la entidad
        _uow.Entradas.Actualizar(entrada);
        await _uow.GuardarCambiosAsync(ct);

        return Result.Success();
    }
}
