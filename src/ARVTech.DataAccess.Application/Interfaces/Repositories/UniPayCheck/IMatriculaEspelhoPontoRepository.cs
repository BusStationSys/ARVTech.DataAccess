namespace ARVTech.DataAccess.Application.Interfaces.Repositories.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Application.Interfaces.Actions;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;

    /// <summary>
    /// 
    /// </summary>
    public interface IMatriculaEspelhoPontoRepository : ICreateRepository<MatriculaEspelhoPontoEntity>, IDeleteRepository<Guid>, IReadRepository<MatriculaEspelhoPontoEntity, Guid>, IUpdateRepository<MatriculaEspelhoPontoEntity, Guid, MatriculaEspelhoPontoEntity>
    {
        void DeleteCalculosAndMarcacoesByCompetenciaAndGuidMatricula(string competencia, Guid guidMatricula);

        IEnumerable<MatriculaEspelhoPontoEntity> Get(string competencia, string matricula);

        MatriculaEspelhoPontoEntity GetByCompetenciaAndMatricula(string competencia, string matricula);

        IEnumerable<MatriculaEspelhoPontoEntity> GetByGuidColaborador(Guid guidColaborador);
    }
}