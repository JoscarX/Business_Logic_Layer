using AutoMapper;
using Biblioteca.Application.DTOs.Libro;
using Biblioteca.Domain.Entities;

namespace Biblioteca.Application.Mappings;

public sealed class LibroProfile : Profile
{
    public LibroProfile()
    {
        CreateMap<Libro, LibroResponseDto>()
            .ForMember(dest => dest.Isbn, opt => opt.MapFrom(src => src.Isbn));
    }
}
