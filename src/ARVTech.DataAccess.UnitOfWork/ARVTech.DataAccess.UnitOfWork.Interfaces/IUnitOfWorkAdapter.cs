namespace ARVTech.DataAccess.UnitOfWork.Interfaces
{
    using System;
    using System.Data;

    public interface IUnitOfWorkAdapter : IDisposable
    {
        IUnitOfWorkRepositoryEquHos RepositoriesEquHos { get; }

        IDbConnection Connection { get; }

        IDbTransaction Transaction { get; }

        string ConnectionString { get; }

        void BeginTransaction();

        void CommitTransaction();

        void Rollback();
    }
}