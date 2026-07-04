using AutoMapper;
using Biblioteca.Application.DTOs.Autor;
using Biblioteca.Domain.Entities;

namespace Biblioteca.Application.Mappings;

public sealed class AutorProfile : Profile
{
    public AutorProfile()
    {
        CreateMap<Autor, AutorResponseDto>();
    }
}
