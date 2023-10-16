namespace ARVTech.DataAccess.Application.Interfaces.Repositories.UniPayCheck
{
    using ARVTech.DataAccess.Application.Interfaces.Actions;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;

    /// <summary>
    /// 
    /// </summary>
    public interface ITotalizadorRepository : ICreateRepository<TotalizadorEntity>, IReadRepository<TotalizadorEntity, int>, IUpdateRepository<TotalizadorEntity, int, TotalizadorEntity>, IDeleteRepository<int>
    {
    }
}