--EXEC [UspObterTotalizadores]

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspInserirEvento]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspInserirEvento]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspInserirEvento]
	@Id AS INT,
	@Descricao AS VARCHAR(75),
	@Tipo AS CHAR(1),
	@Observacoes AS VARCHAR(MAX)

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

INSERT INTO [dbo].[EVENTOS] ([ID],
                             [DESCRICAO],
							 [TIPO],
							 [OBSERVACOES])
                     VALUES (@Id,
					         @Descricao,
							 @Tipo,
							 @Observacoes)

GO