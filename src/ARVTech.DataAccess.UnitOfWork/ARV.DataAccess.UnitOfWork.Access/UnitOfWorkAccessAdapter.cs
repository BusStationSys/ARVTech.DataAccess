namespace ARV.DataAccess.UnitOfWork.Access
{
    using System;
    using System.Data;
    using System.Data.OleDb;
    using ARV.DataAccess.UnitOfWork.Interfaces;

    public class UnitOfWorkAccessAdapter : IDisposable, IUnitOfWorkAdapter
    {
        private bool _disposed = false; // To detect redundant calls

        private OleDbConnection _connection { get; set; }

        private OleDbTransaction _transaction { get; set; }

        public IUnitOfWorkRepositoryParker RepositoriesParker { get; set; } = null as IUnitOfWorkRepositoryParker;

        public IUnitOfWorkRepositoryEmpresarius RepositoriesEmpresarius => throw new NotImplementedException();

        public IUnitOfWorkRepositoryEquHos RepositoriesEquHos => throw new NotImplementedException();

        public UnitOfWorkAccessAdapter(string connectionString)
        {
            try
            {
                this._connection = new OleDbConnection(connectionString);
                this._connection.Open();

                this._transaction = this._connection.BeginTransaction();

                this.RepositoriesParker = new UnitOfWorkAccessRepositoryParker(
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

                    if (this._connection != null && this._connection.State == ConnectionState.Open)
                        this._connection.Close();

                    if (this._connection != null)
                    {
                        this._connection.Dispose();
                        this._connection = null;
                    }
                }

                // TODO: liberar recursos unmanaged (unmanaged objects) e fazer override do finalizador.
                // TODO: campos grandes devem receber valor null.

                this.RepositoriesParker = null;

                this._disposed = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWorkAccessAdapter()
        {
            this.Dispose(false);
        }
    }
}