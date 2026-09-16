--EXEC UspInserirTecnico NULL, 'ANDRÉ'
--EXEC UspInserirTecnico NULL, 'DANIEL'
--EXEC UspInserirTecnico NULL, 'DENILSON'
--EXEC UspInserirTecnico NULL, 'DIEGO'
--EXEC UspInserirTecnico NULL, 'GERALDO'
--EXEC UspInserirTecnico NULL, 'GUSTAVO'
--EXEC UspInserirTecnico NULL, 'RODRIGO'

--SELECT * FROM Tecnico

--EXEC UspInserirTime NULL, 'JUVENTUS', 'AF3B5D73-D5D4-4A0C-9866-9B6085034254'
--EXEC UspInserirTime NULL, 'ORIENTE', '5CA4D8EC-B26D-48F5-8B75-128C315DF280'
--EXEC UspInserirTime NULL, 'RIVER PLATE', '61B74402-6D3B-4BD5-99F4-022B71B2E0DD'
--EXEC UspInserirTime NULL, 'REAL MADRID', '8C124B24-7937-4EBD-BD79-7B4A49D8092F'
--EXEC UspInserirTime NULL, 'MADUREIRA', '64F96EEB-EBE5-49AE-9A65-86F0282DC8E8'
--EXEC UspInserirTime NULL, 'SANTA CRUZ', '6A503586-339F-440C-B4C9-EEEE67E15085'

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspInserirTime]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspInserirTime]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspInserirTime]
	@Id AS UNIQUEIDENTIFIER = NULL,
	@Nome AS VARCHAR(75),
	@IdTecnico AS UNIQUEIDENTIFIER,
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

INSERT INTO [dbo].[Time] ([Id],
                          [Nome],
						  [IdTecnico],
						  [DataHoraInclusao],
						  [DataHoraUltimaAlteracao])
	 VALUES (@Id,
			 @Nome,
			 @IdTecnico,
			 @DataHoraInclusao,
			 @DataHoraUltimaAlteracao)

GO