--EXEC UspInserirCarneJogo NULL, 
--'2026-09-12 18:15:00 -03:00',
--'B51E1C8C-9CBE-4A13-B110-1AFDACD95AD6', --(M)
----'72340412-F246-4FB7-AB79-DCE36EA8D2B5', --(G)
--13,
--'E1156881-B26A-4548-9EE4-957428DB5DFF',
--'AA83A63D-C0A1-4743-BA07-F0B109DD9691',
--3,
--'CDD76DD5-EED9-45F1-A887-3D58B014475D',
--0

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspInserirCarneJogo]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspInserirCarneJogo]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspInserirCarneJogo]
	@Id AS UNIQUEIDENTIFIER = NULL,
	@DataHora AS DATETIMEOFFSET,
	@IdLocal AS UNIQUEIDENTIFIER,
	@IdSituacao AS SMALLINT,
	@IdCompeticaoEtapa AS UNIQUEIDENTIFIER,
	@IdCompeticaoTimeMandante AS UNIQUEIDENTIFIER,
	@PlacarMandante AS TINYINT,
	@IdCompeticaoTimeVisitante AS UNIQUEIDENTIFIER,
	@PlacarVisitante AS TINYINT,
	@DataHoraInclusao AS DATETIMEOFFSET = NULL,
	@DataHoraUltimaAlteracao AS DATETIMEOFFSET = NULL

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @DataAtual AS DATETIMEOFFSET = SYSDATETIMEOFFSET() AT TIME ZONE 'E. South America Standard Time'

SET @Id = COALESCE(@Id, NEWID())

SET @DataHoraInclusao = COALESCE(@DataHoraInclusao, @DataAtual)

SET @DataHoraUltimaAlteracao = COALESCE(@DataHoraUltimaAlteracao, @DataAtual)

INSERT INTO [dbo].[CarneJogo] ([Id],
                               [DataHora],
							   [IdLocal],
							   [IdSituacao],
							   [IdCompeticaoEtapa],
							   [IdCompeticaoTimeMandante],
							   [PlacarMandante],
							   [IdCompeticaoTimeVisitante],
							   [PlacarVisitante],
							   [DataHoraInclusao],
							   [DataHoraUltimaAlteracao])
	 VALUES (@Id,
	         @DataHora,
			 @IdLocal,
			 @IdSituacao,
	         @IdCompeticaoEtapa,
			 @IdCompeticaoTimeMandante,
			 @PlacarMandante,
			 @IdCompeticaoTimeVisitante,
			 @PlacarVisitante,
			 @DataHoraInclusao,
			 @DataHoraUltimaAlteracao)

GO