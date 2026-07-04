using Biblioteca.Application.DTOs.Venta;
using FluentValidation;

namespace Biblioteca.Application.Validators.Venta;

public sealed class CrearVentaValidator : AbstractValidator<CrearVentaDto>
{
    public CrearVentaValidator()
    {
        RuleFor(x => x.VisitanteId)
            .GreaterThan(0).WithMessage("El visitante es obligatorio.");

        RuleFor(x => x.Descuento)
            .GreaterThanOrEqualTo(0).WithMessage("El descuento no puede ser negativo.");

        RuleFor(x => x.Detalles)
            .NotEmpty().WithMessage("La venta debe tener al menos un artículo.");

        RuleForEach(x => x.Detalles).SetValidator(new CrearDetalleVentaValidator());
    }
}

public sealed class CrearDetalleVentaValidator : AbstractValidator<CrearDetalleVentaDto>
{
    public CrearDetalleVentaValidator()
    {
        RuleFor(x => x.LibroId)
            .GreaterThan(0).WithMessage("El libro del detalle es obligatorio.");

        RuleFor(x => x.Cantidad)
            .GreaterThan(0).WithMessage("La cantidad debe ser mayor a cero.");
    }
}
