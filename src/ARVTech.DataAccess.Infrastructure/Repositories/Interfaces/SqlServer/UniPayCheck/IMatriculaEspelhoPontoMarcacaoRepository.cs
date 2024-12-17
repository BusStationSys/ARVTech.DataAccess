namespace ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IMatriculaEspelhoPontoMarcacaoRepository : ICreateRepository<MatriculaEspelhoPontoMarcacaoEntity>, IReadRepository<MatriculaEspelhoPontoMarcacaoEntity, Guid>, IDeleteRepository<Guid>
    {
        MatriculaEspelhoPontoMarcacaoEntity GetByGuidMatriculaEspelhoPontoAndData(Guid guidMatriculaEspelhoPonto, DateTime data);
    }
}