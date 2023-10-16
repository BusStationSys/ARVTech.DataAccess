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

        private readonly string _tableAliasEventos = "E";
        private readonly string _tableAliasMatriculas = "M";
        private readonly string _tableAliasMatriculasDemonstrativosPagamento = "MDP";
        private readonly string _tableAliasMatriculasDemonstrativosPagamentoEventos = "MDPE";
        private readonly string _tableAliasMatriculasDemonstrativosPagamentoTotalizadores = "MDPT";
        private readonly string _tableAliasPessoasFisicas = "PF";
        private readonly string _tableAliasPessoasJuridicas = "PJ";
        private readonly string _tableAliasTotalizadores = "T";

        private readonly string _tableNameEventos = "EVENTOS";
        private readonly string _tableNameMatriculas = "MATRICULAS";
        private readonly string _tableNameMatriculasDemonstrativosPagamento = "MATRICULAS_DEMONSTRATIVOS_PAGAMENTO";
        private readonly string _tableNameMatriculasDemonstrativosPagamentoEventos = "MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_EVENTOS";
        private readonly string _tableNameMatriculasDemonstrativosPagamentoTotalizadores = "MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES";
        private readonly string _tableNamePessoasFisicas = "PESSOAS_FISICAS";
        private readonly string _tableNamePessoasJuridicas = "PESSOAS_JURIDICAS";
        private readonly string _tableNameTotalizadores = "TOTALIZADORES";

        protected string TableAliasEventos
        {
            get
            {
                return this._tableAliasEventos;
            }
        }

        protected string TableAliasMatriculas
        {
            get
            {
                return this._tableAliasMatriculas;
            }
        }

        protected string TableAliasMatriculasDemonstrativosPagamento
        {
            get
            {
                return this._tableAliasMatriculasDemonstrativosPagamento;
            }
        }

        protected string TableAliasMatriculasDemonstrativosPagamentoEventos
        {
            get
            {
                return this._tableAliasMatriculasDemonstrativosPagamentoEventos;
            }
        }

        protected string TableAliasMatriculasDemonstrativosPagamentoTotalizadores
        {
            get
            {
                return this._tableAliasMatriculasDemonstrativosPagamentoTotalizadores;
            }
        }

        protected string TableAliasPessoasFisicas
        {
            get
            {
                return this._tableAliasPessoasFisicas;
            }
        }

        protected string TableAliasPessoasJuridicas
        {
            get
            {
                return this._tableAliasPessoasJuridicas;
            }
        }

        protected string TableAliasTotalizadores
        {
            get
            {
                return this._tableAliasTotalizadores;
            }
        }

        protected string TableNameEventos
        {
            get
            {
                return this._tableNameEventos;
            }
        }

        protected string TableNameMatriculas
        {
            get
            {
                return this._tableNameMatriculas;
            }
        }

        protected string TableNameMatriculasDemonstrativosPagamento
        {
            get
            {
                return this._tableNameMatriculasDemonstrativosPagamento;
            }
        }

        protected string TableNameMatriculasDemonstrativosPagamentoEventos
        {
            get
            {
                return this._tableNameMatriculasDemonstrativosPagamentoEventos;
            }
        }

        protected string TableNameMatriculasDemonstrativosPagamentoTotalizadores
        {
            get
            {
                return this._tableNameMatriculasDemonstrativosPagamentoTotalizadores;
            }
        }

        protected string TableNamePessoasFisicas
        {
            get
            {
                return this._tableNamePessoasFisicas;
            }
        }

        protected string TableNamePessoasJuridicas
        {
            get
            {
                return this._tableNamePessoasJuridicas;
            }
        }

        protected string TableNameTotalizadores
        {
            get
            {
                return this._tableNameTotalizadores;
            }
        }

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