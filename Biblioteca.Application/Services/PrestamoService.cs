using AutoMapper;
using Biblioteca.Application.Common.Interfaces;
using Biblioteca.Application.Common.Results;
using Biblioteca.Application.DTOs.Prestamo;
using Biblioteca.Application.Interfaces;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Enums;
using Biblioteca.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Biblioteca.Application.Services;

/// <summary>
/// PrestamoService coordina el caso de uso de préstamos.
/// La entidad Libro.Prestar() y Prestamo.MarcarComoDevuelto() contienen la lógica de negocio.
/// </summary>
public sealed class PrestamoService : IPrestamoService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<PrestamoService> _logger;

    public PrestamoService(IUnitOfWork uow, IMapper mapper, IDateTimeProvider dateTimeProvider, ILogger<PrestamoService> logger)
    {
        _uow = uow;
        _mapper = mapper;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<PrestamoResponseDto>>> ObtenerTodosAsync(CancellationToken ct = default)
    {
        var prestamos = await _uow.Prestamos.ObtenerTodosAsync(ct);
        return Result<IEnumerable<PrestamoResponseDto>>.Success(_mapper.Map<IEnumerable<PrestamoResponseDto>>(prestamos));
    }

    public async Task<Result<PrestamoResponseDto>> ObtenerPorIdAsync(int id, CancellationToken ct = default)
    {
        var prestamo = await _uow.Prestamos.ObtenerPorIdAsync(id, ct);
        if (prestamo is null)
            return Result<PrestamoResponseDto>.NotFound($"No se encontró el préstamo con Id {id}.");

        return Result<PrestamoResponseDto>.Success(_mapper.Map<PrestamoResponseDto>(prestamo));
    }

    public async Task<Result<IEnumerable<PrestamoResponseDto>>> ObtenerActivosAsync(CancellationToken ct = default)
    {
        var prestamos = await _uow.Prestamos.ObtenerActivosAsync(ct);
        return Result<IEnumerable<PrestamoResponseDto>>.Success(_mapper.Map<IEnumerable<PrestamoResponseDto>>(prestamos));
    }

    public async Task<Result<IEnumerable<PrestamoResponseDto>>> ObtenerVencidosAsync(CancellationToken ct = default)
    {
        var prestamos = await _uow.Prestamos.ObtenerVencidosAsync(_dateTimeProvider.Ahora, ct);
        return Result<IEnumerable<PrestamoResponseDto>>.Success(_mapper.Map<IEnumerable<PrestamoResponseDto>>(prestamos));
    }

    public async Task<Result<IEnumerable<PrestamoResponseDto>>> ObtenerPorVisitanteAsync(int visitanteId, CancellationToken ct = default)
    {
        var prestamos = await _uow.Prestamos.ObtenerPorVisitanteAsync(visitanteId, ct);
        return Result<IEnumerable<PrestamoResponseDto>>.Success(_mapper.Map<IEnumerable<PrestamoResponseDto>>(prestamos));
    }

    public async Task<Result<PrestamoResponseDto>> CrearAsync(CrearPrestamoDto dto, CancellationToken ct = default)
    {
        var visitante = await _uow.Visitantes.ObtenerPorIdAsync(dto.VisitanteId, ct);
        if (visitante is null)
            return Result<PrestamoResponseDto>.NotFound($"No se encontró el visitante con Id {dto.VisitanteId}.");

        var libro = await _uow.Libros.ObtenerPorIdAsync(dto.LibroId, ct);
        if (libro is null)
            return Result<PrestamoResponseDto>.NotFound($"No se encontró el libro con Id {dto.LibroId}.");

        var tienePrestamo = await _uow.Prestamos.TienePrestamoActivoPorLibroAsync(dto.VisitanteId, dto.LibroId, ct);
        if (tienePrestamo)
            return Result<PrestamoResponseDto>.Conflict("El visitante ya tiene un préstamo activo para este libro.");

        // La entidad valida la disponibilidad y reduce el stock
        libro.Prestar(); // ← LibroNoDisponibleException si no hay stock

        var prestamo = Prestamo.Crear(dto.VisitanteId, dto.LibroId, _dateTimeProvider.Ahora, dto.FechaEntrega);
        prestamo.EstablecerCreacion(_dateTimeProvider.Ahora);

        _uow.Libros.Actualizar(libro);
        await _uow.Prestamos.AgregarAsync(prestamo, ct);
        await _uow.GuardarCambiosAsync(ct);

        _logger.LogInformation("Préstamo creado: Libro {LibroId} → Visitante {VisitanteId}.", dto.LibroId, dto.VisitanteId);
        return Result<PrestamoResponseDto>.Success(_mapper.Map<PrestamoResponseDto>(prestamo));
    }

    public async Task<Result> MarcarComoDevueltoAsync(int id, CancellationToken ct = default)
    {
        var prestamo = await _uow.Prestamos.ObtenerPorIdAsync(id, ct);
        if (prestamo is null)
            return Result.NotFound($"No se encontró el préstamo con Id {id}.");

        var libro = await _uow.Libros.ObtenerPorIdAsync(prestamo.LibroId, ct);
        if (libro is null)
            return Result.NotFound($"No se encontró el libro asociado al préstamo.");

        prestamo.MarcarComoDevuelto(_dateTimeProvider.Ahora); // ← lógica en la entidad
        libro.Devolver(); // ← restaura el stock

        _uow.Prestamos.Actualizar(prestamo);
        _uow.Libros.Actualizar(libro);
        await _uow.GuardarCambiosAsync(ct);

        _logger.LogInformation("Préstamo {PrestamoId} marcado como devuelto.", id);
        return Result.Success();
    }

    public async Task<Result> ProcesarVencimientosAsync(CancellationToken ct = default)
    {
        var vencidos = await _uow.Prestamos.ObtenerVencidosAsync(_dateTimeProvider.Ahora, ct);
        var lista = vencidos.ToList();

        foreach (var prestamo in lista)
        {
            prestamo.MarcarComoVencido(); // ← lógica en la entidad
            _uow.Prestamos.Actualizar(prestamo);
        }

        if (lista.Count > 0)
        {
            await _uow.GuardarCambiosAsync(ct);
            _logger.LogInformation("Se procesaron {CantidadVencidos} préstamos vencidos.", lista.Count);
        }

        return Result.Success();
    }
}
