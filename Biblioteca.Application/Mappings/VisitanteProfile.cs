using AutoMapper;
using Biblioteca.Application.DTOs.Visitante;
using Biblioteca.Domain.Entities;

namespace Biblioteca.Application.Mappings;

public sealed class VisitanteProfile : Profile
{
    public VisitanteProfile()
    {
        CreateMap<Visitante, VisitanteResponseDto>();
    }
}
