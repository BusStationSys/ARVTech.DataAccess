namespace ARVTech.DataAccess.Repository.Interfaces.EquHos
{
    using System;
    using ARVTech.DataAccess.Entities.EquHos;
    using ARVTech.DataAccess.Repository.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IAnimalRepository : ICreateRepository<AnimalEntity>, IReadRepository<AnimalEntity, Guid>, IUpdateRepository<AnimalEntity>, IDeleteRepository<Guid>
    {
    }
}