--EXEC [UspObterEventoPorId] 1

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspExcluirMatriculaDemonstrativoPagamentoPorId]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspExcluirMatriculaDemonstrativoPagamentoPorId]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspExcluirMatriculaDemonstrativoPagamentoPorId]
	@Guid UNIQUEIDENTIFIER

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

 DELETE MDP
   FROM [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO] AS MDP
  WHERE MDP.[GUID] = @Guid

GO