namespace ARVTech.DataAccess.CQRS.Queries
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Text;
    using ARVTech.DataAccess.DbFactories;

    public abstract class BaseQuery : IDisposable
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        protected SqlServerFactory _sqlServerFactory;

        protected SqlConnection _connection;

        protected SqlTransaction? _transaction;

        public abstract string CommandTextCreate();

        public abstract string CommandTextDelete();

        public abstract string CommandTextGetAll();

        public abstract string CommandTextGetById();

        public abstract string CommandTextUpdate();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        protected BaseQuery(SqlConnection connection, SqlTransaction? transaction = null)
        {
            this._connection = connection;
            this._transaction = transaction;

            this._sqlServerFactory = new SqlServerFactory(
                connection, 
                transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="alias"></param>
        /// <param name="fieldsToIgnore"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        protected string GetAllColumnsFromTable(string tableName, string alias = "", string fieldsToIgnore = "")
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(
                    nameof(tableName));

            var sbColumns = new StringBuilder();

            string cmdText = $@" SELECT TOP 0 *
                                   FROM [dbo].[{tableName}] WITH(NOLOCK)
                                  WHERE 0 = 1 ";

            using (var reader = this._sqlServerFactory.CreateCommand(
                cmdText).ExecuteReader())
            {
                reader.Read();

                using (var schemaTable = reader.GetSchemaTable())
                {
                    foreach (DataRow column in schemaTable.Rows)
                    {
                        if (sbColumns.Length > 0)
                            sbColumns.Append(", ");

                        if (!string.IsNullOrEmpty(alias))
                            sbColumns.AppendFormat(
                                CultureInfo.InvariantCulture,
                                "{0}.",
                                alias);

                        sbColumns.AppendFormat(
                            CultureInfo.InvariantCulture,
                            "[{0}]",
                            column["ColumnName"].ToString());
                    }
                }

                reader.Close();
            }

            string columns = sbColumns.ToString();

            if (!string.IsNullOrEmpty(fieldsToIgnore))
            {
                foreach (var fieldToIgnore in fieldsToIgnore.Split(';'))
                {
                    if (string.IsNullOrEmpty(fieldToIgnore))
                        continue;

                    columns = columns.Replace(fieldToIgnore, string.Empty).Trim();

                    columns = columns.Replace(", ,", ",").Trim();   // Vírgulas no meio.

                    if (columns.StartsWith(','))                    // Vírgulas no início.
                        columns = columns.Substring(1).Trim();

                    if (columns.EndsWith(','))                      // Vírgulas no fim.
                        columns = columns.Substring(0, columns.Length - 1).Trim();
                }
            }

            return columns;
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

                    this._sqlServerFactory.Dispose();
                    this._sqlServerFactory = null;
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