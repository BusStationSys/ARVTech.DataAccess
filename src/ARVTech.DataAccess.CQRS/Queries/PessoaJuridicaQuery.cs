﻿namespace ARVTech.DataAccess.CQRS.Queries
{
    using System.Data.SqlClient;

    public class PessoaJuridicaQuery : BaseQuery
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        private readonly string _columnsPessoas;
        private readonly string _columnsPessoasJuridicas;
        private readonly string _columnsUnidadesNegocio;

        public override string CommandTextCreate()
        {
            return $@" INSERT INTO [dbo].[{base.TableNamePessoasJuridicas}]
                                   ([GUID],
                                    [GUIDPESSOA],
                                    [CNPJ],
                                    [DATA_INCLUSAO],
                                    [DATA_FUNDACAO],
                                    [RAZAO_SOCIAL],
                                    [IDUNIDADE_NEGOCIO])
                            VALUES (@Guid,
                                    @GuidPessoa,
                                    @Cnpj,
                                    GETUTCDATE(),
                                    @DataFundacao,
                                    @RazaoSocial,
                                    @IdUnidadeNegocio) ";
        }

        public override string CommandTextDelete()
        {
            return $@"    DECLARE @GuidPessoa AS UniqueIdentifier = ( SELECT TOP 1 GUIDPESSOA 
                                                                        FROM [dbo].[{base.TableNamePessoasJuridicas}]
                                                                       WHERE [GUID] = @Guid )

                           DELETE {base.TableAliasUsuarios}
                             FROM [dbo].[{base.TableNameUsuarios}] AS {base.TableAliasUsuarios}
                            WHERE {base.TableAliasUsuarios}.[GUIDEMPREGADOR] = @Guid

                           DELETE {base.TableAliasPessoasJuridicas}
                             FROM [dbo].[{base.TableNamePessoasJuridicas}] AS {base.TableAliasPessoasJuridicas}
                            WHERE {base.TableAliasPessoasJuridicas}.[GUID] = @Guid

                           DELETE {base.TableAliasPessoas}
                             FROM [dbo].[{base.TableNamePessoas}] AS {base.TableAliasPessoas}
                            WHERE {base.TableAliasPessoas}.[GUID] = @GuidPessoa ";
        }

        public override string CommandTextGetAll()
        {
            return $@"     SELECT {this._columnsPessoasJuridicas},
                                  {this._columnsPessoas},
                                  {this._columnsUnidadesNegocio}
                             FROM [dbo].[{base.TableNamePessoasJuridicas}] AS {base.TableAliasPessoasJuridicas} WITH(NOLOCK)
                       INNER JOIN [dbo].[{base.TableNamePessoas}] as {base.TableAliasPessoas} WITH(NOLOCK)
                               ON [{base.TableAliasPessoasJuridicas}].[GUIDPESSOA] = [{base.TableAliasPessoas}].[GUID]
                       INNER JOIN [dbo].[{base.TableNameUnidadesNegocio}] as {base.TableAliasUnidadesNegocio} WITH(NOLOCK)
                               ON [{base.TableAliasPessoasJuridicas}].[IDUNIDADE_NEGOCIO] = [{base.TableAliasUnidadesNegocio}].[ID] ";
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
                             FROM [dbo].[{base.TableNamePessoasJuridicas}] AS {base.TableAliasPessoasJuridicas} WITH(NOLOCK)
                       INNER JOIN [dbo].[{base.TableNamePessoas}] as {base.TableAliasPessoas} WITH(NOLOCK)
                               ON [{base.TableAliasPessoasJuridicas}].[GUIDPESSOA] = [{base.TableAliasPessoas}].[GUID]
                       INNER JOIN [dbo].[{base.TableNameUnidadesNegocio}] as {base.TableAliasUnidadesNegocio} WITH(NOLOCK)
                               ON [{base.TableAliasPessoasJuridicas}].[IDUNIDADE_NEGOCIO] = [{base.TableAliasUnidadesNegocio}].[ID]
                            WHERE [{base.TableAliasPessoasJuridicas}].[GUID] = @Guid ";
        }

        public override string CommandTextUpdate()
        {
            return $@" UPDATE [dbo].[{base.TableNamePessoasJuridicas}]
                          SET [CNPJ] = @Cnpj,
                              [DATA_FUNDACAO] = @DataFundacao,
                              [DATA_ULTIMA_ALTERACAO] = GETUTCDATE(),
                              [RAZAO_SOCIAL] = @RazaoSocial,
                              [IDUNIDADE_NEGOCIO] = @IdUnidadeNegocio
                        WHERE [GUID] = @Guid ";
        }

        public string CommandTextGetByCnpj()
        {
            return $@"     SELECT {this._columnsPessoasJuridicas},
                                  {this._columnsPessoas},
                                  {this._columnsUnidadesNegocio}
                             FROM [dbo].[{base.TableNamePessoasJuridicas}] AS {base.TableAliasPessoasJuridicas} WITH(NOLOCK)
                       INNER JOIN [dbo].[{base.TableNamePessoas}] as {base.TableAliasPessoas} WITH(NOLOCK)
                               ON [{base.TableAliasPessoasJuridicas}].[GUIDPESSOA] = [{base.TableAliasPessoas}].[GUID]
                       INNER JOIN [dbo].[{base.TableNameUnidadesNegocio}] as {base.TableAliasUnidadesNegocio} WITH(NOLOCK)
                               ON [{base.TableAliasPessoasJuridicas}].[IDUNIDADE_NEGOCIO] = [{base.TableAliasUnidadesNegocio}].[ID]
                            WHERE [{base.TableAliasPessoasJuridicas}].[CNPJ] = @Cnpj ";
        }

        public string CommandTextGetByRazaoSocial()
        {
            return $@"     SELECT {this._columnsPessoasJuridicas},
                                  {this._columnsPessoas}
                             FROM [dbo].[{base.TableNamePessoasJuridicas}] AS {base.TableAliasPessoasJuridicas} WITH(NOLOCK)
                       INNER JOIN [dbo].[{base.TableNamePessoas}] as {base.TableAliasPessoas} WITH(NOLOCK)
                               ON [{base.TableAliasPessoasJuridicas}].[GUIDPESSOA] = [{base.TableAliasPessoas}].[GUID]
                            WHERE [{base.TableAliasPessoasJuridicas}].[RAZAO_SOCIAL] = @RazaoSocial ";
        }

        public string CommandTextGetByRazaoSocialAndCnpj()
        {
            return $@"     SELECT {this._columnsPessoasJuridicas},
                                  {this._columnsPessoas}
                             FROM [dbo].[{base.TableNamePessoasJuridicas}] AS {base.TableAliasPessoasJuridicas} WITH(NOLOCK)
                       INNER JOIN [dbo].[{base.TableNamePessoas}] as {base.TableAliasPessoas} WITH(NOLOCK)
                               ON [{base.TableAliasPessoasJuridicas}].[GUIDPESSOA] = [{base.TableAliasPessoas}].[GUID]
                            WHERE [{base.TableAliasPessoasJuridicas}].[RAZAO_SOCIAL] = @RazaoSocial
                              AND [{base.TableAliasPessoasJuridicas}].[CNPJ] = @Cnpj ";
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
                base.TableNamePessoas,
                base.TableAliasPessoas);

            this._columnsPessoasJuridicas = base.GetAllColumnsFromTable(
                base.TableNamePessoasJuridicas,
                base.TableAliasPessoasJuridicas);

            this._columnsUnidadesNegocio = base.GetAllColumnsFromTable(
                base.TableNameUnidadesNegocio,
                base.TableAliasUnidadesNegocio);
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