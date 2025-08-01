--EXEC [UspObterTotalizadores]

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspAtualizarMatriculaDemonstrativoPagamento]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspAtualizarMatriculaDemonstrativoPagamento]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspAtualizarMatriculaDemonstrativoPagamento]
	@Guid AS UNIQUEIDENTIFIER,
	@GuidMatricula AS UNIQUEIDENTIFIER,
	@Competencia AS CHAR(8),
	@DataConfirmacao AS DATETIME2,
	@IpConfirmacao AS VARBINARY(16)

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

 UPDATE MDP
    SET [GUIDMATRICULA] = @GuidMatricula,
        [COMPETENCIA] = @Competencia,
        [DATA_ULTIMA_ALTERACAO] = GETDATE(),
        [DATA_CONFIRMACAO] = @DataConfirmacao,
        [IP_CONFIRMACAO] = @IpConfirmacao
   FROM [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO] AS MDP
  WHERE MDP.[GUID] = @Guid

GO