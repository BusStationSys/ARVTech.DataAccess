namespace ARVTech.DataAccess.Repository.Interfaces.EquHos
{
    using System;
    using ARVTech.DataAccess.Entities.EquHos;
    using ARVTech.DataAccess.Repository.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface ICabanhaRepository : ICreateRepository<CabanhaEntity>, IReadRepository<CabanhaEntity, Guid>, IUpdateRepository<CabanhaEntity>, IDeleteRepository<Guid>
    {
    }
}