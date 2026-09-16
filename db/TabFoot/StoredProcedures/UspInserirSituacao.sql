--EXEC UspInserirSituacao 1, 'AGENDADO/NÃO INICIADO'
--EXEC UspInserirSituacao 2, 'ADIADO'
--EXEC UspInserirSituacao 3, 'CANCELADO'
--EXEC UspInserirSituacao 3, 'EM ANDAMENTO'
--EXEC UspInserirSituacao 13, 'ENCERRADO'


If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspInserirSituacao]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspInserirSituacao]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspInserirSituacao]
	@Id AS SMALLINT,
	@Descricao AS VARCHAR(75),
	@DataHoraInclusao AS DATETIMEOFFSET = NULL,
	@DataHoraUltimaAlteracao AS DATETIMEOFFSET = NULL

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @DataAtual AS DATETIMEOFFSET = SYSDATETIMEOFFSET() AT TIME ZONE 'E. South America Standard Time'

SET @DataHoraInclusao = COALESCE(@DataHoraInclusao, @DataAtual)

SET @DataHoraUltimaAlteracao = COALESCE(@DataHoraUltimaAlteracao, @DataAtual)

INSERT INTO [dbo].[Situacao] ([Id],
                              [Descricao],
						      [DataHoraInclusao],
						      [DataHoraUltimaAlteracao])
	 VALUES (@Id,
			 @Descricao,
			 @DataHoraInclusao,
			 @DataHoraUltimaAlteracao)

GO