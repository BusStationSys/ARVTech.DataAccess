--EXEC [UspAtualizarEvento]

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspAtualizarEvento]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspAtualizarEvento]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspAtualizarEvento]
	@Id AS INT,
	@Descricao AS VARCHAR(75),
	@Tipo AS CHAR(1),
	@Observacoes AS VARCHAR(MAX)

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

  UPDATE [dbo].[EVENTOS]
     SET [DESCRICAO] = @Descricao,
		 [TIPO] = @Tipo,
         [OBSERVACOES] = @Observacoes
   WHERE [ID] = @Id

GO