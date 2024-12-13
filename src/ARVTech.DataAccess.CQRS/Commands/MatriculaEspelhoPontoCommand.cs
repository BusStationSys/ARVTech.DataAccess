namespace ARVTech.DataAccess.CQRS.Commands
{
    using ARVTech.Shared;

    public class MatriculaEspelhoPontoCommand : BaseCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommandTextCreate()
        {
            return $@"    DECLARE @NewGuidMatriculaEspelhoPonto UniqueIdentifier
                             SET @NewGuidMatriculaEspelhoPonto = NEWID()

                     INSERT INTO [dbo].[{Constants.TableNameMatriculasEspelhosPonto}]
                                 ([GUID],
                                  [GUIDMATRICULA],
                                  [COMPETENCIA])
                          VALUES ( @NewGuidMatriculaEspelhoPonto,
                                   @GuidMatricula,
                                   @Competencia )
                          
                           SELECT @NewGuidMatriculaEspelhoPonto ";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommandTextDelete()
        {
            return $@" DELETE
                         FROM [dbo].[{Constants.TableNameMatriculasEspelhosPonto}]
                        WHERE [GUID] = @Guid ";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommandTextUpdate()
        {
            return $@" UPDATE [dbo].[{Constants.TableNameMatriculasEspelhosPonto}]
                          SET [GUIDMATRICULA] = @GuidMatricula,
                              [COMPETENCIA] = @Competencia
                        WHERE [GUID] = @Guid ";
        }
    }
}