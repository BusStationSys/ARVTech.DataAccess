namespace ARVTech.DataAccess.Repository.Interfaces.EquHos
{
    using System;
    using System.Collections.Generic;
    using ARVTech.DataAccess.Entities.EquHos;
    using ARVTech.DataAccess.Repository.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface ICabanhaRepository : ICreateRepository<CabanhaEntity>, IReadRepository<CabanhaEntity, Guid>, IUpdateRepository<CabanhaEntity>, IDeleteRepository<Guid>
    {
        bool ExisteCNPJDuplicado(Guid guid, string cnpj);

        IEnumerable<CabanhaEntity> GetAllByGuidConta(Guid guidConta);

        IEnumerable<CabanhaEntity> GetAllWithPermission(Guid guidConta, Guid guidUsuario);
    }
}