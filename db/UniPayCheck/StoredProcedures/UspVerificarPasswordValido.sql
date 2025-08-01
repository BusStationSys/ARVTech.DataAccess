--exec uspVerificarPasswordValido '1E6C1757-8E9C-48E2-AF2E-3EE8F51CE40D', '(u53rM@1n)'

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspVerificarPasswordValido]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspVerificarPasswordValido]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO
 
 CREATE PROCEDURE [dbo].[UspVerificarPasswordValido]
	@GuidUsuario AS UNIQUEIDENTIFIER,
	@PasswordInput AS VARCHAR(75)

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

IF NOT EXISTS (SELECT TOP 1 1
                 FROM dbo.[USUARIOS]
				WHERE [GUID] = @GuidUsuario)
BEGIN
	RAISERROR ('Usu�rio n�o encontrado.', 16, 1)
	RETURN
END

DECLARE @PasswordHash		AS VARBINARY(8000)
DECLARE @PasswordHashInput	AS VARBINARY(8000)
DECLARE @Salt				AS VARBINARY(128)

-- Obter o Salt e o Password Hash armazenados.
SELECT @PasswordHash = [PASSWORD_HASH],
       @Salt = [SALT]
  FROM dbo.[USUARIOS]
 WHERE [GUID] = @GuidUsuario

-- Combinar o password do par�metro com o Salt e gerar o Hash.
SET @PasswordHashInput = HASHBYTES('SHA2_512', @PasswordInput + CONVERT(VARCHAR(MAX), @Salt))

IF @PasswordHash = @PasswordHashInput
	SELECT @GuidUsuario AS 'GUID'	--	Se os HASHES coincidirem, a Autentica��o foi um sucesso
ELSE
	SELECT NULL	AS 'GUID'			--	Caso contr�rio, a senha informada n�o � v�lida.
--BEGIN
	--RAISERROR ('A Senha informada n�o coincide.', 16, 1)	--	Caso contr�rio, a senha informada n�o � v�lida.
	--RETURN
--END

GO