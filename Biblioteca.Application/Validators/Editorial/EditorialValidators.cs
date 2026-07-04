using Biblioteca.Application.DTOs.Editorial;
using FluentValidation;

namespace Biblioteca.Application.Validators.Editorial;

public sealed class CrearEditorialValidator : AbstractValidator<CrearEditorialDto>
{
    public CrearEditorialValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre de la editorial es obligatorio.")
            .MaximumLength(120).WithMessage("El nombre no puede exceder 120 caracteres.");

        RuleFor(x => x.Pais)
            .MaximumLength(80).WithMessage("El país no puede exceder 80 caracteres.")
            .When(x => x.Pais is not null);
    }
}

public sealed class ActualizarEditorialValidator : AbstractValidator<ActualizarEditorialDto>
{
    public ActualizarEditorialValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre de la editorial es obligatorio.")
            .MaximumLength(120).WithMessage("El nombre no puede exceder 120 caracteres.");
    }
}
