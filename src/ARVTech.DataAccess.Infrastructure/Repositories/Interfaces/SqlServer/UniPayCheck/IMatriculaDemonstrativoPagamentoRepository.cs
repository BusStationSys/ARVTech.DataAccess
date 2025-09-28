namespace ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.Domain.Enums.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IMatriculaDemonstrativoPagamentoRepository : ICreateRepository<MatriculaDemonstrativoPagamentoEntity>, IReadRepository<MatriculaDemonstrativoPagamentoEntity, Guid>, IUpdateRepository<MatriculaDemonstrativoPagamentoEntity, Guid, MatriculaDemonstrativoPagamentoEntity>, IDeleteRepository<Guid>
    {
        void DeleteEventosAndTotalizadoresByCompetenciaAndGuidMatricula(string competencia, Guid guidMatricula);

        IEnumerable<MatriculaDemonstrativoPagamentoEntity> Get(string competencia, string matricula);

        IEnumerable<MatriculaDemonstrativoPagamentoEntity> GetByCompetencia(string competencia);

        IEnumerable<MatriculaDemonstrativoPagamentoEntity> GetByGuidColaborador(Guid guidColaborador);

        IEnumerable<MatriculaDemonstrativoPagamentoEntity> GetByMatricula(string matricula);

        IEnumerable<MatriculaDemonstrativoPagamentoEntity> GetPendencias(DateTime competenciaInicial, DateTime competenciaFinal, SituacaoPendenciaDemonstrativoPagamentoEnum situacao = SituacaoPendenciaDemonstrativoPagamentoEnum.Todos);

        IEnumerable<(Guid guidUsuario, string competencia, decimal valor)> GetSalaryEvolutionChart(Guid guidUsuario, Int16 quantidadeMesesRetroativos);

        (DateTime dataInicio, DateTime dataFim, int quantidadeRegistrosAtualizados, int quantidadeRegistrosInalterados, int quantidadeRegistrosInseridos, int quantidadeRegistrosRejeitados) ImportFileDemonstrativosPagamento(string cnpj, string content);
    }
}