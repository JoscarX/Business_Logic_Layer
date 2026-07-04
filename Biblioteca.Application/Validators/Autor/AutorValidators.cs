using Biblioteca.Application.DTOs.Autor;
using FluentValidation;

namespace Biblioteca.Application.Validators.Autor;

public sealed class CrearAutorValidator : AbstractValidator<CrearAutorDto>
{
    public CrearAutorValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del autor es obligatorio.")
            .MaximumLength(120).WithMessage("El nombre no puede exceder 120 caracteres.");

        RuleFor(x => x.Pais)
            .MaximumLength(80).WithMessage("El país no puede exceder 80 caracteres.")
            .When(x => x.Pais is not null);

        RuleFor(x => x.FechaNacimiento)
            .LessThan(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("La fecha de nacimiento debe ser anterior a hoy.")
            .When(x => x.FechaNacimiento.HasValue);
    }
}

public sealed class ActualizarAutorValidator : AbstractValidator<ActualizarAutorDto>
{
    public ActualizarAutorValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del autor es obligatorio.")
            .MaximumLength(120).WithMessage("El nombre no puede exceder 120 caracteres.");

        RuleFor(x => x.Pais)
            .MaximumLength(80).WithMessage("El país no puede exceder 80 caracteres.")
            .When(x => x.Pais is not null);
    }
}
