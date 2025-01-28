namespace ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IMatriculaDemonstrativoPagamentoTotalizadorRepository : ICreateRepository<MatriculaDemonstrativoPagamentoTotalizadorEntity>, IReadRepository<MatriculaDemonstrativoPagamentoTotalizadorEntity, Guid>, IDeleteRepository<Guid>
    {
        MatriculaDemonstrativoPagamentoTotalizadorEntity GetByGuidMatriculaDemonstrativoPagamentoAndIdTotalizador(Guid guidMatriculaDemonstrativoPagamento, int idTotalizador);
    }
}