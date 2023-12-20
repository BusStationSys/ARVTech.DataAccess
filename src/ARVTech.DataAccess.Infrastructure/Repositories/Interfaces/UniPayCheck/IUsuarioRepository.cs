namespace ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.UniPayCheck
{
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IUsuarioRepository : ICreateRepository<UsuarioEntity>, IReadRepository<UsuarioEntity, Guid>, IUpdateRepository<UsuarioEntity, Guid, UsuarioEntity>, IDeleteRepository<Guid>
    {
        UsuarioEntity CheckPasswordValid(Guid guid, string password);

        IEnumerable<UsuarioEntity> GetByUsername(string cpfEmailUsername);
    }
}