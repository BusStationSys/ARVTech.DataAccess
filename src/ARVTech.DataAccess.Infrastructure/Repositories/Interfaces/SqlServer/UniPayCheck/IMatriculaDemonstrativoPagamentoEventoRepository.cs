namespace ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IMatriculaDemonstrativoPagamentoEventoRepository : ICreateRepository<MatriculaDemonstrativoPagamentoEventoEntity>, IReadRepository<MatriculaDemonstrativoPagamentoEventoEntity, Guid>, IUpdateRepository<MatriculaDemonstrativoPagamentoEventoEntity, Guid, MatriculaDemonstrativoPagamentoEventoEntity>, IDeleteRepository<Guid>
    {
        MatriculaDemonstrativoPagamentoEventoEntity GetByGuidMatriculaDemonstrativoPagamentoAndIdEvento(Guid guidMatriculaDemonstrativoPagamento, int idEvento);
    }
}