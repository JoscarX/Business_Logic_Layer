using AutoMapper;
using Biblioteca.Application.DTOs.Categoria;
using Biblioteca.Domain.Entities;

namespace Biblioteca.Application.Mappings;

public sealed class CategoriaProfile : Profile
{
    public CategoriaProfile()
    {
        CreateMap<Categoria, CategoriaResponseDto>();
    }
}
