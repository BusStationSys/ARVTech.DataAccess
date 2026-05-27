namespace ARVTech.DataAccess.Service.UniPayCheck.Mappings
{
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using AutoMapper;

    public class MatriculaDemonstrativoPagamentoMappingProfile : Profile
    {
        public MatriculaDemonstrativoPagamentoMappingProfile()
        {
            CreateMap<MatriculaDemonstrativoPagamentoRequestCreateDto, MatriculaDemonstrativoPagamentoEntity>().ReverseMap();
            CreateMap<MatriculaDemonstrativoPagamentoRequestUpdateDto, MatriculaDemonstrativoPagamentoEntity>().ReverseMap();
            CreateMap<MatriculaDemonstrativoPagamentoResponseDto, MatriculaDemonstrativoPagamentoEntity>().ReverseMap();
            CreateMap<MatriculaRequestCreateDto, MatriculaEntity>().ReverseMap();
            CreateMap<MatriculaRequestUpdateDto, MatriculaEntity>().ReverseMap();
            CreateMap<MatriculaResponseDto, MatriculaEntity>().ReverseMap();
            CreateMap<PessoaFisicaRequestCreateDto, PessoaFisicaEntity>().ReverseMap();
            CreateMap<PessoaFisicaRequestUpdateDto, PessoaFisicaEntity>().ReverseMap();
            CreateMap<PessoaFisicaResponseDto, PessoaFisicaEntity>().ReverseMap();
            CreateMap<PessoaJuridicaRequestCreateDto, PessoaJuridicaEntity>().ReverseMap();
            CreateMap<PessoaJuridicaRequestUpdateDto, PessoaJuridicaEntity>().ReverseMap();
            CreateMap<PessoaJuridicaResponseDto, PessoaJuridicaEntity>().ReverseMap();
            CreateMap<MatriculaDemonstrativoPagamentoEventoResponseDto, MatriculaDemonstrativoPagamentoEventoEntity>().ReverseMap();
            CreateMap<MatriculaDemonstrativoPagamentoEventoRequestDto, MatriculaDemonstrativoPagamentoEventoEntity>().ReverseMap();
            CreateMap<EventoRequestDto, EventoEntity>().ReverseMap();
            CreateMap<EventoResponseDto, EventoEntity>().ReverseMap();
            CreateMap<MatriculaDemonstrativoPagamentoTotalizadorResponseDto, MatriculaDemonstrativoPagamentoTotalizadorEntity>().ReverseMap();
            CreateMap<MatriculaDemonstrativoPagamentoTotalizadorRequestDto, MatriculaDemonstrativoPagamentoTotalizadorEntity>().ReverseMap();
            CreateMap<TotalizadorRequestDto, TotalizadorEntity>().ReverseMap();
            CreateMap<TotalizadorResponseDto, TotalizadorEntity>().ReverseMap();
            CreateMap<PessoaRequestCreateDto, PessoaEntity>().ReverseMap();
            CreateMap<PessoaResponseDto, PessoaEntity>().ReverseMap();
        }
    }
}