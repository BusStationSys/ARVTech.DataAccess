namespace FlooString.UnitOfWork.PostgreSql
{
    using FlooString.Repository.Interfaces;
    using FlooString.Repository.PostgreSql;
    using FlooString.UnitOfWork.Interfaces;
    using Npgsql;

    /// <summary>
    /// 
    /// </summary>
    public class UnitOfWorkPostgreSqlRepository : IUnitOfWorkRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public IAgenteRepository AgenteRepository { get; } = null as IAgenteRepository;

        /// <summary>
        /// 
        /// </summary>
        public IGrupoRepository GrupoRepository => throw new System.NotImplementedException();

        /// <summary>
        /// 
        /// </summary>
        public ILocalRepository LocalRepository => throw new System.NotImplementedException();

        /// <summary>
        /// 
        /// </summary>
        public IPessoaRepository PessoaRepository => throw new System.NotImplementedException();

        /// <summary>
        /// 
        /// </summary>
        public IUsuarioRepository UsuarioRepository => throw new System.NotImplementedException();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public UnitOfWorkPostgreSqlRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            this.AgenteRepository = new AgenteRepository(
                connection,
                transaction);
        }
    }
}