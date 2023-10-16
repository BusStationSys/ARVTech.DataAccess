namespace ARVTech.DataAccess.Application.Interfaces.Repositories.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Application.Interfaces.Actions;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;

    /// <summary>
    /// 
    /// </summary>
    public interface IMatriculaEspelhoPontoCalculoRepository : ICreateRepository<MatriculaEspelhoPontoCalculoEntity>, IReadRepository<MatriculaEspelhoPontoCalculoEntity, Guid>, IDeleteRepository<Guid>
    {
        MatriculaEspelhoPontoCalculoEntity GetByGuidMatriculaEspelhoPontoAndIdCalculo(Guid guidMatriculaEspelhoPonto, int idCalculo);
    }
}