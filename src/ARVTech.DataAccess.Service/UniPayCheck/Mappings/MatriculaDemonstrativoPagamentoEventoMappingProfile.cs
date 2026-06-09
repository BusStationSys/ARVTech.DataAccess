namespace ARVTech.DataAccess.Service.UniPayCheck.Mappings
{
    using System.Diagnostics.CodeAnalysis;
    using ARVTech.DataAccess.Contracts.PayCheck.Requests;
    using ARVTech.DataAccess.Contracts.PayCheck.Requests.Create;
    using ARVTech.DataAccess.Contracts.PayCheck.Requests.Update;
    using ARVTech.DataAccess.Contracts.PayCheck.Responses;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using AutoMapper;

    [ExcludeFromCodeCoverage]
    public class MatriculaDemonstrativoPagamentoEventoMappingProfile : Profile
    {
        public MatriculaDemonstrativoPagamentoEventoMappingProfile()
        {
            CreateMap<MatriculaDemonstrativoPagamentoEventoRequest, MatriculaDemonstrativoPagamentoEventoEntity>().ReverseMap();
            CreateMap<MatriculaDemonstrativoPagamentoEventoResponse, MatriculaDemonstrativoPagamentoEventoEntity>().ReverseMap();

            CreateMap<MatriculaDemonstrativoPagamentoCreateRequest, MatriculaDemonstrativoPagamentoEntity>().ReverseMap();
            CreateMap<MatriculaDemonstrativoPagamentoUpdateRequest, MatriculaDemonstrativoPagamentoEntity>().ReverseMap();
            CreateMap<MatriculaDemonstrativoPagamentoResponse, MatriculaDemonstrativoPagamentoEntity>().ReverseMap();

            CreateMap<MatriculaCreateRequest, MatriculaEntity>().ReverseMap();
            CreateMap<MatriculaUpdateRequest, MatriculaEntity>().ReverseMap();
            CreateMap<MatriculaResponse, MatriculaEntity>().ReverseMap();

            CreateMap<EventoRequest, EventoEntity>().ReverseMap();
            CreateMap<EventoResponse, EventoEntity>().ReverseMap();
        }
    }
}