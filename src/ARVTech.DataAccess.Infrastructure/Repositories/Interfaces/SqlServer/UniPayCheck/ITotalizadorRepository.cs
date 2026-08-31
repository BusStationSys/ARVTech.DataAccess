namespace ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck
{
    using ARVTech.DataAccess.Domain.Entities.PayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface ITotalizadorRepository : ICreateRepository<TotalizadorEntity>, IReadRepository<TotalizadorEntity, int>, IUpdateRepository<TotalizadorEntity, int, TotalizadorEntity>, IDeleteRepository<int>
    {
    }
}