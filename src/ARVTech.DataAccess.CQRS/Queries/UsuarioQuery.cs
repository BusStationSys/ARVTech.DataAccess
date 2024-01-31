namespace ARVTech.DataAccess.CQRS.Queries
{
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
                base.TableNamePessoas,
                base.TableAliasPessoas);

            this._columnsPessoasFisicas = base.GetAllColumnsFromTable(
                base.TableNamePessoasFisicas,
                base.TableAliasPessoasFisicas);

            this._columnsUsuarios = base.GetAllColumnsFromTable(
                base.TableNameUsuarios,
                base.TableAliasUsuarios);
        }

        public override string CommandTextCreate()
        {
            return $@" DECLARE @NewGuidUsuario UniqueIdentifier
                           SET @NewGuidUsuario = NEWID()

                        INSERT INTO [dbo].[{base.TableNameUsuarios}]
                                    ([GUID],
                                     [GUIDCOLABORADOR],
                                     [IDPERFIL_USUARIO],
                                     [EMAIL],
                                     [USERNAME],
                                     [PASSWORD],
                                     [IDASPNETUSER],
                                     [DATA_PRIMEIRO_ACESSO],
                                     [DATA_INCLUSAO])
                             VALUES (@NewGuidUsuario,
                                     @GuidColaborador,
                                     @IdPerfilUsuario,
                                     @Email,
                                     @Username,
                                     @Password,
                                     @IdAspNetUser,
                                     @DataPrimeiroAcesso,
                                     GETUTCDATE())

                              SELECT @NewGuidUsuario ";
        }

        public override string CommandTextDelete()
        {
            return $@" DELETE
                         FROM [dbo].[{base.TableNameUsuarios}]
                        WHERE [GUID] = @Guid ";
        }

        public override string CommandTextGetAll()
        {
            return $@"          SELECT {this._columnsUsuarios},
                                       {this._columnsPessoasFisicas},
                                       {this._columnsPessoas}
                                  FROM [dbo].[{base.TableNameUsuarios}] AS {base.TableAliasUsuarios} WITH(NOLOCK)
                       LEFT OUTER JOIN [dbo].[{base.TableNamePessoasFisicas}] as {base.TableAliasPessoasFisicas} WITH(NOLOCK)
                                    ON [{base.TableAliasUsuarios}].[GUIDCOLABORADOR] = [{base.TableAliasPessoasFisicas}].[GUID]
                       LEFT OUTER JOIN [dbo].[{base.TableNamePessoas}] as [{base.TableAliasPessoas}] WITH(NOLOCK)
                                    ON [{base.TableAliasPessoasFisicas}].[GUIDPESSOA] = [{base.TableAliasPessoas}].[GUID] ";
        }

        public override string CommandTextGetById()
        {
            return $@"          SELECT {this._columnsUsuarios},
                                       {this._columnsPessoasFisicas},
                                       {this._columnsPessoas}
                                  FROM [dbo].[{base.TableNameUsuarios}] AS {base.TableAliasUsuarios} WITH(NOLOCK)
                       LEFT OUTER JOIN [dbo].[{base.TableNamePessoasFisicas}] as {base.TableAliasPessoasFisicas} WITH(NOLOCK)
                                    ON [{base.TableAliasUsuarios}].[GUIDCOLABORADOR] = [{base.TableAliasPessoasFisicas}].[GUID]
                       LEFT OUTER JOIN [dbo].[{base.TableNamePessoas}] as [{base.TableAliasPessoas}] WITH(NOLOCK)
                                    ON [{base.TableAliasPessoasFisicas}].[GUIDPESSOA] = [{base.TableAliasPessoas}].[GUID] 
                                 WHERE [{base.TableAliasUsuarios}].[GUID] = @Guid ";
        }

        public override string CommandTextUpdate()
        {
            return $@" UPDATE [dbo].[{base.TableNameUsuarios}]
                          SET [GUIDCOLABORADOR] = @GuidColaborador,
                              [IDPERFIL_USUARIO] = @IdPerfilUsuario,
                              [EMAIL] = @Email,
                              [USERNAME] = @Username,
                              [DATA_PRIMEIRO_ACESSO] = @DataPrimeiroAcesso,
                              [DATA_ULTIMA_ALTERACAO] = GETUTCDATE(),
                              [PASSWORD] = @Password
                        WHERE [GUID] = @Guid ";
        }

        public string CommandTextCheckPasswordValid()
        {
            return $@" SELECT TOP 1 Guid
                         FROM [dbo].[{base.TableNameUsuarios}]
                        WHERE [GUID] = @Guid
                          AND PASSWORD = @PasswordQuery COLLATE SQL_Latin1_General_CP1_CS_AS ";
        }

        public string CommandTextGetByUsername()
        {
            return $@"          SELECT {this._columnsUsuarios},
                                       {this._columnsPessoasFisicas},
                                       {this._columnsPessoas}
                                  FROM [dbo].[{base.TableNameUsuarios}] AS {base.TableAliasUsuarios} WITH(NOLOCK)
                       LEFT OUTER JOIN [dbo].[{base.TableNamePessoasFisicas}] as {base.TableAliasPessoasFisicas} WITH(NOLOCK)
                                    ON [{base.TableAliasUsuarios}].[GUIDCOLABORADOR] = [{base.TableAliasPessoasFisicas}].[GUID]
                       LEFT OUTER JOIN [dbo].[{base.TableNamePessoas}] as [{base.TableAliasPessoas}] WITH(NOLOCK)
                                    ON [{base.TableAliasPessoasFisicas}].[GUIDPESSOA] = [{base.TableAliasPessoas}].[GUID]
                                 WHERE ( LOWER([{base.TableAliasPessoasFisicas}].[CPF]) = @Filtro OR
                                         LOWER([{base.TableAliasPessoas}].[Email]) = @Filtro OR
                                         LOWER([{base.TableAliasUsuarios}].[UserName]) = @Filtro ) ";
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