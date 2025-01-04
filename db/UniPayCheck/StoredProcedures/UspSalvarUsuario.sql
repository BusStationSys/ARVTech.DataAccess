--exec uspSalvarUsuario '{"Guid":"00000000-0000-0000-0000-000000000000","Username":"UserMain","Password":"(u53rM@1n)","DataPrimeiroAcesso":"2024-12-29T06:00:00.6234419+00:00","IdPerfilUsuario":1}'

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspSalvarUsuario]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspSalvarUsuario]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO
 
 CREATE PROCEDURE [dbo].[UspSalvarUsuario]
	@DataJson AS VARCHAR(MAX)

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

IF ISJSON(@DataJson) = 0
BEGIN
	RAISERROR ('Parâmetro @DataJson não está no formato JSON.', 16, 1)
	RETURN
END

DECLARE @DataAtual			AS DATETIME2 = GETDATE()

DECLARE @DataPrimeiroAcesso	AS DATETIME2
DECLARE @Email				AS VARCHAR(75)
DECLARE @Guid				AS UNIQUEIDENTIFIER
DECLARE @GuidColaborador	AS UNIQUEIDENTIFIER
DECLARE @IdAspNetUser		AS VARCHAR(450)
DECLARE @IdPerfilUsuario	AS INT
DECLARE @Username			AS VARCHAR(75)
DECLARE @Password			AS VARCHAR(75)

SELECT @Guid = JSON_VALUE(@DataJson, '$.Guid'),
       @GuidColaborador	= JSON_VALUE(@DataJson, '$.GuidColaborador'),
	   @IdPerfilUsuario	= JSON_VALUE(@DataJson, '$.IdPerfilUsuario'),
	   @Email = JSON_VALUE(@DataJson, '$.Email'),
	   @Username = JSON_VALUE(@DataJson, '$.Username'),
	   @Password = JSON_VALUE(@DataJson, '$.Password'),
	   @IdAspNetUser = JSON_VALUE(@DataJson, '$.IdAspNetUser'),
	   @DataPrimeiroAcesso = JSON_VALUE(@DataJson, '$.DataPrimeiroAcesso')

--	Um novo Salt e PasswordHash serão sempre gerados, independentemente de ser um INSERT ou UPDATE.
DECLARE @Salt AS VARBINARY(128) = CRYPT_GEN_RANDOM(128)	--	Gerar um novo Salt.

DECLARE @PasswordHash AS VARBINARY(8000) = HASHBYTES(
										      'SHA2_512',
											   @Password + CONVERT(VARCHAR(MAX), @Salt))	--	Gerar o Hash do Password combinando o Password com o Salt.

--PRINT @Guid
--PRINT @Password
--PRINT CONVERT(VARCHAR(128), @Salt)
--PRINT CONVERT(VARCHAR(8000), @PasswordHash)
--return

IF @Guid IS NULL OR 
   @Guid = CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY, 0)) OR
   NOT EXISTS (SELECT TOP 1 1 FROM dbo.[USUARIOS] WHERE [GUID] = @Guid)		--	Se o Guid for nulo, vazio ou se no caso de informado não existir, vai inserir.
BEGIN
	DECLARE @NewGuid AS UNIQUEIDENTIFIER = NEWID()

	-- Inserir o registro existente.
	INSERT INTO dbo.[USUARIOS] ([GUID], [GUIDCOLABORADOR], [IDPERFIL_USUARIO], [DATA_INCLUSAO], [DATA_ULTIMA_ALTERACAO],
	                            [EMAIL], [USERNAME], [PASSWORD_HASH], [SALT], [IDASPNETUSER], [DATA_PRIMEIRO_ACESSO])
                        VALUES (@NewGuid, @GuidColaborador, @IdPerfilUsuario, @DataAtual, @DataAtual,
						        @Email, @Username, @PasswordHash, @Salt, @IdAspNetUser, @DataPrimeiroAcesso)

	SELECT @NewGuid		--	Retorna o ID inserido.
END
ELSE
BEGIN
	-- Atualizar o registro existente.
    UPDATE dbo.[USUARIOS]
       SET [GUIDCOLABORADOR] = @GuidColaborador,
           [IDPERFIL_USUARIO] = @IdPerfilUsuario,
           [DATA_ULTIMA_ALTERACAO] = @DataAtual,
           [EMAIL] = @Email,
           [USERNAME] = @Username,
           [PASSWORD_HASH] = @PasswordHash,
           [SALT] = @Salt,
           [IDASPNETUSER] = @IdAspNetUser,
           [DATA_PRIMEIRO_ACESSO] = @DataPrimeiroAcesso
     WHERE [GUID] = @Guid

	 SELECT @Guid		--	Retorna o ID atualizado.
END

GO