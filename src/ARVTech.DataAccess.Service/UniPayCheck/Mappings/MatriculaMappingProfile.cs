namespace ARVTech.DataAccess.Service.UniPayCheck.Mappings
{
    using System.Diagnostics.CodeAnalysis;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using AutoMapper;

    [ExcludeFromCodeCoverage]
    public class MatriculaMappingProfile : Profile
    {
        public MatriculaMappingProfile()
        {
            CreateMap<MatriculaRequestCreateDto, MatriculaEntity>().ReverseMap();
            CreateMap<MatriculaRequestUpdateDto, MatriculaEntity>().ReverseMap();
            CreateMap<MatriculaResponseDto, MatriculaEntity>().ReverseMap();
            CreateMap<PessoaFisicaRequestCreateDto, PessoaFisicaEntity>().ReverseMap();
            CreateMap<PessoaFisicaRequestUpdateDto, PessoaFisicaEntity>().ReverseMap();
            CreateMap<PessoaFisicaResponseDto, PessoaFisicaEntity>().ReverseMap();
            CreateMap<PessoaJuridicaRequestCreateDto, PessoaJuridicaEntity>().ReverseMap();
            CreateMap<PessoaJuridicaRequestUpdateDto, PessoaJuridicaEntity>().ReverseMap();
            CreateMap<PessoaJuridicaResponseDto, PessoaJuridicaEntity>().ReverseMap();
        }
    }
}