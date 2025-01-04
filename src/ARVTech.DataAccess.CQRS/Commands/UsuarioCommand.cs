namespace ARVTech.DataAccess.CQRS.Commands
{
    using ARVTech.Shared;

    public class UsuarioCommand : BaseCommand
    {
        public override string CommandTextCreate()
        {
            return $@" DECLARE @NewGuidUsuario UniqueIdentifier
                           SET @NewGuidUsuario = NEWID()
                       DECLARE @DataAtual DATETIME2 = GETDATE()

                        INSERT INTO [dbo].[{Constants.TableNameUsuarios}]
                                    ([GUID],
                                     [GUIDCOLABORADOR],
                                     [IDPERFIL_USUARIO],
                                     [EMAIL],
                                     [USERNAME],
                                     [PASSWORD_HASH],
                                     [SALT],
                                     [IDASPNETUSER],
                                     [DATA_PRIMEIRO_ACESSO],
                                     [DATA_INCLUSAO],
                                     [DATA_ULTIMA_ALTERACAO])
                             VALUES (@NewGuidUsuario,
                                     @GuidColaborador,
                                     @IdPerfilUsuario,
                                     @Email,
                                     @Username,
                                     CONVERT(VARBINARY(8000), ''),
                                     CONVERT(VARBINARY(MAX), ''),
                                     @IdAspNetUser,
                                     @DataPrimeiroAcesso,
                                     @DataAtual,
                                     @DataAtual)

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