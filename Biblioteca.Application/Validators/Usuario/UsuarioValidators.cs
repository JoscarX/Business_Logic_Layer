using Biblioteca.Application.DTOs.Usuario;
using FluentValidation;

namespace Biblioteca.Application.Validators.Usuario;

public sealed class CrearUsuarioValidator : AbstractValidator<CrearUsuarioDto>
{
    public CrearUsuarioValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres.");

        RuleFor(x => x.Cedula)
            .NotEmpty().WithMessage("La cédula es obligatoria.")
            .MaximumLength(20).WithMessage("La cédula no puede exceder 20 caracteres.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es obligatoria.")
            .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
            .Matches("[A-Z]").WithMessage("La contraseña debe contener al menos una letra mayúscula.")
            .Matches("[0-9]").WithMessage("La contraseña debe contener al menos un número.");

        RuleFor(x => x.RolId)
            .GreaterThan(0).WithMessage("El rol seleccionado no es válido.");
    }
}

public sealed class ActualizarUsuarioValidator : AbstractValidator<ActualizarUsuarioDto>
{
    public ActualizarUsuarioValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres.");

        RuleFor(x => x.RolId)
            .GreaterThan(0).WithMessage("El rol seleccionado no es válido.");
    }
}

public sealed class CambiarPasswordValidator : AbstractValidator<CambiarPasswordDto>
{
    public CambiarPasswordValidator()
    {
        RuleFor(x => x.PasswordActual)
            .NotEmpty().WithMessage("La contraseña actual es obligatoria.");

        RuleFor(x => x.NuevoPassword)
            .NotEmpty().WithMessage("La nueva contraseña es obligatoria.")
            .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
            .Matches("[A-Z]").WithMessage("La contraseña debe contener al menos una letra mayúscula.")
            .Matches("[0-9]").WithMessage("La contraseña debe contener al menos un número.");

        RuleFor(x => x.ConfirmarPassword)
            .Equal(x => x.NuevoPassword).WithMessage("Las contraseñas no coinciden.");
    }
}
