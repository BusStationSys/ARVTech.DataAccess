namespace ARVTech.DataAccess.Application.Interfaces.Repositories.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Application.Interfaces.Actions;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;

    /// <summary>
    /// 
    /// </summary>
    public interface IMatriculaEspelhoPontoMarcacaoRepository : ICreateRepository<MatriculaEspelhoPontoMarcacaoEntity>, IReadRepository<MatriculaEspelhoPontoMarcacaoEntity, Guid>, IDeleteRepository<Guid>
    {
        MatriculaEspelhoPontoMarcacaoEntity GetByGuidMatriculaEspelhoPontoAndData(Guid guidMatriculaEspelhoPonto, DateTime data);
    }
}