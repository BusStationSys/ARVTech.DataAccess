namespace ARVTech.DataAccess.CQRS.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PublicacaoQuery : BaseQuery
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        private readonly string _columnsPublicacoes;

        private readonly string _commandTextTemplate;

        public override string CommandTextCreate()
        {
            return $@" INSERT INTO [dbo].[{base.TableNamePublicacoes}]
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
            return $@"     DELETE {base.TableAliasPublicacoes}
                             FROM [dbo].[{base.TableNamePublicacoes}] AS {base.TableAliasPublicacoes}
                            WHERE {base.TableAliasPublicacoes}.[ID] = @Id ";
        }

        public override string CommandTextGetAll()
        {
            return this._commandTextTemplate;
        }

        public override string CommandTextGetById()
        {
            return $@"     {this._commandTextTemplate} 
                            WHERE [{base.TableAliasPublicacoes}].[GUID] = @Guid  ";
        }

        public override string CommandTextGetCustom(string where = "", string orderBy = "", uint? pageNumber = null, uint? pageSize = null)
        {
            return base.RefreshPagination(
                this._commandTextTemplate,
                where,
                orderBy,
                pageNumber,
                pageSize);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public PublicacaoQuery(SqlConnection connection, SqlTransaction? transaction = null) :
            base(connection, transaction)
        {
            this._columnsPublicacoes = base.GetAllColumnsFromTable(
                base.TableNamePublicacoes,
                base.TableAliasPublicacoes);

            this._commandTextTemplate = $@"     SELECT {this._columnsPublicacoes}
                                                  FROM [dbo].[{base.TableNamePublicacoes}] AS {base.TableAliasPublicacoes} WITH(NOLOCK) ";
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