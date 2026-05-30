namespace ARVTech.DataAccess.DbFactories
{
    using System;
    using System.Data;
    using Microsoft.Data.SqlClient;

    public class SqlServerFactory : IDisposable
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        private SqlConnection _connection;

        private SqlTransaction? _transaction;

        public SqlServerFactory(SqlConnection connection, SqlTransaction? transaction = null)
        {
            this._connection = connection;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataTable CreateStructure(string tableName)
        {
            try
            {
                //string tableDb = tableName;

                //if (tableName.Substring(0, 1) != "[")
                //    tableDb = $"[{tableName}]";

                ArgumentException.ThrowIfNullOrWhiteSpace(
                    tableName);

                string tableDb = tableName.StartsWith('[')
                    ? tableName
                    : $"[{tableName}]";

                string cmdText = $@" Select Top 0 * 
                                       From {tableDb}
                                      Where 0 = 1 ";

                return this.ExecuteQuery(
                    cmdText);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string cmdText, CommandType commandType = CommandType.Text, SqlParameter[] parameters = null)
        {
            try
            {
                using (var command = this.CreateCommand(
                    cmdText))
                {
                    command.CommandType = commandType;

                    if (parameters != null)
                        command.Parameters.AddRange(parameters);

                    using (var dataAdapter = new SqlDataAdapter(
                        command))
                    {
                        dataAdapter.SelectCommand.CommandTimeout = this._connection.ConnectionTimeout;

                        var dt = new DataTable();

                        dataAdapter.Fill(
                            dt);

                        return dt;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public DataTable ExecuteQuery(SqlCommand command)
        {
            try
            {
                using (var dataAdapter = new SqlDataAdapter(
                    command))
                {
                    dataAdapter.SelectCommand.CommandTimeout = this._connection.ConnectionTimeout;

                    var dt = new DataTable();

                    dataAdapter.Fill(
                        dt);

                    return dt;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string cmdText, CommandType commandType = CommandType.Text, SqlParameter[] parameters = null)
        {
            try
            {
                using (var command = this.CreateCommand(
                    cmdText))
                {
                    command.CommandType = commandType;

                    if (parameters != null)
                        command.Parameters.AddRange(parameters);

                    using (var dataAdapter = new SqlDataAdapter(
                        command))
                    {
                        var ds = new DataSet();

                        // adpSQL.SelectCommand.CommandTimeout = cn.ConnectionTimeout
                        dataAdapter.Fill(
                            ds);

                        return ds;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(SqlCommand command)
        {
            try
            {
                using (var dataAdapter = new SqlDataAdapter(
                    command))
                {
                    var ds = new DataSet();

                    // adpSQL.SelectCommand.CommandTimeout = cn.ConnectionTimeout
                    dataAdapter.Fill(
                        ds);

                    return ds;
                }
            }
            catch
            {
                throw;
            }
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