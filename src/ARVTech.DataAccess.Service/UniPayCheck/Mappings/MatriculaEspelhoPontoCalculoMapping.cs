namespace ARVTech.DataAccess.Service.UniPayCheck.Mappings
{
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using AutoMapper;

    public class MatriculaEspelhoPontoCalculoMapping : Profile
    {
        public MatriculaEspelhoPontoCalculoMapping()
        {
            CreateMap<MatriculaEspelhoPontoCalculoRequestDto, MatriculaEspelhoPontoCalculoEntity>().ReverseMap();
            CreateMap<MatriculaEspelhoPontoCalculoResponseDto, MatriculaEspelhoPontoCalculoEntity>().ReverseMap();

            CreateMap<MatriculaEspelhoPontoRequestCreateDto, MatriculaEspelhoPontoEntity>().ReverseMap();
            CreateMap<MatriculaEspelhoPontoRequestUpdateDto, MatriculaEspelhoPontoEntity>().ReverseMap();
            CreateMap<MatriculaEspelhoPontoResponseDto, MatriculaEspelhoPontoEntity>().ReverseMap();

            CreateMap<MatriculaRequestCreateDto, MatriculaEntity>().ReverseMap();
            CreateMap<MatriculaRequestUpdateDto, MatriculaEntity>().ReverseMap();
            CreateMap<MatriculaResponseDto, MatriculaEntity>().ReverseMap();

            CreateMap<CalculoRequestDto, CalculoEntity>().ReverseMap();
            CreateMap<CalculoResponseDto, CalculoEntity>().ReverseMap();
        }
    }
}