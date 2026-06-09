namespace ARVTech.DataAccess.Service.UniPayCheck.Mappings
{
    using System.Diagnostics.CodeAnalysis;
    using ARVTech.DataAccess.Contracts.PayCheck.Requests.Create;
    using ARVTech.DataAccess.Contracts.PayCheck.Requests.Update;
    using ARVTech.DataAccess.Contracts.PayCheck.Responses;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using AutoMapper;

    [ExcludeFromCodeCoverage]
    public class UsuarioMappingProfile : Profile
    {
        public UsuarioMappingProfile()
        {
            CreateMap<UsuarioNotificacaoResponse, UsuarioNotificacaoEntity>().ReverseMap();

            CreateMap<PessoaFisicaCreateRequest, PessoaFisicaEntity>().ReverseMap();
            CreateMap<PessoaFisicaUpdateRequest, PessoaFisicaEntity>().ReverseMap();
            CreateMap<PessoaFisicaResponse, PessoaFisicaEntity>().ReverseMap();

            CreateMap<PessoaCreateRequest, PessoaEntity>().ReverseMap();
            CreateMap<PessoaUpdateRequest, PessoaEntity>().ReverseMap();
            CreateMap<PessoaResponse, PessoaEntity>().ReverseMap();

            //  Casos específicos sem ReverseMap() para evitar mapeamento de PasswordHash, que é gerenciado internamente e não deve ser exposto ou atualizado diretamente.
            CreateMap<UsuarioCreateRequest, UsuarioEntity>().ForMember(
                dest => dest.PasswordHash,
                opt => opt.Ignore());

            CreateMap<UsuarioUpdateRequest, UsuarioEntity>().ForMember(
                dest => dest.PasswordHash,
                opt => opt.Ignore());

            CreateMap<UsuarioEntity, UsuarioResponse>();
        }
    }
}