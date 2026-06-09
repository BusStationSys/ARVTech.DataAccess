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
    public class MatriculaDemonstrativoPagamentoMappingProfile : Profile
    {
        public MatriculaDemonstrativoPagamentoMappingProfile()
        {
            CreateMap<MatriculaDemonstrativoPagamentoCreateRequest, MatriculaDemonstrativoPagamentoEntity>().ReverseMap();
            CreateMap<MatriculaDemonstrativoPagamentoUpdateRequest, MatriculaDemonstrativoPagamentoEntity>().ReverseMap();
            CreateMap<MatriculaDemonstrativoPagamentoResponse, MatriculaDemonstrativoPagamentoEntity>().ReverseMap();

            CreateMap<MatriculaCreateRequest, MatriculaEntity>().ReverseMap();
            CreateMap<MatriculaUpdateRequest, MatriculaEntity>().ReverseMap();
            CreateMap<MatriculaResponse, MatriculaEntity>().ReverseMap();

            CreateMap<PessoaFisicaCreateRequest, PessoaFisicaEntity>().ReverseMap();
            CreateMap<PessoaFisicaUpdateRequest, PessoaFisicaEntity>().ReverseMap();
            CreateMap<PessoaFisicaResponse, PessoaFisicaEntity>().ReverseMap();

            CreateMap<PessoaJuridicaCreateRequest, PessoaJuridicaEntity>().ReverseMap();
            CreateMap<PessoaJuridicaUpdateRequest, PessoaJuridicaEntity>().ReverseMap();
            CreateMap<PessoaJuridicaResponse, PessoaJuridicaEntity>().ReverseMap();

            CreateMap<MatriculaDemonstrativoPagamentoEventoRequest, MatriculaDemonstrativoPagamentoEventoEntity>().ReverseMap();
            CreateMap<MatriculaDemonstrativoPagamentoEventoResponse, MatriculaDemonstrativoPagamentoEventoEntity>().ReverseMap();

            CreateMap<EventoRequest, EventoEntity>().ReverseMap();
            CreateMap<EventoResponse, EventoEntity>().ReverseMap();

            CreateMap<MatriculaDemonstrativoPagamentoTotalizadorRequest, MatriculaDemonstrativoPagamentoTotalizadorEntity>().ReverseMap();
            CreateMap<MatriculaDemonstrativoPagamentoTotalizadorResponse, MatriculaDemonstrativoPagamentoTotalizadorEntity>().ReverseMap();

            CreateMap<TotalizadorRequest, TotalizadorEntity>().ReverseMap();
            CreateMap<TotalizadorResponse, TotalizadorEntity>().ReverseMap();

            CreateMap<PessoaCreateRequest, PessoaEntity>().ReverseMap();
            CreateMap<PessoaUpdateRequest, PessoaEntity>().ReverseMap();
            CreateMap<PessoaResponse, PessoaEntity>().ReverseMap();
        }
    }
}