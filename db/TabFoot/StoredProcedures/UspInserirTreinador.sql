--EXEC UspInserirTecnico NULL, 'ANDRÉ'
--EXEC UspInserirTecnico NULL, 'DANIEL'
--EXEC UspInserirTecnico NULL, 'DENILSON'
--EXEC UspInserirTecnico NULL, 'DIEGO'
--EXEC UspInserirTecnico NULL, 'GERALDO'
--EXEC UspInserirTecnico NULL, 'GUSTAVO'
--EXEC UspInserirTecnico NULL, 'RODRIGO'

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspInserirTecnico]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspInserirTecnico]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspInserirTecnico]
	@Id AS UNIQUEIDENTIFIER = NULL,
	@Nome AS VARCHAR(75),
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

INSERT INTO [dbo].[Tecnico] ([Id],
                             [Nome],
							 [DataHoraInclusao],
							 [DataHoraUltimaAlteracao])
	 VALUES (@Id,
			 @Nome,
			 @DataHoraInclusao,
			 @DataHoraUltimaAlteracao)

GO