namespace ARVTech.DataAccess.Repository.Interfaces.EquHos
{
    using ARVTech.DataAccess.Entities.EquHos;
    using ARVTech.DataAccess.Repository.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
	public interface IAssociacaoRepository : ICreateRepository<AssociacaoEntity>, IReadRepository<AssociacaoEntity, int>, IUpdateRepository<AssociacaoEntity>, IDeleteRepository<int>
    {
    }
}