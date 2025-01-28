namespace ARVTech.DataAccess.Service.UniPayCheck.Interfaces
{
    using System;
    using System.Collections.Generic;
    using ARVTech.DataAccess.DTOs;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using ARVTech.Transmission.Engine.UniPayCheck.Results;

    public interface IMatriculaEspelhoPontoService
    {
        MatriculaEspelhoPontoResponseDto Get(Guid guid);

        IEnumerable<MatriculaEspelhoPontoResponseDto> Get(string competencia, string matricula);

        IEnumerable<MatriculaEspelhoPontoResponseDto> GetAll();

        IEnumerable<MatriculaEspelhoPontoResponseDto> GetByGuidColaborador(Guid guidColaborador);

        ExecutionResponseDto<MatriculaEspelhoPontoResponseDto> Import(EspelhoPontoResult espelhoPontoResult);

        ResumoImportacaoEspelhosPontoResponseDto ImportFileEspelhosPonto(string cnpj, string content);

        MatriculaEspelhoPontoResponseDto SaveData(MatriculaEspelhoPontoRequestCreateDto? createDto = null, MatriculaEspelhoPontoRequestUpdateDto? updateDto = null);
    }
}