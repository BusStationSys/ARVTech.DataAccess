namespace ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.EquHos
{
    using System;
    using System.Collections.Generic;
    using ARVTech.DataAccess.Core.Entities.EquHos;
    using ARVTech.DataAccess.Repository.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IAnimalRepository : ICreateRepository<AnimalEntity>, IReadRepository<AnimalEntity, Guid>, IUpdateRepository<AnimalEntity, Guid, AnimalEntity>, IDeleteRepository<Guid>
    {
        IEnumerable<AnimalEntity> GetAllBySexoAndArgumento(Guid guidConta, Guid guidCabanha, string sexo, string argumento);

        IEnumerable<AnimalEntity> GetAllFilhos(string sexo, Guid guid);
    }
}