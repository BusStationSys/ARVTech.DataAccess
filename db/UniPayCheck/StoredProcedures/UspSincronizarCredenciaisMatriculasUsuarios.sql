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
	@GuidColaborador UNIQUEIDENTIFIER = NULL,
	@DataInclusao DATETIME2 = NULL,
	@DataUltimaAlteracao DATETIME2 = NULL

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

IF @GuidColaborador IS NULL AND @DataInclusao IS NULL AND @DataUltimaAlteracao IS NULL
BEGIN
    RAISERROR ('Nenhum parâmetro foi especificado. É necessário informar pelo menos um.', 16, 1)
    RETURN
END

DECLARE @NomeColaboradorCursor AS VARCHAR(100)
DECLARE @MatriculaCursor AS VARCHAR(20)
DECLARE @GuidColaboradorCursor AS UNIQUEIDENTIFIER

--	Declarar o cursor FORWARD_ONLY.
DECLARE CursorColaborador CURSOR FORWARD_ONLY FOR
    SELECT PF.[NOME],
	       M.[MATRICULA],
		   M.[GUIDCOLABORADOR]
	  FROM dbo.[MATRICULAS] M
INNER JOIN dbo.[PESSOAS_FISICAS] PF ON M.[GUIDCOLABORADOR] = PF.[GUID]
     WHERE (@GuidColaborador IS NULL OR PF.[GUID] = @GuidColaborador)
	   AND (@DataInclusao IS NULL OR M.[DATA_INCLUSAO] = @DataInclusao)
	   AND (@DataUltimaAlteracao IS NULL OR M.[DATA_ULTIMA_ALTERACAO] = @DataUltimaAlteracao)

--	Abrir o cursor.
OPEN CursorColaborador

--	Obter o primeiro registro.
FETCH NEXT FROM CursorColaborador INTO @NomeColaboradorCursor, @MatriculaCursor, @GuidColaboradorCursor

--	O laço será executado enquanto houver registros.
WHILE @@FETCH_STATUS = 0
BEGIN
	PRINT @NomeColaboradorCursor

	--	Montagem do Username que será utilizado no registro do Usuário.
	DECLARE @FirstName AS VARCHAR(100) = dbo.[UfnObterPrimeiroNome](@NomeColaboradorCursor)
	DECLARE @LastName AS VARCHAR(100) = dbo.[UfnObterUltimoNome](@NomeColaboradorCursor)

	DECLARE @Username AS VARCHAR(75) = LOWER(CONCAT(@FirstName, '.', @LastName))
	DECLARE @Password AS VARCHAR(75) = @MatriculaCursor
	DECLARE @IdPerfilUsuario AS INT = 999999	--	Perfil de Colaborador

	DECLARE @DataJsonUsuario AS VARCHAR(MAX) = FORMATMESSAGE('{"Username": "%s", "Password": "%s", "IdPerfilUsuario": %d, "GuidColaborador": "%s"}', @Username, @Password, @IdPerfilUsuario, CONVERT(VARCHAR(36), @GuidColaboradorCursor))

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
	END

	--	Move para o próximo registro.
	FETCH NEXT FROM CursorColaborador INTO @NomeColaboradorCursor, @MatriculaCursor, @GuidColaboradorCursor
END

--	Fechar o cursor.
CLOSE CursorColaborador

--	Desalocar o cursor.
DEALLOCATE CursorColaborador

GO