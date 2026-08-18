namespace ARVTech.DataAccess.Service.Destin.Mappings
{
    using System.Diagnostics.CodeAnalysis;
    using ARVTech.DataAccess.Contracts.Destin.Requests;
    using ARVTech.DataAccess.Contracts.Destin.Responses;
    using ARVTech.DataAccess.Domain.Entities.Destin;
    using AutoMapper;

    [ExcludeFromCodeCoverage]
    public class ConcursoMappingProfile : Profile
    {
        public ConcursoMappingProfile()
        {
            CreateMap<ConcursoRequest, ConcursoEntity>().ReverseMap();
            CreateMap<ConcursoResponse, ConcursoEntity>().ReverseMap();
            CreateMap<ConcursoDezenaRequest, ConcursoDezenaEntity>().ReverseMap();
            CreateMap<ConcursoDezenaResponse, ConcursoDezenaEntity>().ReverseMap();
        }
    }
}