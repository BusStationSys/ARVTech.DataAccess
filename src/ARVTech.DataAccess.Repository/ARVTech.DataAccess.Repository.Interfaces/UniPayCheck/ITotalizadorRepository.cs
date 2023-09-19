namespace ARVTech.DataAccess.Repository.Interfaces.UniPayCheck
{
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.Repository.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface ITotalizadorRepository : ICreateRepository<TotalizadorEntity>, IReadRepository<TotalizadorEntity, int>, IUpdateRepository<TotalizadorEntity, int, TotalizadorEntity>, IDeleteRepository<int>
    {
    }
}