namespace ARVTech.DataAccess.UnitOfWork.SqlServer
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using ARVTech.DataAccess.UnitOfWork.Interfaces;
    using ARVTech.DataAccess.UnitOfWork.SqlServer.UniPayCheck;

    public class UnitOfWorkSqlServerAdapter : IUnitOfWorkAdapter
    {
        private bool _disposed = false; // To detect redundant calls

        private SqlConnection _connection = null;

        private SqlTransaction _transaction = null;

        private readonly string _connectionString;

        public IDbConnection Connection
        {
            get
            {
                return this._connection;
            }
        }

        public IDbTransaction Transaction
        {
            get
            {
                return this._transaction;
            }
        }

        public string ConnectionString
        {
            get
            {
                return this._connectionString;
            }
        }

        public IUnitOfWorkRepositoryEquHos RepositoriesEquHos { get; set; } = null;

        public IUnitOfWorkRepositoryUniPayCheck RepositoriesUniPayCheck { get; set; } = null;

        public UnitOfWorkSqlServerAdapter(string connectionString)
        {
            try
            {
                this._connectionString = connectionString;

                this._connection = new SqlConnection(
                    this._connectionString);

                this._connection.Open();

                //this.RepositoriesEquHos = new UnitOfWorkSqlServerRepositoryEquHos(
                //    this._connection);

                this.RepositoriesUniPayCheck = new UnitOfWorkSqlServerRepositoryUniPayCheck(
                    this._connection);
            }
            catch
            {
                throw;
            }
        }

        public void BeginTransaction()
        {

            this._transaction = this._connection.BeginTransaction();

            //this.RepositoriesEquHos = new UnitOfWorkSqlServerRepositoryEquHos(
            //    this._connection,
            //    this._transaction);

            this.RepositoriesUniPayCheck = new UnitOfWorkSqlServerRepositoryUniPayCheck(
                this._connection,
                this._transaction);
        }

        public void CommitTransaction()
        {
            this._transaction.Commit();

            this._transaction.Dispose();
            this._transaction = null;
        }

        public void Rollback()
        {
            this._transaction.Rollback();

            this._transaction.Dispose();
            this._transaction = null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    // TODO: fazer dispose dos managed objects.
                    if (this._transaction != null)
                    {
                        this._transaction.Dispose();
                        this._transaction = null;
                    }

                    //if (this._connection != null && this._connection.State == ConnectionState.Open)
                    //    this._connection.Close();

                    if (this._connection != null)
                    {
                        this._connection.Dispose();
                        this._connection = null;
                    }
                }

                // TODO: liberar recursos unmanaged (unmanaged objects) e fazer override do finalizador.
                // TODO: campos grandes devem receber valor null.

                //this.RepositoriesEquHos = null;
                this.RepositoriesUniPayCheck = null;

                this._disposed = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWorkSqlServerAdapter()
        {
            this.Dispose(false);
        }
    }
}