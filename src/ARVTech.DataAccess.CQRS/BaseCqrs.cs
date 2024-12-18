﻿namespace ARVTech.DataAccess.CQRS
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Text;
    using ARVTech.DataAccess.DbFactories;

    public abstract class BaseCqrs : IDisposable
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
        private readonly string _tableAliasPublicacoes = "PU";
        private readonly string _tableAliasPessoasJuridicas = "PJ";
        private readonly string _tableAliasTotalizadores = "T";
        private readonly string _tableAliasUnidadesNegocio = "UN";
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
        private readonly string _tableNamePublicacoes = "PUBLICACOES";
        private readonly string _tableNameTotalizadores = "TOTALIZADORES";
        private readonly string _tableNameUnidadesNegocio = "UNIDADES_NEGOCIO";
        private readonly string _tableNameUsuarios = "USUARIOS";

        protected string TableAliasCalculos
        {
            get
            {
                return _tableAliasCalculos;
            }
        }

        protected string TableAliasEventos
        {
            get
            {
                return _tableAliasEventos;
            }
        }

        protected string TableAliasMatriculas
        {
            get
            {
                return _tableAliasMatriculas;
            }
        }

        protected string TableAliasMatriculasDemonstrativosPagamento
        {
            get
            {
                return _tableAliasMatriculasDemonstrativosPagamento;
            }
        }

        protected string TableAliasMatriculasDemonstrativosPagamentoEventos
        {
            get
            {
                return _tableAliasMatriculasDemonstrativosPagamentoEventos;
            }
        }

        protected string TableAliasMatriculasDemonstrativosPagamentoTotalizadores
        {
            get
            {
                return _tableAliasMatriculasDemonstrativosPagamentoTotalizadores;
            }
        }

        protected string TableAliasMatriculasEspelhosPonto
        {
            get
            {
                return _tableAliasMatriculasEspelhosPonto;
            }
        }

        protected string TableAliasMatriculasEspelhosPontoCalculos
        {
            get
            {
                return _tableAliasMatriculasEspelhosPontoCalculos;
            }
        }

        protected string TableAliasMatriculasEspelhosPontoMarcacoes
        {
            get
            {
                return _tableAliasMatriculasEspelhosPontoMarcacoes;
            }
        }

        protected string TableAliasPessoas
        {
            get
            {
                return _tableAliasPessoas;
            }
        }

        protected string TableAliasPessoasFisicas
        {
            get
            {
                return _tableAliasPessoasFisicas;
            }
        }

        protected string TableAliasPessoasJuridicas
        {
            get
            {
                return _tableAliasPessoasJuridicas;
            }
        }

        protected string TableAliasPublicacoes
        {
            get
            {
                return _tableAliasPublicacoes;
            }
        }

        protected string TableAliasTotalizadores
        {
            get
            {
                return _tableAliasTotalizadores;
            }
        }

        protected string TableAliasUnidadesNegocio
        {
            get
            {
                return _tableAliasUnidadesNegocio;
            }
        }

        protected string TableAliasUsuarios
        {
            get
            {
                return _tableAliasUsuarios;
            }
        }

        protected string TableNameCalculos
        {
            get
            {
                return _tableNameCalculos;
            }
        }

        protected string TableNameEventos
        {
            get
            {
                return _tableNameEventos;
            }
        }

        protected string TableNameMatriculas
        {
            get
            {
                return _tableNameMatriculas;
            }
        }

        protected string TableNameMatriculasDemonstrativosPagamento
        {
            get
            {
                return _tableNameMatriculasDemonstrativosPagamento;
            }
        }

        protected string TableNameMatriculasDemonstrativosPagamentoEventos
        {
            get
            {
                return _tableNameMatriculasDemonstrativosPagamentoEventos;
            }
        }

        protected string TableNameMatriculasDemonstrativosPagamentoTotalizadores
        {
            get
            {
                return _tableNameMatriculasDemonstrativosPagamentoTotalizadores;
            }
        }

        protected string TableNameMatriculasEspelhosPonto
        {
            get
            {
                return _tableNameMatriculasEspelhosPonto;
            }
        }

        protected string TableNameMatriculasEspelhosPontoCalculos
        {
            get
            {
                return _tableNameMatriculasEspelhosPontoCalculos;
            }
        }

        protected string TableNameMatriculasEspelhosPontoMarcacoes
        {
            get
            {
                return _tableNameMatriculasEspelhosPontoMarcacoes;
            }
        }

        protected string TableNamePessoas
        {
            get
            {
                return _tableNamePessoas;
            }
        }

        protected string TableNamePessoasFisicas
        {
            get
            {
                return _tableNamePessoasFisicas;
            }
        }

        protected string TableNamePessoasJuridicas
        {
            get
            {
                return _tableNamePessoasJuridicas;
            }
        }

        protected string TableNamePublicacoes
        {
            get
            {
                return _tableNamePublicacoes;
            }
        }

        protected string TableNameTotalizadores
        {
            get
            {
                return _tableNameTotalizadores;
            }
        }

        protected string TableNameUnidadesNegocio
        {
            get
            {
                return _tableNameUnidadesNegocio;
            }
        }

        protected string TableNameUsuarios
        {
            get
            {
                return _tableNameUsuarios;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        protected BaseCqrs(SqlConnection connection, SqlTransaction? transaction = null)
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