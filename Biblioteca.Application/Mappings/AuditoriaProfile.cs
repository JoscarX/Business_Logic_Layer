using AutoMapper;
using Biblioteca.Application.DTOs.Auditoria;
using Biblioteca.Domain.Entities;

namespace Biblioteca.Application.Mappings;

public sealed class AuditoriaProfile : Profile
{
    public AuditoriaProfile()
    {
        CreateMap<Auditoria, AuditoriaResponseDto>()
            .ForMember(dest => dest.NombreUsuario,
                opt => opt.MapFrom(src => src.Usuario != null ? src.Usuario.Nombre : null))
            .ForMember(dest => dest.Accion,
                opt => opt.MapFrom(src => src.Accion.ToString()));
    }
}
