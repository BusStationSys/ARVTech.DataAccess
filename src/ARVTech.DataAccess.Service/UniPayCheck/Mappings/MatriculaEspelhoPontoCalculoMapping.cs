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
    public class MatriculaEspelhoPontoCalculoMapping : Profile
    {
        public MatriculaEspelhoPontoCalculoMapping()
        {
            CreateMap<MatriculaEspelhoPontoCalculoRequest, MatriculaEspelhoPontoCalculoEntity>().ReverseMap();
            CreateMap<MatriculaEspelhoPontoCalculoResponse, MatriculaEspelhoPontoCalculoEntity>().ReverseMap();

            CreateMap<MatriculaEspelhoPontoCreateRequest, MatriculaEspelhoPontoEntity>().ReverseMap();
            CreateMap<MatriculaEspelhoPontoUpdateRequest, MatriculaEspelhoPontoEntity>().ReverseMap();
            CreateMap<MatriculaEspelhoPontoResponse, MatriculaEspelhoPontoEntity>().ReverseMap();

            CreateMap<MatriculaCreateRequest, MatriculaEntity>().ReverseMap();
            CreateMap<MatriculaUpdateRequest, MatriculaEntity>().ReverseMap();
            CreateMap<MatriculaResponse, MatriculaEntity>().ReverseMap();

            CreateMap<CalculoRequest, CalculoEntity>().ReverseMap();
            CreateMap<CalculoResponse, CalculoEntity>().ReverseMap();
        }
    }
}