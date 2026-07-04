using AutoMapper;
using Biblioteca.Application.DTOs.Rol;
using Biblioteca.Domain.Entities;

namespace Biblioteca.Application.Mappings;

public sealed class RolProfile : Profile
{
    public RolProfile()
    {
        CreateMap<Rol, RolResponseDto>();
    }
}
