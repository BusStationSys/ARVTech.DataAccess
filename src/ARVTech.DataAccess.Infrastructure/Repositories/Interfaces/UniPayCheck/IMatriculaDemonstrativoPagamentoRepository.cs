namespace ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
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
    }
}