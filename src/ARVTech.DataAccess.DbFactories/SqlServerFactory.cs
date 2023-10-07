namespace ARVTech.DataAccess.DbFactories
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class SqlServerFactory : IDisposable
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        private readonly string _connectionString;

        private SqlConnection _connection;

        private SqlTransaction _transaction;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public SqlServerFactory(string connectionString)
        {
            this._connectionString = connectionString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public SqlServerFactory(SqlConnection connection, SqlTransaction transaction = null)
        {
            this._connection = connection;
            this._connectionString = connection.ConnectionString;

            this._transaction = transaction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public SqlCommand CreateCommand(string cmdText)
        {
            return new SqlCommand(
                cmdText,
                this._connection,
                this._transaction)
            {
                CommandTimeout = 0,
            };
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    if (this._connection?.State == ConnectionState.Open)
                        this._connection.Dispose();

                    this._connection = null;

                    this._transaction?.Dispose();
                    this._transaction = null;
                }

                this._disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}