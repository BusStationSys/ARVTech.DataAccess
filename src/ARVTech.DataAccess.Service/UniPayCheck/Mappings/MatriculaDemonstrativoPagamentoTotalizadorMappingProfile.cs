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
    public class MatriculaDemonstrativoPagamentoTotalizadorMappingProfile : Profile
    {
        public MatriculaDemonstrativoPagamentoTotalizadorMappingProfile()
        {
            CreateMap<MatriculaDemonstrativoPagamentoTotalizadorRequest, MatriculaDemonstrativoPagamentoTotalizadorEntity>().ReverseMap();
            CreateMap<MatriculaDemonstrativoPagamentoTotalizadorResponse, MatriculaDemonstrativoPagamentoTotalizadorEntity>().ReverseMap();

            CreateMap<MatriculaDemonstrativoPagamentoCreateRequest, MatriculaDemonstrativoPagamentoEntity>().ReverseMap();
            CreateMap<MatriculaDemonstrativoPagamentoUpdateRequest, MatriculaDemonstrativoPagamentoEntity>().ReverseMap();
            CreateMap<MatriculaDemonstrativoPagamentoResponse, MatriculaDemonstrativoPagamentoEntity>().ReverseMap();

            CreateMap<MatriculaCreateRequest, MatriculaEntity>().ReverseMap();
            CreateMap<MatriculaUpdateRequest, MatriculaEntity>().ReverseMap();
            CreateMap<MatriculaResponse, MatriculaEntity>().ReverseMap();

            CreateMap<TotalizadorRequest, TotalizadorEntity>().ReverseMap();
            CreateMap<TotalizadorResponse, TotalizadorEntity>().ReverseMap();
        }
    }
}