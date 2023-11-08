﻿namespace ARVTech.DataAccess.Infrastructure.UnitOfWork.SqlServer
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using Microsoft.Extensions.Configuration;

    public class UnitOfWorkSqlServerAdapter : IUnitOfWorkAdapter
    {
        private bool _disposed; // To detect redundant calls

        private readonly int _connectionTimeout;

        private readonly string _applicationName;

        private readonly string _userId = "sa";

        private readonly string _password = "123456";

        private readonly string _databaseName;

        private readonly string _serverName;

        private readonly SqlConnection _connection;

        private string _connectionString;

        private SqlTransaction? _transaction;

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

        // public IUnitOfWorkRepositoryEquHos RepositoriesEquHos { get; set; } = null;

        public IUnitOfWorkRepositoryUniPayCheck RepositoriesUniPayCheck { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkSqlServerAdapter"/> class.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="connectionTimeout"></param>
        /// <param name="applicationName"></param>
        public UnitOfWorkSqlServerAdapter(IConfiguration configuration, int connectionTimeout = 0, string applicationName = "")
        {
            try
            {
                this._databaseName = configuration.GetValue<string>("DataAccess:SqlServer:DatabaseName");
                this._serverName = configuration.GetValue<string>("DataAccess:SqlServer:ServerName");

                if (this._serverName.ToLower().Contains("sqlserver01.totalbyte.com.br"))
                {
                    this._userId = "totalbyte25";
                    this._password = "tot@4050!3Tf_5";
                }

                this._applicationName = applicationName;
                this._connectionTimeout = connectionTimeout;

                this.RefreshConnectionString();

                this._connection = new SqlConnection(
                    this._connectionString);

                this._connection.Open();

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
                    this._transaction?.Dispose();
                    this._transaction = null;

                    if (this._connection?.State == ConnectionState.Open)
                        this._connection.Close();

                    this._connection?.Dispose();
                    //this._connection = null;
                }

                // TODO: liberar recursos unmanaged (unmanaged objects) e fazer override do finalizador.
                // TODO: campos grandes devem receber valor null.

                //  this.RepositoriesEquHos = null;
                this.RepositoriesUniPayCheck = null;

                this._disposed = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void RefreshConnectionString()
        {
            //this._connectionString = $@"user id={this._userId};password={this._password};initial catalog={this._databaseName};data source={this._serverName};Connection Timeout={this._connectionTimeout};Connection Reset=true;Enlist=true;Max Pool Size=100;Min Pool Size=0;Pooling=true";

            this._connectionString = $@"user id={this._userId};password={this._password};initial catalog={this._databaseName};data source={this._serverName};Connection Timeout={this._connectionTimeout};Enlist=true;Max Pool Size=100;Min Pool Size=0;Pooling=true";

            if (!string.IsNullOrEmpty(this._applicationName))
                this._connectionString = string.Concat(
                    this._connectionString,
                    ";Application Name=",
                    this._applicationName);
        }

        ~UnitOfWorkSqlServerAdapter()
        {
            this.Dispose(false);
        }
    }
}