--exec UspSincronizarCredenciaisMatriculasUsuarios @GuidColaborador='73BF04D1-3B7B-44EB-B340-03E2DC2E5226'
--exec UspSincronizarCredenciaisMatriculasUsuarios @DataInclusao='2025-01-04 03:01:12.7400000'
--exec UspSincronizarCredenciaisMatriculasUsuarios @DataUltimaAlteracao='2024-12-31 23:59:59'

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspSincronizarCredenciaisMatriculasUsuarios]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	 DROP PROCEDURE [dbo].[UspSincronizarCredenciaisMatriculasUsuarios]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

 CREATE PROCEDURE [dbo].[UspSincronizarCredenciaisMatriculasUsuarios]
	@Credenciais dbo.CredenciaisType READONLY

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @QuantidadeRegistrosInseridos   AS INT = 0
DECLARE @QuantidadeRegistrosInalterados AS INT = 0

DECLARE @GuidColaboradorCursor AS UNIQUEIDENTIFIER = NULL
DECLARE @UsernameCursor AS VARCHAR(75) = NULL
DECLARE @PasswordHashCursor AS VARCHAR(256) = NULL

--	Declarar o cursor FORWARD_ONLY.
DECLARE CursorCredencial CURSOR FORWARD_ONLY FOR
    SELECT [GUIDCOLABORADOR],
	       [USERNAME],
		   [PASSWORD_HASH]
	  FROM @Credenciais

--	Abrir o cursor.
OPEN CursorCredencial

--	Obter o primeiro registro.
FETCH NEXT FROM CursorCredencial INTO @GuidColaboradorCursor, @UsernameCursor, @PasswordHashCursor

--	O laço será executado enquanto houver registros.
WHILE @@FETCH_STATUS = 0
BEGIN
	PRINT @GuidColaboradorCursor

	--	Montagem do Username que será utilizado no registro do Usuário.
	DECLARE @IdPerfilUsuario AS INT = 999999	--	Perfil de Colaborador

	DECLARE @DataJsonUsuario AS VARCHAR(MAX) = FORMATMESSAGE('{"Username": "%s", "PasswordHash": "%s", "IdPerfilUsuario": %d, "GuidColaborador": "%s"}', @UsernameCursor, @PasswordHashCursor, @IdPerfilUsuario, CONVERT(VARCHAR(36), @GuidColaboradorCursor))

	--print @DataJsonUsuario

	--IF ISJSON(@DataJsonUsuario) = 0
	--BEGIN
	--	PRINT ('JSON inválido.')
	--END

	--	Se não existir o registro do Usuário para o Colaborador do Cursor, salva. Do contrário, não faz nada e vai para o próximo o registro do laço.
	IF NOT EXISTS(SELECT TOP 1 [GUID]
	                FROM dbo.[USUARIOS]
				   WHERE [GUIDCOLABORADOR] = @GuidColaboradorCursor)
	BEGIN
		EXEC uspSalvarUsuario @DataJson = @DataJsonUsuario,
		                      @ExibirRetorno = 0

		SET @QuantidadeRegistrosInseridos = @QuantidadeRegistrosInseridos + 1
	END
	ELSE
	BEGIN
		SET @QuantidadeRegistrosInalterados = @QuantidadeRegistrosInalterados + 1
	END

	--	Move para o próximo registro.
	FETCH NEXT FROM CursorCredencial INTO @GuidColaboradorCursor, @UsernameCursor, @PasswordHashCursor
END

--	Fechar o cursor.
CLOSE CursorCredencial

--	Desalocar o cursor.
DEALLOCATE CursorCredencial

SELECT 0 AS 'QUANTIDADE_REGISTROS_ATUALIZADOS',
       @QuantidadeRegistrosInalterados AS 'QUANTIDADE_REGISTROS_INALTERADOS',
	   @QuantidadeRegistrosInseridos   AS 'QUANTIDADE_REGISTROS_INSERIDOS'

GO