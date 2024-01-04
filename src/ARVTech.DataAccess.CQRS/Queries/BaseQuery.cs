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

        private readonly string _tableAliasCalculos = "C";
        private readonly string _tableAliasEventos = "E";
        private readonly string _tableAliasMatriculas = "M";
        private readonly string _tableAliasMatriculasDemonstrativosPagamento = "MDP";
        private readonly string _tableAliasMatriculasDemonstrativosPagamentoEventos = "MDPE";
        private readonly string _tableAliasMatriculasDemonstrativosPagamentoTotalizadores = "MDPT";
        private readonly string _tableAliasMatriculasEspelhosPonto = "MEP";
        private readonly string _tableAliasMatriculasEspelhosPontoCalculos = "MEPC";
        private readonly string _tableAliasMatriculasEspelhosPontoMarcacoes = "MEPM";
        private readonly string _tableAliasPessoas = "P";
        private readonly string _tableAliasPessoasFisicas = "PF";
        private readonly string _tableAliasPessoasJuridicas = "PJ";
        private readonly string _tableAliasTotalizadores = "T";
        private readonly string _tableAliasUsuarios = "U";

        private readonly string _tableNameCalculos = "CALCULOS";
        private readonly string _tableNameEventos = "EVENTOS";
        private readonly string _tableNameMatriculas = "MATRICULAS";
        private readonly string _tableNameMatriculasDemonstrativosPagamento = "MATRICULAS_DEMONSTRATIVOS_PAGAMENTO";
        private readonly string _tableNameMatriculasDemonstrativosPagamentoEventos = "MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_EVENTOS";
        private readonly string _tableNameMatriculasDemonstrativosPagamentoTotalizadores = "MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES";
        private readonly string _tableNameMatriculasEspelhosPonto = "MATRICULAS_ESPELHOS_PONTO";
        private readonly string _tableNameMatriculasEspelhosPontoCalculos = "MATRICULAS_ESPELHOS_PONTO_CALCULOS";
        private readonly string _tableNameMatriculasEspelhosPontoMarcacoes = "MATRICULAS_ESPELHOS_PONTO_MARCACOES";
        private readonly string _tableNamePessoas = "PESSOAS";
        private readonly string _tableNamePessoasFisicas = "PESSOAS_FISICAS";
        private readonly string _tableNamePessoasJuridicas = "PESSOAS_JURIDICAS";
        private readonly string _tableNameTotalizadores = "TOTALIZADORES";
        private readonly string _tableNameUsuarios = "USUARIOS";

        protected string TableAliasCalculos
        {
            get
            {
                return this._tableAliasCalculos;
            }
        }

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

        protected string TableAliasMatriculasEspelhosPonto
        {
            get
            {
                return this._tableAliasMatriculasEspelhosPonto;
            }
        }

        protected string TableAliasMatriculasEspelhosPontoCalculos
        {
            get
            {
                return this._tableAliasMatriculasEspelhosPontoCalculos;
            }
        }

        protected string TableAliasMatriculasEspelhosPontoMarcacoes
        {
            get
            {
                return this._tableAliasMatriculasEspelhosPontoMarcacoes;
            }
        }

        protected string TableAliasPessoas
        {
            get
            {
                return this._tableAliasPessoas;
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

        protected string TableAliasUsuarios
        {
            get
            {
                return this._tableAliasUsuarios;
            }
        }

        protected string TableNameCalculos
        {
            get
            {
                return this._tableNameCalculos;
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

        protected string TableNameMatriculasEspelhosPonto
        {
            get
            {
                return this._tableNameMatriculasEspelhosPonto;
            }
        }

        protected string TableNameMatriculasEspelhosPontoCalculos
        {
            get
            {
                return this._tableNameMatriculasEspelhosPontoCalculos;
            }
        }

        protected string TableNameMatriculasEspelhosPontoMarcacoes
        {
            get
            {
                return this._tableNameMatriculasEspelhosPontoMarcacoes;
            }
        }

        protected string TableNamePessoas
        {
            get
            {
                return this._tableNamePessoas;
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

        protected string TableNameUsuarios
        {
            get
            {
                return this._tableNameUsuarios;
            }
        }

        public abstract string CommandTextCreate();

        public abstract string CommandTextDelete();

        public abstract string CommandTextGetAll();

        public abstract string CommandTextGetById();

        public abstract string CommandTextGetCustom(
            string where = "",
            string orderBy = "",
            uint? pageNumber = null,
            uint? pageSize = null);

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
                    nameof(
                        tableName),
                    "Nome da Tabela deve ser informado.");

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