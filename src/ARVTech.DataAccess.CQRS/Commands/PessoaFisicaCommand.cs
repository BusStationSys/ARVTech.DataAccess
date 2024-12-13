namespace ARVTech.DataAccess.CQRS.Commands
{
    using ARVTech.Shared;

    public class PessoaFisicaCommand : BaseCommand
    {
        public override string CommandTextCreate()
        {
            return $@" INSERT INTO [dbo].[{Constants.TableNamePessoasFisicas}]
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
                                                                        FROM [dbo].[{Constants.TableNamePessoasFisicas}]
                                                                       WHERE [GUID] = @Guid )

                           DELETE {Constants.TableAliasUsuarios}
                             FROM [dbo].[{Constants.TableNameUsuarios}] AS {Constants.TableAliasUsuarios}
                            WHERE {Constants.TableAliasUsuarios}.[GUIDCOLABORADOR] = @Guid

                           DELETE {Constants.TableAliasPessoasFisicas}
                             FROM [dbo].[{Constants.TableNamePessoasFisicas}] AS {Constants.TableAliasPessoasFisicas}
                            WHERE {Constants.TableAliasPessoasFisicas}.[GUID] = @Guid

                           DELETE {Constants.TableAliasPessoas}
                             FROM [dbo].[{Constants.TableNamePessoas}] AS {Constants.TableAliasPessoas}
                            WHERE {Constants.TableAliasPessoas}.[GUID] = @GuidPessoa ";
        }

        public override string CommandTextUpdate()
        {
            return $@"     UPDATE [dbo].[{Constants.TableNamePessoasFisicas}]
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
    }
}