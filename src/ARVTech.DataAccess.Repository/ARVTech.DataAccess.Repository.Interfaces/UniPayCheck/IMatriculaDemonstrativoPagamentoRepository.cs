namespace ARVTech.DataAccess.Repository.Interfaces.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Entities.UniPayCheck;
    using ARVTech.DataAccess.Repository.Interfaces.Actions;

    public interface IMatriculaDemonstrativoPagamentoRepository : ICreateRepository<MatriculaDemonstrativoPagamentoEntity>, IReadRepository<MatriculaDemonstrativoPagamentoEntity, Guid>, IUpdateRepository<MatriculaDemonstrativoPagamentoEntity>, IDeleteRepository<Guid>
    {
        void DeleteByCompetenciaAndGuidMatricula(string competencia, Guid guidMatricula);

        MatriculaDemonstrativoPagamentoEntity GetByCompetenciaAndMatricula(string competencia, string matricula);
    }
}