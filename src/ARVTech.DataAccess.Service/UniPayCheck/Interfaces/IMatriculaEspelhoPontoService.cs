namespace ARVTech.DataAccess.Service.UniPayCheck.Interfaces
{
    using System;
    using System.Collections.Generic;
    using ARVTech.DataAccess.Contracts.PayCheck.Requests.Create;
    using ARVTech.DataAccess.Contracts.PayCheck.Requests.Update;
    using ARVTech.DataAccess.Contracts.PayCheck.Responses;

    public interface IMatriculaEspelhoPontoService
    {
        MatriculaEspelhoPontoResponse Get(Guid guid);

        IEnumerable<MatriculaEspelhoPontoResponse> Get(string competencia, string matricula);

        IEnumerable<MatriculaEspelhoPontoResponse> GetAll();

        IEnumerable<MatriculaEspelhoPontoResponse> GetByGuidColaborador(Guid guidColaborador);

        ResumoImportacaoEspelhosPontoResponse ImportFileEspelhosPonto(string cnpj, string content);

        MatriculaEspelhoPontoResponse SaveData(MatriculaEspelhoPontoCreateRequest? createRequest = null, MatriculaEspelhoPontoUpdateRequest? updateRequest = null);
    }
}