--EXEC [UspObterEventoPorId] 1

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspExcluirTotalizadorPorId]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspExcluirTotalizadorPorId]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspExcluirTotalizadorPorId]
	@Id INT

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

 DELETE T
   FROM [dbo].[TOTALIZADORES] AS T
  WHERE T.[ID] = @Id

GO