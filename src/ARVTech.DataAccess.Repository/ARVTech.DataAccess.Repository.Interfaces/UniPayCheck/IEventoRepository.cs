namespace ARVTech.DataAccess.Repository.Interfaces.UniPayCheck
{
    using ARVTech.DataAccess.Entities.UniPayCheck;
    using ARVTech.DataAccess.Repository.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IEventoRepository : ICreateRepository<EventoEntity>, IReadRepository<EventoEntity, int>, IUpdateRepository<EventoEntity>, IDeleteRepository<int>
    {
    }
}