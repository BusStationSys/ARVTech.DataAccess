namespace ARVTech.DataAccess.Business.UniPayCheck.Interfaces
{
    using System;
    using ARVTech.DataAccess.DTOs.UniPayCheck;

    public interface IMatriculaBusiness
    {
        void Delete(Guid guid);

        MatriculaResponseDto Get(Guid guid);

        //IEnumerable<MatriculaEspelhoPontoResponseDto> Get(string competencia, string matricula);

        //IEnumerable<MatriculaResponseDto> GetAll();

        IEnumerable<MatriculaResponseDto> GetAniversariantesEmpresa(int mes);

        MatriculaResponseDto GetByMatricula(string matricula);

        ResumoImportacaoMatriculasResponseDto ImportFileMatriculas(string cnpj, string content);

        MatriculaResponseDto SaveData(MatriculaRequestCreateDto? createDto = null, MatriculaRequestUpdateDto? updateDto = null);
    }
}