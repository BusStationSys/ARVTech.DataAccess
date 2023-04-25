namespace ARVTech.DataAccess.UnitOfWork.Interfaces
{
    using ARVTech.DataAccess.Repository.Interfaces.Floostring;

    public interface IUnitOfWorkRepositoryFlooString
    {
        IGrupoRepository GrupoRepository { get; }

        IUsuarioRepository UsuarioRepository { get; }
    }
}