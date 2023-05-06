namespace ARVTech.DataAccess.UnitOfWork.Interfaces
{
    using ARVTech.DataAccess.Repository.Interfaces.EquHos;

    public interface IUnitOfWorkRepositoryEquHos
    {
        IAnimalRepository AnimalRepository { get; }

        IAssociacaoRepository AssociacaoRepository { get; }

        ICabanhaRepository CabanhaRepository { get; }

        IContaRepository ContaRepository { get; }

        IPelagemRepository PelagemRepository { get; }

        ITipoRepository TipoRepository { get; }

        IUsuarioRepository UsuarioRepository { get; }
    }
}