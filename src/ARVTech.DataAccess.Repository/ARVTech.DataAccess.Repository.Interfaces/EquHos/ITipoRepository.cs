namespace ARVTech.DataAccess.Repository.Interfaces.EquHos
{
    using ARVTech.DataAccess.Core.Entities.EquHos;
    using ARVTech.DataAccess.Repository.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface ITipoRepository : ICreateRepository<TipoEntity>, IReadRepository<TipoEntity, int>, IUpdateRepository<TipoEntity, int, TipoEntity>, IDeleteRepository<int>
    {
    }
}