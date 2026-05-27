namespace ARVTech.DataAccess.Service.UniPayCheck.Mappings
{
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using AutoMapper;

    public class EventoMappingProfile : Profile
    {
        public EventoMappingProfile()
        {
            CreateMap<EventoRequestDto, EventoEntity>().ReverseMap();
            CreateMap<EventoResponseDto, EventoEntity>().ReverseMap();
        }
    }
}