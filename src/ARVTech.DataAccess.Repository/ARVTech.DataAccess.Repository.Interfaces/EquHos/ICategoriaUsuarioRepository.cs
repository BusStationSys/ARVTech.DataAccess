namespace ARVTech.DataAccess.Repository.Interfaces.EquHos
{
    using System;
    using ARVTech.DataAccess.Entities.EquHos;
    using ARVTech.DataAccess.Repository.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface ICategoriaUsuarioRepository : ICreateRepository<CategoriaUsuarioEntity>, IReadRepository<CategoriaUsuarioEntity, int>, IUpdateRepository<CategoriaUsuarioEntity>, IDeleteRepository<int>
    {
    }
}