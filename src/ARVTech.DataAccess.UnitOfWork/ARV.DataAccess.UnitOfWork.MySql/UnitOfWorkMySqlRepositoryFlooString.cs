namespace FlooString.UnitOfWork.MySql
{
    using FlooString.Repository.Interfaces;
    using FlooString.Repository.MySql;
    using FlooString.UnitOfWork.Interfaces;
    using MySqlConnector;

    public class UnitOfWorkMySqlRepositoryFlooString : IUnitOfWorkRepositoryFlooString
    {
        public IGrupoRepository GrupoRepository { get; }

        public IUsuarioRepository UsuarioRepository { get; }

        public UnitOfWorkMySqlRepositoryFlooString(MySqlConnection connection, MySqlTransaction transaction)
        {
            this.GrupoRepository = new GrupoRepository(connection, transaction);
            this.UsuarioRepository = new UsuarioRepository(connection, transaction);
        }
    }
}