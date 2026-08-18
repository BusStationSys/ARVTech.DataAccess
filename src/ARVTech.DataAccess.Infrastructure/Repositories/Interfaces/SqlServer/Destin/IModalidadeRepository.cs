namespace ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.Destin
{
    using ARVTech.DataAccess.Domain.Entities.Destin;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IModalidadeRepository : ICreateRepository<ModalidadeEntity>, IReadRepository<ModalidadeEntity, int>, IUpdateRepository<ModalidadeEntity, int, ModalidadeEntity>, IDeleteRepository<int>
    {
        int GetLastId();
    }
}