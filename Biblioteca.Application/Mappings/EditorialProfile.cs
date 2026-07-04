using AutoMapper;
using Biblioteca.Application.DTOs.Editorial;
using Biblioteca.Domain.Entities;

namespace Biblioteca.Application.Mappings;

public sealed class EditorialProfile : Profile
{
    public EditorialProfile()
    {
        CreateMap<Editorial, EditorialResponseDto>();
    }
}
