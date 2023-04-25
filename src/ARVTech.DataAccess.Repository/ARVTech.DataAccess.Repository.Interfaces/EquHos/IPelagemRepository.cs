namespace ARVTech.DataAccess.Repository.Interfaces.EquHos
{
    using ARVTech.DataAccess.Entities.EquHos;
    using ARVTech.DataAccess.Repository.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
	public interface IPelagemRepository : ICreateRepository<PelagemEntity>, IReadRepository<PelagemEntity, int>, IUpdateRepository<PelagemEntity>, IDeleteRepository<int>
    {
    }
}