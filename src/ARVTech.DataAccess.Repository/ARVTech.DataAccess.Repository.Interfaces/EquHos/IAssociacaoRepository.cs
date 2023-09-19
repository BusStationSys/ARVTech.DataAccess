namespace ARVTech.DataAccess.Repository.Interfaces.EquHos
{
    using ARVTech.DataAccess.Core.Entities.EquHos;
    using ARVTech.DataAccess.Repository.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
	public interface IAssociacaoRepository : ICreateRepository<AssociacaoEntity>, IReadRepository<AssociacaoEntity, int>, IUpdateRepository<AssociacaoEntity, int, AssociacaoEntity>, IDeleteRepository<int>
    {
    }
}