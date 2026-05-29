namespace ARVTech.DataAccess.Service.UniPayCheck.Mappings
{
    using System.Diagnostics.CodeAnalysis;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using AutoMapper;

    [ExcludeFromCodeCoverage]
    public class MatriculaDemonstrativoPagamentoEventoMappingProfile : Profile
    {
        public MatriculaDemonstrativoPagamentoEventoMappingProfile()
        {
            CreateMap<MatriculaDemonstrativoPagamentoEventoRequestDto, MatriculaDemonstrativoPagamentoEventoEntity>().ReverseMap();
            CreateMap<MatriculaDemonstrativoPagamentoEventoResponseDto, MatriculaDemonstrativoPagamentoEventoEntity>().ReverseMap();

            CreateMap<MatriculaDemonstrativoPagamentoResponseDto, MatriculaDemonstrativoPagamentoEntity>().ReverseMap();
            CreateMap<MatriculaDemonstrativoPagamentoResponseDto, MatriculaDemonstrativoPagamentoEntity>().ReverseMap();

            CreateMap<MatriculaRequestCreateDto, MatriculaEntity>().ReverseMap();
            CreateMap<MatriculaRequestUpdateDto, MatriculaEntity>().ReverseMap();
            CreateMap<MatriculaResponseDto, MatriculaEntity>().ReverseMap();

            CreateMap<EventoRequestDto, EventoEntity>().ReverseMap();
            CreateMap<EventoResponseDto, EventoEntity>().ReverseMap();
        }
    }
}