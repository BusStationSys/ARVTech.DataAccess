namespace ARVTech.DataAccess.CQRS.Queries
{
    using System;
    using System.Collections.Concurrent;
    using System.Data;
    using System.Globalization;
    using System.Text;
    using ARVTech.DataAccess.DbFactories;
    using Microsoft.Data.SqlClient;

    public abstract class BaseQuery : IDisposable
    {
        private bool _disposedValue;

        private SqlServerFactory? _sqlServerFactory;    // Lazy: instantiated only on the first call to GetAllColumnsFromTable.

        private static readonly ConcurrentDictionary<string, string> _columnsCache = new();

        protected SqlConnection? _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseQuery"/> class without a database connection.
        /// Use this constructor when the query operates exclusively with stored procedures.
        /// </summary>
        protected BaseQuery()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseQuery"/> class with a database connection.
        /// Use this constructor when the query executes inline SQL statements.
        /// </summary>
        /// <param name="connection">The database connection used to execute the query.</param>
        protected BaseQuery(SqlConnection connection)
        {
            ArgumentNullException.ThrowIfNull(connection, nameof(connection));

            this._connection = connection;
        }

        /// <summary>
        /// Retrieves a comma-separated list of all column names from the specified database table, optionally prefixed with an alias and excluding specified fields.
        /// </summary>
        /// <param name="tableName">The name of the database table.</param>
        /// <param name="alias">An optional alias to prefix each column name.</param>
        /// <param name="fieldsToIgnore">A semicolon-separated list of fields to exclude.</param>
        /// <returns>A comma-separated list of column names.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        protected string GetAllColumnsFromTable(string tableName, string alias = "", string fieldsToIgnore = "")
        {
            ArgumentException.ThrowIfNullOrEmpty(
                tableName,
                nameof(tableName));

            if (this._connection is null)
                throw new InvalidOperationException(
                    $"{nameof(GetAllColumnsFromTable)} requer uma conexão ativa." +
                    $"Utilize o construtor que recebe {nameof(SqlConnection)}.");

            string cacheKey = $"{tableName}|{alias}|{fieldsToIgnore}";

            return _columnsCache.GetOrAdd(
                cacheKey,
                _ => this.BuildColumnsFromTable(tableName, alias, fieldsToIgnore));
        }

        /// <summary>
        /// Refreshes the provided SQL command text template by appending WHERE, ORDER BY, and pagination clauses based on the specified parameters.
        /// </summary>
        /// <param name="commandTextTemplate">The base SQL command text template.</param>
        /// <param name="where">The WHERE clause to filter the results.</param>
        /// <param name="orderBy">The ORDER BY clause to sort the results.</param>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The number of rows per page for pagination.</param>
        /// <returns>The refreshed SQL command text with applied filters, sorting, and pagination.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected string RefreshPagination(
            string commandTextTemplate,
            string where = "",
            string orderBy = "",
            uint? pageNumber = null,
            uint? pageSize = null)
        {
            ArgumentException.ThrowIfNullOrEmpty(
                commandTextTemplate,
                nameof(commandTextTemplate));

            if (!string.IsNullOrEmpty(orderBy))
            {
                if (pageNumber is 0)
                    throw new ArgumentOutOfRangeException(
                        nameof(pageNumber),
                        "Parâmetro deve ser maior que zero.");

                if (pageSize is 0)
                    throw new ArgumentOutOfRangeException(
                        nameof(pageSize),
                        "Parâmetro deve ser maior que zero.");
            }

            if (string.IsNullOrEmpty(where) &&
                string.IsNullOrEmpty(orderBy))
                return commandTextTemplate;

            var commandText = $" {commandTextTemplate} WHERE 1 = 1 ";

            if (!string.IsNullOrEmpty(where))
                commandText = $" {commandText} AND ( {where} ) ";

            if (!string.IsNullOrEmpty(orderBy))
            {
                commandText = $" {commandText} ORDER BY {orderBy} ";

                if (pageNumber != null && pageSize != null)
                    commandText = $@" {commandText}
                                      OFFSET ({pageNumber} - 1) * {pageSize} ROWS
                                  FETCH NEXT {pageSize} ROWS ONLY ";
            }

            return commandText;
        }

        /// <summary>
        /// Gets the SQL command text to retrieve all records from the table.
        /// </summary>
        /// <returns>The SQL command text to retrieve all records.</returns>
        public abstract string CommandTextGetAll();

        /// <summary>
        /// Gets the SQL command text to retrieve a record by its ID.   
        /// </summary>
        /// <returns>The SQL command text to retrieve a record by its ID.</returns>
        public abstract string CommandTextGetById();

        /// <summary>
        /// Gets the SQL command text to retrieve records based on custom filters, sorting, and pagination.
        /// </summary>
        /// <param name="where">The WHERE clause to filter the results.</param>
        /// <param name="orderBy">The ORDER BY clause to sort the results.</param>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The number of rows per page for pagination.</param>
        /// <returns>The SQL command text to retrieve records based on custom filters, sorting, and pagination.</returns>
        public abstract string CommandTextGetCustom(
            string where = "",
            string orderBy = "",
            uint? pageNumber = null,
            uint? pageSize = null);

        /// <summary>
        /// Builds the column list for a SQL query based on the specified table, alias, and fields to ignore.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="alias">The alias to use for the table.</param>
        /// <param name="fieldsToIgnore">A semicolon-separated list of fields to ignore.</param>
        /// <returns>The column list for the SQL query.</returns>
        private string BuildColumnsFromTable(string tableName, string alias, string fieldsToIgnore)
        {
            // Lazy: instancia somente na primeira chamada.
            this._sqlServerFactory ??= new SqlServerFactory(
                this._connection!);     //  Não vai chegar NULO aqui porque GetAllColumnsFromTable lança InvalidOperationException se _connection for nulo.

            var sbColumns = new StringBuilder();

            string cmdText = $@" SELECT TOP 0 *
                                   FROM [dbo].[{tableName}] WITH(NOLOCK)
                                  WHERE 0 = 1 ";

            using var reader = this._sqlServerFactory
                .CreateCommand(cmdText)
                .ExecuteReader();

            using var schemaTable = reader.GetSchemaTable();

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
                    column["ColumnName"]);
            }

            if (string.IsNullOrEmpty(fieldsToIgnore))
                return sbColumns.ToString();

            var columns = sbColumns.ToString();

            foreach (var fieldToIgnore in fieldsToIgnore.Split(';'))
            {
                if (string.IsNullOrEmpty(fieldToIgnore))
                    continue;

                columns = columns.Replace(fieldToIgnore, string.Empty).Trim();
                columns = columns.Replace(", ,", ",").Trim();   // Vírgulas no meio.

                if (columns.StartsWith(','))
                    columns = columns[1..].Trim();              // Vírgulas no início.

                if (columns.EndsWith(','))
                    columns = columns[..^1].Trim();             // Vírgulas no fim.
            }

            return columns;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this._disposedValue)
                return;

            if (disposing)
                this._sqlServerFactory?.Dispose();

            this._disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}