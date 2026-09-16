--EXEC [UspObterEventoPorId] 1

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspExcluirPessoaFisicaPorId]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspExcluirPessoaFisicaPorId]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspExcluirPessoaFisicaPorId]
	@Guid UNIQUEIDENTIFIER

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @GuidPessoa AS UNIQUEIDENTIFIER = ( SELECT TOP 1 GUIDPESSOA 
                                              FROM [dbo].[PESSOAS_FISICAS]
											 WHERE [GUID] = @Guid )

DELETE U
  FROM [dbo].[USUARIOS] AS U
 WHERE U.[GUIDCOLABORADOR] = @Guid

DELETE PF
  FROM [dbo].[PESSOAS_FISICAS] AS PF
 WHERE PF.[GUID] = @Guid

DELETE P
  FROM [dbo].[PESSOAS] AS P
 WHERE P.[GUID] = @GuidPessoa

GO