namespace ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck
{
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IEventoRepository : ICreateRepository<EventoEntity>, IReadRepository<EventoEntity, int>, IUpdateRepository<EventoEntity, int, EventoEntity>, IDeleteRepository<int>
    {
        int GetLastId();
    }
}