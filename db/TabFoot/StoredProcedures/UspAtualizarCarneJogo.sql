If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspAtualizarCarneJogo]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspAtualizarCarneJogo]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[UspAtualizarCarneJogo]
	@Id AS UNIQUEIDENTIFIER,
	@DataHora AS DATETIME,
	@IdLocal AS UNIQUEIDENTIFIER,
	@IdSituacao AS INT,
	@IdCompeticaoEtapa AS UNIQUEIDENTIFIER,
	@IdCompeticaoTimeMandante AS UNIQUEIDENTIFIER,
	@PlacarMandante AS TINYINT = NULL,
	@IdCompeticaoTimeVisitante AS UNIQUEIDENTIFIER,
	@PlacarVisitante AS TINYINT = NULL,
	@DataHoraUltimaAlteracao AS DATETIME = NULL

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @DataAtual AS DATETIMEOFFSET = SYSDATETIMEOFFSET() AT TIME ZONE 'E. South America Standard Time'

SET @DataHoraUltimaAlteracao = COALESCE(@DataHoraUltimaAlteracao, @DataAtual)

  UPDATE [dbo].[CarneJogo]
     SET [DataHora] = @DataHora,
	     [IdLocal] = @IdLocal,
	     [IdSituacao] = @IdSituacao,
	     [IdCompeticaoEtapa] = @IdCompeticaoEtapa,
	     [IdCompeticaoTimeMandante] = @IdCompeticaoTimeMandante,
	     [PlacarMandante] = @PlacarMandante,
	     [IdCompeticaoTimeVisitante] = @IdCompeticaoTimeVisitante,
	     [PlacarVisitante] = @PlacarVisitante,
	     [DataHoraUltimaAlteracao] = @DataHoraUltimaAlteracao
   WHERE [ID] = @Id
GO