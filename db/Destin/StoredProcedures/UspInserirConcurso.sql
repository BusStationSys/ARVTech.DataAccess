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
	@Dezenas AS NVARCHAR(MAX) = NULL

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

INSERT INTO [dbo].[Concurso] ([Id],
                              [IdModalidade],
							  [Numero],
							  [DataApuracao])
	 VALUES (@Id,
	         @IdModalidade,
			 @Numero,
			 @DataApuracao)

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