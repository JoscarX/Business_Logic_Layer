using AutoMapper;
using Biblioteca.Application.DTOs.Usuario;
using Biblioteca.Domain.Entities;

namespace Biblioteca.Application.Mappings;

public sealed class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<Usuario, UsuarioResponseDto>()
            .ForMember(dest => dest.NombreRol, opt => opt.MapFrom(src => src.Rol != null ? src.Rol.Nombre : string.Empty));
    }
}
