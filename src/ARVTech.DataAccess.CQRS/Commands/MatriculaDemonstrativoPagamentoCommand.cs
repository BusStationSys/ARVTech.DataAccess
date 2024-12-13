namespace ARVTech.DataAccess.CQRS.Commands
{
    using ARVTech.Shared;

    public class MatriculaDemonstrativoPagamentoCommand : BaseCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommandTextCreate()
        {
            return $@"     DECLARE @NewGuidMatriculaDemonstrativoPagamento UniqueIdentifier
                               SET @NewGuidMatriculaDemonstrativoPagamento = NEWID()

                            INSERT INTO [dbo].[{Constants.TableNameMatriculasDemonstrativosPagamento}]
                                        ([GUID],
                                         [GUIDMATRICULA],
                                         [COMPETENCIA])
                                 VALUES (@NewGuidMatriculaDemonstrativoPagamento,
                                         @GuidMatricula,
                                         @Competencia)

                                 SELECT @NewGuidMatriculaDemonstrativoPagamento ";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommandTextDelete()
        {
            return $@" DELETE
                         FROM [dbo].[{Constants.TableNameMatriculasDemonstrativosPagamento}]
                        WHERE [GUID] = @Guid ";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommandTextUpdate()
        {
            return $@" UPDATE [dbo].[{Constants.TableNameMatriculasDemonstrativosPagamento}]
                          SET [GUIDMATRICULA] = @GuidMatricula,
                              [COMPETENCIA] = @Competencia,
                              [DATA_ULTIMA_ALTERACAO] = GETUTCDATE(),
                              [DATA_CONFIRMACAO] = @DataConfirmacao,
                              [IP_CONFIRMACAO] = @IpConfirmacao
                        WHERE [GUID] = @Guid ";
        }
    }
}