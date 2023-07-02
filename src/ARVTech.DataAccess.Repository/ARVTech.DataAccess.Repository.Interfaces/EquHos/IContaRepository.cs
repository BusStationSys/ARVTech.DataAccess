namespace ARVTech.DataAccess.Repository.Interfaces.EquHos
{
    using System;
    using ARVTech.DataAccess.Core.Entities.EquHos;
    using ARVTech.DataAccess.Repository.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IContaRepository : ICreateRepository<ContaEntity>, IReadRepository<ContaEntity, Guid>, IUpdateRepository<ContaEntity>, IDeleteRepository<Guid>
    {
    }
}