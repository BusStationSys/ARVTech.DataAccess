namespace ARVTech.DataAccess.Service.UniPayCheck.Mappings
{
    using System.Diagnostics.CodeAnalysis;
    using ARVTech.DataAccess.Contracts.PayCheck.Requests.Create;
    using ARVTech.DataAccess.Contracts.PayCheck.Requests.Update;
    using ARVTech.DataAccess.Contracts.PayCheck.Responses;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using AutoMapper;

    [ExcludeFromCodeCoverage]
    public class PessoaJuridicaMappingProfile : Profile
    {
        public PessoaJuridicaMappingProfile()
        {
            CreateMap<PessoaCreateRequest, PessoaEntity>().ReverseMap();
            CreateMap<PessoaUpdateRequest, PessoaEntity>().ReverseMap();
            CreateMap<PessoaResponse, PessoaEntity>().ReverseMap();

            CreateMap<PessoaJuridicaCreateRequest, PessoaJuridicaEntity>().ReverseMap();
            CreateMap<PessoaJuridicaUpdateRequest, PessoaJuridicaEntity>().ReverseMap();
            CreateMap<PessoaJuridicaResponse, PessoaJuridicaEntity>().ReverseMap();

            CreateMap<UnidadeNegocioResponse, UnidadeNegocioEntity>().ReverseMap();
        }
    }
}