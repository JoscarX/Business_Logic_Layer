using AutoMapper;
using Biblioteca.Application.DTOs.Venta;
using Biblioteca.Domain.Entities;

namespace Biblioteca.Application.Mappings;

public sealed class VentaProfile : Profile
{
    public VentaProfile()
    {
        CreateMap<DetalleVenta, DetalleVentaResponseDto>()
            .ForMember(dest => dest.TituloLibro,
                opt => opt.MapFrom(src => src.Libro != null ? src.Libro.Nombre : string.Empty));

        CreateMap<Venta, VentaResponseDto>()
            .ForMember(dest => dest.NombreVisitante,
                opt => opt.MapFrom(src => src.Visitante != null ? src.Visitante.NombreCompleto : string.Empty))
            .ForMember(dest => dest.Estado,
                opt => opt.MapFrom(src => src.Estado.ToString()))
            .ForMember(dest => dest.Detalles,
                opt => opt.MapFrom(src => src.Detalles));
    }
}
