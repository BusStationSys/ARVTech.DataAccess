namespace ARVTech.DataAccess.UnitOfWork.Interfaces
{
    using ARVTech.DataAccess.Repository.Interfaces.EquHos;

    public interface IUnitOfWorkRepositoryEquHos
    {
        IAssociacaoRepository AssociacaoRepository { get; }

        ICabanhaRepository CabanhaRepository { get; }

        IContaRepository ContaRepository { get; }

        IPelagemRepository PelagemRepository { get; }

        //IPessoaRepository PessoaRepository { get; }

        //IRecursoRepository RecursoRepository { get; }

        ITipoRepository TipoRepository { get; }

        //IUsuarioRepository UsuarioRepository { get; }
    }
}