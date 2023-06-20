namespace ARVTech.DataAccess.Repository.Interfaces.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Entities.UniPayCheck;
    using ARVTech.DataAccess.Repository.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IMatriculaEspelhoPontoRepository : ICreateRepository<MatriculaEspelhoPontoEntity>, IDeleteRepository<Guid>, IReadRepository<MatriculaEspelhoPontoEntity, Guid>, IUpdateRepository<MatriculaEspelhoPontoEntity>
    {
        void DeleteCalculosByCompetenciaAndGuidMatricula(string competencia, Guid guidMatricula);

        MatriculaEspelhoPontoEntity GetByCompetenciaAndMatricula(string competencia, string matricula);
    }
}