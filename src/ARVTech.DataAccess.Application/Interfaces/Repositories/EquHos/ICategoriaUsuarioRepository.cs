namespace ARVTech.DataAccess.Application.Interfaces.Repositories.EquHos
{
    using System;
    using ARVTech.DataAccess.Core.Entities.EquHos;
    using ARVTech.DataAccess.Repository.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface ICategoriaUsuarioRepository : ICreateRepository<CategoriaUsuarioEntity>, IReadRepository<CategoriaUsuarioEntity, int>, IUpdateRepository<CategoriaUsuarioEntity, int, CategoriaUsuarioEntity>, IDeleteRepository<int>
    {
    }
}