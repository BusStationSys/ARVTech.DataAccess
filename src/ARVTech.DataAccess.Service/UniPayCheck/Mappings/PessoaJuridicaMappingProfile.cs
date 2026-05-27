namespace ARVTech.DataAccess.Service.UniPayCheck.Mappings
{
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using AutoMapper;

    public class PessoaJuridicaMappingProfile : Profile
    {
        public PessoaJuridicaMappingProfile()
        {
            CreateMap<PessoaRequestCreateDto, PessoaEntity>().ReverseMap();
            CreateMap<PessoaRequestUpdateDto, PessoaEntity>().ReverseMap();
            CreateMap<PessoaResponseDto, PessoaEntity>().ReverseMap();
            CreateMap<PessoaJuridicaRequestCreateDto, PessoaJuridicaEntity>().ReverseMap();
            CreateMap<PessoaJuridicaRequestUpdateDto, PessoaJuridicaEntity>().ReverseMap();
            CreateMap<PessoaJuridicaResponseDto, PessoaJuridicaEntity>().ReverseMap();
            CreateMap<UnidadeNegocioResponseDto, UnidadeNegocioEntity>().ReverseMap();
        }
    }
}