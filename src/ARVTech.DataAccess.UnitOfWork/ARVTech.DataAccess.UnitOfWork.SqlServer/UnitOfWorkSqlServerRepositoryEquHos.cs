namespace ARVTech.DataAccess.UnitOfWork.SqlServer.EquHos
{
    using System.Data.SqlClient;
    using ARVTech.DataAccess.Repository.Interfaces.EquHos;
    using ARVTech.DataAccess.Repository.SqlServer.EquHos;
    using ARVTech.DataAccess.UnitOfWork.Interfaces;

    public class UnitOfWorkSqlServerRepositoryEquHos : IUnitOfWorkRepositoryEquHos
    {
        public IAnimalRepository AnimalRepository { get; }

        public IAssociacaoRepository AssociacaoRepository { get; }

        public ICabanhaRepository CabanhaRepository { get; }

        public IContaRepository ContaRepository { get; }

        public IPelagemRepository PelagemRepository { get; }

        //public IPessoaRepository PessoaRepository => throw new System.NotImplementedException();

        //public IRecursoRepository RecursoRepository { get; }

        public ITipoRepository TipoRepository { get; }

        public IUsuarioRepository UsuarioRepository { get; }

        public UnitOfWorkSqlServerRepositoryEquHos(SqlConnection connection)
        {
            this.AnimalRepository = new AnimalRepository(connection);
            this.AssociacaoRepository = new AssociacaoRepository(connection);
            this.CabanhaRepository = new CabanhaRepository(connection);
            this.ContaRepository = new ContaRepository(connection);
            this.PelagemRepository = new PelagemRepository(connection);
            //this.RecursoRepository = new RecursoRepository(connection, transaction);
            this.TipoRepository = new TipoRepository(connection);
            this.UsuarioRepository = new UsuarioRepository(connection);
        }

        public UnitOfWorkSqlServerRepositoryEquHos(SqlConnection connection, SqlTransaction transaction)
        {
            this.AnimalRepository = new AnimalRepository(connection, transaction);
            this.AssociacaoRepository = new AssociacaoRepository(connection, transaction);
            this.CabanhaRepository = new CabanhaRepository(connection, transaction);
            this.ContaRepository = new ContaRepository(connection, transaction);
            this.PelagemRepository = new PelagemRepository(connection, transaction);
            //this.RecursoRepository = new RecursoRepository(connection, transaction);
            this.TipoRepository = new TipoRepository(connection, transaction);
            this.UsuarioRepository = new UsuarioRepository(connection, transaction);
        }
    }
}