namespace ARVTech.DataAccess.Service.UniPayCheck.Mappings
{
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using AutoMapper;

    public class MatriculaEspelhoPontoMarcacaoMappingProfile : Profile
    {
        public MatriculaEspelhoPontoMarcacaoMappingProfile()
        {
            CreateMap<MatriculaEspelhoPontoMarcacaoRequestDto, MatriculaEspelhoPontoMarcacaoEntity>().ReverseMap();
            CreateMap<MatriculaEspelhoPontoMarcacaoResponseDto, MatriculaEspelhoPontoMarcacaoEntity>().ReverseMap();

            CreateMap<MatriculaEspelhoPontoRequestCreateDto, MatriculaEspelhoPontoEntity>().ReverseMap();
            CreateMap<MatriculaEspelhoPontoRequestUpdateDto, MatriculaEspelhoPontoEntity>().ReverseMap();
            CreateMap<MatriculaEspelhoPontoResponseDto, MatriculaEspelhoPontoEntity>().ReverseMap();

            CreateMap<MatriculaRequestCreateDto, MatriculaEntity>().ReverseMap();
            CreateMap<MatriculaRequestUpdateDto, MatriculaEntity>().ReverseMap();
            CreateMap<MatriculaResponseDto, MatriculaEntity>().ReverseMap();
        }
    }
}