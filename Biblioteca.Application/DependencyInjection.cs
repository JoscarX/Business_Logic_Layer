using Biblioteca.Application.Interfaces;
using Biblioteca.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Biblioteca.Application;

/// <summary>
/// Registra todos los servicios de la capa Application en el contenedor de DI.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // AutoMapper — escanea todos los profiles en el assembly
        services.AddAutoMapper(cfg => cfg.AddMaps(typeof(DependencyInjection).Assembly));

        // FluentValidation — registra todos los validators en el assembly
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, lifetime: ServiceLifetime.Scoped);

        // Servicios de aplicación
        services.AddScoped<IRolService, RolService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IVisitanteService, VisitanteService>();
        services.AddScoped<IAutorService, AutorService>();
        services.AddScoped<IEditorialService, EditorialService>();
        services.AddScoped<ICategoriaService, CategoriaService>();
        services.AddScoped<ILibroService, LibroService>();
        services.AddScoped<IEntradaService, EntradaService>();
        services.AddScoped<IPrestamoService, PrestamoService>();
        services.AddScoped<IVentaService, VentaService>();
        services.AddScoped<IAuditoriaService, AuditoriaService>();

        return services;
    }
}
