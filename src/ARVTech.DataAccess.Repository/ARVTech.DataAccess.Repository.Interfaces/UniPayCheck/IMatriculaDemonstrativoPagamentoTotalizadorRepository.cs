namespace ARVTech.DataAccess.Repository.Interfaces.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Entities.UniPayCheck;
    using ARVTech.DataAccess.Repository.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IMatriculaDemonstrativoPagamentoTotalizadorRepository : ICreateRepository<MatriculaDemonstrativoPagamentoTotalizadorEntity>, IReadRepository<MatriculaDemonstrativoPagamentoTotalizadorEntity, Guid>, IDeleteRepository<Guid>
    {
        MatriculaDemonstrativoPagamentoTotalizadorEntity GetByGuidMatriculaDemonstrativoPagamentoAndIdTotalizador(Guid guidMatriculaDemonstrativoPagamento, int idTotalizador);
    }
}