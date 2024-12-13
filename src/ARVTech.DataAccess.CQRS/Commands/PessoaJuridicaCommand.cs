namespace ARVTech.DataAccess.CQRS.Commands
{
    using ARVTech.Shared;

    public class PessoaJuridicaCommand : BaseCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommandTextCreate()
        {
            return $@" INSERT INTO [dbo].[{Constants.TableNamePessoasJuridicas}]
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommandTextDelete()
        {
            return $@"    DECLARE @GuidPessoa AS UniqueIdentifier = ( SELECT TOP 1 GUIDPESSOA 
                                                                        FROM [dbo].[{Constants.TableNamePessoasJuridicas}]
                                                                       WHERE [GUID] = @Guid )

                           DELETE {Constants.TableAliasUsuarios}
                             FROM [dbo].[{Constants.TableNameUsuarios}] AS {Constants.TableAliasUsuarios}
                            WHERE {Constants.TableAliasUsuarios}.[GUIDEMPREGADOR] = @Guid

                           DELETE {Constants.TableAliasPessoasJuridicas}
                             FROM [dbo].[{Constants.TableNamePessoasJuridicas}] AS {Constants.TableAliasPessoasJuridicas}
                            WHERE {Constants.TableAliasPessoasJuridicas}.[GUID] = @Guid

                           DELETE {Constants.TableAliasPessoas}
                             FROM [dbo].[{Constants.TableNamePessoas}] AS {Constants.TableAliasPessoas}
                            WHERE {Constants.TableAliasPessoas}.[GUID] = @GuidPessoa ";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommandTextUpdate()
        {
            return $@" UPDATE [dbo].[{Constants.TableNamePessoasJuridicas}]
                          SET [CNPJ] = @Cnpj,
                              [DATA_FUNDACAO] = @DataFundacao,
                              [DATA_ULTIMA_ALTERACAO] = GETUTCDATE(),
                              [RAZAO_SOCIAL] = @RazaoSocial,
                              [IDUNIDADE_NEGOCIO] = @IdUnidadeNegocio
                        WHERE [GUID] = @Guid ";
        }
    }
}