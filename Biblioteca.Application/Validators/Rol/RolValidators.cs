using Biblioteca.Application.DTOs.Rol;
using FluentValidation;

namespace Biblioteca.Application.Validators.Rol;

public sealed class CrearRolValidator : AbstractValidator<CrearRolDto>
{
    public CrearRolValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del rol es obligatorio.")
            .MaximumLength(50).WithMessage("El nombre del rol no puede exceder 50 caracteres.");

        RuleFor(x => x.Descripcion)
            .MaximumLength(200).WithMessage("La descripción no puede exceder 200 caracteres.")
            .When(x => x.Descripcion is not null);
    }
}

public sealed class ActualizarRolValidator : AbstractValidator<ActualizarRolDto>
{
    public ActualizarRolValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del rol es obligatorio.")
            .MaximumLength(50).WithMessage("El nombre del rol no puede exceder 50 caracteres.");

        RuleFor(x => x.Descripcion)
            .MaximumLength(200).WithMessage("La descripción no puede exceder 200 caracteres.")
            .When(x => x.Descripcion is not null);
    }
}
