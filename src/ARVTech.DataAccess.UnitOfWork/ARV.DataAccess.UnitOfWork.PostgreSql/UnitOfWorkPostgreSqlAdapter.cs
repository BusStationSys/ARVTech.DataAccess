namespace FlooString.UnitOfWork.PostgreSql
{
    using System;
    using FlooString.UnitOfWork.Interfaces;
    using Npgsql;

    /// <summary>
    /// 
    /// </summary>
    public class UnitOfWorkPostgreSqlAdapter : IUnitOfWorkAdapter
    {
        /// <summary>
        /// 
        /// </summary>
        private NpgsqlConnection _connection { get; set; } = null as NpgsqlConnection;

        /// <summary>
        /// 
        /// </summary>
        private NpgsqlTransaction _transaction { get; set; } = null as NpgsqlTransaction;

        /// <summary>
        /// 
        /// </summary>
        public IUnitOfWorkRepositoryEquHos RepositoriesEquHos { get; set; } = null as IUnitOfWorkRepositoryEquHos;

        /// <summary>
        /// 
        /// </summary>
        public IUnitOfWorkRepositoryFlooString RepositoriesFlooString { get; set; } = null as IUnitOfWorkRepositoryFlooString;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public UnitOfWorkPostgreSqlAdapter(string connectionString)
        {
            try
            {
                this._connection = new NpgsqlConnection(connectionString);
                this._connection.Open();

                this._transaction = this._connection.BeginTransaction();

                //this.Repositories = new UnitOfWorkPostgreSqlRepository(
                //    this._connection,
                //    this._transaction);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Commit()
        {
            this._transaction.Commit();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (this._transaction != null)
                this._transaction.Dispose();

            if (this._connection != null)
            {
                this._connection.Close();
                this._connection.Dispose();
            }

            this.RepositoriesEquHos = null;
            this.RepositoriesFlooString = null;
        }
    }
}