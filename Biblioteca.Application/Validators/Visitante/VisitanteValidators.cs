using Biblioteca.Application.DTOs.Visitante;
using FluentValidation;

namespace Biblioteca.Application.Validators.Visitante;

public sealed class CrearVisitanteValidator : AbstractValidator<CrearVisitanteDto>
{
    public CrearVisitanteValidator()
    {
        RuleFor(x => x.NombreCompleto)
            .NotEmpty().WithMessage("El nombre completo es obligatorio.")
            .MaximumLength(120).WithMessage("El nombre no puede exceder 120 caracteres.");

        RuleFor(x => x.Cedula)
            .NotEmpty().WithMessage("La cédula es obligatoria.")
            .MaximumLength(20).WithMessage("La cédula no puede exceder 20 caracteres.");

        RuleFor(x => x.Telefono)
            .NotEmpty().WithMessage("El teléfono es obligatorio.")
            .MaximumLength(20).WithMessage("El teléfono no puede exceder 20 caracteres.");

        RuleFor(x => x.Direccion)
            .NotEmpty().WithMessage("La dirección es obligatoria.")
            .MaximumLength(200).WithMessage("La dirección no puede exceder 200 caracteres.");

        RuleFor(x => x.Correo)
            .EmailAddress().WithMessage("El formato del correo electrónico no es válido.")
            .MaximumLength(120).WithMessage("El correo no puede exceder 120 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.Correo));
    }
}

public sealed class ActualizarVisitanteValidator : AbstractValidator<ActualizarVisitanteDto>
{
    public ActualizarVisitanteValidator()
    {
        RuleFor(x => x.NombreCompleto)
            .NotEmpty().WithMessage("El nombre completo es obligatorio.")
            .MaximumLength(120).WithMessage("El nombre no puede exceder 120 caracteres.");

        RuleFor(x => x.Telefono)
            .NotEmpty().WithMessage("El teléfono es obligatorio.")
            .MaximumLength(20).WithMessage("El teléfono no puede exceder 20 caracteres.");

        RuleFor(x => x.Direccion)
            .NotEmpty().WithMessage("La dirección es obligatoria.")
            .MaximumLength(200).WithMessage("La dirección no puede exceder 200 caracteres.");

        RuleFor(x => x.Correo)
            .EmailAddress().WithMessage("El formato del correo electrónico no es válido.")
            .When(x => !string.IsNullOrWhiteSpace(x.Correo));
    }
}
