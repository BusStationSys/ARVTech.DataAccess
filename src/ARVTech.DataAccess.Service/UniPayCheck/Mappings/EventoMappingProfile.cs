namespace ARVTech.DataAccess.Service.UniPayCheck.Mappings
{
    using System.Diagnostics.CodeAnalysis;
    using ARVTech.DataAccess.Contracts.PayCheck.Requests;
    using ARVTech.DataAccess.Contracts.PayCheck.Responses;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using AutoMapper;

    [ExcludeFromCodeCoverage]
    public class EventoMappingProfile : Profile
    {
        public EventoMappingProfile()
        {
            CreateMap<EventoRequest, EventoEntity>().ReverseMap();
            CreateMap<EventoResponse, EventoEntity>().ReverseMap();
        }
    }
}