namespace ARVTech.DataAccess.CQRS.Queries
{
    using ARVTech.Shared;
    using System.Data.SqlClient;

    public class UsuarioQuery : BaseQuery
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        private readonly string _columnsPessoas;
        private readonly string _columnsPessoasFisicas;
        private readonly string _columnsUsuarios;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsuarioQuery"/> class.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public UsuarioQuery(SqlConnection connection, SqlTransaction? transaction = null) :
            base(connection, transaction)
        {
            this._columnsPessoas = base.GetAllColumnsFromTable(
                Constants.TableNamePessoas,
                Constants.TableAliasPessoas);

            this._columnsPessoasFisicas = base.GetAllColumnsFromTable(
                Constants.TableNamePessoasFisicas,
                Constants.TableAliasPessoasFisicas);

            this._columnsUsuarios = base.GetAllColumnsFromTable(
                Constants.TableNameUsuarios,
                Constants.TableAliasUsuarios);
        }

        public override string CommandTextGetAll()
        {
            return $@"          SELECT {this._columnsUsuarios},
                                       {this._columnsPessoasFisicas},
                                       {this._columnsPessoas}
                                  FROM [dbo].[{Constants.TableNameUsuarios}] AS {Constants.TableAliasUsuarios} WITH(NOLOCK)
                       LEFT OUTER JOIN [dbo].[{Constants.TableNamePessoasFisicas}] as {Constants.TableAliasPessoasFisicas} WITH(NOLOCK)
                                    ON [{Constants.TableAliasUsuarios}].[GUIDCOLABORADOR] = [{Constants.TableAliasPessoasFisicas}].[GUID]
                       LEFT OUTER JOIN [dbo].[{Constants.TableNamePessoas}] as [{Constants.TableAliasPessoas}] WITH(NOLOCK)
                                    ON [{Constants.TableAliasPessoasFisicas}].[GUIDPESSOA] = [{Constants.TableAliasPessoas}].[GUID] ";
        }

        public override string CommandTextGetById()
        {
            return $@"          SELECT {this._columnsUsuarios},
                                       {this._columnsPessoasFisicas},
                                       {this._columnsPessoas}
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
            return $@"          SELECT {this._columnsUsuarios},
                                       {this._columnsPessoasFisicas},
                                       {this._columnsPessoas}
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
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    //  TODO: dispose managed state (managed objects).
                }

                this._disposedValue = true;
            }

            // Call base class implementation.
            base.Dispose(disposing);
        }

        public override string CommandTextGetCustom(string where = "", string orderBy = "", uint? pageNumber = null, uint? pageSize = null)
        {
            throw new NotImplementedException();
        }
    }
}