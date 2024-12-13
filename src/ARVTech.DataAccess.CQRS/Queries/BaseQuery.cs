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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        protected BaseQuery(SqlConnection connection, SqlTransaction? transaction = null)
        {
            _connection = connection;
            _transaction = transaction;

            _sqlServerFactory = new SqlServerFactory(
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
                    nameof(
                        tableName),
                    "Nome da Tabela deve ser informado.");

            var sbColumns = new StringBuilder();

            string cmdText = $@" SELECT TOP 0 *
                                   FROM [dbo].[{tableName}] WITH(NOLOCK)
                                  WHERE 0 = 1 ";

            using (var reader = _sqlServerFactory.CreateCommand(
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

            return columns;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandTextTemplate"></param>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        protected string RefreshPagination(
            string commandTextTemplate,
            string where = "",
            string orderBy = "",
            uint? pageNumber = null,
            uint? pageSize = null)
        {
            if (string.IsNullOrEmpty(commandTextTemplate))
                throw new ArgumentNullException(
                    nameof(
                        commandTextTemplate),
                    "Parâmetro deve estar preenchido.");

            if (!string.IsNullOrEmpty(orderBy))
            {
                if (pageNumber.HasValue &&
                    pageNumber == 0)
                    throw new ArgumentNullException(
                        nameof(
                            pageNumber),
                        "Parâmetro deve ser maior que zero.");

                if (pageSize.HasValue &&
                    pageSize == 0)
                    throw new ArgumentNullException(
                        nameof(
                            pageSize),
                        "Parâmetro deve ser maior que zero.");
            }

            if (string.IsNullOrEmpty(where) &&
                string.IsNullOrEmpty(orderBy))
                return commandTextTemplate;

            string commandText = $@" {commandTextTemplate} WHERE 1 = 1 ";

            if (!string.IsNullOrEmpty(where))
                commandText = string.Concat(
                    commandText,
                    " AND ",
                    " ( ",
                    where,
                    " ) ");

            if (!string.IsNullOrEmpty(orderBy))
            {
                commandText = $@" {commandText}
                                    ORDER BY {orderBy} ";

                if (pageNumber != null &&
                    pageSize != null)
                    commandText = $@" {commandText}
                                      OFFSET ({pageNumber} - 1) * {pageSize} ROWS
                                  FETCH NEXT {pageSize} ROWS ONLY ";
            }

            return commandText;
        }

        public abstract string CommandTextGetAll();

        public abstract string CommandTextGetById();

        public abstract string CommandTextGetCustom(
            string where = "",
            string orderBy = "",
            uint? pageNumber = null,
            uint? pageSize = null);

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (_connection?.State == ConnectionState.Open)
                        _connection.Dispose();

                    _connection = null;

                    _transaction?.Dispose();
                    _transaction = null;

                    _sqlServerFactory.Dispose();
                    _sqlServerFactory = null;
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}