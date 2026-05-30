namespace ARVTech.DataAccess.Infrastructure.Repositories.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Dapper;
    using Microsoft.Data.SqlClient;

    public abstract class BaseRepository : IDisposable
    {
        protected readonly string TableAliasCalculos = "C";
        protected readonly string TableNameCalculos = "CALCULOS";

        protected readonly string TableAliasEventos = "E";
        protected readonly string TableNameEventos = "EVENTOS";

        protected readonly string TableAliasMatriculas = "M";
        protected readonly string TableNameMatriculas = "MATRICULAS";

        protected readonly string TableAliasMatriculasDemonstrativosPagamento = "MDP";
        protected readonly string TableNameMatriculasDemonstrativosPagamento = "MATRICULAS_DEMONSTRATIVOS_PAGAMENTO";

        protected readonly string TableAliasMatriculasDemonstrativosPagamentoEventos = "MDPE";
        protected readonly string TableNameMatriculasDemonstrativosPagamentoEventos = "MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_EVENTOS";

        protected readonly string TableAliasMatriculasDemonstrativosPagamentoTotalizadores = "MDPT";
        protected readonly string TableNameMatriculasDemonstrativosPagamentoTotalizadores = "MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES";

        protected readonly string TableAliasMatriculasEspelhosPonto = "MEP";
        protected readonly string TableNameMatriculasEspelhosPonto = "MATRICULAS_ESPELHOS_PONTO";

        protected readonly string TableAliasMatriculasEspelhosPontoCalculos = "MEPC";
        protected readonly string TableNameMatriculasEspelhosPontoCalculos = "MATRICULAS_ESPELHOS_PONTO_CALCULOS";

        protected readonly string TableAliasMatriculasEspelhosPontoMarcacoes = "MEPM";
        protected readonly string TableNameMatriculasEspelhosPontoMarcacoes = "MATRICULAS_ESPELHOS_PONTO_MARCACOES";

        protected readonly string TableAliasPessoas = "P";
        protected readonly string TableNamePessoas = "PESSOAS";

        protected readonly string TableAliasPessoasFisicas = "PF";
        protected readonly string TableNamePessoasFisicas = "PESSOAS_FISICAS";

        protected readonly string TableAliasPessoasJuridicas = "PJ";
        protected readonly string TableNamePessoasJuridicas = "PESSOAS_JURIDICAS";

        protected readonly string TableAliasTotalizadores = "T";
        protected readonly string TableNameTotalizadores = "TOTALIZADORES";

        private bool _disposedValue = false;    //  To detect redundant calls.

        protected readonly string ParameterSymbol = "@";

        protected SqlConnection? _connection;

        protected SqlTransaction? _transaction;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository"/> class with the specified SQL connection and optional transaction.
        /// </summary>
        /// <param name="connection">The SQL connection to use.</param>
        /// <param name="transaction">An optional SQL transaction to use.</param>
        protected BaseRepository(SqlConnection connection, SqlTransaction? transaction)
        {
            this._connection = connection;
            this._transaction = transaction;
        }

        /// <summary>
        /// Retrieves a <see cref="DataTable"/> from the specified <see cref="IDbCommand"/>.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns>A <see cref="DataTable"/> containing the results of the command.</returns>
        protected DataTable GetDataTableFromDataAdapter(IDbCommand command)
        {
            if (command is null)
                throw new ArgumentNullException(
                    nameof(
                        command));

            using (var sqlCommand = this.CreateCommand(
                command.CommandText))
            {
                var dataTable = new DataTable();

                using (var adapter = new SqlDataAdapter(
                    sqlCommand))
                {
                    adapter.Fill(
                        dataTable);
                }

                return dataTable;
            }
        }

        /// <summary>
        /// Creates a SQL command with the specified command text, command type, and optional parameters.   
        /// </summary>
        /// <param name="cmdText">The SQL command text to execute.</param>
        /// <param name="commandType">The type of command. Defaults to <see cref="CommandType.Text"/>.</param>
        /// <param name="parameters">An optional array of SQL parameters to add to the command.</param>
        /// <returns>A configured <see cref="SqlCommand"/> with the specified settings.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="cmdText"/> is null, empty, or contains only whitespace.</exception>
        protected SqlCommand CreateCommand(string cmdText, CommandType commandType = CommandType.Text, SqlParameter[]? parameters = null)
        {
            if (string.IsNullOrWhiteSpace(cmdText))
                throw new ArgumentException("O comando SQL não pode ser nulo ou vazio.", nameof(cmdText));

            var command = new SqlCommand(
                cmdText,
                this._connection,
                this._transaction)
            {
                CommandTimeout = 0,
                CommandType = commandType,
            };

            if (parameters != null &&
                parameters.Any())
                foreach (var parameter in parameters)
                    command.Parameters.AddWithValue(
                        parameter.ParameterName,
                        parameter.Value);

            return command;
        }

        /// <summary>
        /// Creates a SQL parameter with the specified name and value.
        /// </summary>
        /// <param name="parameterName">The name of the SQL parameter.</param>
        /// <param name="value">The value of the SQL parameter.</param>
        /// <returns>A configured <see cref="SqlParameter"/> with the specified settings.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="parameterName"/> is null, empty, or contains only whitespace.</exception>
        protected SqlParameter CreateDataParameter(string parameterName, object value)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
                throw new ArgumentException("O nome do parâmetro não pode ser nulo ou vazio.", nameof(parameterName));

            return new SqlParameter(
                parameterName,
                value ?? DBNull.Value);
        }

        /// <summary>
        /// Loads data from the database using the specified SQL query and parameters.
        /// </summary>
        /// <typeparam name="T">The type of the objects to be returned.</typeparam>
        /// <typeparam name="U">The type of the parameters object.</typeparam>
        /// <param name="sql">The SQL query to execute.</param>
        /// <param name="parameters">The parameters to pass to the SQL query.</param>
        /// <returns>An enumerable collection of objects of type <typeparamref name="T"/>.</returns>
        protected IEnumerable<T> LoadData<T, U>(string sql, U parameters)
        {
            return this._connection.Query<T>(
                sql,
                parameters,
                transaction: this._transaction);
        }

        /// <summary>
        /// Retrieves a comma-separated list of all column names from the specified database table.
        /// </summary>
        /// <param name="tableName">The name of the database table.</param>
        /// <param name="alias">Optional table alias to prefix each column name.</param>
        /// <param name="fieldsToIgnore">Semicolon-separated list of column names to exclude from the result.</param>
        /// <returns>A comma-separated string containing all column names from the table, with optional alias prefixes and exclusions applied.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="tableName"/> is <see langword="null"/> or empty.</exception>
        protected string GetAllColumnsFromTable(string tableName, string alias = "", string fieldsToIgnore = "")
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            var sbColumns = new StringBuilder();

            string cmdText = $@"SELECT TOP 0 * FROM [dbo].[{tableName}] WHERE 0 = 1";

            using (var command = this.CreateCommand(cmdText))
            {
                using (var reader = command.ExecuteReader())
                {
                    using (var schemaTable = reader.GetSchemaTable())
                    {
                        foreach (DataRow column in schemaTable.Rows)
                        {
                            var columnName = column["ColumnName"].ToString();

                            if (!string.IsNullOrEmpty(
                                alias))
                                sbColumns.AppendFormat(
                                    CultureInfo.InvariantCulture,
                                    "{0}.{1}, ",
                                    alias,
                                    columnName);
                            else
                                sbColumns.AppendFormat(
                                    CultureInfo.InvariantCulture,
                                    "[{0}], ",
                                    columnName);
                        }
                    }
                }
            }

            string columns = sbColumns.ToString().TrimEnd(',', ' ');

            if (!string.IsNullOrEmpty(fieldsToIgnore))
                foreach (var field in from field in fieldsToIgnore.Split(';')
                                      where !string.IsNullOrEmpty(field)
                                      select field)
                {
                    columns = columns.Replace(field, string.Empty).Trim();
                }

            return columns;
        }

        /// <summary>
        /// Maps the properties of the specified entity type to their corresponding database columns based on the DescriptionAttribute.
        /// </summary>
        /// <param name="entityType">The type of the entity to map.</param>
        protected void MapAttributeToField(Type entityType)
        {
            var map = new CustomPropertyTypeMap(
                entityType,
                (type, columnName) => type.GetProperties().FirstOrDefault(
                    prop => this.GetDescriptionFromAttribute(prop) == columnName));

            SqlMapper.SetTypeMap(
                entityType,
                map);
        }

        /// <summary>
        /// Retrieves the description from the DescriptionAttribute of the specified member.
        /// </summary>
        /// <param name="member">The member to retrieve the description from.</param>
        /// <returns>The description from the DescriptionAttribute, or <c>null</c> if not found.</returns>
        protected string GetDescriptionFromAttribute(MemberInfo member)
        {
            if (member == null)
                return null;

            var descriptionAttribute = Attribute.GetCustomAttribute(member, typeof(DescriptionAttribute), false) as DescriptionAttribute;
            return descriptionAttribute?.Description;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    // Apenas limpa as referências locais.
                    // A conexão e a transação são de propriedade exclusiva do UnitOfWork.
                    this._connection = null;
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