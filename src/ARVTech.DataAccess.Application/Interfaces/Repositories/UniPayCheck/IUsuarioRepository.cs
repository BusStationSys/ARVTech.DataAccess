namespace ARVTech.DataAccess.Application.Interfaces.Repositories.UniPayCheck
{
    using ARVTech.DataAccess.Application.Interfaces.Actions;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;

    /// <summary>
    /// 
    /// </summary>
    public interface IUsuarioRepository : ICreateRepository<UsuarioEntity>, IReadRepository<UsuarioEntity, Guid>, IUpdateRepository<UsuarioEntity, Guid, UsuarioEntity>, IDeleteRepository<Guid>
    {
        UsuarioEntity CheckPasswordValid(Guid guid, string password);

        IEnumerable<UsuarioEntity> GetByUsername(string username);
    }
}