namespace ARVTech.DataAccess.Service.UniPayCheck.Interfaces
{
    using System;
    using ARVTech.DataAccess.Contracts.PayCheck.Requests.Create;
    using ARVTech.DataAccess.Contracts.PayCheck.Requests.Update;
    using ARVTech.DataAccess.Contracts.PayCheck.Responses;

    public interface IMatriculaService
    {
        void Delete(Guid guid);

        MatriculaResponse Get(Guid guid);

        IEnumerable<MatriculaResponse> GetAniversariantesEmpresa(int mes);

        MatriculaResponse GetByMatricula(string matricula);

        ResumoImportacaoMatriculasResponse ImportFileMatriculas(string cnpj, string content);

        MatriculaResponse SaveData(MatriculaCreateRequest? createRequest = null, MatriculaUpdateRequest? updateRequest = null);
    }
}