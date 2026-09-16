--EXEC [UspObterTotalizadores]

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspInserirTotalizador]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspInserirTotalizador]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspInserirTotalizador]
	@Id AS INT,
	@Descricao AS VARCHAR(75),
	@Observacoes AS VARCHAR(MAX)

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

INSERT INTO [dbo].[TOTALIZADORES] ([ID],
                                   [DESCRICAO],
								   [OBSERVACOES])
                           VALUES (@Id,
						           @Descricao,
								   @Observacoes)

GO