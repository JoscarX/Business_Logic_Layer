using AutoMapper;
using Biblioteca.Application.DTOs.Prestamo;
using Biblioteca.Domain.Entities;

namespace Biblioteca.Application.Mappings;

public sealed class PrestamoProfile : Profile
{
    public PrestamoProfile()
    {
        CreateMap<Prestamo, PrestamoResponseDto>()
            .ForMember(dest => dest.NombreVisitante,
                opt => opt.MapFrom(src => src.Visitante != null ? src.Visitante.NombreCompleto : string.Empty))
            .ForMember(dest => dest.TituloLibro,
                opt => opt.MapFrom(src => src.Libro != null ? src.Libro.Nombre : string.Empty))
            .ForMember(dest => dest.Estado,
                opt => opt.MapFrom(src => src.Estado.ToString()));
    }
}
