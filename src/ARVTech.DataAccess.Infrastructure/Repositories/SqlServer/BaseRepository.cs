namespace ARVTech.DataAccess.Infrastructure.Repositories.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Dapper;

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

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="connection"></param>
        //protected BaseRepository(SqlConnection connection)
        //{
        //    this._connection = connection;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        protected BaseRepository(SqlConnection connection, SqlTransaction? transaction)
        {
            this._connection = connection;
            this._transaction = transaction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        protected DataTable GetDataTableFromDataAdapter(IDbCommand command)
        {
            try
            {
                if (command is null)
                    throw new ArgumentNullException(
                        nameof(
                            command));

                using (var sqlCommand = new SqlCommand(
                    command.CommandText.ToString(),
                    this._connection))
                {
                    var dataTable = new DataTable();

                    using (var adapter = new SqlDataAdapter(sqlCommand))
                    {
                        adapter.Fill(dataTable);
                    }

                    return dataTable;
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
        protected SqlCommand CreateCommand(string cmdText, CommandType commandType = CommandType.Text, SqlParameter[]? parameters = null)
        {
            try
            {
                //using (var command = new SqlCommand(
                //    cmdText,
                //    this._connection,
                //    this._transaction))
                //{
                //    command.CommandTimeout = 0;
                //    command.CommandType = commandType;

                //    if (parameters != null &&
                //        parameters.Any())
                //        foreach (var parameter in parameters)
                //            command.Parameters.AddWithValue(
                //                parameter.ParameterName,
                //                parameter.Value);

                //    return command;
                //}

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
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected SqlParameter CreateDataParameter(string parameterName, object value)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
                throw new ArgumentException("O nome do parâmetro não pode ser nulo ou vazio.", nameof(parameterName));

            return new SqlParameter(
                parameterName,
                value ?? DBNull.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected IEnumerable<T> LoadData<T, U>(string sql, U parameters)
        {
            return this._connection.Query<T>(
                sql,
                parameters);
        }

        /// <summary>
        /// Recupera uma lista de colunas de uma tabela do banco de dados, com a opção de adicionar um alias para as colunas e ignorar colunas específicas.
        /// </summary>
        /// <param name="tableName">O nome da tabela da qual as colunas serão recuperadas.</param>
        /// <param name="alias">Um alias opcional para as colunas. Caso fornecido, o alias será prefixado aos nomes das colunas.</param>
        /// <param name="fieldsToIgnore">Uma lista de colunas a serem ignoradas, separadas por ponto e vírgula.</param>
        /// <returns>Uma string contendo os nomes das colunas da tabela, formatados de acordo com os parâmetros fornecidos.</returns>
        /// <exception cref="ArgumentNullException">Lançado se o nome da tabela for nulo ou vazio.</exception>
        protected string GetAllColumnsFromTable(string tableName, string alias = "", string fieldsToIgnore = "")
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            var sbColumns = new StringBuilder();

            string cmdText = $@"SELECT TOP 0 * FROM [dbo].[{tableName}] WHERE 0 = 1";

            using (var reader = this.CreateCommand(
                cmdText).ExecuteReader())
            {
                reader.Read();

                using (var schemaTable = reader.GetSchemaTable())
                {
                    foreach (DataRow column in schemaTable.Rows)
                    {
                        var columnName = column["ColumnName"].ToString();

                        if (!string.IsNullOrEmpty(alias))
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
        /// 
        /// </summary>
        /// <param name="entityType"></param>
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
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        private string GetDescriptionFromAttribute(MemberInfo member)
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
                    // TODO: dispose managed state (managed objects)

                    if (this._connection != null &&
                        this._connection.State == ConnectionState.Open)
                    {
                        this._connection.Dispose();
                        this._connection = null;
                    }

                    if (this._transaction != null)
                    {
                        this._transaction.Dispose();
                        this._transaction = null;
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
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