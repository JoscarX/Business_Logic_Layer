using AutoMapper;
using Biblioteca.Application.Common.Interfaces;
using Biblioteca.Application.Common.Results;
using Biblioteca.Application.DTOs.Venta;
using Biblioteca.Application.Interfaces;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Biblioteca.Application.Services;

/// <summary>
/// VentaService coordina las ventas.
/// La lógica de AgregarDetalle, CalcularTotal y Anular viven en la entidad Venta.
/// </summary>
public sealed class VentaService : IVentaService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<VentaService> _logger;

    public VentaService(IUnitOfWork uow, IMapper mapper, IDateTimeProvider dateTimeProvider, ILogger<VentaService> logger)
    {
        _uow = uow;
        _mapper = mapper;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<VentaResponseDto>>> ObtenerTodosAsync(CancellationToken ct = default)
    {
        var ventas = await _uow.Ventas.ObtenerConDetallesAsync(ct);
        return Result<IEnumerable<VentaResponseDto>>.Success(_mapper.Map<IEnumerable<VentaResponseDto>>(ventas));
    }

    public async Task<Result<VentaResponseDto>> ObtenerPorIdAsync(int id, CancellationToken ct = default)
    {
        var venta = await _uow.Ventas.ObtenerPorIdConDetallesAsync(id, ct);
        if (venta is null)
            return Result<VentaResponseDto>.NotFound($"No se encontró la venta con Id {id}.");

        return Result<VentaResponseDto>.Success(_mapper.Map<VentaResponseDto>(venta));
    }

    public async Task<Result<IEnumerable<VentaResponseDto>>> ObtenerPorVisitanteAsync(int visitanteId, CancellationToken ct = default)
    {
        var ventas = await _uow.Ventas.ObtenerPorVisitanteAsync(visitanteId, ct);
        return Result<IEnumerable<VentaResponseDto>>.Success(_mapper.Map<IEnumerable<VentaResponseDto>>(ventas));
    }

    public async Task<Result<VentaResponseDto>> CrearAsync(CrearVentaDto dto, CancellationToken ct = default)
    {
        var visitante = await _uow.Visitantes.ObtenerPorIdAsync(dto.VisitanteId, ct);
        if (visitante is null)
            return Result<VentaResponseDto>.NotFound($"No se encontró el visitante con Id {dto.VisitanteId}.");

        var venta = Venta.Crear(dto.VisitanteId, _dateTimeProvider.Ahora, dto.Descuento);
        venta.EstablecerCreacion(_dateTimeProvider.Ahora);

        foreach (var detalleDto in dto.Detalles)
        {
            var libro = await _uow.Libros.ObtenerPorIdAsync(detalleDto.LibroId, ct);
            if (libro is null)
                return Result<VentaResponseDto>.NotFound($"No se encontró el libro con Id {detalleDto.LibroId}.");

            // Entidad Libro valida stock y lanza StockInsuficienteException si es necesario
            libro.ReducirStockParaVenta(detalleDto.Cantidad);

            var detalle = DetalleVenta.Crear(detalleDto.LibroId, detalleDto.Cantidad, libro.Precio);
            venta.AgregarDetalle(detalle); // ← lógica en la entidad: recalcula totales

            _uow.Libros.Actualizar(libro);
        }

        venta.Completar(_dateTimeProvider.Ahora); // ← publica VentaRealizadaEvent
        await _uow.Ventas.AgregarAsync(venta, ct);
        await _uow.GuardarCambiosAsync(ct);

        _logger.LogInformation("Venta {VentaId} creada por visitante {VisitanteId}. Total: {Total}.",
            venta.Id, dto.VisitanteId, venta.Total);

        var ventaConDetalles = await _uow.Ventas.ObtenerPorIdConDetallesAsync(venta.Id, ct) ?? venta;
        return Result<VentaResponseDto>.Success(_mapper.Map<VentaResponseDto>(ventaConDetalles));
    }

    public async Task<Result> AnularAsync(int id, CancellationToken ct = default)
    {
        var venta = await _uow.Ventas.ObtenerPorIdConDetallesAsync(id, ct);
        if (venta is null)
            return Result.NotFound($"No se encontró la venta con Id {id}.");

        // Restaurar stock de cada libro
        foreach (var detalle in venta.Detalles)
        {
            var libro = await _uow.Libros.ObtenerPorIdAsync(detalle.LibroId, ct);
            if (libro is not null)
            {
                libro.AgregarExistencias(detalle.Cantidad);
                _uow.Libros.Actualizar(libro);
            }
        }

        venta.Anular(); // ← lógica en la entidad
        _uow.Ventas.Actualizar(venta);
        await _uow.GuardarCambiosAsync(ct);

        _logger.LogInformation("Venta {VentaId} anulada. Stock restaurado.", id);
        return Result.Success();
    }
}
