--EXEC UspInserirCompeticaoEtapa, NULL, '1A37501E-9AF5-41D5-9F49-59173954D64D', 'TEMPORADA 2026'
--EXEC UspInserirCompeticaoEtapa NULL, '1A37501E-9AF5-41D5-9F49-59173954D64D', 'RODADA 1', '2C8A9E25-27D4-4A89-96E0-9B0E1CA9E5B0', 1
--EXEC UspInserirCompeticaoEtapa NULL, '1A37501E-9AF5-41D5-9F49-59173954D64D', 'RODADA 2', '2C8A9E25-27D4-4A89-96E0-9B0E1CA9E5B0', 2
--EXEC UspInserirCompeticaoEtapa NULL, '1A37501E-9AF5-41D5-9F49-59173954D64D', 'RODADA 3', '2C8A9E25-27D4-4A89-96E0-9B0E1CA9E5B0', 3
--EXEC UspInserirCompeticaoEtapa NULL, '1A37501E-9AF5-41D5-9F49-59173954D64D', 'RODADA 4', '2C8A9E25-27D4-4A89-96E0-9B0E1CA9E5B0', 4
--EXEC UspInserirCompeticaoEtapa NULL, '1A37501E-9AF5-41D5-9F49-59173954D64D', 'RODADA 5', '2C8A9E25-27D4-4A89-96E0-9B0E1CA9E5B0', 5
--EXEC UspInserirCompeticaoEtapa NULL, '1A37501E-9AF5-41D5-9F49-59173954D64D', 'RODADA 6', '2C8A9E25-27D4-4A89-96E0-9B0E1CA9E5B0', 6
--EXEC UspInserirCompeticaoEtapa NULL, '1A37501E-9AF5-41D5-9F49-59173954D64D', 'RODADA 7', '2C8A9E25-27D4-4A89-96E0-9B0E1CA9E5B0', 7
--EXEC UspInserirCompeticaoEtapa NULL, '1A37501E-9AF5-41D5-9F49-59173954D64D', 'RODADA 8', '2C8A9E25-27D4-4A89-96E0-9B0E1CA9E5B0', 8
--EXEC UspInserirCompeticaoEtapa NULL, '1A37501E-9AF5-41D5-9F49-59173954D64D', 'RODADA 9', '2C8A9E25-27D4-4A89-96E0-9B0E1CA9E5B0', 9
--EXEC UspInserirCompeticaoEtapa NULL, '1A37501E-9AF5-41D5-9F49-59173954D64D', 'RODADA 10', '2C8A9E25-27D4-4A89-96E0-9B0E1CA9E5B0', 10

--select * 
--select * from Competicaoetapa

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspInserirCompeticaoEtapa]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspInserirCompeticaoEtapa]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspInserirCompeticaoEtapa]
	@Id AS UNIQUEIDENTIFIER = NULL,
	@IdCompeticao AS UNIQUEIDENTIFIER,
	@Descricao AS VARCHAR(75),
	@IdVinculo AS UNIQUEIDENTIFIER = NULL,
	@Ordem AS SMALLINT = 1,
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

INSERT INTO [dbo].[CompeticaoEtapa] ([Id],
                                     [IdCompeticao],
                                     [Descricao],
									 [IdVinculo],
									 [Ordem],
									 [DataHoraInclusao],
									 [DataHoraUltimaAlteracao])
	 VALUES (@Id,
	         @IdCompeticao,
			 @Descricao,
			 @IdVinculo,
			 @Ordem,
			 @DataHoraInclusao,
			 @DataHoraUltimaAlteracao)

GO