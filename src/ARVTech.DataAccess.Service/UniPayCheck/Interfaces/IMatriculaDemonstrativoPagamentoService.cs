namespace ARVTech.DataAccess.Service.UniPayCheck.Interfaces
{
    using System;
    using System.Collections.Generic;
    using ARVTech.DataAccess.Contracts.PayCheck.Requests.Create;
    using ARVTech.DataAccess.Contracts.PayCheck.Requests.Update;
    using ARVTech.DataAccess.Contracts.PayCheck.Responses;
    using ARVTech.Shared.Enums;

    public interface IMatriculaDemonstrativoPagamentoService
    {
        MatriculaDemonstrativoPagamentoResponse Get(Guid guid);

        IEnumerable<MatriculaDemonstrativoPagamentoResponse> Get(string competencia, string matricula);

        IEnumerable<MatriculaDemonstrativoPagamentoResponse> GetAll();

        IEnumerable<MatriculaDemonstrativoPagamentoResponse> GetByGuidColaborador(Guid guidColaborador);

        IEnumerable<MatriculaDemonstrativoPagamentoResponse> GetPendencias(DateTime competenciaInicial, DateTime competenciaFinal, SituacaoPendenciaDemonstrativoPagamento situacao = SituacaoPendenciaDemonstrativoPagamento.Todos);

        IEnumerable<GraficoComposicaoSalarialResponse> GetSalaryCompositionChart(Guid guidUsuario, string competencia);

        IEnumerable<GraficoEvolucaoSalarialResponse> GetSalaryEvolutionChart(Guid guidUsuario, Int16 quantidadeMesesRetroativos);

        ResumoImportacaoDemonstrativosPagamentoResponse ImportFileDemonstrativosPagamento(string cnpj, string content);

        MatriculaDemonstrativoPagamentoResponse SaveData(MatriculaDemonstrativoPagamentoCreateRequest? createRequest = null, MatriculaDemonstrativoPagamentoUpdateRequest? updateRequest = null);
    }
}