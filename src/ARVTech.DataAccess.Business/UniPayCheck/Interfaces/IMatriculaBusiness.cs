namespace ARVTech.DataAccess.Business.UniPayCheck.Interfaces
{
    using System;
    using ARVTech.DataAccess.DTOs;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using ARVTech.Transmission.Engine.UniPayCheck.Results;

    public interface IMatriculaBusiness
    {
        void Delete(Guid guid);

        MatriculaResponseDto Get(Guid guid);

        //IEnumerable<MatriculaEspelhoPontoResponseDto> Get(string competencia, string matricula);

        //IEnumerable<MatriculaResponseDto> GetAll();

        MatriculaResponseDto GetByMatricula(string matricula);

        ExecutionResponseDto<MatriculaResponseDto> Import(MatriculaResult matriculaResult);

        MatriculaResponseDto SaveData(MatriculaRequestCreateDto? createDto = null, MatriculaRequestUpdateDto? updateDto = null);
    }
}