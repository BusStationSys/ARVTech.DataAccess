namespace ARVTech.DataAccess.Application.Interfaces.Repositories.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Application.Interfaces.Actions;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;

    /// <summary>
    /// 
    /// </summary>
    public interface IMatriculaDemonstrativoPagamentoEventoRepository : ICreateRepository<MatriculaDemonstrativoPagamentoEventoEntity>, IReadRepository<MatriculaDemonstrativoPagamentoEventoEntity, Guid>, IUpdateRepository<MatriculaDemonstrativoPagamentoEventoEntity, Guid, MatriculaDemonstrativoPagamentoEventoEntity>, IDeleteRepository<Guid>
    {
        MatriculaDemonstrativoPagamentoEventoEntity GetByGuidMatriculaDemonstrativoPagamentoAndIdEvento(Guid guidMatriculaDemonstrativoPagamento, int idEvento);
    }
}