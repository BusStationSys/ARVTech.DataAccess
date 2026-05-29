namespace ARVTech.DataAccess.Service.UniPayCheck.Mappings
{
    using System.Diagnostics.CodeAnalysis;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using AutoMapper;

    [ExcludeFromCodeCoverage]
    public class MatriculaDemonstrativoPagamentoTotalizadorMappingProfile : Profile
    {
        public MatriculaDemonstrativoPagamentoTotalizadorMappingProfile()
        {
            CreateMap<MatriculaDemonstrativoPagamentoTotalizadorRequestDto, MatriculaDemonstrativoPagamentoTotalizadorEntity>().ReverseMap();
            CreateMap<MatriculaDemonstrativoPagamentoTotalizadorResponseDto, MatriculaDemonstrativoPagamentoTotalizadorEntity>().ReverseMap();
            CreateMap<MatriculaDemonstrativoPagamentoRequestCreateDto, MatriculaDemonstrativoPagamentoEntity>().ReverseMap();
            CreateMap<MatriculaDemonstrativoPagamentoRequestUpdateDto, MatriculaDemonstrativoPagamentoEntity>().ReverseMap();
            CreateMap<MatriculaDemonstrativoPagamentoResponseDto, MatriculaDemonstrativoPagamentoEntity>().ReverseMap();
            CreateMap<MatriculaRequestCreateDto, MatriculaEntity>().ReverseMap();
            CreateMap<MatriculaRequestUpdateDto, MatriculaEntity>().ReverseMap();
            CreateMap<MatriculaResponseDto, MatriculaEntity>().ReverseMap();
            CreateMap<TotalizadorRequestDto, TotalizadorEntity>().ReverseMap();
            CreateMap<TotalizadorResponseDto, TotalizadorEntity>().ReverseMap();
        }
    }
}