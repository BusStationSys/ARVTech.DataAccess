namespace ARVTech.DataAccess.Repository.Interfaces.EquHos
{
    using ARVTech.DataAccess.Entities.EquHos;
    using ARVTech.DataAccess.Repository.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface ITipoRepository : ICreateRepository<TipoEntity>, IReadRepository<TipoEntity, int>, IUpdateRepository<TipoEntity>, IDeleteRepository<int>
    {
    }
}