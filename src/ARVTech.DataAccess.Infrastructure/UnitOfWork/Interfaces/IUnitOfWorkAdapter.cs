﻿namespace ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces
{
    using System;
    using System.Data;

    public interface IUnitOfWorkAdapter : IDisposable
    {
        // IUnitOfWorkRepositoryEquHos RepositoriesEquHos { get; }

        IUnitOfWorkRepositoryUniPayCheck RepositoriesUniPayCheck { get; }

        IDbConnection Connection { get; }

        IDbTransaction Transaction { get; }

        string ConnectionString { get; }

        void BeginTransaction();

        void CommitTransaction();

        void Rollback();
    }
}