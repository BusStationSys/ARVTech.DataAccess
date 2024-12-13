namespace ARVTech.DataAccess.CQRS.Queries
{
    using System.Data.SqlClient;
    using ARVTech.Shared;

    public class PessoaJuridicaQuery : BaseQuery
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        private readonly string _columnsPessoas;
        private readonly string _columnsPessoasJuridicas;
        private readonly string _columnsUnidadesNegocio;

        public override string CommandTextGetAll()
        {
            return $@"     SELECT {this._columnsPessoasJuridicas},
                                  {this._columnsPessoas},
                                  {this._columnsUnidadesNegocio}
                             FROM [dbo].[{Constants.TableNamePessoasJuridicas}] AS {Constants.TableAliasPessoasJuridicas} WITH(NOLOCK)
                       INNER JOIN [dbo].[{Constants.TableNamePessoas}] as {Constants.TableAliasPessoas} WITH(NOLOCK)
                               ON [{Constants.TableAliasPessoasJuridicas}].[GUIDPESSOA] = [{Constants.TableAliasPessoas}].[GUID]
                       INNER JOIN [dbo].[{Constants.TableNameUnidadesNegocio}] as {Constants.TableAliasUnidadesNegocio} WITH(NOLOCK)
                               ON [{Constants.TableAliasPessoasJuridicas}].[IDUNIDADE_NEGOCIO] = [{Constants.TableAliasUnidadesNegocio}].[ID] ";
        }

        public override string CommandTextGetCustom(string where = "", string orderBy = "", uint? pageNumber = null, uint? pageSize = null)
        {
            throw new NotImplementedException();
        }

        public override string CommandTextGetById()
        {
            return $@"     SELECT {this._columnsPessoasJuridicas},
                                  {this._columnsPessoas},
                                  {this._columnsUnidadesNegocio}
                             FROM [dbo].[{Constants.TableNamePessoasJuridicas}] AS {Constants.TableAliasPessoasJuridicas} WITH(NOLOCK)
                       INNER JOIN [dbo].[{Constants.TableNamePessoas}] as {Constants.TableAliasPessoas} WITH(NOLOCK)
                               ON [{Constants.TableAliasPessoasJuridicas}].[GUIDPESSOA] = [{Constants.TableAliasPessoas}].[GUID]
                       INNER JOIN [dbo].[{Constants.TableNameUnidadesNegocio}] as {Constants.TableAliasUnidadesNegocio} WITH(NOLOCK)
                               ON [{Constants.TableAliasPessoasJuridicas}].[IDUNIDADE_NEGOCIO] = [{Constants.TableAliasUnidadesNegocio}].[ID]
                            WHERE [{Constants.TableAliasPessoasJuridicas}].[GUID] = @Guid ";
        }

        public string CommandTextGetByCnpj()
        {
            return $@"     SELECT {this._columnsPessoasJuridicas},
                                  {this._columnsPessoas},
                                  {this._columnsUnidadesNegocio}
                             FROM [dbo].[{Constants.TableNamePessoasJuridicas}] AS {Constants.TableAliasPessoasJuridicas} WITH(NOLOCK)
                       INNER JOIN [dbo].[{Constants.TableNamePessoas}] as {Constants.TableAliasPessoas} WITH(NOLOCK)
                               ON [{Constants.TableAliasPessoasJuridicas}].[GUIDPESSOA] = [{Constants.TableAliasPessoas}].[GUID]
                       INNER JOIN [dbo].[{Constants.TableNameUnidadesNegocio}] as {Constants.TableAliasUnidadesNegocio} WITH(NOLOCK)
                               ON [{Constants.TableAliasPessoasJuridicas}].[IDUNIDADE_NEGOCIO] = [{Constants.TableAliasUnidadesNegocio}].[ID]
                            WHERE [{Constants.TableAliasPessoasJuridicas}].[CNPJ] = @Cnpj ";
        }

        public string CommandTextGetByRazaoSocial()
        {
            return $@"     SELECT {this._columnsPessoasJuridicas},
                                  {this._columnsPessoas}
                             FROM [dbo].[{Constants.TableNamePessoasJuridicas}] AS {Constants.TableAliasPessoasJuridicas} WITH(NOLOCK)
                       INNER JOIN [dbo].[{Constants.TableNamePessoas}] as {Constants.TableAliasPessoas} WITH(NOLOCK)
                               ON [{Constants.TableAliasPessoasJuridicas}].[GUIDPESSOA] = [{Constants.TableAliasPessoas}].[GUID]
                            WHERE [{Constants.TableAliasPessoasJuridicas}].[RAZAO_SOCIAL] = @RazaoSocial ";
        }

        public string CommandTextGetByRazaoSocialAndCnpj()
        {
            return $@"     SELECT {this._columnsPessoasJuridicas},
                                  {this._columnsPessoas}
                             FROM [dbo].[{Constants.TableNamePessoasJuridicas}] AS {Constants.TableAliasPessoasJuridicas} WITH(NOLOCK)
                       INNER JOIN [dbo].[{Constants.TableNamePessoas}] as {Constants.TableAliasPessoas} WITH(NOLOCK)
                               ON [{Constants.TableAliasPessoasJuridicas}].[GUIDPESSOA] = [{Constants.TableAliasPessoas}].[GUID]
                            WHERE [{Constants.TableAliasPessoasJuridicas}].[RAZAO_SOCIAL] = @RazaoSocial
                              AND [{Constants.TableAliasPessoasJuridicas}].[CNPJ] = @Cnpj ";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public PessoaJuridicaQuery(SqlConnection connection, SqlTransaction? transaction = null) :
        base(connection, transaction)
        {
            this._columnsPessoas = base.GetAllColumnsFromTable(
                Constants.TableNamePessoas,
                Constants.TableAliasPessoas);

            this._columnsPessoasJuridicas = base.GetAllColumnsFromTable(
                Constants.TableNamePessoasJuridicas,
                Constants.TableAliasPessoasJuridicas);

            this._columnsUnidadesNegocio = base.GetAllColumnsFromTable(
                Constants.TableNameUnidadesNegocio,
                Constants.TableAliasUnidadesNegocio);
        }

        // Protected implementation of Dispose pattern. https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
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
    }
}