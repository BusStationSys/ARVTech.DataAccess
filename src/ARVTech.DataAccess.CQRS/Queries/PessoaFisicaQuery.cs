namespace ARVTech.DataAccess.CQRS.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PessoaFisicaQuery : BaseQuery
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        private readonly string _columnsPessoas;
        private readonly string _columnsPessoasFisicas;

        public override string CommandTextCreate()
        {
            return $@" INSERT INTO [dbo].[{base.TableNamePessoasFisicas}]
                                         ([GUID],
                                          [GUIDPESSOA],
                                          [CPF],
                                          [RG],
                                          [DATA_NASCIMENTO],
                                          [DATA_INCLUSAO],
                                          [NOME],
                                          [NUMERO_CTPS],
                                          [SERIE_CTPS],
                                          [UF_CTPS])
                                  VALUES (@Guid,
                                          @GuidPessoa,
                                          @Cpf,
                                          @Rg,
                                          @DataNascimento,
                                          GETUTCDATE(),
                                          @Nome,
                                          @NumeroCtps,
                                          @SerieCtps,
                                          @UfCtps) ";
        }

        public override string CommandTextDelete()
        {
            return $@"    DECLARE @GuidPessoa AS UniqueIdentifier = ( SELECT TOP 1 GUIDPESSOA 
                                                                        FROM [dbo].[{base.TableNamePessoasFisicas}]
                                                                       WHERE [GUID] = @Guid )

                           DELETE {base.TableAliasUsuarios}
                             FROM [dbo].[{base.TableNameUsuarios}] AS {base.TableAliasUsuarios}
                            WHERE {base.TableAliasUsuarios}.[GUIDCOLABORADOR] = @Guid

                           DELETE {base.TableAliasPessoasFisicas}
                             FROM [dbo].[{base.TableNamePessoasFisicas}] AS {base.TableAliasPessoasFisicas}
                            WHERE {base.TableAliasPessoasFisicas}.[GUID] = @Guid

                           DELETE {base.TableAliasPessoas}
                             FROM [dbo].[{base.TableNamePessoas}] AS {base.TableAliasPessoas}
                            WHERE {base.TableAliasPessoas}.[GUID] = @GuidPessoa ";
        }

        public override string CommandTextGetAll()
        {
            return $@"     SELECT {this._columnsPessoasFisicas},
                                  {this._columnsPessoas}
                             FROM [dbo].[{base.TableNamePessoasFisicas}] AS {base.TableAliasPessoasFisicas} WITH(NOLOCK)
                       INNER JOIN [dbo].[{base.TableNamePessoas}] AS {base.TableAliasPessoas} WITH(NOLOCK)
                               ON [{base.TableAliasPessoasFisicas}].[GUIDPESSOA] = [{base.TableAliasPessoas}].[GUID] ";
        }

        public override string CommandTextGetById()
        {
            return $@"     SELECT {this._columnsPessoasFisicas},
                                  {this._columnsPessoas}
                             FROM [dbo].[{base.TableNamePessoasFisicas}] AS {base.TableAliasPessoasFisicas} WITH(NOLOCK)
                       INNER JOIN [dbo].[{base.TableNamePessoas}] AS {base.TableAliasPessoas} WITH(NOLOCK)
                               ON [{base.TableAliasPessoasFisicas}].[GUIDPESSOA] = [{base.TableAliasPessoas}].[GUID] 
                            WHERE [{base.TableAliasPessoasFisicas}].[GUID] = @Guid  ";
        }

        public override string CommandTextUpdate()
        {
            return $@"     UPDATE [dbo].[{base.TableNamePessoasFisicas}]
                              SET [CPF] = @Cpf,
                                  [RG] = @Rg,
                                  [DATA_NASCIMENTO] = @DataNascimento,
                                  [DATA_ULTIMA_ALTERACAO] = GETUTCDATE(),
                                  [NOME] = @Nome,
                                  [NUMERO_CTPS] = @NumeroCtps,
                                  [SERIE_CTPS] = @SerieCtps,
                                  [UF_CTPS] = @UfCtps
                            WHERE [GUID] = @Guid ";
        }

        public string CommandTextGetByCpf()
        {
            return $@"     SELECT {this._columnsPessoasFisicas},
                                  {this._columnsPessoas}
                             FROM [dbo].[{base.TableNamePessoasFisicas}] AS {base.TableAliasPessoasFisicas} WITH(NOLOCK)
                       INNER JOIN [dbo].[{base.TableNamePessoas}] AS {base.TableAliasPessoas} WITH(NOLOCK)
                               ON [{base.TableAliasPessoasFisicas}].[GUIDPESSOA] = [{base.TableAliasPessoas}].[GUID] 
                            WHERE [{base.TableAliasPessoasFisicas}].[CPF] = @Cpf ";
        }

        public string CommandTextGetByNome()
        {
            return $@"     SELECT {this._columnsPessoasFisicas},
                                  {this._columnsPessoas}
                             FROM [dbo].[{base.TableNamePessoasFisicas}] AS {base.TableAliasPessoasFisicas} WITH(NOLOCK)
                       INNER JOIN [dbo].[{base.TableNamePessoas}] AS {base.TableAliasPessoas} WITH(NOLOCK)
                               ON [{base.TableAliasPessoasFisicas}].[GUIDPESSOA] = [{base.TableAliasPessoas}].[GUID] 
                            WHERE [{base.TableAliasPessoasFisicas}].[NOME] = @Nome ";
        }

        public string CommandTextGetByNomeNumeroCtpsSerieCtpsAndUfCtps()
        {
            return $@"     SELECT {this._columnsPessoasFisicas},
                                  {this._columnsPessoas}
                             FROM [dbo].[{base.TableNamePessoasFisicas}] AS {base.TableAliasPessoasFisicas} WITH(NOLOCK)
                       INNER JOIN [dbo].[{base.TableNamePessoas}] AS {base.TableAliasPessoas} WITH(NOLOCK)
                               ON [{base.TableAliasPessoasFisicas}].[GUIDPESSOA] = [{base.TableAliasPessoas}].[GUID] 
                            WHERE [{base.TableAliasPessoasFisicas}].[NOME] = @Nome 
                              AND [{base.TableAliasPessoasFisicas}].[NUMERO_CTPS] = @NumeroCtps
                              AND [{base.TableAliasPessoasFisicas}].[SERIE_CTPS] = @SerieCtps
                              AND [{base.TableAliasPessoasFisicas}].[UF_CTPS] = @UfCtps ";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public PessoaFisicaQuery(SqlConnection connection, SqlTransaction? transaction = null) :
            base(connection, transaction)
        {
            this._columnsPessoas = base.GetAllColumnsFromTable(
                base.TableNamePessoas,
                base.TableAliasPessoas);

            this._columnsPessoasFisicas = base.GetAllColumnsFromTable(
                base.TableNamePessoasFisicas,
                base.TableAliasPessoasFisicas);
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