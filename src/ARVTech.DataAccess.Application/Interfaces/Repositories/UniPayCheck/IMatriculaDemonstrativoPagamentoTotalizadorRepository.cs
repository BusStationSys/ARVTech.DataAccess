namespace ARVTech.DataAccess.Application.Interfaces.Repositories.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Application.Interfaces.Actions;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;

    /// <summary>
    /// 
    /// </summary>
    public interface IMatriculaDemonstrativoPagamentoTotalizadorRepository : ICreateRepository<MatriculaDemonstrativoPagamentoTotalizadorEntity>, IReadRepository<MatriculaDemonstrativoPagamentoTotalizadorEntity, Guid>, IDeleteRepository<Guid>
    {
        MatriculaDemonstrativoPagamentoTotalizadorEntity GetByGuidMatriculaDemonstrativoPagamentoAndIdTotalizador(Guid guidMatriculaDemonstrativoPagamento, int idTotalizador);
    }
}