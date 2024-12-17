namespace ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IMatriculaEspelhoPontoCalculoRepository : ICreateRepository<MatriculaEspelhoPontoCalculoEntity>, IReadRepository<MatriculaEspelhoPontoCalculoEntity, Guid>, IDeleteRepository<Guid>
    {
        MatriculaEspelhoPontoCalculoEntity GetByGuidMatriculaEspelhoPontoAndIdCalculo(Guid guidMatriculaEspelhoPonto, int idCalculo);
    }
}