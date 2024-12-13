namespace ARVTech.DataAccess.CQRS.Queries
{
    using System.Data.SqlClient;
    using ARVTech.Shared;

    public class PublicacaoQuery : BaseQuery
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        private readonly string _commandTextTemplate;

        public override string CommandTextGetAll()
        {
            return this._commandTextTemplate;
        }

        public override string CommandTextGetById()
        {
            return $@"     {this._commandTextTemplate} 
                            WHERE [{Constants.TableAliasPublicacoes}].[ID] = @Id  ";
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

        public string CommandTextGetImageById()
        {
            return $@" SELECT [{Constants.TableAliasPublicacoes}].[ID],
                              [{Constants.TableAliasPublicacoes}].[CONTEUDO_IMAGEM],
                              [{Constants.TableAliasPublicacoes}].[EXTENSAO_IMAGEM],
                              [{Constants.TableAliasPublicacoes}].[NOME_IMAGEM]
                         FROM [{Constants.TableNamePublicacoes}] AS {Constants.TableAliasPublicacoes}
                        WHERE [{Constants.TableAliasPublicacoes}].[ID] = @Id  ";
        }

        public string CommandTextSobreNos()
        {
            return $@"     {this._commandTextTemplate}
                            WHERE [{Constants.TableAliasPublicacoes}].[OCULTAR_PUBLICACAO] = 0
                              AND @DataAtual >= [{Constants.TableAliasPublicacoes}].[DATA_APRESENTACAO]
                              AND ( @DataAtual <= [{Constants.TableAliasPublicacoes}].[DATA_VALIDADE] OR
                                    [{Constants.TableAliasPublicacoes}].[DATA_VALIDADE] IS NULL ) ";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public PublicacaoQuery(SqlConnection connection, SqlTransaction? transaction = null) :
            base(connection, transaction)
        {
            //  string columnsPublicacoes = base.GetAllColumnsFromTable(
            //      Constants.TableNamePublicacoes,
            //      Constants.TableAliasPublicacoes,
            //      $"{Constants.TableAliasPublicacoes}.[CONTEUDO_ARQUIVO];{Constants.TableAliasPublicacoes}.[CONTEUDO_IMAGEM]");

            string columnsPublicacoes = base.GetAllColumnsFromTable(
                Constants.TableNamePublicacoes,
                Constants.TableAliasPublicacoes);

            this._commandTextTemplate = $@"     SELECT {columnsPublicacoes}
                                                  FROM [dbo].[{Constants.TableNamePublicacoes}] AS {Constants.TableAliasPublicacoes} WITH(NOLOCK) ";
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