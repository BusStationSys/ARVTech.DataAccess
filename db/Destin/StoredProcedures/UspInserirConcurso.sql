If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspInserirConcurso]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspInserirConcurso]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspInserirConcurso]
	@Id AS UNIQUEIDENTIFIER,
	@IdModalidade AS INT,
	@Numero AS INT,
	@DataApuracao AS DATE,
	@DataHoraInclusao AS DATETIMEOFFSET = NULL,
	@DataHoraUltimaAlteracao AS DATETIMEOFFSET = NULL,
	@Dezenas AS NVARCHAR(MAX) = NULL

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @DataAtual AS DATETIMEOFFSET = SYSDATETIMEOFFSET() AT TIME ZONE 'E. South America Standard Time'

SET @DataHoraInclusao = COALESCE(@DataHoraInclusao, @DataAtual)

SET @DataHoraUltimaAlteracao = COALESCE(@DataHoraUltimaAlteracao, @DataAtual)

INSERT INTO [dbo].[Concurso] ([Id],
                              [IdModalidade],
							  [Numero],
							  [DataApuracao],
							  [DataHoraInclusao],
							  [DataHoraUltimaAlteracao])
	 VALUES (@Id,
	         @IdModalidade,
			 @Numero,
			 @DataApuracao,
			 @DataHoraInclusao,
			 @DataHoraUltimaAlteracao)

DELETE CD
  FROM [dbo].[ConcursoDezena] CD
 WHERE CD.[IdConcurso] = @Id

--	Processa as Dezenas (JSON).
SET @Dezenas = NULLIF(LTRIM(RTRIM(@Dezenas)), '')

IF @Dezenas IS NULL
	SET @Dezenas = N'[]'

IF ISJSON(@Dezenas) = 1
BEGIN
	INSERT INTO [dbo].[ConcursoDezena] ([Id],
	                                    [IdConcurso],
										[Dezena])
         SELECT NEWID(),
		        @Id,
				Dezena
	       FROM OPENJSON(@Dezenas)
	       WITH
		   (
		      Dezena TINYINT '$.Dezena'
		   );
END

GO