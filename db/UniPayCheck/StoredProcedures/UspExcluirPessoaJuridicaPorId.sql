--EXEC [UspObterEventoPorId] 1

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspExcluirPessoaJuridicaPorId]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspExcluirPessoaJuridicaPorId]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspExcluirPessoaJuridicaPorId]
	@Guid UNIQUEIDENTIFIER

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @GuidPessoa AS UNIQUEIDENTIFIER = ( SELECT TOP 1 GUIDPESSOA 
                                              FROM [dbo].[PESSOAS_JURIDICAS]
											 WHERE [GUID] = @Guid )

DELETE PJ
  FROM [dbo].[PESSOAS_JURIDICAS] AS PJ
 WHERE PJ.[GUID] = @Guid

DELETE P
  FROM [dbo].[PESSOAS] AS P
 WHERE P.[GUID] = @GuidPessoa

GO