using AutoMapper;
using Biblioteca.Application.Common.Interfaces;
using Biblioteca.Application.Common.Results;
using Biblioteca.Application.DTOs.Auth;
using Biblioteca.Application.DTOs.Usuario;
using Biblioteca.Application.Interfaces;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Biblioteca.Application.Services;

/// <summary>
/// Servicio de usuarios. Orquesta creación, autenticación y gestión de estados.
/// La lógica de negocio (Activar, Desactivar, CambiarPassword) delega en la entidad.
/// </summary>
public sealed class UsuarioService : IUsuarioService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtGenerator;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<UsuarioService> _logger;

    public UsuarioService(
        IUnitOfWork uow,
        IMapper mapper,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtGenerator,
        IDateTimeProvider dateTimeProvider,
        ILogger<UsuarioService> logger)
    {
        _uow = uow;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _jwtGenerator = jwtGenerator;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<UsuarioResponseDto>>> ObtenerTodosAsync(CancellationToken ct = default)
    {
        var usuarios = await _uow.Usuarios.ObtenerTodosAsync(ct);
        return Result<IEnumerable<UsuarioResponseDto>>.Success(_mapper.Map<IEnumerable<UsuarioResponseDto>>(usuarios));
    }

    public async Task<Result<UsuarioResponseDto>> ObtenerPorIdAsync(int id, CancellationToken ct = default)
    {
        var usuario = await _uow.Usuarios.ObtenerPorIdAsync(id, ct);
        if (usuario is null)
            return Result<UsuarioResponseDto>.NotFound($"No se encontró el usuario con Id {id}.");

        return Result<UsuarioResponseDto>.Success(_mapper.Map<UsuarioResponseDto>(usuario));
    }

    public async Task<Result<UsuarioResponseDto>> CrearAsync(CrearUsuarioDto dto, CancellationToken ct = default)
    {
        var cedulaExiste = await _uow.Usuarios.ExistePorCedulaAsync(dto.Cedula, ct);
        if (cedulaExiste)
            return Result<UsuarioResponseDto>.Conflict($"Ya existe un usuario con la cédula '{dto.Cedula}'.");

        var rolExiste = await _uow.Roles.ObtenerPorIdAsync(dto.RolId, ct);
        if (rolExiste is null)
            return Result<UsuarioResponseDto>.ValidationError($"El rol con Id {dto.RolId} no existe.");

        var hash = _passwordHasher.Hash(dto.Password);
        var usuario = Usuario.Crear(dto.Nombre, dto.Cedula, hash, dto.RolId);
        usuario.EstablecerCreacion(_dateTimeProvider.Ahora);

        await _uow.Usuarios.AgregarAsync(usuario, ct);
        await _uow.GuardarCambiosAsync(ct);

        _logger.LogInformation("Usuario '{NombreUsuario}' creado con Id {UsuarioId}.", usuario.Nombre, usuario.Id);
        return Result<UsuarioResponseDto>.Success(_mapper.Map<UsuarioResponseDto>(usuario));
    }

    public async Task<Result> ActualizarAsync(int id, ActualizarUsuarioDto dto, CancellationToken ct = default)
    {
        var usuario = await _uow.Usuarios.ObtenerPorIdAsync(id, ct);
        if (usuario is null)
            return Result.NotFound($"No se encontró el usuario con Id {id}.");

        var rolExiste = await _uow.Roles.ObtenerPorIdAsync(dto.RolId, ct);
        if (rolExiste is null)
            return Result.ValidationError($"El rol con Id {dto.RolId} no existe.");

        usuario.ActualizarPerfil(dto.Nombre, dto.RolId);
        usuario.EstablecerModificacion(_dateTimeProvider.Ahora);
        _uow.Usuarios.Actualizar(usuario);
        await _uow.GuardarCambiosAsync(ct);

        return Result.Success();
    }

    public async Task<Result> CambiarPasswordAsync(int id, CambiarPasswordDto dto, CancellationToken ct = default)
    {
        var usuario = await _uow.Usuarios.ObtenerPorIdAsync(id, ct);
        if (usuario is null)
            return Result.NotFound($"No se encontró el usuario con Id {id}.");

        if (!_passwordHasher.Verificar(dto.PasswordActual, usuario.PasswordHash))
            return Result.ValidationError("La contraseña actual no es correcta.");

        var nuevoHash = _passwordHasher.Hash(dto.NuevoPassword);
        usuario.CambiarPassword(nuevoHash); // ← lógica en la entidad
        usuario.EstablecerModificacion(_dateTimeProvider.Ahora);
        _uow.Usuarios.Actualizar(usuario);
        await _uow.GuardarCambiosAsync(ct);

        return Result.Success();
    }

    public async Task<Result> ActivarAsync(int id, CancellationToken ct = default)
    {
        var usuario = await _uow.Usuarios.ObtenerPorIdAsync(id, ct);
        if (usuario is null)
            return Result.NotFound($"No se encontró el usuario con Id {id}.");

        usuario.Activar(); // ← lógica en la entidad
        usuario.EstablecerModificacion(_dateTimeProvider.Ahora);
        _uow.Usuarios.Actualizar(usuario);
        await _uow.GuardarCambiosAsync(ct);

        return Result.Success();
    }

    public async Task<Result> DesactivarAsync(int id, CancellationToken ct = default)
    {
        var usuario = await _uow.Usuarios.ObtenerPorIdAsync(id, ct);
        if (usuario is null)
            return Result.NotFound($"No se encontró el usuario con Id {id}.");

        usuario.Desactivar(); // ← lógica en la entidad
        usuario.EstablecerModificacion(_dateTimeProvider.Ahora);
        _uow.Usuarios.Actualizar(usuario);
        await _uow.GuardarCambiosAsync(ct);

        return Result.Success();
    }

    public async Task<Result<TokenResponseDto>> LoginAsync(LoginDto dto, CancellationToken ct = default)
    {
        var usuario = await _uow.Usuarios.ObtenerPorCedulaAsync(dto.Cedula, ct);
        if (usuario is null || !_passwordHasher.Verificar(dto.Password, usuario.PasswordHash))
        {
            _logger.LogWarning("Intento de login fallido para cédula {Cedula}.", dto.Cedula);
            return Result<TokenResponseDto>.ValidationError("Credenciales inválidas.");
        }

        if (!usuario.Activo)
            return Result<TokenResponseDto>.Failure("El usuario está inactivo. Contacte al administrador.");

        var token = _jwtGenerator.GenerarToken(usuario);
        var expiracion = _dateTimeProvider.Ahora.AddHours(8);

        _logger.LogInformation("Login exitoso para usuario '{NombreUsuario}'.", usuario.Nombre);

        return Result<TokenResponseDto>.Success(new TokenResponseDto(
            Token: token,
            Expiracion: expiracion,
            NombreUsuario: usuario.Nombre,
            Rol: usuario.Rol?.Nombre ?? string.Empty));
    }
}
