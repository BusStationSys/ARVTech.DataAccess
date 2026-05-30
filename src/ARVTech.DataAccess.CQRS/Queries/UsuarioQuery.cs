namespace ARVTech.DataAccess.CQRS.Queries
{
    using ARVTech.Shared;
    using Microsoft.Data.SqlClient;

    public class UsuarioQuery : BaseQuery
    {
        private string? _columnsPessoas;

        private string? _columnsPessoasFisicas;

        private string? _columnsUsuarios;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsuarioQuery"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public UsuarioQuery(SqlConnection connection, SqlTransaction? transaction = null) :
            base(connection, transaction)
        { }

        public override string CommandTextGetAll()
        {
            return $@"          SELECT {this.ColumnsUsuarios},
                                       {this.ColumnsPessoasFisicas},
                                       {this.ColumnsPessoas}
                                  FROM [dbo].[{Constants.TableNameUsuarios}] AS {Constants.TableAliasUsuarios} WITH(NOLOCK)
                       LEFT OUTER JOIN [dbo].[{Constants.TableNamePessoasFisicas}] as {Constants.TableAliasPessoasFisicas} WITH(NOLOCK)
                                    ON [{Constants.TableAliasUsuarios}].[GUIDCOLABORADOR] = [{Constants.TableAliasPessoasFisicas}].[GUID]
                       LEFT OUTER JOIN [dbo].[{Constants.TableNamePessoas}] as [{Constants.TableAliasPessoas}] WITH(NOLOCK)
                                    ON [{Constants.TableAliasPessoasFisicas}].[GUIDPESSOA] = [{Constants.TableAliasPessoas}].[GUID] ";
        }

        public override string CommandTextGetById()
        {
            return $@"          SELECT {this.ColumnsUsuarios},
                                       {this.ColumnsPessoasFisicas},
                                       {this.ColumnsPessoas}
                                  FROM [dbo].[{Constants.TableNameUsuarios}] AS {Constants.TableAliasUsuarios} WITH(NOLOCK)
                       LEFT OUTER JOIN [dbo].[{Constants.TableNamePessoasFisicas}] as {Constants.TableAliasPessoasFisicas} WITH(NOLOCK)
                                    ON [{Constants.TableAliasUsuarios}].[GUIDCOLABORADOR] = [{Constants.TableAliasPessoasFisicas}].[GUID]
                       LEFT OUTER JOIN [dbo].[{Constants.TableNamePessoas}] as [{Constants.TableAliasPessoas}] WITH(NOLOCK)
                                    ON [{Constants.TableAliasPessoasFisicas}].[GUIDPESSOA] = [{Constants.TableAliasPessoas}].[GUID] 
                                 WHERE [{Constants.TableAliasUsuarios}].[GUID] = @Guid ";
        }

        public string CommandTextCheckPasswordValid()
        {
            return $@" SELECT TOP 1 Guid
                         FROM [dbo].[{Constants.TableNameUsuarios}]
                        WHERE [GUID] = @Guid
                          AND PASSWORD = @PasswordQuery COLLATE SQL_Latin1_General_CP1_CS_AS ";
        }

        public string CommandTextGetByUsername()
        {
            return $@"          SELECT {this.ColumnsUsuarios},
                                       {this.ColumnsPessoasFisicas},
                                       {this.ColumnsPessoas}
                                  FROM [dbo].[{Constants.TableNameUsuarios}] AS {Constants.TableAliasUsuarios} WITH(NOLOCK)
                       LEFT OUTER JOIN [dbo].[{Constants.TableNamePessoasFisicas}] as {Constants.TableAliasPessoasFisicas} WITH(NOLOCK)
                                    ON [{Constants.TableAliasUsuarios}].[GUIDCOLABORADOR] = [{Constants.TableAliasPessoasFisicas}].[GUID]
                       LEFT OUTER JOIN [dbo].[{Constants.TableNamePessoas}] as [{Constants.TableAliasPessoas}] WITH(NOLOCK)
                                    ON [{Constants.TableAliasPessoasFisicas}].[GUIDPESSOA] = [{Constants.TableAliasPessoas}].[GUID]
                                 WHERE ( LOWER([{Constants.TableAliasPessoasFisicas}].[CPF]) = @Filtro OR
                                         LOWER([{Constants.TableAliasPessoas}].[Email]) = @Filtro OR
                                         LOWER([{Constants.TableAliasUsuarios}].[UserName]) = @Filtro ) ";
        }

        protected override void Dispose(bool disposing)
        {
            // Call base class implementation.
            base.Dispose(disposing);
        }

        public override string CommandTextGetCustom(string where = "", string orderBy = "", uint? pageNumber = null, uint? pageSize = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all column names from the "Pessoas" table with alias applied.
        /// </summary>
        private string ColumnsPessoas
        {
            get
            {
                if (this._columnsPessoas is null)
                    this._columnsPessoas = base.GetAllColumnsFromTable(
                        Constants.TableNamePessoas,
                        Constants.TableAliasPessoas);

                return this._columnsPessoas;
            }
        }

        /// <summary>
        /// Gets all column names from the "Pessoas Físicas" table with alias applied.
        /// </summary>
        private string ColumnsPessoasFisicas
        {
            get
            {
                if (this._columnsPessoasFisicas is null)
                    this._columnsPessoasFisicas = base.GetAllColumnsFromTable(
                        Constants.TableNamePessoasFisicas,
                        Constants.TableAliasPessoasFisicas);

                return this._columnsPessoasFisicas;
            }
        }

        /// <summary>
        /// Gets all column names from the "Usuários" table with alias applied.
        /// </summary>
        private string ColumnsUsuarios
        {
            get
            {
                if (this._columnsUsuarios is null)
                    this._columnsUsuarios = base.GetAllColumnsFromTable(
                        Constants.TableNameUsuarios,
                        Constants.TableAliasUsuarios);

                return this._columnsUsuarios;
            }
        }
    }
}