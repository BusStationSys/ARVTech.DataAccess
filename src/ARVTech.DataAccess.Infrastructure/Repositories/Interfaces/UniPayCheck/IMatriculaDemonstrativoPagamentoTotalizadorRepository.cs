namespace ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IMatriculaDemonstrativoPagamentoTotalizadorRepository : ICreateRepository<MatriculaDemonstrativoPagamentoTotalizadorEntity>, IReadRepository<MatriculaDemonstrativoPagamentoTotalizadorEntity, Guid>, IDeleteRepository<Guid>
    {
        MatriculaDemonstrativoPagamentoTotalizadorEntity GetByGuidMatriculaDemonstrativoPagamentoAndIdTotalizador(Guid guidMatriculaDemonstrativoPagamento, int idTotalizador);
    }
}