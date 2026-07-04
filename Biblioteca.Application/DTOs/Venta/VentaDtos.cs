using Biblioteca.Domain.Enums;

namespace Biblioteca.Application.DTOs.Venta;

public sealed record CrearDetalleVentaDto(
    int LibroId,
    int Cantidad);

public sealed record CrearVentaDto(
    int VisitanteId,
    decimal Descuento,
    IEnumerable<CrearDetalleVentaDto> Detalles);

public sealed record DetalleVentaResponseDto(
    int Id,
    int LibroId,
    string TituloLibro,
    int Cantidad,
    decimal Precio,
    decimal Total);

public sealed record VentaResponseDto(
    int Id,
    int VisitanteId,
    string NombreVisitante,
    DateTime Fecha,
    decimal Subtotal,
    decimal Descuento,
    decimal Total,
    string Estado,
    IEnumerable<DetalleVentaResponseDto> Detalles);
