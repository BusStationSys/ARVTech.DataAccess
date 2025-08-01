--EXEC [UspObterTotalizadores]

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspInserirMatriculaDemonstrativoPagamento]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspInserirMatriculaDemonstrativoPagamento]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspInserirMatriculaDemonstrativoPagamento]
    @GuidMatricula AS UNIQUEIDENTIFIER,
	@Competencia AS CHAR(8)

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @NewGuidMatriculaDemonstrativoPagamento AS UNIQUEIDENTIFIER = NEWID()

 INSERT INTO [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO] ([GUID],
                                                          [GUIDMATRICULA],
														  [COMPETENCIA])
                                                  VALUES (@NewGuidMatriculaDemonstrativoPagamento,
												          @GuidMatricula,
														  @Competencia)

      SELECT @NewGuidMatriculaDemonstrativoPagamento 

GO