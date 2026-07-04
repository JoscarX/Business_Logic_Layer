using AutoMapper;
using Biblioteca.Application.Common.Results;
using Biblioteca.Application.DTOs.Rol;
using Biblioteca.Application.Interfaces;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Biblioteca.Application.Services;

/// <summary>
/// Servicio de aplicación para gestión de roles.
/// Orquesta casos de uso; no contiene lógica de negocio.
/// </summary>
public sealed class RolService : IRolService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly ILogger<RolService> _logger;

    public RolService(IUnitOfWork uow, IMapper mapper, ILogger<RolService> logger)
    {
        _uow = uow;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<RolResponseDto>>> ObtenerTodosAsync(CancellationToken ct = default)
    {
        var roles = await _uow.Roles.ObtenerTodosAsync(ct);
        return Result<IEnumerable<RolResponseDto>>.Success(_mapper.Map<IEnumerable<RolResponseDto>>(roles));
    }

    public async Task<Result<RolResponseDto>> ObtenerPorIdAsync(int id, CancellationToken ct = default)
    {
        var rol = await _uow.Roles.ObtenerPorIdAsync(id, ct);
        if (rol is null)
        {
            _logger.LogWarning("Rol con Id {RolId} no encontrado.", id);
            return Result<RolResponseDto>.NotFound($"No se encontró el rol con Id {id}.");
        }

        return Result<RolResponseDto>.Success(_mapper.Map<RolResponseDto>(rol));
    }

    public async Task<Result<RolResponseDto>> CrearAsync(CrearRolDto dto, CancellationToken ct = default)
    {
        var existe = await _uow.Roles.ExistePorNombreAsync(dto.Nombre, ct);
        if (existe)
            return Result<RolResponseDto>.Conflict($"Ya existe un rol con el nombre '{dto.Nombre}'.");

        var rol = Rol.Crear(dto.Nombre, dto.Descripcion);
        await _uow.Roles.AgregarAsync(rol, ct);
        await _uow.GuardarCambiosAsync(ct);

        _logger.LogInformation("Rol '{NombreRol}' creado con Id {RolId}.", rol.Nombre, rol.Id);
        return Result<RolResponseDto>.Success(_mapper.Map<RolResponseDto>(rol));
    }

    public async Task<Result> ActualizarAsync(int id, ActualizarRolDto dto, CancellationToken ct = default)
    {
        var rol = await _uow.Roles.ObtenerPorIdAsync(id, ct);
        if (rol is null)
            return Result.NotFound($"No se encontró el rol con Id {id}.");

        var nombreOcupado = await _uow.Roles.ExistePorNombreAsync(dto.Nombre, ct);
        if (nombreOcupado && !rol.Nombre.Equals(dto.Nombre, StringComparison.OrdinalIgnoreCase))
            return Result.Conflict($"Ya existe un rol con el nombre '{dto.Nombre}'.");

        rol.Actualizar(dto.Nombre, dto.Descripcion);
        _uow.Roles.Actualizar(rol);
        await _uow.GuardarCambiosAsync(ct);

        return Result.Success();
    }

    public async Task<Result> EliminarAsync(int id, CancellationToken ct = default)
    {
        var rol = await _uow.Roles.ObtenerPorIdAsync(id, ct);
        if (rol is null)
            return Result.NotFound($"No se encontró el rol con Id {id}.");

        _uow.Roles.Eliminar(rol);
        await _uow.GuardarCambiosAsync(ct);

        return Result.Success();
    }
}
