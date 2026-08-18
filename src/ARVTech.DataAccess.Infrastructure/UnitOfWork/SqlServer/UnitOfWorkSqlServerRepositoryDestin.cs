namespace ARVTech.DataAccess.Infrastructure.UnitOfWork.SqlServer
{
    using System;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.Destin;
    using ARVTech.DataAccess.Infrastructure.Repositories.SqlServer.Destin;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using Microsoft.Data.SqlClient;

    public class UnitOfWorkSqlServerRepositoryDestin : IUnitOfWorkRepositoryDestin
    {
        private bool _disposedValue;

        public IConcursoRepository ConcursoRepository { get; private set; }

        public IModalidadeRepository ModalidadeRepository { get; private set; }

        public UnitOfWorkSqlServerRepositoryDestin(SqlConnection connection, SqlTransaction? transaction = null)
        {
            this.ConcursoRepository = new ConcursoRepository(
                connection,
                transaction);

            this.ModalidadeRepository = new ModalidadeRepository(
                connection,
                transaction);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    (this.ConcursoRepository as IDisposable)?.Dispose();

                    this.ConcursoRepository = null;

                    (this.ModalidadeRepository as IDisposable)?.Dispose();

                    this.ModalidadeRepository = null;
                }

                this._disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}