--EXEC [UspObterTotalizadores]

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspAtualizarMatriculaEspelhoPonto]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspAtualizarMatriculaEspelhoPonto]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspAtualizarMatriculaEspelhoPonto]
	@Guid AS UNIQUEIDENTIFIER,
	@GuidMatricula AS UNIQUEIDENTIFIER,
	@Competencia AS CHAR(8),
	@DataConfirmacao AS DATETIME2,
	@IpConfirmacao AS VARBINARY(16)

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

 UPDATE MEP
    SET [GUIDMATRICULA] = @GuidMatricula,
        [COMPETENCIA] = @Competencia,
        [DATA_ULTIMA_ALTERACAO] = GETDATE(),
		[DATA_CONFIRMACAO] = @DataConfirmacao,
		[IP_CONFIRMACAO] = @IpConfirmacao
   FROM [dbo].[MATRICULAS_ESPELHOS_PONTO] AS MEP
  WHERE MEP.[GUID] = @Guid

GO