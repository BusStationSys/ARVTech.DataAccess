If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspInserirModalidade]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspInserirModalidade]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspInserirModalidade]
	@Id AS INT,
	@Descricao AS VARCHAR(75),
	@UltimoConcursoApurado AS INT = NULL

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

INSERT INTO [dbo].[Modalidade] ([Id],
                                [Descricao],
								[UltimoConcursoApurado])
	 VALUES (@Id,
	         @Descricao,
			 @UltimoConcursoApurado)

GO