namespace ARVTech.DataAccess.CQRS.Queries
{
    using System.Data.SqlClient;

    public class PublicacaoQuery : BaseQuery
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        private readonly string _commandTextTemplate;

        public override string CommandTextCreate()
        {
            return $@" INSERT INTO [dbo].[{base.TableNamePublicacoes}]
                                         ([TITULO],
                                          [RESUMO],
                                          [TEXTO],
                                          [CONTEUDO_IMAGEM],
                                          [EXTENSAO_IMAGEM],
                                          [NOME_IMAGEM],
                                          [CONTEUDO_ARQUIVO],
                                          [EXTENSAO_ARQUIVO],
                                          [NOME_ARQUIVO],
                                          [DATA_APRESENTACAO],
                                          [DATA_INCLUSAO],
                                          [DATA_VALIDADE],
                                          [OCULTAR_PUBLICACAO])
                                  VALUES (@Titulo,
                                          @Resumo,
                                          @Texto,
                                          @ConteudoImagem,
                                          @ExtensaoImagem,
                                          @NomeImagem,
                                          @ConteudoArquivo,
                                          @ExtensaoArquivo,
                                          @NomeArquivo,
                                          @DataApresentacao,
                                          GETUTCDATE(),
                                          @DataValidade,
                                          @OcultarPublicacao)

                           SELECT IDENT_CURRENT('{base.TableNamePublicacoes}') ";
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
                            WHERE [{base.TableAliasPublicacoes}].[ID] = @Id  ";
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
            return $@"     UPDATE [dbo].[{base.TableNamePublicacoes}]
                              SET [TITULO] = @Titulo,
                                  [RESUMO] = @Resumo,
                                  [TEXTO] = @Texto,
                                  [CONTEUDO_IMAGEM] = @ConteudoImagem,
                                  [EXTENSAO_IMAGEM] = @ExtensaoImagem,
                                  [NOME_IMAGEM] = @NomeImagem,
                                  [CONTEUDO_ARQUIVO] = @ConteudoArquivo,
                                  [EXTENSAO_ARQUIVO] = @ExtensaoArquivo,
                                  [NOME_ARQUIVO] = @NomeArquivo,
                                  [DATA_APRESENTACAO] = @DataApresentacao,
                                  [DATA_ULTIMA_ALTERACAO] = GETUTCDATE(),
                                  [DATA_VALIDADE] = @DataValidade,
                                  [OCULTAR_PUBLICACAO] = @OcultarPublicacao
                            WHERE [ID] = @Id ";
        }

        public string CommandTextGetImageById()
        {
            return $@" SELECT [{base.TableAliasPublicacoes}].[ID],
                              [{base.TableAliasPublicacoes}].[CONTEUDO_IMAGEM],
                              [{base.TableAliasPublicacoes}].[EXTENSAO_IMAGEM],
                              [{base.TableAliasPublicacoes}].[NOME_IMAGEM]
                         FROM [{base.TableNamePublicacoes}] AS {base.TableAliasPublicacoes}
                        WHERE [{base.TableAliasPublicacoes}].[ID] = @Id  ";
        }

        public string CommandTextSobreNos()
        {
            return $@"     {this._commandTextTemplate}
                            WHERE [{base.TableAliasPublicacoes}].[OCULTAR_PUBLICACAO] = 0
                              AND @DataAtual >= [{base.TableAliasPublicacoes}].[DATA_APRESENTACAO]
                              AND ( @DataAtual <= [{base.TableAliasPublicacoes}].[DATA_VALIDADE] OR
                                    [{base.TableAliasPublicacoes}].[DATA_VALIDADE] IS NULL ) ";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public PublicacaoQuery(SqlConnection connection, SqlTransaction? transaction = null) :
            base(connection, transaction)
        {
            string columnsPublicacoes = base.GetAllColumnsFromTable(
                base.TableNamePublicacoes,
                base.TableAliasPublicacoes,
                $"{base.TableAliasPublicacoes}.[CONTEUDO_ARQUIVO];{base.TableAliasPublicacoes}.[CONTEUDO_IMAGEM]");

            this._commandTextTemplate = $@"     SELECT {columnsPublicacoes}
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