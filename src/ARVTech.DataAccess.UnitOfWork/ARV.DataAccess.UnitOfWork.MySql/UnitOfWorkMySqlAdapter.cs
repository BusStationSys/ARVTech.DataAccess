namespace FlooString.UnitOfWork.MySql
{
    using System;
    using FlooString.UnitOfWork.Interfaces;
    using MySqlConnector;

    public class UnitOfWorkMySqlAdapter : IUnitOfWorkAdapter, IDisposable
    {
        private MySqlConnection _connection { get; set; }

        private MySqlTransaction _transaction { get; set; }

        public IUnitOfWorkRepositoryEquHos RepositoriesEquHos { get; set; } = null as IUnitOfWorkRepositoryEquHos;

        public IUnitOfWorkRepositoryFlooString RepositoriesFlooString { get; set; } = null as IUnitOfWorkRepositoryFlooString;

        public UnitOfWorkMySqlAdapter(string connectionString)
        {
            try
            {
                this._connection = new MySqlConnection(connectionString);
                this._connection.Open();

                this._transaction = this._connection.BeginTransaction();

                this.RepositoriesFlooString = new UnitOfWorkMySqlRepositoryFlooString(
                    this._connection,
                    this._transaction);
            }
            catch
            {
                throw;
            }
        }

        public void Commit()
        {
            this._transaction.Commit();
        }

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