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
            CreateMap<PessoaFisicaRequestCreateDto, PessoaFisicaEntity>().ReverseMap();
            CreateMap<PessoaFisicaRequestUpdateDto, PessoaFisicaEntity>().ReverseMap();
            CreateMap<PessoaFisicaResponseDto, PessoaFisicaEntity>().ReverseMap();
            CreateMap<PessoaRequestCreateDto, PessoaEntity>().ReverseMap();
            CreateMap<PessoaRequestUpdateDto, PessoaEntity>().ReverseMap();
            CreateMap<PessoaResponseDto, PessoaEntity>().ReverseMap();

            //  Casos específicos sem ReverseMap() para evitar mapeamento de PasswordHash, que é gerenciado internamente e não deve ser exposto ou atualizado diretamente.
            CreateMap<UsuarioRequestCreateDto, UsuarioEntity>().ForMember(
                dest => dest.PasswordHash,
                opt => opt.Ignore());

            CreateMap<UsuarioRequestUpdateDto, UsuarioEntity>().ForMember(
                dest => dest.PasswordHash,
                opt => opt.Ignore());

            CreateMap<UsuarioEntity, UsuarioResponseDto>();
        }
    }
}