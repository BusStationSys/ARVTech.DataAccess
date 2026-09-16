--exec [dbo].[UspInserirCompeticao] NULL, 'COPA CLÁUDIO CARSUGHI', '2026-09-12', '2026-09-12', 3, 1

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspInserirCompeticao]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspInserirCompeticao]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspInserirCompeticao]
	@Id AS UNIQUEIDENTIFIER = NULL,
	@Nome AS VARCHAR(75),
	@DataInicio AS DATE,
	@DataTermino AS DATE,
	@PontoVitoria AS TINYINT,
	@PontoEmpate AS TINYINT,
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

INSERT INTO [dbo].[Competicao] ([Id],
                                [Descricao],
					            [DataInicio],
								[DataTermino],
								[PontoVitoria],
								[PontoEmpate],
								[DataHoraInclusao],
								[DataHoraUltimaAlteracao])
	 VALUES (@Id,
			 @Nome,
			 @DataInicio,
			 @DataTermino,
			 @PontoVitoria,
			 @PontoEmpate,
			 @DataHoraInclusao,
			 @DataHoraUltimaAlteracao)

GO