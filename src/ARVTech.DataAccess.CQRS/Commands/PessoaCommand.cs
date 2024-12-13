namespace ARVTech.DataAccess.CQRS.Commands
{
    using ARVTech.Shared;

    public class PessoaCommand : BaseCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommandTextCreate()
        {
            return $@"     DECLARE @NewGuidPessoa UniqueIdentifier
                               SET @NewGuidPessoa = NEWID()

                       INSERT INTO [dbo].[{Constants.TableNamePessoas}]
                                   ([GUID],
                                    [BAIRRO],
                                    [CEP],
                                    [CIDADE],
                                    [COMPLEMENTO],
                                    [DATA_INCLUSAO],
                                    [EMAIL],
                                    [ENDERECO],
                                    [NUMERO],
                                    [TELEFONE],
                                    [UF])
                            VALUES (@NewGuidPessoa,
                                    @Bairro,
                                    @Cep,
                                    @Cidade,
                                    @Complemento,
                                    GETUTCDATE(),
                                    @Email,
                                    @Endereco,
                                    @Numero,
                                    @Telefone,
                                    @Uf) 

                             SELECT @NewGuidPessoa ";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override string CommandTextDelete()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommandTextUpdate()
        {
            return $@"     UPDATE [dbo].[{Constants.TableNamePessoas}]
                              SET [BAIRRO] = @Bairro,
                                  [CEP] = @Cep,
                                  [CIDADE] = @Cidade,
                                  [COMPLEMENTO] = @Complemento,
                                  [DATA_ULTIMA_ALTERACAO] = GETUTCDATE(),
                                  [EMAIL] = @Email,
                                  [ENDERECO] = @Endereco,
                                  [NUMERO] = @Numero,
                                  [TELEFONE] = @Telefone,
                                  [UF] = @Uf
                            WHERE [GUID] = @Guid ";
        }
    }
}