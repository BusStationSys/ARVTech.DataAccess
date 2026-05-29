namespace ARVTech.DataAccess.Service.UniPayCheck.Mappings
{
    using System.Diagnostics.CodeAnalysis;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using AutoMapper;

    [ExcludeFromCodeCoverage]
    public class UsuarioMappingProfile : Profile
    {
        public UsuarioMappingProfile()
        {
            CreateMap<UsuarioNotificacaoResponseDto, UsuarioNotificacaoEntity>().ReverseMap();
            CreateMap<UsuarioRequestCreateDto, UsuarioEntity>().ReverseMap();
            CreateMap<UsuarioRequestUpdateDto, UsuarioEntity>().ReverseMap();
            CreateMap<UsuarioResponseDto, UsuarioEntity>().ReverseMap();
            CreateMap<PessoaFisicaRequestCreateDto, PessoaFisicaEntity>().ReverseMap();
            CreateMap<PessoaFisicaRequestUpdateDto, PessoaFisicaEntity>().ReverseMap();
            CreateMap<PessoaFisicaResponseDto, PessoaFisicaEntity>().ReverseMap();
            CreateMap<PessoaRequestCreateDto, PessoaEntity>().ReverseMap();
            CreateMap<PessoaRequestUpdateDto, PessoaEntity>().ReverseMap();
            CreateMap<PessoaResponseDto, PessoaEntity>().ReverseMap();
        }
    }
}