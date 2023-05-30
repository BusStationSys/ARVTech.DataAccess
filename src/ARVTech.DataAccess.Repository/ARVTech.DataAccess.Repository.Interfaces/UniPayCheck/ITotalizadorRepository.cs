namespace ARVTech.DataAccess.Repository.Interfaces.UniPayCheck
{
    using ARVTech.DataAccess.Entities.UniPayCheck;
    using ARVTech.DataAccess.Repository.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface ITotalizadorRepository : ICreateRepository<TotalizadorEntity>, IReadRepository<TotalizadorEntity, int>, IUpdateRepository<TotalizadorEntity>, IDeleteRepository<int>
    {
    }
}