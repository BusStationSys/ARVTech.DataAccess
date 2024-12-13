namespace ARVTech.DataAccess.CQRS.Commands
{
    using ARVTech.Shared;

    public class MatriculaEspelhoPontoMarcacaoCommand : BaseCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommandTextCreate()
        {
            return $@"     DECLARE @NewGuidMepm UniqueIdentifier
                                            SET @NewGuidMepm = NEWID()

                                    INSERT INTO [dbo].[{Constants.TableNameMatriculasEspelhosPontoMarcacoes}]
                                                ([GUID],
                                                 [GUIDMATRICULA_ESPELHO_PONTO],
                                                 [DATA],
                                                 [MARCACAO],
                                                 [HORAS_EXTRAS_050],
                                                 [HORAS_EXTRAS_070],
                                                 [HORAS_EXTRAS_100],
                                                 [HORAS_CREDITO_BH],
                                                 [HORAS_DEBITO_BH],
                                                 [HORAS_FALTAS],
                                                 [HORAS_TRABALHADAS])
                                         VALUES (@NewGuidMepm,
                                                 @GuidMatriculaEspelhoPonto,
                                                 @Data,
                                                 @Marcacao,
                                                 @HorasExtras050,
                                                 @HorasExtras070,
                                                 @HorasExtras100,
                                                 @HorasCreditoBH,
                                                 @HorasDebitoBH,
                                                 @HorasFaltas,
                                                 @HorasTrabalhadas)

                                          SELECT @NewGuidMepm ";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommandTextDelete()
        {
            return $@" DELETE
                         FROM [dbo].[{Constants.TableNameMatriculasEspelhosPontoMarcacoes}]
                        WHERE [GUID] = @Guid ";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override string CommandTextUpdate()
        {
            throw new NotImplementedException();
        }
    }
}
