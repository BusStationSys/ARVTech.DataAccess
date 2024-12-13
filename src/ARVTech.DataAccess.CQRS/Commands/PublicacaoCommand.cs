namespace ARVTech.DataAccess.CQRS.Commands
{
    using ARVTech.Shared;

    public class PublicacaoCommand : BaseCommand
    {
        public override string CommandTextCreate()
        {
            return $@" INSERT INTO [dbo].[{Constants.TableNamePublicacoes}]
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

                           SELECT IDENT_CURRENT('{Constants.TableNamePublicacoes}') ";
        }

        public override string CommandTextDelete()
        {
            return $@"     DELETE {Constants.TableAliasPublicacoes}
                             FROM [dbo].[{Constants.TableNamePublicacoes}] AS {Constants.TableAliasPublicacoes}
                            WHERE {Constants.TableAliasPublicacoes}.[ID] = @Id ";
        }

        public override string CommandTextUpdate()
        {
            return $@"     UPDATE [dbo].[{Constants.TableNamePublicacoes}]
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
    }
}