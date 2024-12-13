namespace ARVTech.DataAccess.CQRS.Commands
{
    using ARVTech.Shared;

    public class UsuarioCommand : BaseCommand
    {
        public override string CommandTextCreate()
        {
            return $@" DECLARE @NewGuidUsuario UniqueIdentifier
                           SET @NewGuidUsuario = NEWID()

                        INSERT INTO [dbo].[{Constants.TableNameUsuarios}]
                                    ([GUID],
                                     [GUIDCOLABORADOR],
                                     [IDPERFIL_USUARIO],
                                     [EMAIL],
                                     [USERNAME],
                                     [PASSWORD],
                                     [IDASPNETUSER],
                                     [DATA_PRIMEIRO_ACESSO],
                                     [DATA_INCLUSAO])
                             VALUES (@NewGuidUsuario,
                                     @GuidColaborador,
                                     @IdPerfilUsuario,
                                     @Email,
                                     @Username,
                                     @Password,
                                     @IdAspNetUser,
                                     @DataPrimeiroAcesso,
                                     GETUTCDATE())

                              SELECT @NewGuidUsuario ";
        }

        public override string CommandTextDelete()
        {
            return $@" DELETE
                         FROM [dbo].[{Constants.TableNameUsuarios}]
                        WHERE [GUID] = @Guid ";
        }

        public override string CommandTextUpdate()
        {
            return $@" UPDATE [dbo].[{Constants.TableNameUsuarios}]
                          SET [GUIDCOLABORADOR] = @GuidColaborador,
                              [IDPERFIL_USUARIO] = @IdPerfilUsuario,
                              [EMAIL] = @Email,
                              [USERNAME] = @Username,
                              [DATA_PRIMEIRO_ACESSO] = @DataPrimeiroAcesso,
                              [DATA_ULTIMA_ALTERACAO] = GETUTCDATE(),
                              [PASSWORD] = @Password
                        WHERE [GUID] = @Guid ";
        }
    }
}