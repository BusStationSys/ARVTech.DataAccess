If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspAtualizarTime]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspAtualizarTime]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[UspAtualizarTime]
	@Id AS UNIQUEIDENTIFIER,
	@Nome AS VARCHAR(75),
	@IdTecnico AS UNIQUEIDENTIFIER,
	@DataHoraUltimaAlteracao AS DATETIME = NULL

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @DataAtual AS DATETIMEOFFSET = SYSDATETIMEOFFSET() AT TIME ZONE 'E. South America Standard Time'

SET @DataHoraUltimaAlteracao = COALESCE(@DataHoraUltimaAlteracao, @DataAtual)

  UPDATE [dbo].[Time]
     SET [Nome] = @Nome,
	     [IdTecnico] = @IdTecnico,
	     [DataHoraUltimaAlteracao] = @DataHoraUltimaAlteracao
   WHERE [ID] = @Id
GO