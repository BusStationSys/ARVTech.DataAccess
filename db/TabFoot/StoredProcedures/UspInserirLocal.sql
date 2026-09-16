--EXEC UspInserirLocal NULL, 'GIGANTE DOS EUCALIPTOS'
--EXEC UspInserirLocal NULL, 'MONUMENTAL DE CHÁCARA BARRETO'

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspInserirLocal]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspInserirLocal]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspInserirLocal]
	@Id AS UNIQUEIDENTIFIER = NULL,
	@Descricao AS VARCHAR(75),
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

INSERT INTO [dbo].[Local] ([Id],
                           [Descricao],
						   [DataHoraInclusao],
						   [DataHoraUltimaAlteracao])
	 VALUES (@Id,
			 @Descricao,
			 @DataHoraInclusao,
			 @DataHoraUltimaAlteracao)

GO