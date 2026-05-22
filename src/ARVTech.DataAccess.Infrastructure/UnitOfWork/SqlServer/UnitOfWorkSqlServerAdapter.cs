namespace ARVTech.DataAccess.Infrastructure.UnitOfWork.SqlServer
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using Microsoft.Extensions.Configuration;

    public class UnitOfWorkSqlServerAdapter : IUnitOfWorkAdapter
    {
        private bool _disposedValue; // To detect redundant calls

        private readonly SqlConnectionStringBuilder _connectionStringBuilder;

        private readonly int? _connectionTimeout;

        private readonly string _applicationName;

        private readonly string _userId = "sa";

        private readonly string _password = "123456";

        private readonly string _databaseName;

        private readonly string _serverName;

        private SqlConnection _connection;

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
                return this._connectionStringBuilder.ConnectionString;
            }
        }

        // public IUnitOfWorkRepositoryEquHos RepositoriesEquHos { get; set; } = null;

        public IUnitOfWorkRepositoryUniPayCheck RepositoriesUniPayCheck { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkSqlServerAdapter"/> class.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="connectionTimeout"></param>
        /// <param name="applicationName"></param>
        public UnitOfWorkSqlServerAdapter(IConfiguration configuration, int? connectionTimeout = null, string applicationName = "")
        {
            this._databaseName = configuration.GetValue<string>("DataAccess:SqlServer:DatabaseName");
            this._serverName = configuration.GetValue<string>("DataAccess:SqlServer:ServerName");

            if (this._serverName.ToLower().Contains("mssql2017.hostingzone.com.br"))
            {
                this._userId = "arvtech";
                this._password = "194619BBBA75";
            }

            if (connectionTimeout.HasValue)
                this._connectionTimeout = connectionTimeout.Value;

            this._applicationName = applicationName;

            this._connectionStringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = this._serverName,
                InitialCatalog = this._databaseName,
                UserID = this._userId,
                Password = this._password,
                //Enlist = true,
                //MaxPoolSize = 100,
                //MinPoolSize = 0,
                //Pooling = true,
                //ApplicationName = string.IsNullOrEmpty(this._applicationName) ?
                //    "." : // valor neutro exigido pelo builder
                //    this._applicationName,
            };

            if (this._connectionTimeout.HasValue)
                this._connectionStringBuilder.ConnectTimeout = this._connectionTimeout.Value;

            if (!string.IsNullOrEmpty(
                this._applicationName))
                this._connectionStringBuilder.ApplicationName = this._applicationName;

            ////  Remove ApplicationName da string se não foi informado.
            //if (string.IsNullOrEmpty(this._applicationName))
            //    this._connectionStringBuilder.Remove("Application Name");

            this.RepositoriesUniPayCheck = new UnitOfWorkSqlServerRepositoryUniPayCheck(
                this.GetConnection());
        }

        public void BeginTransaction()
        {
            if (this._transaction is not null)
                throw new InvalidOperationException("Já existe uma transação ativa. Realize o Commit ou Rollback antes de iniciar uma nova.");

            // Descarta repos ANTES de obter/recriar a conexão
            this.RepositoriesUniPayCheck?.Dispose();
            this.RepositoriesUniPayCheck = null;

            this.GetConnection(); //    pode recriar _connection se descartada

            this._transaction = this._connection.BeginTransaction();

            this.RepositoriesUniPayCheck = new UnitOfWorkSqlServerRepositoryUniPayCheck(
                this._connection,
                this._transaction);
        }

        public void CommitTransaction()
        {
            if (this._transaction is null)
                throw new InvalidOperationException("Nenhuma transação ativa para commitar.");

            this._transaction.Commit();

            this._transaction.Dispose();
            this._transaction = null;

            //  Recria repos sem transação para que continuem utilizáveis
            this.RepositoriesUniPayCheck?.Dispose();
            this.RepositoriesUniPayCheck = new UnitOfWorkSqlServerRepositoryUniPayCheck(
                this._connection);
        }

        public void Rollback()
        {
            if (this._transaction is null)
                throw new InvalidOperationException("Nenhuma transação ativa para rollback.");

            this._transaction.Rollback();

            this._transaction.Dispose();
            this._transaction = null;

            //  Recria repos sem transação para que continuem utilizáveis
            this.RepositoriesUniPayCheck?.Dispose();
            this.RepositoriesUniPayCheck = new UnitOfWorkSqlServerRepositoryUniPayCheck(
                this._connection);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    this.RepositoriesUniPayCheck?.Dispose();    //  Dispõe os repositórios (limpam referências internas).
                    this.RepositoriesUniPayCheck = null;

                    this._transaction?.Dispose();   //  Dispõe a transaction(UoW é proprietário).
                    this._transaction = null;

                    this._connection?.Dispose();    // Close() já é chamado internamente.
                    this._connection = null;
                }

                this._disposedValue = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private SqlConnection GetConnection()
        {
            if (this._connection is null ||
                string.IsNullOrEmpty(
                    this._connection.ConnectionString) ||
                this._connection.State == ConnectionState.Broken)
            {
                this._connection?.Dispose();

                this._connection = new SqlConnection(
                    this._connectionStringBuilder.ConnectionString);
            }

            if (this._connection.State != ConnectionState.Open)
                this._connection.Open();

            return this._connection;
        }
    }
}