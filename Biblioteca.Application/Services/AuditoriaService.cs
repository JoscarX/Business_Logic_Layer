using AutoMapper;
using Biblioteca.Application.Common.Results;
using Biblioteca.Application.DTOs.Auditoria;
using Biblioteca.Application.Interfaces;
using Biblioteca.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Biblioteca.Application.Services;

public sealed class AuditoriaService : IAuditoriaService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly ILogger<AuditoriaService> _logger;

    public AuditoriaService(IUnitOfWork uow, IMapper mapper, ILogger<AuditoriaService> logger)
    {
        _uow = uow;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<AuditoriaResponseDto>>> ObtenerTodosAsync(CancellationToken ct = default)
    {
        var registros = await _uow.Auditorias.ObtenerTodosAsync(ct);
        return Result<IEnumerable<AuditoriaResponseDto>>.Success(_mapper.Map<IEnumerable<AuditoriaResponseDto>>(registros));
    }

    public async Task<Result<IEnumerable<AuditoriaResponseDto>>> ObtenerPorTablaAsync(string tabla, CancellationToken ct = default)
    {
        var registros = await _uow.Auditorias.ObtenerPorTablaAsync(tabla, ct);
        return Result<IEnumerable<AuditoriaResponseDto>>.Success(_mapper.Map<IEnumerable<AuditoriaResponseDto>>(registros));
    }

    public async Task<Result<IEnumerable<AuditoriaResponseDto>>> ObtenerPorUsuarioAsync(int usuarioId, CancellationToken ct = default)
    {
        var registros = await _uow.Auditorias.ObtenerPorUsuarioAsync(usuarioId, ct);
        return Result<IEnumerable<AuditoriaResponseDto>>.Success(_mapper.Map<IEnumerable<AuditoriaResponseDto>>(registros));
    }

    public async Task<Result<IEnumerable<AuditoriaResponseDto>>> ObtenerPorRangoFechaAsync(DateTime desde, DateTime hasta, CancellationToken ct = default)
    {
        if (desde > hasta)
            return Result<IEnumerable<AuditoriaResponseDto>>.ValidationError("La fecha 'desde' no puede ser posterior a 'hasta'.");

        var registros = await _uow.Auditorias.ObtenerPorRangoFechaAsync(desde, hasta, ct);
        return Result<IEnumerable<AuditoriaResponseDto>>.Success(_mapper.Map<IEnumerable<AuditoriaResponseDto>>(registros));
    }
}
