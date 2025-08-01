--EXEC [UspObterTotalizadores]

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspInserirMatriculaEspelhoPonto]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspInserirMatriculaEspelhoPonto]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspInserirMatriculaEspelhoPonto]
    @GuidMatricula AS UNIQUEIDENTIFIER,
	@Competencia AS CHAR(8)

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @NewGuidMatriculaEspelhoPonto AS UNIQUEIDENTIFIER = NEWID()

 INSERT INTO [dbo].[MATRICULAS_ESPELHOS_PONTO] ([GUID],
                                                [GUIDMATRICULA],
												[COMPETENCIA])
                                        VALUES (@NewGuidMatriculaEspelhoPonto,
										        @GuidMatricula,
												@Competencia)

      SELECT @NewGuidMatriculaEspelhoPonto 

GO