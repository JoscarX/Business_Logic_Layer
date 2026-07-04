using AutoMapper;
using Biblioteca.Application.DTOs.Entrada;
using Biblioteca.Domain.Entities;

namespace Biblioteca.Application.Mappings;

public sealed class EntradaProfile : Profile
{
    public EntradaProfile()
    {
        CreateMap<Entrada, EntradaResponseDto>()
            .ForMember(dest => dest.NombreVisitante,
                opt => opt.MapFrom(src => src.Visitante != null ? src.Visitante.NombreCompleto : string.Empty))
            .ForMember(dest => dest.EstaActiva, opt => opt.MapFrom(src => src.EstaActiva))
            .ForMember(dest => dest.DuracionVisita, opt => opt.MapFrom(src => src.DuracionVisita));
    }
}
