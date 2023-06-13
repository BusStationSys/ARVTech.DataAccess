namespace ARVTech.DataAccess.Repository.Interfaces.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Entities.UniPayCheck;
    using ARVTech.DataAccess.Repository.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IMatriculaDemonstrativoPagamentoEventoRepository : ICreateRepository<MatriculaDemonstrativoPagamentoEventoEntity>, IReadRepository<MatriculaDemonstrativoPagamentoEventoEntity, Guid>, IDeleteRepository<Guid>
    {
        MatriculaDemonstrativoPagamentoEventoEntity GetByGuidMatriculaDemonstrativoPagamentoAndIdEvento(Guid guidMatriculaDemonstrativoPagamento, int idEvento);
    }
}