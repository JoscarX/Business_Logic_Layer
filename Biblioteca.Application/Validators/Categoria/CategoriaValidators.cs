using Biblioteca.Application.DTOs.Categoria;
using FluentValidation;

namespace Biblioteca.Application.Validators.Categoria;

public sealed class CrearCategoriaValidator : AbstractValidator<CrearCategoriaDto>
{
    public CrearCategoriaValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre de la categoría es obligatorio.")
            .MaximumLength(50).WithMessage("El nombre no puede exceder 50 caracteres.");
    }
}

public sealed class ActualizarCategoriaValidator : AbstractValidator<ActualizarCategoriaDto>
{
    public ActualizarCategoriaValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre de la categoría es obligatorio.")
            .MaximumLength(50).WithMessage("El nombre no puede exceder 50 caracteres.");
    }
}
