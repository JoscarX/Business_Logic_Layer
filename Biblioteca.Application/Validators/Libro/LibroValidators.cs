using Biblioteca.Application.DTOs.Libro;
using FluentValidation;

namespace Biblioteca.Application.Validators.Libro;

public sealed class CrearLibroValidator : AbstractValidator<CrearLibroDto>
{
    public CrearLibroValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del libro es obligatorio.")
            .MaximumLength(120).WithMessage("El nombre no puede exceder 120 caracteres.");

        RuleFor(x => x.Paginas)
            .GreaterThan(0).WithMessage("El número de páginas debe ser mayor a cero.");

        RuleFor(x => x.CantidadInicial)
            .GreaterThanOrEqualTo(0).WithMessage("La cantidad inicial no puede ser negativa.");

        RuleFor(x => x.Precio)
            .GreaterThanOrEqualTo(0).WithMessage("El precio no puede ser negativo.");

        RuleFor(x => x.AutorId)
            .GreaterThan(0).WithMessage("El autor es obligatorio.");

        RuleFor(x => x.EditorialId)
            .GreaterThan(0).WithMessage("La editorial es obligatoria.");

        RuleFor(x => x.Sinopsis)
            .MaximumLength(500).WithMessage("La sinopsis no puede exceder 500 caracteres.")
            .When(x => x.Sinopsis is not null);

        RuleFor(x => x.Isbn)
            .MaximumLength(20).WithMessage("El ISBN no puede exceder 20 caracteres.")
            .When(x => x.Isbn is not null);

        RuleFor(x => x.CategoriaIds)
            .NotEmpty().WithMessage("El libro debe tener al menos una categoría.");
    }
}

public sealed class ActualizarLibroValidator : AbstractValidator<ActualizarLibroDto>
{
    public ActualizarLibroValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del libro es obligatorio.")
            .MaximumLength(120).WithMessage("El nombre no puede exceder 120 caracteres.");

        RuleFor(x => x.Paginas)
            .GreaterThan(0).WithMessage("El número de páginas debe ser mayor a cero.");

        RuleFor(x => x.Precio)
            .GreaterThanOrEqualTo(0).WithMessage("El precio no puede ser negativo.");

        RuleFor(x => x.AutorId)
            .GreaterThan(0).WithMessage("El autor es obligatorio.");

        RuleFor(x => x.EditorialId)
            .GreaterThan(0).WithMessage("La editorial es obligatoria.");

        RuleFor(x => x.CategoriaIds)
            .NotEmpty().WithMessage("El libro debe tener al menos una categoría.");
    }
}

public sealed class AgregarExistenciasValidator : AbstractValidator<AgregarExistenciasDto>
{
    public AgregarExistenciasValidator()
    {
        RuleFor(x => x.Cantidad)
            .GreaterThan(0).WithMessage("La cantidad a agregar debe ser mayor a cero.");
    }
}

public sealed class ActualizarPrecioValidator : AbstractValidator<ActualizarPrecioDto>
{
    public ActualizarPrecioValidator()
    {
        RuleFor(x => x.NuevoPrecio)
            .GreaterThanOrEqualTo(0).WithMessage("El precio no puede ser negativo.");
    }
}
