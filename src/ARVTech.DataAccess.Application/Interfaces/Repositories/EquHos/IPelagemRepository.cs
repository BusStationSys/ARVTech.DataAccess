namespace ARVTech.DataAccess.Application.Interfaces.Repositories.EquHos
{
    using ARVTech.DataAccess.Core.Entities.EquHos;
    using ARVTech.DataAccess.Repository.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
	public interface IPelagemRepository : ICreateRepository<PelagemEntity>, IReadRepository<PelagemEntity, int>, IUpdateRepository<PelagemEntity, int, PelagemEntity>, IDeleteRepository<int>
    {
    }
}