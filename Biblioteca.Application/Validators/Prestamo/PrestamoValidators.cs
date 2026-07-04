using Biblioteca.Application.DTOs.Prestamo;
using FluentValidation;

namespace Biblioteca.Application.Validators.Prestamo;

public sealed class CrearPrestamoValidator : AbstractValidator<CrearPrestamoDto>
{
    public CrearPrestamoValidator()
    {
        RuleFor(x => x.VisitanteId)
            .GreaterThan(0).WithMessage("El visitante es obligatorio.");

        RuleFor(x => x.LibroId)
            .GreaterThan(0).WithMessage("El libro es obligatorio.");

        RuleFor(x => x.FechaEntrega)
            .GreaterThan(DateTime.Today)
            .WithMessage("La fecha de entrega debe ser posterior a hoy.");
    }
}
