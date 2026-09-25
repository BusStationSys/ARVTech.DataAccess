If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspAtualizarCompeticao]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspAtualizarCompeticao]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspAtualizarCompeticao]
	@Id AS UNIQUEIDENTIFIER,
	@Descricao AS VARCHAR(75),
	@DataInicio AS DATETIME,
	@DataTermino AS DATETIME = NULL,
	@PontoVitoria AS INT = NULL,
	@PontoEmpate AS INT = NULL,
	@DataHoraUltimaAlteracao AS DATETIME = NULL

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @DataAtual AS DATETIMEOFFSET = SYSDATETIMEOFFSET() AT TIME ZONE 'E. South America Standard Time'

SET @DataHoraUltimaAlteracao = COALESCE(@DataHoraUltimaAlteracao, @DataAtual)

  UPDATE [dbo].[Competicao]
     SET [Descricao] = @Descricao,
	     [DataInicio] = @DataInicio,
	     [DataTermino] = @DataTermino,
	     [PontoVitoria] = @PontoVitoria,
	     [PontoEmpate] = @PontoEmpate,
	     [DataHoraUltimaAlteracao] = @DataHoraUltimaAlteracao
   WHERE [ID] = @Id

GO