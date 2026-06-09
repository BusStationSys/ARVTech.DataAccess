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
    public class MatriculaEspelhoPontoMappingProfile : Profile
    {
        public MatriculaEspelhoPontoMappingProfile()
        {
            CreateMap<CalculoRequest, CalculoEntity>().ReverseMap();
            CreateMap<CalculoResponse, CalculoEntity>().ReverseMap();

            CreateMap<MatriculaEspelhoPontoCalculoRequest, MatriculaEspelhoPontoCalculoEntity>().ReverseMap();
            CreateMap<MatriculaEspelhoPontoCalculoResponse, MatriculaEspelhoPontoCalculoEntity>().ReverseMap();

            CreateMap<MatriculaEspelhoPontoMarcacaoResponse, MatriculaEspelhoPontoMarcacaoEntity>().ReverseMap();
            CreateMap<MatriculaEspelhoPontoMarcacaoRequest, MatriculaEspelhoPontoMarcacaoEntity>().ReverseMap();
            CreateMap<MatriculaEspelhoPontoMarcacaoResponse, MatriculaEspelhoPontoEntity>().ReverseMap();

            CreateMap<MatriculaEspelhoPontoCreateRequest, MatriculaEspelhoPontoEntity>().ReverseMap();
            CreateMap<MatriculaEspelhoPontoUpdateRequest, MatriculaEspelhoPontoEntity>().ReverseMap();
            CreateMap<MatriculaEspelhoPontoResponse, MatriculaEspelhoPontoEntity>().ReverseMap();

            CreateMap<MatriculaCreateRequest, MatriculaEntity>().ReverseMap();
            CreateMap<MatriculaUpdateRequest, MatriculaEntity>().ReverseMap();
            CreateMap<MatriculaResponse, MatriculaEntity>().ReverseMap();

            CreateMap<PessoaFisicaCreateRequest, PessoaFisicaEntity>().ReverseMap();
            CreateMap<PessoaFisicaUpdateRequest, PessoaFisicaEntity>().ReverseMap();
            CreateMap<PessoaFisicaResponse, PessoaFisicaEntity>().ReverseMap();

            CreateMap<PessoaJuridicaCreateRequest, PessoaJuridicaEntity>().ReverseMap();
            CreateMap<PessoaJuridicaUpdateRequest, PessoaJuridicaEntity>().ReverseMap();
            CreateMap<PessoaJuridicaResponse, PessoaJuridicaEntity>().ReverseMap();
        }
    }
}