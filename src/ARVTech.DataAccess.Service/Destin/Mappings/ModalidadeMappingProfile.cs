namespace ARVTech.DataAccess.Service.Destin.Mappings
{
    using System.Diagnostics.CodeAnalysis;
    using ARVTech.DataAccess.Contracts.Destin.Requests;
    using ARVTech.DataAccess.Contracts.Destin.Responses;
    using ARVTech.DataAccess.Domain.Entities.Destin;
    using AutoMapper;

    [ExcludeFromCodeCoverage]
    public class ModalidadeMappingProfile : Profile
    {
        public ModalidadeMappingProfile()
        {
            CreateMap<ModalidadeRequest, ModalidadeEntity>().ReverseMap();
            CreateMap<ModalidadeResponse, ModalidadeEntity>().ReverseMap();
        }
    }
}